using System;
using System.Collections.Generic;

namespace TechnoSystem.Models;

public partial class PaymentWay
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
