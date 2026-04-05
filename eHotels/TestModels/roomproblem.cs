using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("roomnumber", "hotelid", "problem")]
public partial class roomproblem
{
    [Key]
    public int roomnumber { get; set; }

    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(40)]
    public string problem { get; set; } = null!;

    [ForeignKey("roomnumber, hotelid")]
    [InverseProperty("roomproblem")]
    public virtual room room { get; set; } = null!;
}
