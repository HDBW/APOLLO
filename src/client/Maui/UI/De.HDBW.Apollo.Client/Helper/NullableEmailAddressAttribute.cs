// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace De.HDBW.Apollo.Client.Helper
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NullableEmailAddressAttribute : DataTypeAttribute
    {
        public NullableEmailAddressAttribute()
            : base(DataType.EmailAddress)
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string valueAsString))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return true;
            }

            var validator = new EmailAddressAttribute() { ErrorMessageResourceType = ErrorMessageResourceType, ErrorMessageResourceName = ErrorMessageResourceName };
            return validator.IsValid(value);
        }
    }
}
