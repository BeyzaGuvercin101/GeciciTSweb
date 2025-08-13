using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeciciTSweb.Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!; // Primary user identifier

    public bool IsDeleted { get; set; } = false;
}
