using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateConsoleDto
    {
        public string Name { get; set; } = null!;
        public int CompanyId { get; set; }
    }
}
