﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.ViewModels
{
    public class ListPostsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }


        public string Username { get; set; }

        public string UserProfilePicture { get; set; }

        public bool isLiked { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public int Likes { get; set; }

     
    }
}
