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

        private readonly int httpget_parameter_count = 4;

        public LogService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LoggerDb"));

            database = client.GetDatabase("LoggerDb");
            _logs = database.GetCollection<LogModel>("Logs");
        }

        public List<LogModel> Get()
        {
            return _logs.Find(log => true).ToList();
        }

        public bool Ping()
        {
            return database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
        }

        public LogModel GetById(string id)
        {
            return _logs.Find(log => log.Id == id).FirstOrDefault();
        }

        public List<LogModel> GetByParameters(string parameters)
        {
            var params_parts = parameters.Split('$');
            if (params_parts.Count() != httpget_parameter_count)
                return null;

            List<LogModel> result = _logs.Find(log => true).ToList();
            var type = params_parts[0];
            var user_name = params_parts[1];
            var sender = params_parts[2];
            var time_type = params_parts[3];

            if (type != "")
                result = result.Where(log => log.Type == type).ToList();
            if (user_name != "")
                result = result.Where(log => log.UserName == user_name).ToList();
            if (sender != "")
                result = result.Where(log => log.SenderApp == sender).ToList();

            if (time_type != "")
            {
                if (time_type == "hour")
                {
                    result = result.Where(log =>
                    {
                        var date_parts = log.Date.Split(' ');
                        var left_parts = date_parts[0].Split('-');
                        var right_parts = date_parts[1].Split(':');

                        int year = int.Parse(left_parts[0]);
                        int month = int.Parse(left_parts[1]);
                        int day = int.Parse(left_parts[2]);
                        int hour = int.Parse(right_parts[0]);
                        int min = int.Parse(right_parts[1]);
                        int sec = int.Parse(right_parts[2]);

                        DateTime actual = DateTime.Now;
                        DateTime border = actual.AddHours(-1);

                        DateTime current_log = new DateTime(year, month, day, hour, min, sec);

                        if (current_log >= border)
                            return true;
                        else
                            return false;
                    }     
                    ).ToList();
                }
                else if (time_type == "day")
                {
                    result = result.Where(log =>
                    {
                        var date_parts = log.Date.Split(' ');
                        var left_parts = date_parts[0].Split('-');
                        var right_parts = date_parts[1].Split(':');

                        int year = int.Parse(left_parts[0]);
                        int month = int.Parse(left_parts[1]);
                        int day = int.Parse(left_parts[2]);
                        int hour = int.Parse(right_parts[0]);
                        int min = int.Parse(right_parts[1]);
                        int sec = int.Parse(right_parts[2]);

                        DateTime actual = DateTime.Now;
                        DateTime border = actual.AddDays(-1);

                        DateTime current_log = new DateTime(year, month, day, hour, min, sec);

                        if (current_log >= border)
                            return true;
                        else
                            return false;
                    }
                    ).ToList();
                }
                else if (time_type == "month")
                {
                    result = result.Where(log =>
                    {
                        var date_parts = log.Date.Split(' ');
                        var left_parts = date_parts[0].Split('-');
                        var right_parts = date_parts[1].Split(':');

                        int year = int.Parse(left_parts[0]);
                        int month = int.Parse(left_parts[1]);
                        int day = int.Parse(left_parts[2]);
                        int hour = int.Parse(right_parts[0]);
                        int min = int.Parse(right_parts[1]);
                        int sec = int.Parse(right_parts[2]);

                        DateTime actual = DateTime.Now;
                        DateTime border = actual.AddMonths(-1);

                        DateTime current_log = new DateTime(year, month, day, hour, min, sec);

                        if (current_log >= border)
                            return true;
                        else
                            return false;
                    }
                    ).ToList();
                }
                else if (time_type == "year")
                {
                    result = result.Where(log =>
                    {
                        var date_parts = log.Date.Split(' ');
                        var left_parts = date_parts[0].Split('-');
                        var right_parts = date_parts[1].Split(':');

                        int year = int.Parse(left_parts[0]);
                        int month = int.Parse(left_parts[1]);
                        int day = int.Parse(left_parts[2]);
                        int hour = int.Parse(right_parts[0]);
                        int min = int.Parse(right_parts[1]);
                        int sec = int.Parse(right_parts[2]);

                        DateTime actual = DateTime.Now;
                        DateTime border = actual.AddYears(-1);

                        DateTime current_log = new DateTime(year, month, day, hour, min, sec);

                        if (current_log >= border)
                            return true;
                        else
                            return false;
                    }
                    ).ToList();
                }
            }

            return result;
        }

        public LogModel Create(LogModel log)
        {
            _logs.InsertOne(log);

            return log;
        }

        public void Remove(string id)
        {
            _logs.DeleteOne(log => log.Id == id);
        }
    }
}

