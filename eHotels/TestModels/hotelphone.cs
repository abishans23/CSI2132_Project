using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("hotelid", "phonenumber")]
public partial class hotelphone
{
    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(10)]
    public string phonenumber { get; set; } = null!;

    [ForeignKey("hotelid")]
    [InverseProperty("hotelphone")]
    public virtual hotel hotel { get; set; } = null!;
}
