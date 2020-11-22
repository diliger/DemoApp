using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DemoApp.Models
{
    [Table("RX_Job")]
    public partial class RxJob
    {
        public Guid Id { get; set; }
        [Column("ContractorID")]
        public int? ContractorId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public int? Floor { get; set; }
        public int? Room { get; set; }
        [StringLength(50)]
        public string DelayReason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCompleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateDelayed { get; set; }
        public int? StatusNum { get; set; }
        [Column("RJobID")]
        public int? RjobId { get; set; }

        [ForeignKey(nameof(RoomType))]
        public Guid? RoomTypeId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey(nameof(RoomTypeId))]
        public virtual RxRoomType RoomType { get; set; }
    }
}
