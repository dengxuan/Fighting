using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Dapper;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Storaging;
using Pomelo.Data.MySql;
using System;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class SportsMatchApplicationService : ApplicationService, ISportsMatchApplicationService
    {
        private readonly StorageOptions _storageOptions;
        public SportsMatchApplicationService(ICacheManager cacheManager, StorageOptions storageOptions) : base(cacheManager)
        {
            _storageOptions = storageOptions;
        }

        public async Task CreateMatchAsync(LotterySportsMatch match)
        {
            using (MySqlConnection connection = new MySqlConnection(_storageOptions.DefaultNameOrConnectionString))
            {
                string sql = @"INSERT INTO `bbcpzcevents`(`Id`,`MatchId`,`Date`,`Week`,`PlayId`,`League`,`Color`,`HostTeam`,`VisitTeam`,`StartTime`,`EndTime`,`SaleStatus`,`Score`,`HalfScore`,`SpfIsSupSinglePass`,`RqspfIsSupSinglePass`,
`ScoreIsSupSinglePass`,`TotalGoalsIsSupSinglePass`,`HalfScoreIsSupSinglePass`,`SpfIsSupPass`,`RqSpfIsSupPass`,`ScoreIsSupPass`,`TotalGoalsIsSupPass`,`HalfScoreIsSupPass`,
`RqspfRateCount`,`SpfOdds3`,`SpfOdds1`,`SpfOdds0`,`RqspfOdds3`,`RqspfOdds1`,`RqspfOdds0`,`ScoreOdds10`,`ScoreOdds20`,`ScoreOdds21`,`ScoreOdds30`,`ScoreOdds31`,`ScoreOdds32`,
`ScoreOdds40`,`ScoreOdds41`,`ScoreOdds42`,`ScoreOdds50`,`ScoreOdds51`,`ScoreOdds52`,`ScoreOdds90`,`ScoreOdds00`,`ScoreOdds11`,`ScoreOdds22`,`ScoreOdds33`,`ScoreOdds99`,`ScoreOdds01`,
`ScoreOdds02`,`ScoreOdds03`,`ScoreOdds12`,`ScoreOdds13`,`ScoreOdds23`,`ScoreOdds04`,`ScoreOdds14`,`ScoreOdds24`,`ScoreOdds05`,`ScoreOdds15`,`ScoreOdds25`,`ScoreOdds09`,`TotalGoalsOdds0`,
`TotalGoalsOdds1`,`TotalGoalsOdds2`,`TotalGoalsOdds3`,`TotalGoalsOdds4`,`TotalGoalsOdds5`,`TotalGoalsOdds6`,`TotalGoalsOdds7`,`HalfScore33`,`HalfScore31`,`HalfScore30`,
`HalfScore13`,`HalfScore11`,`HalfScore10`,`HalfScore03`,`HalfScore01`,`HalfScore00`,`CreationTime`)VALUES(@Id,@MatchId,@Date,@Week,@PlayId,@League,@Color,@HostTeam,
@VisitTeam,@StartTime,@EndTime,@SaleStatus,@Score,@HalfScore,@SpfIsSupSinglePass,@RqspfIsSupSinglePass,@ScoreIsSupSinglePass,@TotalGoalsIsSupSinglePass,@HalfScoreIsSupSinglePass,
@SpfIsSupPass,@RqSpfIsSupPass,@ScoreIsSupPass,@TotalGoalsIsSupPass,@HalfScoreIsSupPass,@RqspfRateCount,@SpfOdds3,@SpfOdds1,@SpfOdds0,@RqspfOdds3,@RqspfOdds1,@RqspfOdds0,@ScoreOdds10,@ScoreOdds20,@ScoreOdds21,@ScoreOdds30,
@ScoreOdds31,@ScoreOdds32,@ScoreOdds40,@ScoreOdds41,@ScoreOdds42,@ScoreOdds50,@ScoreOdds51,@ScoreOdds52,@ScoreOdds90,@ScoreOdds00,@ScoreOdds11,@ScoreOdds22,@ScoreOdds33,@ScoreOdds99,
@ScoreOdds01,@ScoreOdds02,@ScoreOdds03,@ScoreOdds12,@ScoreOdds13,@ScoreOdds23,@ScoreOdds04,@ScoreOdds14,@ScoreOdds24,@ScoreOdds05,@ScoreOdds15,@ScoreOdds25,@ScoreOdds09,@TotalGoalsOdds0,@TotalGoalsOdds1,
@TotalGoalsOdds2,@TotalGoalsOdds3,@TotalGoalsOdds4,@TotalGoalsOdds5,@TotalGoalsOdds6,@TotalGoalsOdds7,@HalfScore33,@HalfScore31,@HalfScore30,@HalfScore13,@HalfScore11,@HalfScore10,
@HalfScore03,@HalfScore01,@HalfScore00,@CreationTime);
                ";
                await connection.ExecuteAsync(sql, match);
            }
        }

        public async Task<LotterySportsMatch> FindMatchAsync(int matchId)
        {
            using (MySqlConnection connection = new MySqlConnection(_storageOptions.DefaultNameOrConnectionString))
            {
                string sql = "SELECT * FROM `bbcpzcevents` WHERE `Id` = @Id;";
                LotterySportsMatch matchEntity = await connection.QuerySingleAsync<LotterySportsMatch>(sql, new { Id = matchId });
                return matchEntity;
            }
        }

        public Task UpdateMatchAsync(LotterySportsMatch match)
        {
            throw new NotImplementedException();
        }
    }
}
