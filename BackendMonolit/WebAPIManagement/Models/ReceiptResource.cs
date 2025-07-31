using System;
using System.Collections.Generic;

namespace WebAPIManagement.Models;

public partial class ReceiptResource
{
    public int Id { get; set; }

    public int ResourceId { get; set; }

    public int UnitId { get; set; }

    public decimal Quantity { get; set; }

    public int ReceiptDocumentId { get; set; }

    public virtual ReceiptDocument ReceiptDocument { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
