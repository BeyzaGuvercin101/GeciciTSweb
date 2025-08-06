using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }= false;


    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
