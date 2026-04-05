using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("roomnumber", "hotelid")]
public partial class room
{
    [Key]
    public int roomnumber { get; set; }

    [Key]
    public int hotelid { get; set; }

    [Precision(8, 2)]
    public decimal? price { get; set; }

    public int? capacity { get; set; }

    [StringLength(30)]
    public string? view { get; set; }

    public bool? extendable { get; set; }

    [InverseProperty("room")]
    public virtual ICollection<booking> booking { get; set; } = new List<booking>();

    [ForeignKey("hotelid")]
    [InverseProperty("room")]
    public virtual hotel hotel { get; set; } = null!;

    [InverseProperty("room")]
    public virtual ICollection<renting> renting { get; set; } = new List<renting>();

    [InverseProperty("room")]
    public virtual ICollection<roomamenity> roomamenity { get; set; } = new List<roomamenity>();

    [InverseProperty("room")]
    public virtual ICollection<roomproblem> roomproblem { get; set; } = new List<roomproblem>();
}
