using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.DTOs
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string KeycloakUserId { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
    }
}
