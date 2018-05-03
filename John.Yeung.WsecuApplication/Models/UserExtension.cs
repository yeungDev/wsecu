using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace John.Yeung.WsecuApplication.Models
{
    public partial class User : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(new ValidationResult("Name is required."));
            if (string.IsNullOrWhiteSpace(Email))
                errors.Add(new ValidationResult("Email is required."));
            if (string.IsNullOrWhiteSpace(UserName))
                errors.Add(new ValidationResult("Username is required."));

            return errors;
        }
    }
}