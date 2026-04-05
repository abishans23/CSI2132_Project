using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class hotelchain
{
    [Key]
    public int chainid { get; set; }

    [StringLength(50)]
    public string chainname { get; set; } = null!;

    [StringLength(10)]
    public string chainpostalcode { get; set; } = null!;

    [InverseProperty("chain")]
    public virtual ICollection<hotel> hotel { get; set; } = new List<hotel>();

    [InverseProperty("chain")]
    public virtual ICollection<hotelchainemail> hotelchainemail { get; set; } = new List<hotelchainemail>();

    [InverseProperty("chain")]
    public virtual ICollection<hotelchainphone> hotelchainphone { get; set; } = new List<hotelchainphone>();
}
