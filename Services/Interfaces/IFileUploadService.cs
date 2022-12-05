using System.Collections.Generic;
using System.Threading.Tasks;
using ArizaApp.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace ArizaApp.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<List<UploadedFileRecords>> UploadFileAsync(IList<IFormFile> files, int notificationId, AppUser user);
    }
}