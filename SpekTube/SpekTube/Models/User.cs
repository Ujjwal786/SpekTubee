using System;
using System.Collections.Generic;
namespace SpekTube.Models
{

    public class User
    {
        public int Id { get; set; }
        public string? Oauth_Provider { get; set; }
        public string? Oauth_Id { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Email { get; set; }
        public string? Picture { get; set; }

    }

}



