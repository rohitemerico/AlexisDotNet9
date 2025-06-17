using System.Data;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Enroll
{
    public abstract class MDM_EnrollmentBase
    {
        public abstract DataTable GetData_EnrollmentDataByProfileID(Guid BranchID);

        public abstract DataTable GetData_Enrollment();

        public abstract DataTable GetData_Enrollment(En_Enrollment En_Enroll);

        public abstract DataTable GetData_Enrollment_byMdmID(En_Enrollment En_Enroll);

        public abstract DataTable GetData_Enrollment_byImei(En_Enrollment En_Enroll);

        public abstract DataTable GetData_Enrollment_byUDID(En_Enrollment En_Enroll);

        public abstract Guid GetData_Enrollment_Machine_ProfileID(En_Enrollment En_Enroll);

        public abstract Guid GetData_Enrollment_byUDID_getMDMID(En_Enrollment En_Enroll);

        public abstract bool Enrollment_UpdateStatus(En_Enrollment En_Enroll);

        public abstract bool Enrollment_Insert(En_Enrollment En_Enroll);

        public abstract bool Enrollment_Update(En_Enrollment En_Enroll);

        public abstract bool Enrollment_Delete(En_Enrollment En_Enroll);

    }
}
