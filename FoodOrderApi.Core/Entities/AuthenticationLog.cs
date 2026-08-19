using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderApi.Core.Entities
{
    public class AuthenticationLog :BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }

    }
}
