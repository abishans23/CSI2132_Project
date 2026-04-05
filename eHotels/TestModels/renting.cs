using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class renting
{
    [Key]
    public int rentingid { get; set; }

    [StringLength(20)]
    public string status { get; set; } = null!;

    public DateOnly startdate { get; set; }

    public DateOnly enddate { get; set; }

    [StringLength(20)]
    public string paymentmethod { get; set; } = null!;

    public int amount { get; set; }

    public DateOnly processeddate { get; set; }

    public int roomnumber { get; set; }

    public int hotelid { get; set; }

    [StringLength(30)]
    public string idtype { get; set; } = null!;

    [StringLength(30)]
    public string idnumber { get; set; } = null!;

    [ForeignKey("idtype, idnumber")]
    [InverseProperty("renting")]
    public virtual customer customer { get; set; } = null!;

    [ForeignKey("roomnumber, hotelid")]
    [InverseProperty("renting")]
    public virtual room room { get; set; } = null!;
}
