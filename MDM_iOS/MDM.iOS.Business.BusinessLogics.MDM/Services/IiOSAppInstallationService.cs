using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexis.Dashboard.Pages.Report.AppInstallation;

namespace MDM.iOS.Business.BusinessLogics.MDM
{
    public interface IiOSAppInstallationService
    {
        Task<List<iOSAppInstallationSummary>> GetInstallationSummaryAsync(DateTime startDate, DateTime endDate);
    }
} 