﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data.DTO
{
    public class PhotosForDetailedDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}
