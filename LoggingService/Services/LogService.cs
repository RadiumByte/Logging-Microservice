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

        private readonly int httpget_parameter_count = 3;

        private long request_count = 0;
        private long average_time = 0;
        

        public LogService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LoggerDb"));

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

        public LogModel GetById(string id)
        {
            request_count++;
            Stopwatch timer = Stopwatch.StartNew();

            var result = _logs.Find(log => log.Id == id).FirstOrDefault();

            timer.Stop();
            average_time = timer.ElapsedMilliseconds / request_count;

            return result;
        }

        public List<LogModel> GetByParameters(string parameters)
        {
            var params_parts = parameters.Split('&');
            if (params_parts.Count() != httpget_parameter_count)
                return null;

            List<LogModel> result = _logs.Find(log => true).ToList();
            var type = params_parts[0];
            var user_name = params_parts[1];
            var sender = params_parts[2];

            // type check
            if (type != "")
                result = result.Where(log => log.Type == type).ToList();
            if (user_name != "")
                result = result.Where(log => log.UserName == user_name).ToList();
            if (sender != "")
                result = result.Where(log => log.SenderApp == sender).ToList();

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

