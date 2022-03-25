using System;
using System.Collections.Generic;
using System.Text;

namespace ClimateDatabaseByLenni.Models
{
    internal class Person
    {
        public int? Id { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }
        public int ObservationId { get; set; }
    }
    
}
