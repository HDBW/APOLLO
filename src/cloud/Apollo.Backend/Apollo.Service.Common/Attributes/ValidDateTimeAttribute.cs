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
        public class ValidDateTimeAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime)
                {
                return true;
                }

                return false; // Not a DateTime
            }
        }
}
