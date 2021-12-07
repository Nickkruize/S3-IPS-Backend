using System.Collections.Generic;

namespace S3_webshop.Resources
{
    public class AuthResult
    {
        public bool Succes { get; set; }
        public List<string> Errors { get; set; }
    }
}
