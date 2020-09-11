using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using DatingApp.Data.Models;
using DatingApp.Data;
using DatingApp.Data.DTO;

namespace DatingApp.Data.Helper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDTO>()
                    .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo.FirstOrDefault(x => x.IsMain).Url))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailedDTO>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForEditProfileDTO, User>();
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<MessageForCreationDto, Messages>().ReverseMap();
            CreateMap<MessageForCreationDto, QueueMessages>().ReverseMap();
            CreateMap<QueueMessages, Messages>().ReverseMap();

            CreateMap<Messages, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photo.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photo.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
