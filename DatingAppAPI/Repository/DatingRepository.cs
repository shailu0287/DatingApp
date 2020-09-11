using DatingAppAPI.Helpers;
using DatingAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
    public class DatingRepository : IDatingRepository
    {
        DatingAppContext db;
        public DatingRepository(DatingAppContext _db)
        {
            db = _db;
        }
        public void Add<T>(T entity) where T : class
        {
            db.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            db.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await db.User.Include(p => p.Photo).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Photo> GetPhoto(int id)
        {
            return await db.Photo.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Photo> GetMainPhoto(int userId)
        {
            return await db.Photo.FirstOrDefaultAsync(u => u.UserId == userId && u.IsMain==true);
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
            {
           var users = db.User.Include(p => p.Photo).OrderByDescending(u=>u.LastActive).AsQueryable();
           users = users.Where(x => x.Id != userParams.UserId);

           users= users.Where(x=> x.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MinAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                users = users.Where(x => x.DateOfBirth <= minDob && x.DateOfBirth >= maxDob);
            }
            if (!string.IsNullOrEmpty(userParams.Orderby))
            {
                switch (userParams.Orderby) {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }

            }
                return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
         }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await db.User
                    .Include(p=>p.LikesLiker)
                    .Include(l=>l.LikesLikee)
                    .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                var u= user.LikesLiker.Where(u => u.LikeeId == id).Select(i => i.LikerId);
                return u;
            }
            else
            {
                var u= user.LikesLikee.Where(u => u.LikerId == id).Select(i => i.LikeeId);
                return u;
            }
            
        }

        public async Task<bool> SaveAll()
        {
          return await db.SaveChangesAsync()>0;
        }

        public async Task<Likes> GetLike(int userId, int recipientId)
        {
            return await db.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }
        public async Task<Messages> GetMessage(int id)
        {
            return await db.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Messages>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = db.Messages.Include(p => p.Recipient)
                .Include(s => s.Sender)
                .Include(o=>o.Sender.Photo)
                .Include(x=>x.Recipient.Photo)
                .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId
                        && u.SenderDeleted == false);
                    break; 
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false && (u.IsRead == false || u.IsRead==null));
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Messages>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Messages>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await db.Messages.Include(p=>p.Sender.Photo).Include(x=>x.Recipient)
                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                    && m.SenderId == recipientId
                    || m.RecipientId == recipientId && m.SenderId == userId
                    && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }
    }
}
