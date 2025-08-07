using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateCompaniesDto
    {
        public string Name { get; set; } = null!;
        public string IsDeleted { get; set; } = "false"; // Default value is false, can be changed later
    }

}
