using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.User.ViewModel
{
    public class NationalIdentity_VM
    {
        [JsonProperty("matched")]
        public bool Matched { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("father_name")]
        public string FatherName { get; set; }

        [JsonProperty("national_code")]
        public string NationalCode { get; set; }

        [JsonProperty("is_dead")]
        public bool IsDead { get; set; }

        [JsonProperty("alive")]
        public bool Alive { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }
    }
}
