using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data.Helper
{
    public static class Extension
    {
      
        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
       
    }
}
