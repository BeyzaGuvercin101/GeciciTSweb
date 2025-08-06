using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class ConsoleListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CompanyId { get; set; }
    }
}
