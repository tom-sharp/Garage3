namespace Garage3.Models.Services
{
	public interface IPricing
	{
		int GetPrice(Vehicle vehicle, int membershiplevel);
	}
}
