using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("hotelid", "email")]
public partial class hotelemail
{
    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(30)]
    public string email { get; set; } = null!;

    [ForeignKey("hotelid")]
    [InverseProperty("hotelemail")]
    public virtual hotel hotel { get; set; } = null!;
}
