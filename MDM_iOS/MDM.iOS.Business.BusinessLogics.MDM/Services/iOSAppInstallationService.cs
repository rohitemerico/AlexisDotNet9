using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexis.Dashboard.Pages.Report.AppInstallation;
using MDM.iOS.Common.Data.Component;
using Microsoft.EntityFrameworkCore;

namespace MDM.iOS.Business.BusinessLogics.MDM
{
    public class iOSAppInstallationService : IiOSAppInstallationService
    {
        private readonly IiOSAppInstallationRepository _repository;

        public iOSAppInstallationService(IiOSAppInstallationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<iOSAppInstallationSummary>> GetInstallationSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var installations = await _repository.GetInstallationsByDateRangeAsync(startDate, endDate);

            var summaries = installations
                .GroupBy(i => new { i.AppName, i.BundleIdentifier, i.Version })
                .Select(g => new iOSAppInstallationSummary
                {
                    AppName = g.Key.AppName,
                    BundleIdentifier = g.Key.BundleIdentifier,
                    Version = g.Key.Version,
                    SuccessfulInstallations = g.Count(i => i.Status == InstallationStatus.Successful),
                    FailedInstallations = g.Count(i => i.Status == InstallationStatus.Failed),
                    PendingInstallations = g.Count(i => i.Status == InstallationStatus.Pending),
                    LastInstallationAttempt = g.Max(i => i.InstallationDate)
                })
                .OrderByDescending(s => s.LastInstallationAttempt)
                .ToList();

            return summaries;
        }
    }

    public enum InstallationStatus
    {
        Pending,
        Successful,
        Failed
    }
} 