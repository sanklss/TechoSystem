using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Tarif> Tarifs { get; set; } = new List<Tarif>();
}
