﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public record Test
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }
}
