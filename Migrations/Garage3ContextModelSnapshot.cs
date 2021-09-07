﻿// <auto-generated />
using System;
using Garage3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Garage3.Migrations
{
    [DbContext(typeof(Garage3Context))]
    partial class Garage3ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Garage3.Models.Garage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<int>("SlotSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Garages");
                });

            modelBuilder.Entity("Garage3.Models.ParkedVehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChargeAmount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CheckInTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOutTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("VehicleId");

                    b.ToTable("ParkedVehicle");
                });

            modelBuilder.Entity("Garage3.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemberType")
                        .HasColumnType("int");

                    b.Property<string>("SSN")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Garage3.Models.Slot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GarageId")
                        .HasColumnType("int");

                    b.Property<int>("InUse")
                        .HasColumnType("int");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GarageId");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("Garage3.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Make")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Garage3.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("ParkedVehicleSlot", b =>
                {
                    b.Property<int>("ParkedVehiclesId")
                        .HasColumnType("int");

                    b.Property<int>("SlotsId")
                        .HasColumnType("int");

                    b.HasKey("ParkedVehiclesId", "SlotsId");

                    b.HasIndex("SlotsId");

                    b.ToTable("ParkedVehicleSlot");
                });

            modelBuilder.Entity("Garage3.Models.ParkedVehicle", b =>
                {
                    b.HasOne("Garage3.Models.Person", "Person")
                        .WithMany("ParkedVehicles")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Garage3.Models.Slot", b =>
                {
                    b.HasOne("Garage3.Models.Garage", "Garage")
                        .WithMany("Slots")
                        .HasForeignKey("GarageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Garage");
                });

            modelBuilder.Entity("Garage3.Models.Vehicle", b =>
                {
                    b.HasOne("Garage3.Models.Person", "Person")
                        .WithMany("Vehicles")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3.Models.VehicleType", "VehicleType")
                        .WithMany("Vehicle")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("ParkedVehicleSlot", b =>
                {
                    b.HasOne("Garage3.Models.ParkedVehicle", null)
                        .WithMany()
                        .HasForeignKey("ParkedVehiclesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3.Models.Slot", null)
                        .WithMany()
                        .HasForeignKey("SlotsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Garage3.Models.Garage", b =>
                {
                    b.Navigation("Slots");
                });

            modelBuilder.Entity("Garage3.Models.Person", b =>
                {
                    b.Navigation("ParkedVehicles");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Garage3.Models.VehicleType", b =>
                {
                    b.Navigation("Vehicle");
                });
#pragma warning restore 612, 618
        }
    }
}
