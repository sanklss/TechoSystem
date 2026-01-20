using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class Request
{
    public int Id { get; set; }

    public int TarifId { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public int RequestStatusId { get; set; }

    public int LicenseCount { get; set; }

    public int FullPrice { get; set; }

    public string? Comment { get; set; }

    public virtual RequestStatus RequestStatus { get; set; } = null!;

    public virtual Tarif Tarif { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
