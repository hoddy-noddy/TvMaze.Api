using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvMaze.WebApi.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
