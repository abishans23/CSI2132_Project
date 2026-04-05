using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("chainid", "phonenumber")]
public partial class hotelchainphone
{
    [Key]
    public int chainid { get; set; }

    [Key]
    [StringLength(10)]
    public string phonenumber { get; set; } = null!;

    [ForeignKey("chainid")]
    [InverseProperty("hotelchainphone")]
    public virtual hotelchain chain { get; set; } = null!;
}
