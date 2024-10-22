using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Digiphy
{
    public class ChessInstanceController : NetworkBehaviour
    {
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private GameObject _grabbable;

        public override void Spawned()
        {
            base.Spawned();
            TestingSetupManager.Instance.ChessCreated(Object);

            _meshCollider.enabled = TestingSetupManager.Instance.IsChessMoveable;
            _grabbable.SetActive(TestingSetupManager.Instance.IsChessMoveable);

            TestingSetupManager.Instance.ChessMoveableChanged += ChessMoveableChanged;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            TestingSetupManager.Instance.ChessDeleted(Object);
            TestingSetupManager.Instance.ChessMoveableChanged -= ChessMoveableChanged;
            base.Despawned(runner, hasState);
        }

        private void ChessMoveableChanged(bool value)
        {
            _meshCollider.enabled = value;
            _grabbable.SetActive(value);
        }
    }
}