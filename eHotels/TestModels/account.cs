using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

public partial class account
{
    [Key]
    [StringLength(30)]
    public string email { get; set; } = null!;

    [StringLength(20)]
    public string username { get; set; } = null!;

    [StringLength(15)]
    public string password { get; set; } = null!;

    [InverseProperty("emailNavigation")]
    public virtual ICollection<employee> employee { get; set; } = new List<employee>();
}
