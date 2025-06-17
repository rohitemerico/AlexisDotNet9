namespace Dashboard.Infra.EF.Models.Ad
{
    public enum AdExtType
    {
        JPG,
        JPEG,
        PNG,
        BMP,
        GIF,
        WEBM,
        MP4,
        M4V
    }

    public enum AdFileType
    {
        NA,
        IMAGE,
        VIDEO
    }


    public class Ad
    {
        /// <summary>
        /// Get File Type (e.g. image, video) from ext type. See class AdFileType.
        /// <para>E.g. extensionType = JPG</para> 
        /// </summary>
        /// <param name="extensionType"></param>
        /// <returns></returns>
        public static AdFileType GetFileType(string extensionType)
        {
            //Default NA
            AdFileType fileType = AdFileType.NA;

            //Check input if it's .JPG or just JPG
            var segments = extensionType.Split('.');

            if (segments.Length != 1)
            {
                if (segments.Length == 2)
                    extensionType = segments[1];
                if (segments.Length > 2)
                    return AdFileType.NA;
            }

            switch (extensionType.ToUpper())
            {
                case nameof(AdExtType.JPG):
                case nameof(AdExtType.JPEG):
                case nameof(AdExtType.PNG):
                case nameof(AdExtType.GIF):
                case nameof(AdExtType.BMP):
                    fileType = AdFileType.IMAGE;
                    break;
                case nameof(AdExtType.WEBM):
                case nameof(AdExtType.MP4):
                case nameof(AdExtType.M4V):
                    fileType = AdFileType.VIDEO;
                    break;
                default:
                    fileType = AdFileType.NA;
                    break;
            }

            return fileType;
        }
    }
}
