namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_Cellular
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string ConfiguredAPNType { get; set; }

        [StringLength(50)]
        public string DefaultAPN_Name { get; set; }

        [StringLength(50)]
        public string DefaultAPN_AuthenticationType { get; set; }

        [StringLength(50)]
        public string DefaultAPN_UserName { get; set; }

        [StringLength(50)]
        public string DefaultAPN_Password { get; set; }

        [StringLength(50)]
        public string DataAPN_Name { get; set; }

        [StringLength(50)]
        public string DataAPN_AuthenticationType { get; set; }

        [StringLength(50)]
        public string DataAPN_UserName { get; set; }

        [StringLength(50)]
        public string DataAPN_Password { get; set; }

        [StringLength(50)]
        public string DataAPN_ProxyServer { get; set; }
    }
}
