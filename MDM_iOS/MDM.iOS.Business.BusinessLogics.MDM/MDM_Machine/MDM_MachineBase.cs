using System.Data;
using MDM.iOS.Entities;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine
{
    public abstract class MDM_MachineBase
    {
        public abstract DataTable getMachine_byBranch(Guid My_BranchId);

        public abstract bool UpdateMachineStatus(DeviceEn MachineEntity);

        public abstract bool UpdateMachineBranchID(DeviceEn MachineEntity);

        public abstract bool UpdateMachineBranchIDOnly(DeviceEn MachineEntity);
    }
}
