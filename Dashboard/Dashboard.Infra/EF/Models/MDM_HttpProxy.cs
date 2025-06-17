namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_HttpProxy
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string ProxyType { get; set; }

        [StringLength(50)]
        public string ProxyServer { get; set; }

        [StringLength(50)]
        public string Port { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string AllowByPassingProxy { get; set; }
    }
}
