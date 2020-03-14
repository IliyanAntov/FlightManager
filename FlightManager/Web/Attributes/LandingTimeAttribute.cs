using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class LandingTimeAttribute : ValidationAttribute
    {
        private string _departureTime;

        public LandingTimeAttribute(string departureTime)
        {
            _departureTime = departureTime;
            

        }

        private static readonly string notBeforeDeparture = "Landing must be after departure";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime landingTime = (DateTime)value;

            DateTime departureTime = (DateTime)validationContext.ObjectType.GetProperty(_departureTime).GetValue(validationContext.ObjectInstance, null);


            if (landingTime <= departureTime)
            {
                return new ValidationResult(notBeforeDeparture);
            }

            return ValidationResult.Success;
        }


    }
}
