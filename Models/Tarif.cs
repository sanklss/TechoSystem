using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class Tarif
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ServiceId { get; set; }

    public int MonthCount { get; set; }

    public DateOnly Date { get; set; }

    public int Price { get; set; }

    public int UserLimit { get; set; }

    public int FreeLicense { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Service Service { get; set; } = null!;
}
