using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HospitalApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("hospital-images");
        }
        public async Task<bool> DeleteBlob(string name)
        {
            var blobClient = _blobContainerClient.GetBlobClient(name);

            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs()
        {
            var blobs = _blobContainerClient.GetBlobsAsync();

            var blobString = new List<string>();

            await foreach (var item in blobs)
            {
                blobString.Add(item.Name);
            }

            return blobString;

        }

        public async Task<List<Blob>> GetAllBlobsWithUri()
        {
            throw new System.NotImplementedException();
        }

        public string GetBlob(string name)
        {
            BlobClient blob = _blobContainerClient.GetBlobClient(name);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, Blob blob)
        {
            
            var blobClient = _blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

            return result != null;

        }
    }
}
