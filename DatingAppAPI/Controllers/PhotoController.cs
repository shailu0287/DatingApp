using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Azure.Storage.Blobs;
using DatingAppAPI.Azure;
using DatingApp.Data;
using DatingApp.Data.Models;
using DatingApp.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DatingApp.Data.DTO;

namespace DatingAppAPI.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private  IOptions<MyConfig> config;
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBlobStorage _blob;
       // BlobStorage blob = new BlobStorage(this.con);
        public PhotoController(IOptions<MyConfig> config, IDatingRepository repo, IMapper mapper, IBlobStorage blob)
        {
            this.config = config;
            _mapper = mapper;
            _repo = repo;
            _blob = blob;
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
           
            var userFromRepo = await _repo.GetUser(userId);
            Guid obj = Guid.NewGuid();

            var fileName = photoForCreationDto.File.FileName + obj.ToString();
            
            var blobInfo = await _blob.UploadImage(photoForCreationDto,fileName);
            
            photoForCreationDto.Url = blobInfo.ToString();
            photoForCreationDto.PublicId = fileName;
            
            var photo = _mapper.Map<Photo>(photoForCreationDto);
            
            if (!userFromRepo.Photo.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photo.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return Ok(photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }
        

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photo.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");


            if (photoFromRepo.PublicId != null)
            {
                if(await _blob.DeleteImage(photoFromRepo.PublicId))
                _repo.Delete(photoFromRepo);
            }
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");


        }
        [HttpPost("{id}/setMain")]  
        public async Task<IActionResult> SetMainPhoto(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                throw new Exception("Wrong user");

            var user = await _repo.GetUser(userId);

            if (!user.Photo.Any(p => p.Id == id))
                throw new Exception("Check your photo");

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("Could not set photo to main");

        }
      
    }
}