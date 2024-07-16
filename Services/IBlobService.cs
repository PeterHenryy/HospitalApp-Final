using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HospitalApp.Services
{
    public interface IBlobService
    {
        string GetBlob(string name);
        Task<List<string>> GetAllBlobs();
        Task<List<Blob>> GetAllBlobsWithUri();
        Task<bool> UploadBlob(string name, IFormFile file, Blob blob);
        Task<bool> DeleteBlob(string name);
    }
}
