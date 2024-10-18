using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace Digiphy
{
    public class TestingSetupManager : Singleton<TestingSetupManager>
    {
        [Header("Menus")]
        [SerializeField] private GameObject _testMenuControllsAndSize;
        [SerializeField] private GameObject _testMenuCouch;
        [Header("Menu Toggles")]
        [SerializeField] private Toggle _testMenuControllsAndSizeToggle;
        [SerializeField] private Toggle _testMenuCouchToggle;
        [Header("Setup Toggles")]
        [SerializeField] private Toggle _toggleSmallKinematic;
        [SerializeField] private Toggle _toggleSmallLogic;
        [SerializeField] private Toggle _toggleSmallSnapping;
        [SerializeField] private Toggle _toggleLargeKinematic;
        [SerializeField] private Toggle _toggleLargeLogic;
        [SerializeField] private Toggle _toggleLargeSnapping;
        [SerializeField] private Toggle _toggleChessInBetween;
        [SerializeField] private Toggle _toggleChessInFront;
        [Header("Chess Prefabs")]
        [SerializeField] private GameObject _chessKinematic;
        [SerializeField] private GameObject _chessLogic;
        [SerializeField] private GameObject _chessSnapping;
        [Header("Chess Scales")]
        [SerializeField] private Vector3 _scaleSmall;
        [SerializeField] private Vector3 _scaleLarge;
        [Header("Chess Locations")]
        [SerializeField] private Transform _chessLocationTable;
        [SerializeField] private Transform _chessLocationCouchInFront;
        [SerializeField] private Transform _chessLocationCouchInBetween;
        [SerializeField] private Transform _chessLocationLarge;
        private NetworkRunner _runner;
        private NetworkObject _currentChess;

        public void Init(NetworkRunner runer)
        {
            _runner = runer;
            _testMenuControllsAndSizeToggle.onValueChanged.AddListener(value => 
                _testMenuControllsAndSize.SetActive(value));
            _testMenuCouchToggle.onValueChanged.AddListener(value =>
                _testMenuCouch.SetActive(value));

            _toggleSmallKinematic.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessKinematic, _chessLocationTable, _scaleSmall);
            });
            _toggleSmallLogic.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessLogic, _chessLocationTable, _scaleSmall);
            });
            _toggleSmallSnapping.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessSnapping, _chessLocationTable, _scaleSmall);
            });
            _toggleLargeKinematic.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessKinematic, _chessLocationLarge, _scaleLarge);
            });
            _toggleLargeLogic.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessLogic, _chessLocationLarge, _scaleLarge);
            });
            _toggleLargeSnapping.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessSnapping, _chessLocationLarge, _scaleLarge);
            });
            _toggleChessInBetween.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessLogic, _chessLocationCouchInBetween, _scaleSmall);
            });
            _toggleChessInFront.onValueChanged.AddListener(value => {
                if (value) CreateChess(_chessLogic, _chessLocationCouchInFront, _scaleSmall);
            });
        }

        public void CreateChess(GameObject chessPrefab, Transform transform, Vector3 scale)
        {
            if (_currentChess)
            {
                if (!_currentChess.HasStateAuthority) _currentChess.RequestStateAuthority();
                _runner.Despawn(_currentChess);
            }
            NetworkObject chess = _runner.Spawn(chessPrefab, transform.position, transform.rotation);
            chess.transform.localScale = scale;
        }

        public void ChessCreated(NetworkObject chess)
        {
            if (_currentChess && _currentChess.HasStateAuthority)
            {
                _runner.Despawn(_currentChess);
            }
            _currentChess = chess;
        }

        public void ChessDeleted(NetworkObject chess)
        {
            if (_currentChess == chess) _currentChess = null;
        }
    }
}
