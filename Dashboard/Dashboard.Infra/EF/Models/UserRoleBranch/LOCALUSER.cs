namespace Dashboard.Infra.EF.Models.UserRoleBranch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LOCALUSER")]
    public partial class LOCALUSER
    {
        [Key]
        [StringLength(36)]
        public string LOGINID { get; set; }

        [StringLength(500)]
        public string LPASSWORD { get; set; }

        [StringLength(150)]
        public string LEMAIL { get; set; }

        [StringLength(200)]
        public string LRESETPASSWORD { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LRESETPASSWORDDATETIME { get; set; }
    }
}
