using Microsoft.AspNetCore.Cors;
using MyFriends.Models;
using System.ComponentModel.DataAnnotations;

namespace MyFriends.ViewsModels
{
    public class VMFriendWithImage
    {
        public VMFriendWithImage() { Friend = new Friend(); }
        public Friend Friend { get; set; }

        [Display(Name ="הוספת תמונה")]
        public IFormFile File { get; set; } 
    }
}
