using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Role
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
