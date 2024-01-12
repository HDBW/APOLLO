// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Api
{
    /// <summary>
    /// Implements various validation methods.
    /// </summary>
    public partial class ApolloApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="throwIfInvalid"></param>
        /// <returns></returns>
        /// <exception cref="ApolloApiException"></exception>
        public  bool IsValidId(string value, bool throwIfInvalid = false)
        {

            if (throwIfInvalid)
                throw new ApolloApiException(ErrorCodes.GeneralErrors.InvalidId, "Invalid Id.");

            return false;
        }
    }
}
