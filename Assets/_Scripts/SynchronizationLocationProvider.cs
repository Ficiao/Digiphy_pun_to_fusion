using Fusion;

namespace Digiphy
{
    public class SynchronizationLocationProvider : NetworkBehaviour
    {
        public override void Spawned()
        {
            TestingSetupManager.Instance.Init(Runner);
            if (VrRoomSynchronizer.Instance == null) return;

            VrRoomSynchronizer.Instance.SynchronizeRoomWithAr(Runner, transform);
        }
    }
}
