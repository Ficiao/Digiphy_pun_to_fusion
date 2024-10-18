using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Digiphy
{
    public class ChessInstanceController : NetworkBehaviour
    {
        public override void Spawned()
        {
            base.Spawned();
            TestingSetupManager.Instance.ChessCreated(Object);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            TestingSetupManager.Instance.ChessDeleted(Object);
        }
    }
}