using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("roomnumber", "hotelid", "amenity")]
public partial class roomamenity
{
    [Key]
    public int roomnumber { get; set; }

    [Key]
    public int hotelid { get; set; }

    [Key]
    [StringLength(40)]
    public string amenity { get; set; } = null!;

    [ForeignKey("roomnumber, hotelid")]
    [InverseProperty("roomamenity")]
    public virtual room room { get; set; } = null!;
}
