using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class hotel
{
    [Key]
    public int hotelid { get; set; }

    public int? chainid { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    [StringLength(10)]
    public string postalcode { get; set; } = null!;

    public int? stars { get; set; }

    [StringLength(20)]
    public string? manager { get; set; }

    [StringLength(200)]
    public string? description { get; set; }

    [ForeignKey("chainid")]
    [InverseProperty("hotel")]
    public virtual hotelchain? chain { get; set; }

    [InverseProperty("hotel")]
    public virtual ICollection<employee> employee { get; set; } = new List<employee>();

    [InverseProperty("hotel")]
    public virtual ICollection<hotelamenity> hotelamenity { get; set; } = new List<hotelamenity>();

    [InverseProperty("hotel")]
    public virtual ICollection<hotelemail> hotelemail { get; set; } = new List<hotelemail>();

    [InverseProperty("hotel")]
    public virtual ICollection<hotelimage> hotelimage { get; set; } = new List<hotelimage>();

    [InverseProperty("hotel")]
    public virtual ICollection<hotelphone> hotelphone { get; set; } = new List<hotelphone>();

    [ForeignKey("manager")]
    [InverseProperty("hotelNavigation")]
    public virtual employee? managerNavigation { get; set; }

    [InverseProperty("hotel")]
    public virtual ICollection<review> review { get; set; } = new List<review>();

    [InverseProperty("hotel")]
    public virtual ICollection<room> room { get; set; } = new List<room>();
}
