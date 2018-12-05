﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanningPoker.Entities.Migrations
{
    public partial class ItemsFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Items",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Items");
        }
    }
}