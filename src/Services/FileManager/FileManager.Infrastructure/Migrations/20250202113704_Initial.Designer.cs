﻿// <auto-generated />
using System;
using FileManager.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FileManager.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250202113704_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventBus.Models.EventLog", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("event_date");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("character varying(70)")
                        .HasColumnName("event_name");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<int>("TimesSent")
                        .HasColumnType("integer")
                        .HasColumnName("times_sent");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("topic");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.HasKey("EventId")
                        .HasName("pk_event_logs");

                    b.HasIndex("State", "TransactionId")
                        .HasDatabaseName("ix_event_logs_state_transaction_id");

                    b.ToTable("event_logs", (string)null);
                });

            modelBuilder.Entity("FileManager.Core.Models.File", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<string>("Group")
                        .HasColumnType("text")
                        .HasColumnName("group");

                    b.Property<byte>("GroupPermission")
                        .HasColumnType("smallint")
                        .HasColumnName("group_permission");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("boolean")
                        .HasColumnName("is_temporary");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mime_type");

                    b.Property<byte>("OtherPermission")
                        .HasColumnType("smallint")
                        .HasColumnName("other_permission");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<byte>("OwnerPermission")
                        .HasColumnType("smallint")
                        .HasColumnName("owner_permission");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_files");

                    b.HasIndex("IsTemporary")
                        .HasDatabaseName("ix_files_is_temporary");

                    b.HasIndex("Group", "GroupPermission")
                        .HasDatabaseName("ix_files_group_group_permission");

                    b.HasIndex("OwnerId", "OwnerPermission")
                        .HasDatabaseName("ix_files_owner_id_owner_permission");

                    b.ToTable("files", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
