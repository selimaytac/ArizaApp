using System;

namespace ArizaApp.Models.Entities
{
    public class LogRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
    }
}