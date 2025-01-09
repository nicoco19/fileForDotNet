using System;
using System.Collections.Generic;

namespace examenV1Semptembre.Models;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
