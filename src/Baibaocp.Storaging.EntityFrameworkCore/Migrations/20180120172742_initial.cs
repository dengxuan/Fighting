using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Baibaocp.Storaging.EntityFrameworkCore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BbcpCredentialsTypes",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false),
                    Text = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpCredentialsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteryPlays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteryPlays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteryTypes",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false),
                    Text = table.Column<string>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpMerchanterTypes",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpMerchanterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpProvinces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpProvinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpUserLotteryBuyerAccountTypes",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpUserLotteryBuyerAccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteryCategories",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LotteryTypeId = table.Column<short>(nullable: false),
                    Text = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteryCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryCategories_BbcpLotteryTypes_LotteryTypeId",
                        column: x => x.LotteryTypeId,
                        principalTable: "BbcpLotteryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpMerchants",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 20, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MerchanterTypeId = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    OutTicketMoney = table.Column<decimal>(nullable: false),
                    RestPreMoney = table.Column<decimal>(nullable: false),
                    RewardMoney = table.Column<decimal>(nullable: false),
                    SecretKey = table.Column<string>(maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpMerchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpMerchants_BbcpMerchanterTypes_MerchanterTypeId",
                        column: x => x.MerchanterTypeId,
                        principalTable: "BbcpMerchanterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpCities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpCities_BbcpProvinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "BbcpProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpUserLotteryBuyerAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    LotteryBuyerAccountTypeId = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpUserLotteryBuyerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerAccounts_BbcpUserLotteryBuyerAccountTypes_LotteryBuyerAccountTypeId",
                        column: x => x.LotteryBuyerAccountTypeId,
                        principalTable: "BbcpUserLotteryBuyerAccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    BbcpLotteryId = table.Column<short>(nullable: true),
                    LotteryCategoryId = table.Column<short>(nullable: false),
                    Prefix = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpLotteries_BbcpLotteryCategories_BbcpLotteryId",
                        column: x => x.BbcpLotteryId,
                        principalTable: "BbcpLotteryCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BbcpLotteries_BbcpLotteryCategories_LotteryCategoryId",
                        column: x => x.LotteryCategoryId,
                        principalTable: "BbcpLotteryCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpTowns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpTowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpTowns_BbcpCities_CityId",
                        column: x => x.CityId,
                        principalTable: "BbcpCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteryMerchanterMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommissionRate = table.Column<decimal>(nullable: false),
                    LotteryId = table.Column<int>(nullable: false),
                    MerchanterId = table.Column<int>(nullable: false),
                    NoticeAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteryMerchanterMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryMerchanterMappings_BbcpLotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "BbcpLotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryMerchanterMappings_BbcpMerchants_MerchanterId",
                        column: x => x.MerchanterId,
                        principalTable: "BbcpMerchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotteryPlayMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LotteryId = table.Column<int>(nullable: false),
                    LotteryPlayId = table.Column<int>(nullable: true),
                    PlayId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotteryPlayMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryPlayMappings_BbcpLotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "BbcpLotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryPlayMappings_BbcpLotteryPlays_LotteryPlayId",
                        column: x => x.LotteryPlayId,
                        principalTable: "BbcpLotteryPlays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BbcpLotteryPlayMappings_BbcpLotteryPlays_PlayId",
                        column: x => x.PlayId,
                        principalTable: "BbcpLotteryPlays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpLotterySportsMatches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Color = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Date = table.Column<string>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    HalfScore = table.Column<string>(nullable: true),
                    HalfScore00 = table.Column<decimal>(nullable: true),
                    HalfScore01 = table.Column<decimal>(nullable: true),
                    HalfScore03 = table.Column<decimal>(nullable: true),
                    HalfScore10 = table.Column<decimal>(nullable: true),
                    HalfScore11 = table.Column<decimal>(nullable: true),
                    HalfScore13 = table.Column<decimal>(nullable: true),
                    HalfScore30 = table.Column<decimal>(nullable: true),
                    HalfScore31 = table.Column<decimal>(nullable: true),
                    HalfScore33 = table.Column<decimal>(nullable: true),
                    HalfScoreIsSupSinglePass = table.Column<bool>(nullable: false),
                    HostTeam = table.Column<string>(nullable: false),
                    League = table.Column<string>(nullable: false),
                    LotteryId = table.Column<int>(nullable: false),
                    PlayId = table.Column<string>(nullable: false),
                    RqspfIsSupSinglePass = table.Column<bool>(nullable: false),
                    RqspfOdds0 = table.Column<decimal>(nullable: true),
                    RqspfOdds1 = table.Column<decimal>(nullable: true),
                    RqspfOdds3 = table.Column<decimal>(nullable: true),
                    RqspfRateCount = table.Column<int>(nullable: false),
                    SaleStatus = table.Column<int>(nullable: false),
                    Score = table.Column<string>(nullable: true),
                    ScoreIsSupSinglePass = table.Column<bool>(nullable: false),
                    ScoreOdds00 = table.Column<decimal>(nullable: true),
                    ScoreOdds01 = table.Column<decimal>(nullable: true),
                    ScoreOdds02 = table.Column<decimal>(nullable: true),
                    ScoreOdds03 = table.Column<decimal>(nullable: true),
                    ScoreOdds04 = table.Column<decimal>(nullable: true),
                    ScoreOdds05 = table.Column<decimal>(nullable: true),
                    ScoreOdds09 = table.Column<decimal>(nullable: true),
                    ScoreOdds10 = table.Column<decimal>(nullable: true),
                    ScoreOdds11 = table.Column<decimal>(nullable: true),
                    ScoreOdds12 = table.Column<decimal>(nullable: true),
                    ScoreOdds13 = table.Column<decimal>(nullable: true),
                    ScoreOdds14 = table.Column<decimal>(nullable: true),
                    ScoreOdds15 = table.Column<decimal>(nullable: true),
                    ScoreOdds20 = table.Column<decimal>(nullable: true),
                    ScoreOdds21 = table.Column<decimal>(nullable: true),
                    ScoreOdds22 = table.Column<decimal>(nullable: true),
                    ScoreOdds23 = table.Column<decimal>(nullable: true),
                    ScoreOdds24 = table.Column<decimal>(nullable: true),
                    ScoreOdds25 = table.Column<decimal>(nullable: true),
                    ScoreOdds30 = table.Column<decimal>(nullable: true),
                    ScoreOdds31 = table.Column<decimal>(nullable: true),
                    ScoreOdds32 = table.Column<decimal>(nullable: true),
                    ScoreOdds33 = table.Column<decimal>(nullable: true),
                    ScoreOdds40 = table.Column<decimal>(nullable: true),
                    ScoreOdds41 = table.Column<decimal>(nullable: true),
                    ScoreOdds42 = table.Column<decimal>(nullable: true),
                    ScoreOdds50 = table.Column<decimal>(nullable: true),
                    ScoreOdds51 = table.Column<decimal>(nullable: true),
                    ScoreOdds52 = table.Column<decimal>(nullable: true),
                    ScoreOdds90 = table.Column<decimal>(nullable: true),
                    ScoreOdds99 = table.Column<decimal>(nullable: true),
                    SpfIsSupSinglePass = table.Column<bool>(nullable: false),
                    SpfOdds0 = table.Column<decimal>(nullable: true),
                    SpfOdds1 = table.Column<decimal>(nullable: true),
                    SpfOdds3 = table.Column<decimal>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TotalGoalsIsSupSinglePass = table.Column<bool>(nullable: false),
                    TotalGoalsOdds0 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds1 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds2 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds3 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds4 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds5 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds6 = table.Column<decimal>(nullable: true),
                    TotalGoalsOdds7 = table.Column<decimal>(nullable: true),
                    VisitTeam = table.Column<string>(nullable: false),
                    Week = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpLotterySportsMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpLotterySportsMatches_BbcpLotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "BbcpLotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpPhases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AwardPoolAmount = table.Column<long>(nullable: true),
                    DrawNumber = table.Column<string>(nullable: true),
                    DrawTime = table.Column<DateTime>(nullable: true),
                    EndOrderTime = table.Column<DateTime>(nullable: true),
                    EndSaleTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    IsAsync = table.Column<bool>(nullable: false),
                    IssueExtdatas = table.Column<string>(nullable: true),
                    IssueNumber = table.Column<int>(nullable: false),
                    LotteryId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    StartSaleTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TotalAwardAmount = table.Column<long>(nullable: true),
                    TotalSaleAmount = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpPhases_BbcpLotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "BbcpLotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpUserLotteryBuyers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: true),
                    CredentialsNumber = table.Column<string>(maxLength: 18, nullable: true),
                    CredentialsTypeId = table.Column<short>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(maxLength: 10, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: false),
                    Realname = table.Column<string>(maxLength: 10, nullable: true),
                    TownId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpUserLotteryBuyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyers_BbcpCities_CityId",
                        column: x => x.CityId,
                        principalTable: "BbcpCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyers_BbcpCredentialsTypes_CredentialsTypeId",
                        column: x => x.CredentialsTypeId,
                        principalTable: "BbcpCredentialsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyers_BbcpProvinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "BbcpProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyers_BbcpTowns_TownId",
                        column: x => x.TownId,
                        principalTable: "BbcpTowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BbcpPhaseBonuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BonusAmount = table.Column<int>(nullable: false),
                    BonusLevel = table.Column<int>(nullable: false),
                    BonusName = table.Column<string>(nullable: true),
                    IssueId = table.Column<int>(nullable: true),
                    LotteryPhaseId = table.Column<int>(nullable: false),
                    TotalWinnerCount = table.Column<int>(nullable: false),
                    WinnerCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpPhaseBonuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpPhaseBonuses_BbcpPhases_IssueId",
                        column: x => x.IssueId,
                        principalTable: "BbcpPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BbcpPhaseBonuses_BbcpPhases_LotteryPhaseId",
                        column: x => x.LotteryPhaseId,
                        principalTable: "BbcpPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BbcpUserLotteryBuyerOrders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BonusAmount = table.Column<int>(nullable: false),
                    InvestAmount = table.Column<int>(nullable: false),
                    InvestCode = table.Column<string>(nullable: true),
                    InvestCount = table.Column<int>(nullable: false),
                    InvestTimes = table.Column<int>(nullable: false),
                    InvestType = table.Column<int>(nullable: false),
                    IsBonusNotify = table.Column<int>(nullable: true),
                    IsTicketNotify = table.Column<int>(nullable: true),
                    IssueNumber = table.Column<int>(nullable: false),
                    LotteryBuyerId = table.Column<long>(nullable: false),
                    LotteryId = table.Column<int>(nullable: false),
                    LotteryPlayId = table.Column<int>(nullable: false),
                    LotteryResellerId = table.Column<int>(nullable: false),
                    LotterySupplierId = table.Column<int>(nullable: false),
                    ResellerUserId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TicketOdds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BbcpUserLotteryBuyerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerOrders_BbcpUserLotteryBuyers_LotteryBuyerId",
                        column: x => x.LotteryBuyerId,
                        principalTable: "BbcpUserLotteryBuyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerOrders_BbcpLotteries_LotteryId",
                        column: x => x.LotteryId,
                        principalTable: "BbcpLotteries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerOrders_BbcpLotteryPlays_LotteryPlayId",
                        column: x => x.LotteryPlayId,
                        principalTable: "BbcpLotteryPlays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerOrders_BbcpMerchants_LotteryResellerId",
                        column: x => x.LotteryResellerId,
                        principalTable: "BbcpMerchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BbcpUserLotteryBuyerOrders_BbcpMerchants_LotterySupplierId",
                        column: x => x.LotterySupplierId,
                        principalTable: "BbcpMerchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BbcpCities_ProvinceId",
                table: "BbcpCities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteries_BbcpLotteryId",
                table: "BbcpLotteries",
                column: "BbcpLotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteries_LotteryCategoryId",
                table: "BbcpLotteries",
                column: "LotteryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryCategories_LotteryTypeId",
                table: "BbcpLotteryCategories",
                column: "LotteryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryMerchanterMappings_LotteryId",
                table: "BbcpLotteryMerchanterMappings",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryMerchanterMappings_MerchanterId",
                table: "BbcpLotteryMerchanterMappings",
                column: "MerchanterId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryPlayMappings_LotteryId",
                table: "BbcpLotteryPlayMappings",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryPlayMappings_LotteryPlayId",
                table: "BbcpLotteryPlayMappings",
                column: "LotteryPlayId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotteryPlayMappings_PlayId",
                table: "BbcpLotteryPlayMappings",
                column: "PlayId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpLotterySportsMatches_LotteryId",
                table: "BbcpLotterySportsMatches",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpMerchants_MerchanterTypeId",
                table: "BbcpMerchants",
                column: "MerchanterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpPhaseBonuses_IssueId",
                table: "BbcpPhaseBonuses",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpPhaseBonuses_LotteryPhaseId",
                table: "BbcpPhaseBonuses",
                column: "LotteryPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpPhases_LotteryId",
                table: "BbcpPhases",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpTowns_CityId",
                table: "BbcpTowns",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerAccounts_LotteryBuyerAccountTypeId",
                table: "BbcpUserLotteryBuyerAccounts",
                column: "LotteryBuyerAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerOrders_LotteryBuyerId",
                table: "BbcpUserLotteryBuyerOrders",
                column: "LotteryBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerOrders_LotteryId",
                table: "BbcpUserLotteryBuyerOrders",
                column: "LotteryId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerOrders_LotteryPlayId",
                table: "BbcpUserLotteryBuyerOrders",
                column: "LotteryPlayId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerOrders_LotteryResellerId",
                table: "BbcpUserLotteryBuyerOrders",
                column: "LotteryResellerId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyerOrders_LotterySupplierId",
                table: "BbcpUserLotteryBuyerOrders",
                column: "LotterySupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyers_CityId",
                table: "BbcpUserLotteryBuyers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyers_CredentialsTypeId",
                table: "BbcpUserLotteryBuyers",
                column: "CredentialsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyers_ProvinceId",
                table: "BbcpUserLotteryBuyers",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_BbcpUserLotteryBuyers_TownId",
                table: "BbcpUserLotteryBuyers",
                column: "TownId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BbcpLotteryMerchanterMappings");

            migrationBuilder.DropTable(
                name: "BbcpLotteryPlayMappings");

            migrationBuilder.DropTable(
                name: "BbcpLotterySportsMatches");

            migrationBuilder.DropTable(
                name: "BbcpPhaseBonuses");

            migrationBuilder.DropTable(
                name: "BbcpUserLotteryBuyerAccounts");

            migrationBuilder.DropTable(
                name: "BbcpUserLotteryBuyerOrders");

            migrationBuilder.DropTable(
                name: "BbcpPhases");

            migrationBuilder.DropTable(
                name: "BbcpUserLotteryBuyerAccountTypes");

            migrationBuilder.DropTable(
                name: "BbcpUserLotteryBuyers");

            migrationBuilder.DropTable(
                name: "BbcpLotteryPlays");

            migrationBuilder.DropTable(
                name: "BbcpMerchants");

            migrationBuilder.DropTable(
                name: "BbcpLotteries");

            migrationBuilder.DropTable(
                name: "BbcpCredentialsTypes");

            migrationBuilder.DropTable(
                name: "BbcpTowns");

            migrationBuilder.DropTable(
                name: "BbcpMerchanterTypes");

            migrationBuilder.DropTable(
                name: "BbcpLotteryCategories");

            migrationBuilder.DropTable(
                name: "BbcpCities");

            migrationBuilder.DropTable(
                name: "BbcpLotteryTypes");

            migrationBuilder.DropTable(
                name: "BbcpProvinces");
        }
    }
}
