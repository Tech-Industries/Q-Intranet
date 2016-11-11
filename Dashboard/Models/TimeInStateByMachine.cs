namespace Dashboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeInStateByMachine")]
    public partial class TimeInStateByMachine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(50)]
        public string Mode { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(20)]
        public string Machine { get; set; }

        [StringLength(50)]
        public string CurrentMode { get; set; }

        public decimal? TotalTimeInMode { get; set; }
    }
}
