﻿// <auto-generated />
using System;
using Bank.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bank.Data.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20220825115135_0001_Initial")]
    partial class _0001_Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("Bank.Data.Entities.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0m);

                    b.Property<int>("HolderId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MinBalance")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HolderId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccountTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Transactions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BankAccountTransaction");
                });

            modelBuilder.Entity("Bank.Data.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccountDepositTransaction", b =>
                {
                    b.HasBaseType("Bank.Data.Entities.BankAccountTransaction");

                    b.HasDiscriminator().HasValue("BankAccountDepositTransaction");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccountTransferTransaction", b =>
                {
                    b.HasBaseType("Bank.Data.Entities.BankAccountTransaction");

                    b.Property<int>("DestinationBankAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceBankAccountId")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("BankAccountTransferTransaction");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccountWithdrawalTransaction", b =>
                {
                    b.HasBaseType("Bank.Data.Entities.BankAccountTransaction");

                    b.HasDiscriminator().HasValue("BankAccountWithdrawalTransaction");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccount", b =>
                {
                    b.HasOne("Bank.Data.Entities.Person", "Holder")
                        .WithMany("Accounts")
                        .HasForeignKey("HolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holder");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccountTransaction", b =>
                {
                    b.HasOne("Bank.Data.Entities.BankAccount", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Bank.Data.Entities.BankAccount", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Bank.Data.Entities.Person", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
