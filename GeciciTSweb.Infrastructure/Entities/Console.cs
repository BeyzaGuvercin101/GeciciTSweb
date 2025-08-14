using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class Console
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CompanyId { get; set; } = null;
    public bool IsDeleted { get; set; }= false;

    [ForeignKey("CompanyId")]
    public virtual Companies? Company { get; set; }

    public virtual ICollection<Unit> Units { get; set; }
}
