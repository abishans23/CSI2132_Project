using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("email", "hotelid")]
public partial class review
{
    [Key]
    [StringLength(30)]
    public string email { get; set; } = null!;

    [Key]
    public int hotelid { get; set; }

    public int? rating { get; set; }

    public DateOnly? date { get; set; }

    [StringLength(200)]
    public string? comments { get; set; }

    [ForeignKey("hotelid")]
    [InverseProperty("review")]
    public virtual hotel hotel { get; set; } = null!;
}
