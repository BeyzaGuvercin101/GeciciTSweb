using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class CreateUserDto
    {
        public string KeycloakUserId { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
    }
}
