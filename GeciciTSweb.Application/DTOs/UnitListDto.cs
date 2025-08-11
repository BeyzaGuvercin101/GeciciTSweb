using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class UnitListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ConsoleId { get; set; }
        
        // Navigation properties for display
        public string ConsoleName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        
        // Computed property to handle null ConsoleId display
        public string ConsoleDisplay => ConsoleId.HasValue ? ConsoleName : "-";
    }
}
