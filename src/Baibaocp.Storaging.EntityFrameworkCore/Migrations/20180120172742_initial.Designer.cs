﻿// <auto-generated />
using Baibaocp.Storaging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Baibaocp.Storaging.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(BaibaocpStorageContext))]
    [Migration("20180120172742_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Baibaocp.Storaging.Entities.City", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("ProvinceId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("BbcpCities");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.CredentialsType", b =>
                {
                    b.Property<short>("Id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("BbcpCredentialsTypes");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.Lottery", b =>
                {
                    b.Property<int>("Id");

                    b.Property<short?>("BbcpLotteryId");

                    b.Property<short>("LotteryCategoryId");

                    b.Property<int>("Prefix");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("BbcpLotteryId");

                    b.HasIndex("LotteryCategoryId");

                    b.ToTable("BbcpLotteries");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryCategory", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("LotteryTypeId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("LotteryTypeId");

                    b.ToTable("BbcpLotteryCategories");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPhase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AwardPoolAmount");

                    b.Property<string>("DrawNumber");

                    b.Property<DateTime?>("DrawTime");

                    b.Property<DateTime?>("EndOrderTime");

                    b.Property<DateTime>("EndSaleTime");

                    b.Property<DateTime>("EndTime");

                    b.Property<bool>("IsAsync");

                    b.Property<string>("IssueExtdatas");

                    b.Property<int>("IssueNumber");

                    b.Property<int>("LotteryId");

                    b.Property<string>("Source");

                    b.Property<DateTime>("StartSaleTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<int>("Status");

                    b.Property<long?>("TotalAwardAmount");

                    b.Property<long?>("TotalSaleAmount");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId");

                    b.ToTable("BbcpPhases");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPhaseBonus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BonusAmount");

                    b.Property<int>("BonusLevel");

                    b.Property<string>("BonusName");

                    b.Property<int?>("IssueId");

                    b.Property<int>("LotteryPhaseId");

                    b.Property<int>("TotalWinnerCount");

                    b.Property<int>("WinnerCount");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.HasIndex("LotteryPhaseId");

                    b.ToTable("BbcpPhaseBonuses");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPlay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("BbcpLotteryPlays");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPlayMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LotteryId");

                    b.Property<int?>("LotteryPlayId");

                    b.Property<int>("PlayId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId");

                    b.HasIndex("LotteryPlayId");

                    b.HasIndex("PlayId");

                    b.ToTable("BbcpLotteryPlayMappings");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotterySportsMatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color")
                        .IsRequired();

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Date")
                        .IsRequired();

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("HalfScore");

                    b.Property<decimal?>("HalfScore00");

                    b.Property<decimal?>("HalfScore01");

                    b.Property<decimal?>("HalfScore03");

                    b.Property<decimal?>("HalfScore10");

                    b.Property<decimal?>("HalfScore11");

                    b.Property<decimal?>("HalfScore13");

                    b.Property<decimal?>("HalfScore30");

                    b.Property<decimal?>("HalfScore31");

                    b.Property<decimal?>("HalfScore33");

                    b.Property<bool>("HalfScoreIsSupSinglePass");

                    b.Property<string>("HostTeam")
                        .IsRequired();

                    b.Property<string>("League")
                        .IsRequired();

                    b.Property<int>("LotteryId");

                    b.Property<string>("PlayId")
                        .IsRequired();

                    b.Property<bool>("RqspfIsSupSinglePass");

                    b.Property<decimal?>("RqspfOdds0");

                    b.Property<decimal?>("RqspfOdds1");

                    b.Property<decimal?>("RqspfOdds3");

                    b.Property<int>("RqspfRateCount");

                    b.Property<int>("SaleStatus");

                    b.Property<string>("Score");

                    b.Property<bool>("ScoreIsSupSinglePass");

                    b.Property<decimal?>("ScoreOdds00");

                    b.Property<decimal?>("ScoreOdds01");

                    b.Property<decimal?>("ScoreOdds02");

                    b.Property<decimal?>("ScoreOdds03");

                    b.Property<decimal?>("ScoreOdds04");

                    b.Property<decimal?>("ScoreOdds05");

                    b.Property<decimal?>("ScoreOdds09");

                    b.Property<decimal?>("ScoreOdds10");

                    b.Property<decimal?>("ScoreOdds11");

                    b.Property<decimal?>("ScoreOdds12");

                    b.Property<decimal?>("ScoreOdds13");

                    b.Property<decimal?>("ScoreOdds14");

                    b.Property<decimal?>("ScoreOdds15");

                    b.Property<decimal?>("ScoreOdds20");

                    b.Property<decimal?>("ScoreOdds21");

                    b.Property<decimal?>("ScoreOdds22");

                    b.Property<decimal?>("ScoreOdds23");

                    b.Property<decimal?>("ScoreOdds24");

                    b.Property<decimal?>("ScoreOdds25");

                    b.Property<decimal?>("ScoreOdds30");

                    b.Property<decimal?>("ScoreOdds31");

                    b.Property<decimal?>("ScoreOdds32");

                    b.Property<decimal?>("ScoreOdds33");

                    b.Property<decimal?>("ScoreOdds40");

                    b.Property<decimal?>("ScoreOdds41");

                    b.Property<decimal?>("ScoreOdds42");

                    b.Property<decimal?>("ScoreOdds50");

                    b.Property<decimal?>("ScoreOdds51");

                    b.Property<decimal?>("ScoreOdds52");

                    b.Property<decimal?>("ScoreOdds90");

                    b.Property<decimal?>("ScoreOdds99");

                    b.Property<bool>("SpfIsSupSinglePass");

                    b.Property<decimal?>("SpfOdds0");

                    b.Property<decimal?>("SpfOdds1");

                    b.Property<decimal?>("SpfOdds3");

                    b.Property<DateTime>("StartTime");

                    b.Property<bool>("TotalGoalsIsSupSinglePass");

                    b.Property<decimal?>("TotalGoalsOdds0");

                    b.Property<decimal?>("TotalGoalsOdds1");

                    b.Property<decimal?>("TotalGoalsOdds2");

                    b.Property<decimal?>("TotalGoalsOdds3");

                    b.Property<decimal?>("TotalGoalsOdds4");

                    b.Property<decimal?>("TotalGoalsOdds5");

                    b.Property<decimal?>("TotalGoalsOdds6");

                    b.Property<decimal?>("TotalGoalsOdds7");

                    b.Property<string>("VisitTeam")
                        .IsRequired();

                    b.Property<int>("Week");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId");

                    b.ToTable("BbcpLotterySportsMatches");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryType", b =>
                {
                    b.Property<short>("Id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.ToTable("BbcpLotteryTypes");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Merchants.Merchanter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20);

                    b.Property<short>("MerchanterTypeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<decimal>("OutTicketMoney");

                    b.Property<decimal>("RestPreMoney");

                    b.Property<decimal>("RewardMoney");

                    b.Property<string>("SecretKey")
                        .IsRequired()
                        .HasMaxLength(24);

                    b.HasKey("Id");

                    b.HasIndex("MerchanterTypeId");

                    b.ToTable("BbcpMerchants");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Merchants.MerchanterLotteryMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("CommissionRate");

                    b.Property<int>("LotteryId");

                    b.Property<int>("MerchanterId");

                    b.Property<string>("NoticeAddress");

                    b.HasKey("Id");

                    b.HasIndex("LotteryId");

                    b.HasIndex("MerchanterId");

                    b.ToTable("BbcpLotteryMerchanterMappings");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Merchants.MerchanterType", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("BbcpMerchanterTypes");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Province", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("BbcpProvinces");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Town", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("CityId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("BbcpTowns");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<int?>("CityId");

                    b.Property<string>("CredentialsNumber")
                        .HasMaxLength(18);

                    b.Property<short>("CredentialsTypeId");

                    b.Property<string>("Email");

                    b.Property<string>("Nickname")
                        .HasMaxLength(10);

                    b.Property<string>("Password")
                        .HasMaxLength(128);

                    b.Property<string>("Phone");

                    b.Property<int>("ProvinceId");

                    b.Property<string>("Realname")
                        .HasMaxLength(10);

                    b.Property<int?>("TownId");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CredentialsTypeId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("TownId");

                    b.ToTable("BbcpUserLotteryBuyers");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<short>("LotteryBuyerAccountTypeId");

                    b.HasKey("Id");

                    b.HasIndex("LotteryBuyerAccountTypeId");

                    b.ToTable("BbcpUserLotteryBuyerAccounts");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerAccountType", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("BbcpUserLotteryBuyerAccountTypes");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BonusAmount");

                    b.Property<int>("InvestAmount");

                    b.Property<string>("InvestCode");

                    b.Property<int>("InvestCount");

                    b.Property<int>("InvestTimes");

                    b.Property<int>("InvestType");

                    b.Property<int?>("IsBonusNotify");

                    b.Property<int?>("IsTicketNotify");

                    b.Property<int?>("IssueNumber")
                        .IsRequired();

                    b.Property<long>("LotteryBuyerId");

                    b.Property<int>("LotteryId");

                    b.Property<int>("LotteryPlayId");

                    b.Property<int>("LotteryResellerId");

                    b.Property<int>("LotterySupplierId");

                    b.Property<long>("ResellerUserId");

                    b.Property<int>("Status");

                    b.Property<string>("TicketOdds");

                    b.HasKey("Id");

                    b.HasIndex("LotteryBuyerId");

                    b.HasIndex("LotteryId");

                    b.HasIndex("LotteryPlayId");

                    b.HasIndex("LotteryResellerId");

                    b.HasIndex("LotterySupplierId");

                    b.ToTable("BbcpUserLotteryBuyerOrders");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.City", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.Lottery", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryCategory")
                        .WithMany("Lotteries")
                        .HasForeignKey("BbcpLotteryId");

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryCategory", "LotteryCategory")
                        .WithMany()
                        .HasForeignKey("LotteryCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryCategory", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryType", "LotteryType")
                        .WithMany("LotteryCategories")
                        .HasForeignKey("LotteryTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPhase", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.Lottery", "Lottery")
                        .WithMany()
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPhaseBonus", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryPhase")
                        .WithMany("LotteryIssueBonuses")
                        .HasForeignKey("IssueId");

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryPhase", "LotteryPhase")
                        .WithMany()
                        .HasForeignKey("LotteryPhaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotteryPlayMapping", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.Lottery", "Lottery")
                        .WithMany("LotteryPlayMappings")
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryPlay")
                        .WithMany("LotteryPlayMappings")
                        .HasForeignKey("LotteryPlayId");

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryPlay", "LotteryPlay")
                        .WithMany()
                        .HasForeignKey("PlayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Lotteries.LotterySportsMatch", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.Lottery", "Lottery")
                        .WithMany()
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Merchants.Merchanter", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Merchants.MerchanterType", "MerchanterType")
                        .WithMany()
                        .HasForeignKey("MerchanterTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Merchants.MerchanterLotteryMapping", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.Lottery", "Lottery")
                        .WithMany()
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Merchants.Merchanter", "Merchanter")
                        .WithMany()
                        .HasForeignKey("MerchanterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Town", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.City", "City")
                        .WithMany("Towns")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyer", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("Baibaocp.Storaging.Entities.CredentialsType", "CredentialsType")
                        .WithMany()
                        .HasForeignKey("CredentialsTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Town", "Town")
                        .WithMany()
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerAccount", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerAccountType", "LotteryBuyerAccountType")
                        .WithMany()
                        .HasForeignKey("LotteryBuyerAccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baibaocp.Storaging.Entities.Users.UserLotteryBuyerOrder", b =>
                {
                    b.HasOne("Baibaocp.Storaging.Entities.Users.UserLotteryBuyer", "BbcpUserLotteryBuyer")
                        .WithMany()
                        .HasForeignKey("LotteryBuyerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.Lottery", "Lottery")
                        .WithMany()
                        .HasForeignKey("LotteryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Lotteries.LotteryPlay", "LotteryPlay")
                        .WithMany()
                        .HasForeignKey("LotteryPlayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Merchants.Merchanter", "LotteryReseller")
                        .WithMany()
                        .HasForeignKey("LotteryResellerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baibaocp.Storaging.Entities.Merchants.Merchanter", "LotterySupplier")
                        .WithMany()
                        .HasForeignKey("LotterySupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}