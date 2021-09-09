using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Garage3.Models.Validations
{
    // Class for dealing with person identity numbers. There are three
    // overall types of identities for a person:
    // - Personnummer. This is the standard number assigned to any person
    //   who is accepted as a Swedish citizen. It's unique and kept for life.
    // - Samordningsnummer. This is assigned to someone who is a
    //   registered visitor/occupant in Sweden and needs an identifier.
    //   The number is unique and is assigned by Skatteverket.
    // - Reservnummer. This is assigned e.g. when a visitor to a hospital
    //   does not wish or is unable to state their personnummer. At the
    //   time of writing there is no established method for coordinating
    //   this, hence numbers are not unique and may not follow any
    //   standardized format. Also, a single person may have multiple
    //   reservnummer IDs from different visits.
    //
    // Personnummer has one of the formats:
    // 1. YYYYMMDDNNNN
    // 2. YYYYMMDD-NNNN
    // 3. YYMMDD[+-]NNNN
    // The date component is the birth date of the person. The last four
    // digits historically encoded information about geographical birthplace
    // and gender, but nowadays only the gender is encoded. The last digit
    // is a checksum. The second to last digit is even for females, and odd
    // for males.
    //
    // Samordningsnummer has the same format as personnummer, but 60 has
    // been added to the birth day.
    //
    // Reservnummer could have any format. Wikipedia documents the following:
    //
    //  Socialstyrelsen rekommenderar ett korrekt födelsedatum
    //  samt minst en bokstav i de fyra sista siffrorna för
    //  att man inte ska råka skapa ett befintligt
    //  personnummer, men utöver det finns det inget fastlagt format.[3]
    //
    //  Socialstyrelsen hittade tre olika varianter av reservnummer i patientregistret för 2008:
    //
    //  Ca 12 000 hade en eller flera bokstäver i de fyra sista fälten och korrekt födelsedatum
    //  Ca 10 000 hade ett nummer som börjar med 50 - 99 och resten är ett löpnummer av olika sort
    //  Ca 3 000 har ett löpnummer
    //
    // We don't really have a good reason to distrust the validity of a
    // reservnummer; trying to "fail" parsing here will likely lead to
    // more problems. However, there are instances where the reservnummer
    // contains a legitimate birth date. We try to detect this and use
    // the century hint to derive an actual birth date if possible.
    // The problem with reservnummer, since it can look like anything, is
    // that it compromises our ability to validate a personnummer. If the
    // checksum is incorrect, does that mean it's a personnummer entered
    // incorrectly, or a reservnummer? Based on the text above, we can't
    // really assume that something is supposed to be a personnummer just
    // because it looks like one. So a failed checksum check only means
    // that we treat it as a reservenummer, not that we fail parsing
    // entirely. In general, there's a fair bit of heuristics applied
    // here based on assumptions about reality. Because why would we want
    // a national system for IDs that is actually consistent and
    // machine-readable without having to guess? :)
    //
    // We won't say something is a personnummer if it doesn't have a
    // valid checksum though. So if you did expect one (but why), and the
    // parser says it's something else, then you can detect errors that way.
    public class PersonIdentity
    {
        public enum PersonIdentityType
        {
            Personnummer,
            Samordningsnummer,
            Reservnummer
        }
        public enum GenderComponent
        {
            Male,
            Female,
            Unspecified
        }
        private static Regex PersonnummerRegexYYYY = new Regex(@"^(\d{4})(\d{2})(\d{2})-?(\d{4})$");
        private static Regex PersonnummerRegexYY = new Regex(@"^(\d{2})(\d{2})(\d{2})([-+])?(\d{4})$");
        private static Regex ReservnummerRegexYYYY = new Regex(@"^(\d{4})(\d{2})(\d{2})-?([0-9A-Za-z]{4})$");
        private static Regex ReservnummerRegexYY = new Regex(@"^(\d{2})(\d{2})(\d{2})[-]([0-9A-Za-z]{4})$");

        public PersonIdentityType IdentityType { get; }
        public string Value { get; }
        public DateTime BirthDate { get; }
        public GenderComponent Gender { get; }
        public int Age { get; }

        private PersonIdentity(PersonIdentityType type, string id, DateTime dateTime, GenderComponent gender)
        {
            IdentityType = type;
            Value = id;
            BirthDate = dateTime;
            Gender = gender;
       
            Age = DateTime.Today.Year - BirthDate.Year;

        }

        public static PersonIdentity Parse(string id, string centuryHint)
        {
            id = id.Trim();
            if (id.Length == 0)
                throw new ArgumentException("Person identifier cannot be empty");
            PersonIdentity pid = ParseAsPersonnummerYYYY(id, centuryHint);
            if (pid != null)
                return pid;
            pid = ParseAsPersonnummerYYYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsPersonnummerYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsReservnummerYYYY(id, centuryHint);
            if (pid != null)
                return pid;
            pid = ParseAsReservnummerYYYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsReservnummerYY(id);
            if (pid != null)
                return pid;
            return new PersonIdentity(PersonIdentityType.Reservnummer, id, new DateTime(2100-01-01), GenderComponent.Unspecified);
        }

        public static PersonIdentity Parse(string id)
        {
            id = id.Trim();
            if (id.Length == 0)
                throw new ArgumentException("Person identifier cannot be empty");
            PersonIdentity pid = ParseAsPersonnummerYYYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsPersonnummerYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsReservnummerYYYY(id);
            if (pid != null)
                return pid;
            pid = ParseAsReservnummerYY(id);
            if (pid != null)
                return pid;
            return new PersonIdentity(PersonIdentityType.Reservnummer, id, new DateTime(2100-01-01), GenderComponent.Unspecified);
        }

        private static PersonIdentity ParseAsPersonnummerYYYY(string id, string century)
        {
            return ParseAsPersonnummerYYYY(century + id);
        }

        private static PersonIdentity ParseAsPersonnummerYYYY(string id)
        {
            var m = PersonnummerRegexYYYY.Match(id);
            if (!m.Success)
                return null;
            int year = int.Parse(m.Groups[1].Value);
            int month = int.Parse(m.Groups[2].Value);
            int day = int.Parse(m.Groups[3].Value);

            PersonIdentityType type = PersonIdentityType.Personnummer;
            if (day >= 60)
            {
                day -= 60;
                type = PersonIdentityType.Samordningsnummer;
            }

            if (!IsValidDate(year, month, day))
            {
                return null;
            }

            var idStr = m.Groups[1].Value + m.Groups[2].Value + m.Groups[3].Value + m.Groups[4].Value;
            if (!VerifyChecksum(idStr))
            {
                return null;
            }

            var gender = GenderComponent.Male;
            if ((m.Groups[4].Value[2] - '0') % 2 == 0)
            {
                gender = GenderComponent.Female;
            }
            return new PersonIdentity(type, idStr, new DateTime(year, month, day), gender);
        }

        private static PersonIdentity ParseAsPersonnummerYY(string id)
        {
            var m = PersonnummerRegexYY.Match(id);
            if (!m.Success)
                return null;
            int year = int.Parse(m.Groups[1].Value);
            int month = int.Parse(m.Groups[2].Value);
            int day = int.Parse(m.Groups[3].Value);

            PersonIdentityType type = PersonIdentityType.Personnummer;
            if (day >= 60)
            {
                day -= 60;
                type = PersonIdentityType.Samordningsnummer;
            }

            var today = DateTime.Today;
            var century = today.Year / 100;
            year += 100 * century;
            if ((year, month, day).CompareTo((today.Year, today.Month, today.Day)) > 0)
            {
                year -= 100;
            }

            if (m.Groups[4].Value == "+")
            {
                year -= 100;
            }

            if (!IsValidDate(year, month, day))
            {
                return null;
            }

            var idStr = year.ToString() + m.Groups[2].Value + m.Groups[3].Value + m.Groups[5].Value;
            if (!VerifyChecksum(idStr))
            {
                return null;
            }

            var gender = GenderComponent.Male;
            if ((m.Groups[5].Value[3] - '0') % 2 == 0)
            {
                gender = GenderComponent.Female;
            }
            return new PersonIdentity(type, idStr, new DateTime(year, month, day), gender);
        }

        private static PersonIdentity ParseAsReservnummerYYYY(string id, string century)
        {
            return ParseAsReservnummerYYYY(century + id);
        }

        private static PersonIdentity ParseAsReservnummerYYYY(string id)
        {
            var m = ReservnummerRegexYYYY.Match(id);
            if (!m.Success)
                return null;

            int year = int.Parse(m.Groups[1].Value);
            int month = int.Parse(m.Groups[2].Value);
            int day = int.Parse(m.Groups[3].Value);

            if (!IsValidDate(year, month, day))
            {
                return null;
            }

            var idStr = year.ToString() + m.Groups[2].Value + m.Groups[3].Value + m.Groups[4].Value;
            return new PersonIdentity(PersonIdentityType.Reservnummer, idStr, new DateTime(year, month, day), GenderComponent.Unspecified);
        }

        private static PersonIdentity ParseAsReservnummerYY(string id)
        {
            var m = ReservnummerRegexYY.Match(id);
            if (!m.Success)
                return null;

            return new PersonIdentity(PersonIdentityType.Reservnummer,
                m.Groups[1].Value + m.Groups[2].Value, new DateTime(2100-01-01),
                GenderComponent.Unspecified);
        }

        private static bool IsValidDate(int year, int month, int day)
        {
            try
            {
                new DateTime(year, month, day);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        private static bool VerifyChecksum(string id)
        {
            if (id.Length != 12 || id.Any(c => c < '0' || c > '9'))
                return false;

            var checksum = ComputeChecksum(id);
            return checksum == id[11] - '0';
        }

        private static int ComputeChecksum(string id)
        {
            var checksum = 0;
            for (var i = 2; i != 2 + 9; i++)
            {
                var multiplier = ((i + 1) % 2) + 1;
                var p = multiplier * (id[i] - '0');
                checksum += p / 10 + p % 10;
            }
            return (10 - (checksum % 10)) % 10;
        }
    }
}
