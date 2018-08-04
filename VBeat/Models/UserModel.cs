using System;
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
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime TimeOfLastLogin { get; set; }
        virtual public ICollection<PlaylistModel> SavedPlaylists { get; set; } 
    }
}
