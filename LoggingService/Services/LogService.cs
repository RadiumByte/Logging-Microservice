﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggingService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace LoggingService.Services
{
    public class LogService
    {
        private readonly IMongoCollection<LogModel> _logs;
        private IMongoDatabase database;

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
            return _logs.Find(log => true).ToList();
        }

        public bool Ping()
        {
            return database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
        }

        public LogModel Get(string id)
        {
            return _logs.Find<LogModel>(log => log.Id == id).FirstOrDefault();
        }

        public LogModel Create(LogModel log)
        {
            _logs.InsertOne(log);
            return log;
        }

        public void Update(string id, LogModel bookIn)
        {
            _logs.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void Remove(LogModel logIn)
        {
            _logs.DeleteOne(log => log.Id == logIn.Id);
        }

        public void Remove(string id)
        {
            _logs.DeleteOne(log => log.Id == id);
        }
    }
}

