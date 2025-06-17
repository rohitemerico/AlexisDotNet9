namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Commands
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
