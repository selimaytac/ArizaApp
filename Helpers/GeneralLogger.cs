using System;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Entities;

namespace ArizaApp.Helpers
{
    public static class GeneralLogger
    {
        public static void AddLog(ArizaDbContext dbContext, LogRecord logRecord)
        {
            dbContext.LogRecords.Add(logRecord);
            dbContext.SaveChanges();
        }
    }
}