using System;
using System.Collections.Generic;

namespace DatingAppAPI.Models
{
    public partial class Likes
    {
        public int LikerId { get; set; }
        public int LikeeId { get; set; }

        public virtual User Likee { get; set; }
        public virtual User Liker { get; set; }
    }
}
