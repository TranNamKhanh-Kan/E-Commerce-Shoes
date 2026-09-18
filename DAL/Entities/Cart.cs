#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Cart
{
    public Guid CartId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; }
}
