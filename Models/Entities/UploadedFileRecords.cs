using System;
using System.Collections.Generic;
using ArizaApp.Models.Dtos;

namespace ArizaApp.Models.Entities
{
    public class UploadedFileRecords
    {
        public int Id { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public DateTime UploadDate { get; set; }
        public int NotificationId { get; set; }
        public ArizaModel Notification { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}