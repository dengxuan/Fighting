﻿// <auto-generated />
using Fighting.Scheduling.Abstractions;
using Fighting.Scheduling.Mysql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Fighting.Scheduling.Mysql.Migrations
{
    [DbContext(typeof(ScheduleDbContext))]
    partial class ScheduleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Fighting.Scheduling.Abstractions.Schedule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTime");

                    b.Property<bool>("IsAbandoned");

                    b.Property<DateTime?>("LastTryTime");

                    b.Property<DateTime>("NextTryTime");

                    b.Property<byte>("Priority");

                    b.Property<string>("SchedulerArgs")
                        .IsRequired()
                        .HasMaxLength(1048576);

                    b.Property<string>("SchedulerType")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<short>("TryCount");

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
