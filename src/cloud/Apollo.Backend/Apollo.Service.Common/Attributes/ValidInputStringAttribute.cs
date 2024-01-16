// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Attributes
{
   
        [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
        public class ValidInputStringAttribute : ValidationAttribute
        {

            private readonly string _pattern = "^[a-zA-Z0-9]+$";

            public override bool IsValid(object? value)
            {
                if (value is string stringValue)
                {
                    // Check if the string matches the specified pattern
                    return System.Text.RegularExpressions.Regex.IsMatch(stringValue, _pattern);
                }

                return false; // Return true for non-string values (or you can change this behavior)
            }
    }
    
}
