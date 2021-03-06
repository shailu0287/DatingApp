﻿using DatingApp.Data.Helper;
using DatingApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data.Repository
{
   public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhoto(int userId);
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userparams);
        Task<User> GetUser(int id);

        Task<Likes> GetLike(int userId, int recipientId);
        Task<Messages> GetMessage(int id);
        Task<PagedList<Messages>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Messages>> GetMessageThread(int userId, int recipientId);
    }
}
