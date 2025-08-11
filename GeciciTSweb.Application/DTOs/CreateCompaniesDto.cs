using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateCompaniesDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;
    }

}
