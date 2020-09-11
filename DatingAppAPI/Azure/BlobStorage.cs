using Azure.Storage.Blobs;
using DatingApp.Data;
using DatingApp.Data.DTO;
using DatingApp.Data.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Azure
{
    public class BlobStorage: IBlobStorage
    {
        private readonly IOptions<MyConfig> _config;
        public BlobStorage(IOptions<MyConfig> config)
        {
            _config =config ;
        }
        public async Task<string> UploadImage(PhotoForCreationDto photoForCreationDto, string fileName)
        {
            try
            {

                BlobServiceClient blobServiceClient = new BlobServiceClient(_config.Value.StorageConnection);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_config.Value.Container);

                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                using Stream uploadFileStream = photoForCreationDto.File.OpenReadStream();

                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> DeleteImage(string PublicId)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_config.Value.StorageConnection);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_config.Value.Container);


            BlobClient blobClient = containerClient.GetBlobClient(PublicId);
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            return true;

        }


    }
}
