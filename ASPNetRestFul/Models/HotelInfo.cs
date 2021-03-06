﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetRestFul.Models
{
    public class HotelInfo 
    {
        public string Title { get; set; }

        public string Tagline { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Href { get; set; }

        public Address Location { get; set; }

      public class Address
        {

            public string Street { get; set; }

            public string Country { get; set; }

            public string City { get; set; }
        }


    }
}
