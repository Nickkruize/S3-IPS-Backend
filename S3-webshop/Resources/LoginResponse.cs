﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Resources
{
    public class LoginResponse : AuthResult
    {
        public string Token { get; set; }
    }
}
