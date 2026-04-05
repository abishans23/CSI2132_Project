using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eHotels.TestModels;

[Keyless]
public partial class roomnum
{
    public int? hotelid { get; set; }

    public long? room_count { get; set; }
}
