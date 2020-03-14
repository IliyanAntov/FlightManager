using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models.Users
{
    public class UsersEditViewModel
    {

        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "UCN must be 10 digits long")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter a valid UCN")]
        public string UCN { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter a valid phone nubmer")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

    }
}
