using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TvMaze.DAL
{
    public class Show
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ShowId { get; set; }
        public string Name { get; set; }
        public string Cast { get; set; }
    }
}
