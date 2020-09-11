using DatingApp.Data;
using DatingApp.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Azure
{
    public interface IBlobStorage
    {
        Task<string> UploadImage(PhotoForCreationDto photoForCreationDto, string fileName);
        Task<bool> DeleteImage(string PublicId);
    }
}
