using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[PrimaryKey("idtype", "idnumber")]
[Index("email", Name = "customer_email_key", IsUnique = true)]
public partial class customer
{
    [Key]
    [StringLength(30)]
    public string idtype { get; set; } = null!;

    [Key]
    [StringLength(30)]
    public string idnumber { get; set; } = null!;

    [StringLength(20)]
    public string firstname { get; set; } = null!;

    [StringLength(20)]
    public string lastname { get; set; } = null!;

    public DateOnly registrationdate { get; set; }

    [StringLength(10)]
    public string phonenumber { get; set; } = null!;

    [StringLength(6)]
    public string postalcode { get; set; } = null!;

    [StringLength(30)]
    public string? email { get; set; }

    [InverseProperty("customer")]
    public virtual ICollection<booking> booking { get; set; } = new List<booking>();

    [ForeignKey("postalcode")]
    [InverseProperty("customer")]
    public virtual address postalcodeNavigation { get; set; } = null!;

    [InverseProperty("customer")]
    public virtual ICollection<renting> renting { get; set; } = new List<renting>();
}
