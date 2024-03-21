// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;

namespace Apollo.Api
{
    internal class CsvFormatter
    {
        private string _cDelimiter = "|";  

        public CsvFormatter(string cDelimiter)
        {
            _cDelimiter = cDelimiter;
        }

        public string Format(Training training)
        {
            return String.Empty;//todo
        }
    }
}
