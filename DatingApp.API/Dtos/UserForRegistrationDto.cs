using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegistrationDto
    {
         [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4 , ErrorMessage="You must specify password legnth between 4 and 8 character")]
        public string Password { get; set; }
    }
}