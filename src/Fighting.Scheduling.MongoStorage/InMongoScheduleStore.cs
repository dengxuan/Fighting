using Fighting.Scheduling.Abstractions;
using Fighting.Timing;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fighting.Scheduling.MongoStorage
{
    public class InMongoScheduleStore : IScheduleStore
    {
        private readonly MongoClient _mongoClient;

        private readonly IMongoCollection<Schedule> scheduleCollection;

        public InMongoScheduleStore(SchedulingConfiguration schedulingOptions)
        {
            _mongoClient = new MongoClient(schedulingOptions.DefaultConnection);
            scheduleCollection = _mongoClient.GetDatabase("Baibaocp").GetCollection<Schedule>("Scheduulings");
        }

        public Task DeleteAsync(Schedule schedule)
        {
            return scheduleCollection.DeleteOneAsync(filter => filter.Id == schedule.Id);
        }

        public Task<Schedule> GetAsync(long id)
        {
            return scheduleCollection.Find(filter => filter.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Schedule>> GetWaitingSchedulesAsync(int maxResultCount)
        {
            return scheduleCollection.Find(filter => filter.IsAbandoned == false && filter.NextTryTime < Clock.Now)
                                     .SortByDescending(field => field.Priority)
                                     .ThenBy(field => field.TryCount)
                                     .ThenBy(field => field.NextTryTime)
                                     .Limit(maxResultCount)
                                     .ToListAsync();
        }

        public Task InsertAsync(Schedule schedule)
        {
            return scheduleCollection.InsertOneAsync(schedule);
        }

        public Task UpdateAsync(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
