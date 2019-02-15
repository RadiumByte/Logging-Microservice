using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Diagnostics;

namespace LoggingService.Services
{
    public class LogService
    {
        private readonly IMongoCollection<LogModel> _logs;
        private IMongoDatabase database;
        private long request_count = 0;
        private long average_time = 0;
        

        public LogService(IConfiguration config)
        {
            // Don't uncomment this
            //var url = "mongodb://localhost:27016";

            var url = new MongoUrl("mongodb://localhost:27017");
            var client = new MongoClient(url);

            database = client.GetDatabase("LoggerDb");
            _logs = database.GetCollection<LogModel>("Logs");
        }

        public List<LogModel> Get()
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            var result = _logs.Find(log => true).ToList();

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;

            return result;
        }

        public long GetRequestAverageTime()
        {
            return average_time;
        }

        public bool Ping()
        {
            return database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
        }

        public LogModel Get(string id)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            var result = _logs.Find<LogModel>(log => log.Id == id).FirstOrDefault();

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;

            return result;
        }

        public LogModel Create(LogModel log)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            _logs.InsertOne(log);

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;

            return log;
        }

        public void Update(string id, LogModel logIn)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            _logs.ReplaceOne(log => log.Id == id, logIn);

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;
        }

        public void Remove(LogModel logIn)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            _logs.DeleteOne(log => log.Id == logIn.Id);

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;
        }

        public void Remove(string id)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            _logs.DeleteOne(log => log.Id == id);

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;
        }
    }
}

