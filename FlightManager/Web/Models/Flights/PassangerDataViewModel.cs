using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models.Flights
{
    public class PassangerDataViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "UCN must be 10 digits long")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter a valid UCN")]
        public string UCN { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter a valid phone nubmer")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Display(Name = "Ticket type")]
        public TicketTypeEnum TicketType { get; set; }
    }
}
