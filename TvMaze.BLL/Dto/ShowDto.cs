using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TvMaze.BLL.Dto
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PersonDto> Cast { get; set; }
    }
}
