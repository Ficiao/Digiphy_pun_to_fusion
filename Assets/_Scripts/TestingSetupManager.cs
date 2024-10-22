using Fusion;
using System;
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
        [SerializeField] private Button _buttonSmallKinematic;
        [SerializeField] private Button _buttonSmallLogic;
        [SerializeField] private Button _buttonSmallSnapping;
        [SerializeField] private Button _buttonLargeKinematic;
        [SerializeField] private Button _buttonLargeLogic;
        [SerializeField] private Button _buttonLargeSnapping;
        [SerializeField] private Button _buttonChessInBetween;
        [SerializeField] private Button _buttonChessInFront;
        [Header("Chess Prefabs")]
        [SerializeField] private GameObject _chessKinematic;
        [SerializeField] private GameObject _chessLogic;
        [SerializeField] private GameObject _chessSnapping;
        [Header("Chess Scales")]
        [SerializeField] private Vector3 _scaleSmall;
        [SerializeField] private Vector3 _scaleLarge;
        [Header("Additional toggles")]
        [SerializeField] private Toggle _chessMoveableToggle;
        private Transform _chessLocationTable;
        private Transform _chessLocationCouchInFront;
        private Transform _chessLocationCouchInBetween;
        private Transform _chessLocationLarge;
        private NetworkRunner _runner;
        private NetworkObject _currentChess;

        public bool IsChessMoveable => _chessMoveableToggle.isOn;
        public Action<bool> ChessMoveableChanged;

        public void Init(NetworkRunner runer)
        {
            _runner = runer;
            _testMenuControllsAndSizeToggle.onValueChanged.AddListener(value => 
                _testMenuControllsAndSize.SetActive(value));
            _testMenuCouchToggle.onValueChanged.AddListener(value =>
                _testMenuCouch.SetActive(value));

            _buttonSmallKinematic.onClick.AddListener(() => 
                CreateChess(_chessKinematic, _chessLocationTable, _scaleSmall));
            _buttonSmallLogic.onClick.AddListener(() => 
                CreateChess(_chessLogic, _chessLocationTable, _scaleSmall));
            _buttonSmallSnapping.onClick.AddListener(() => 
                CreateChess(_chessSnapping, _chessLocationTable, _scaleSmall));
            _buttonLargeKinematic.onClick.AddListener(() =>
                CreateChess(_chessKinematic, _chessLocationLarge, _scaleLarge));
            _buttonLargeLogic.onClick.AddListener(() =>
                CreateChess(_chessLogic, _chessLocationLarge, _scaleLarge));
            _buttonLargeSnapping.onClick.AddListener(() =>
                CreateChess(_chessSnapping, _chessLocationLarge, _scaleLarge));
            _buttonChessInBetween.onClick.AddListener(() =>
                CreateChess(_chessLogic, _chessLocationCouchInBetween, _scaleSmall));
            _buttonChessInFront.onClick.AddListener(() =>
                CreateChess(_chessLogic, _chessLocationCouchInFront, _scaleSmall));

            _chessMoveableToggle.onValueChanged.AddListener(value => ChessMoveableChanged.Invoke(value));
        }

        public void SetChessLocations(Transform chessTable, Transform chessCouchInFront, 
            Transform chessCouchInBetween, Transform chessLarge)
        {
            _chessLocationTable = chessTable;
            _chessLocationCouchInFront = chessCouchInFront;
            _chessLocationCouchInBetween = chessCouchInBetween;
            _chessLocationLarge = chessLarge;
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
