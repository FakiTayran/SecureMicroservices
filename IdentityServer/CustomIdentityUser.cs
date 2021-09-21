using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class CustomIdentityUser : IdentityUser
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]

        public string Surname { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        public int UserType => 0; // SQLDapparStore SQLEFIdentity 'nin oluştrduğu modeli kullanmadığı için buna ihtiyaç duyuyor 0 kalmasında sakınca yok biz kullanmıyoruz.
    }
}
