﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSS.API.Common
{
    public class CSRedisClientOptions
    {
        public string ConnectionString { get; set; }
        public Func<string, string> NodeRule { get; set; }
        public string[] ConnectionStrings { get; set; }
    }
}
