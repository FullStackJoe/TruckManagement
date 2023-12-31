﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Shared;

#nullable disable

namespace EfcDataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.14");

            modelBuilder.Entity("WebApplication1.Shared.ChargingDBSchedule", b =>
                {
                    b.Property<int>("ChargingDBScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChargerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("TEXT");

                    b.HasKey("ChargingDBScheduleId");

                    b.ToTable("ChargingDBSchedule");
                });

            modelBuilder.Entity("WebApplication1.Shared.Measurement", b =>
                {
                    b.Property<int>("MeasurementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChargerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MeasurementKWH")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("MeasurementId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("WebApplication1.Shared.Settings", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DailyDeadline")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("SmartCharging")
                        .HasColumnType("INTEGER");

                    b.HasKey("SettingsId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("WebApplication1.Shared.SystemStatus", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("SystemStatus");
                });

            modelBuilder.Entity("WebApplication1.Shared.TruckType", b =>
                {
                    b.Property<int>("TruckTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BatterySizeAh")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TruckTypeId");

                    b.ToTable("TruckType");
                });

            modelBuilder.Entity("WebApplication1.Shared.WallCharger", b =>
                {
                    b.Property<int>("ChargerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChargerAmpere")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TurnOffUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TurnOnUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ChargerId");

                    b.ToTable("WallCharger");
                });
#pragma warning restore 612, 618
        }
    }
}
