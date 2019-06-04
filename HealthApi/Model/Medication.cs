﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApi.Model
{
    public class Medication
    {
        [Key]
        public string  Name { get; set; }
        public string  Doses { get; set; }
    }
}
