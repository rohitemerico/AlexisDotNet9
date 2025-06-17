namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_WIFI
    {
        public Guid Profile_ID { get; set; }

        [Key]
        public Guid Profile_WIFI_ID { get; set; }

        [StringLength(50)]
        public string ServiceSetIdentifier { get; set; }

        [StringLength(50)]
        public string HiddenNetwork { get; set; }

        [StringLength(50)]
        public string AutoJoin { get; set; }

        [StringLength(50)]
        public string DisableCaptiveNetworkDetection { get; set; }

        [StringLength(50)]
        public string ProxySetup { get; set; }

        [StringLength(50)]
        public string SecurityType { get; set; }

        [StringLength(50)]
        public string SecurityTypePassword { get; set; }

        [StringLength(50)]
        public string NetworkType { get; set; }

        [StringLength(50)]
        public string FastLaneQosMarking { get; set; }

        [StringLength(50)]
        public string EnableQosMarking { get; set; }

        [StringLength(50)]
        public string WhitelistAppleAudioVideoCalling { get; set; }

        [StringLength(50)]
        public string ServerIPAddress { get; set; }

        [StringLength(50)]
        public string ServerPort { get; set; }

        [StringLength(50)]
        public string ServerProxyURL { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string App_Identifity { get; set; }

        [StringLength(50)]
        public string ProxyPACFallbackAllowed { get; set; }
    }
}
