﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfRegistration { get; set; }

        [Display(Name = "Last Login")]
        public DateTime TimeOfLastLogin { get; set; }

        virtual public ICollection<PlaylistModel> SavedPlaylists { get; } = new List<PlaylistModel>();
    }
}
