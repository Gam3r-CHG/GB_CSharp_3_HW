﻿// <auto-generated />
using System;
using HW5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HW5.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20231223121736_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HW5.Models.Message", b =>
                {
                    b.Property<int?>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("MessageId"));

                    b.Property<DateTime>("DateSend")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("messageData");

                    b.Property<bool>("IsSent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_sent");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("messageText");

                    b.Property<int?>("UserFromId")
                        .HasColumnType("integer");

                    b.Property<int?>("UserToId")
                        .HasColumnType("integer");

                    b.HasKey("MessageId")
                        .HasName("messagePk");

                    b.HasIndex("UserFromId");

                    b.HasIndex("UserToId");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("HW5.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("FullName");

                    b.HasKey("Id")
                        .HasName("userPk");

                    b.HasIndex("FullName")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("HW5.Models.Message", b =>
                {
                    b.HasOne("HW5.Models.User", "UserFrom")
                        .WithMany("MessagesFrom")
                        .HasForeignKey("UserFromId")
                        .HasConstraintName("messageFromUserFK");

                    b.HasOne("HW5.Models.User", "UserTo")
                        .WithMany("MessagesTo")
                        .HasForeignKey("UserToId")
                        .HasConstraintName("messageToUserFK");

                    b.Navigation("UserFrom");

                    b.Navigation("UserTo");
                });

            modelBuilder.Entity("HW5.Models.User", b =>
                {
                    b.Navigation("MessagesFrom");

                    b.Navigation("MessagesTo");
                });
#pragma warning restore 612, 618
        }
    }
}