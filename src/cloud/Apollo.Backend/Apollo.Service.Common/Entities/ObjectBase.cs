// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// The base type for all types that are used as collections in the database.
    /// For example: User, Training, Profile.
    /// </summary>
    public class ObjectBase
    {
    /// <summary>
    /// This is the Unique Identifier set by Apollo for the object.
    /// </summary>
    public string? Id { get; set; }

    }
}
