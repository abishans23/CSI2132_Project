using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class employee
{
    [Key]
    [StringLength(20)]
    public string ssn { get; set; } = null!;

    [StringLength(20)]
    public string firstname { get; set; } = null!;

    [StringLength(20)]
    public string lastname { get; set; } = null!;

    [StringLength(10)]
    public string postalcode { get; set; } = null!;

    [StringLength(20)]
    public string position { get; set; } = null!;

    public int hotelid { get; set; }

    [StringLength(30)]
    public string email { get; set; } = null!;

    [ForeignKey("email")]
    [InverseProperty("employee")]
    public virtual account emailNavigation { get; set; } = null!;

    [ForeignKey("hotelid")]
    [InverseProperty("employee")]
    public virtual hotel hotel { get; set; } = null!;

    [InverseProperty("managerNavigation")]
    public virtual ICollection<hotel> hotelNavigation { get; set; } = new List<hotel>();
}
