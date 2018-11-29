﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlanningPoker.Entities;

namespace PlanningPoker.Entities.Migrations
{
    [DbContext(typeof(PlanningPokerContext))]
    [Migration("20181128174742_ItemsFixMigration")]
    partial class ItemsFixMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlanningPoker.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int?>("SessionId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PlanningPoker.Entities.ItemEstimate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Estimate");

                    b.Property<string>("ItemTitle")
                        .IsRequired();

                    b.Property<int?>("SummaryId");

                    b.HasKey("Id");

                    b.HasIndex("SummaryId");

                    b.ToTable("ItemEstimates");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ItemId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SessionKey")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Summary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SessionId");

                    b.HasKey("Id");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("PlanningPoker.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<bool>("IsHost");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("SessionId");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Estimate");

                    b.Property<int?>("RoundId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Item", b =>
                {
                    b.HasOne("PlanningPoker.Entities.Session")
                        .WithMany("Items")
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("PlanningPoker.Entities.ItemEstimate", b =>
                {
                    b.HasOne("PlanningPoker.Entities.Summary")
                        .WithMany("ItemEstimates")
                        .HasForeignKey("SummaryId");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Round", b =>
                {
                    b.HasOne("PlanningPoker.Entities.Item")
                        .WithMany("Rounds")
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("PlanningPoker.Entities.User", b =>
                {
                    b.HasOne("PlanningPoker.Entities.Session")
                        .WithMany("Users")
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("PlanningPoker.Entities.Vote", b =>
                {
                    b.HasOne("PlanningPoker.Entities.Round")
                        .WithMany("Votes")
                        .HasForeignKey("RoundId");
                });
#pragma warning restore 612, 618
        }
    }
}
