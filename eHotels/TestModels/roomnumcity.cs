using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[Keyless]
public partial class roomnumcity
{
    [StringLength(20)]
    public string? city { get; set; }

    public long? count { get; set; }
}
