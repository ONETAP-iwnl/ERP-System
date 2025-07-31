using System;
using System.Collections.Generic;

namespace WebAPIManagement.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();
}
