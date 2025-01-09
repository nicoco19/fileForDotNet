using System;
using System.Collections.Generic;

namespace examenV1Semptembre.Models;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
