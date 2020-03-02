using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class TicketNumAttribute : ValidationAttribute
    {

        private string _regularSeats;
        private string _businessSeats;

        public TicketNumAttribute(string regularSeats, string businessSeats)
        {
            _regularSeats = regularSeats;
            _businessSeats = businessSeats;

        }

        private static readonly string notEnoughSeats = "Not enough available seats";
        private static readonly string atLeastOneSeat = "Please choose at least one seat";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int ticketNum = (int)value;

            int availableRegularTickets = (int)validationContext.ObjectType.GetProperty(_regularSeats).GetValue(validationContext.ObjectInstance, null);
            int availableBusinessTickets = (int)validationContext.ObjectType.GetProperty(_businessSeats).GetValue(validationContext.ObjectInstance, null);


            if(ticketNum <= 0)
            {
                return new ValidationResult(atLeastOneSeat);
            }
            else if (ticketNum > availableRegularTickets+availableBusinessTickets)
            {
                return new ValidationResult(notEnoughSeats);
            }

            return ValidationResult.Success;
        }
    }
}
