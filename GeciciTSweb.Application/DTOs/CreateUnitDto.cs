using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateUnitDto
    {
        public string Name { get; set; } = null!;
        public int ConsoleId { get; set; }
    }
}
