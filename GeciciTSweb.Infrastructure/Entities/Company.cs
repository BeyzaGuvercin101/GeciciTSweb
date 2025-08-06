using System;
using System.Collections.Generic;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }= false;


    public virtual ICollection<Console> Consoles { get; set; } = new List<Console>();
}
