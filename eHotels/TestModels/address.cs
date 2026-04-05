using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class address
{
    public int streetnum { get; set; }

    [StringLength(50)]
    public string streetname { get; set; } = null!;

    [Key]
    [StringLength(6)]
    public string postalcode { get; set; } = null!;

    [StringLength(10)]
    public string province { get; set; } = null!;

    [StringLength(20)]
    public string country { get; set; } = null!;

    [StringLength(20)]
    public string? city { get; set; }

    [InverseProperty("postalcodeNavigation")]
    public virtual ICollection<customer> customer { get; set; } = new List<customer>();
}
