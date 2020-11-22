using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Table("RX_RoomType")]
    public partial class RxRoomType
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(28)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}
