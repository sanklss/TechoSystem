using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public int Price { get; set; }

    public int PaymentWayId { get; set; }

    public int PaymentStatusId { get; set; }

    public virtual PaymentStatus PaymentStatus { get; set; } = null!;

    public virtual PaymentWay PaymentWay { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
