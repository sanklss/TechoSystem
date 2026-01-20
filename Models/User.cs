using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role Role { get; set; } = null!;
}
