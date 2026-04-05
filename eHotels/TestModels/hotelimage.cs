using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("hotelid", "filename")]
public partial class hotelimage
{
    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(25)]
    public string filename { get; set; } = null!;

    [StringLength(100)]
    public string? imagedesc { get; set; }

    [ForeignKey("hotelid")]
    [InverseProperty("hotelimage")]
    public virtual hotel hotel { get; set; } = null!;
}
