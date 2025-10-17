using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class RepairAccessory
{
    public int RepairAccessoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RepairItem> RepairItems { get; set; } = new List<RepairItem>();
}
