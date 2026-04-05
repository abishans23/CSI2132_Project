using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("hotelid", "amenityname")]
public partial class hotelamenity
{
    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(20)]
    public string amenityname { get; set; } = null!;

    [StringLength(100)]
    public string? amenitydesc { get; set; }

    [ForeignKey("hotelid")]
    [InverseProperty("hotelamenity")]
    public virtual hotel hotel { get; set; } = null!;
}
