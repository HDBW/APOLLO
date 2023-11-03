using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.MongoDal.Entitties
{
    public class MongoDalConfig
    {
        public string MongoConnStr { get; set; }

        public string MongoDatabase { get; set; }


        public void Validate()
        {
            // Create a validation context for the current object.
            var context = new ValidationContext(this, serviceProvider: null, items: null);

            // Initialize an empty list to store validation results.
            var results = new List<ValidationResult>();

            // Attempt to validate the current object against specified validation rules.
            if (!Validator.TryValidateObject(this, context, results, validateAllProperties: true))
            {
                // If validation fails and there are errors, extract error messages.
                var validationErrors = results.Select(r => r.ErrorMessage);

                // Throw an exception with a message indicating the configuration is invalid,
                // and include the validation error messages in the exception message.
                throw new InvalidOperationException($"Configuration is invalid: {string.Join(", ", validationErrors)}");
            }
        }

    }
}
