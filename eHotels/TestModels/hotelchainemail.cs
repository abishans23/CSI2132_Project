using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("chainid", "email")]
public partial class hotelchainemail
{
    [Key]
    public int chainid { get; set; }

    [Key]
    [StringLength(30)]
    public string email { get; set; } = null!;

    [ForeignKey("chainid")]
    [InverseProperty("hotelchainemail")]
    public virtual hotelchain chain { get; set; } = null!;
}
