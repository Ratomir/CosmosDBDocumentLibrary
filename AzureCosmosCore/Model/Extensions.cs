﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AzureCosmosCore.Model
{
    public static class Extensions
    {
        public static int ToUnixTime(this DateTime input)
        {
            return (int)input.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
