using System;
using System.Collections.Generic;

namespace DatingAppAPI.Models
{
    public partial class User
    {
        public User()
        {
            LikesLikee = new HashSet<Likes>();
            LikesLiker = new HashSet<Likes>();
            MessagesRecipient = new HashSet<Messages>();
            MessagesSender = new HashSet<Messages>();
            Photo = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Likes> LikesLikee { get; set; }
        public virtual ICollection<Likes> LikesLiker { get; set; }
        public virtual ICollection<Messages> MessagesRecipient { get; set; }
        public virtual ICollection<Messages> MessagesSender { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
