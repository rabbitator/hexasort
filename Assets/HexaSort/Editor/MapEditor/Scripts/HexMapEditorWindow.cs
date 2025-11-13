using HexaSort.Game.Level;
using UnityEditor;
using UnityEngine;

namespace HexaSort.Editor.MapEditor
{
    public class HexMapEditorWindow : EditorWindow
    {
        private HexMapAsset _previousAsset;
        private HexMapAsset _currentAsset;
        private Texture2D _hexTexture;
        private Vector2 _scrollPos;
        private Vector2Int _mapSize;
        private float _hexSize = 40.0f;

        [MenuItem("Window/Hex Map Editor")]
        private static void ShowWindow()
        {
            GetWindow<HexMapEditorWindow>("Hex Editor");
        }

        private void OnEnable()
        {
            _hexTexture = Resources.Load<Texture2D>("hex_icon");
        }

        private void OnGUI()
        {
            _currentAsset = (HexMapAsset)EditorGUILayout.ObjectField("Map Asset", _currentAsset, typeof(HexMapAsset), false);

            if (!_currentAsset || !_hexTexture) return;

            if (_currentAsset != _previousAsset)
            {
                _mapSize = _currentAsset.size;
                _previousAsset = _currentAsset;
            }

            if (_currentAsset.cells.Length != _currentAsset.size.x * _currentAsset.size.y) _currentAsset.cells = new bool[_currentAsset.size.x * _currentAsset.size.y];

            var sizeX = EditorGUILayout.IntSlider("Map Width", _mapSize.x, 1, 64);
            var sizeY = EditorGUILayout.IntSlider("Map Height", _mapSize.y, 1, 64);

            if (sizeX != _mapSize.x || sizeY != _mapSize.y)
            {
                _mapSize = new Vector2Int(sizeX, sizeY);
                ResizeMap(sizeX, sizeY);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("8x8")) ResizeMap(8, 8);
            if (GUILayout.Button("16x16")) ResizeMap(16, 16);
            if (GUILayout.Button("Clear")) ClearMap();
            GUILayout.EndHorizontal();

            _hexSize = EditorGUILayout.Slider("Hex Size", _hexSize, 20.0f, 80.0f);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            DrawHexGrid();

            EditorGUILayout.EndScrollView();
        }

        private void DrawHexGrid()
        {
            var hexWidth = _hexSize * 0.866f;
            var hexHeight = _hexSize * 0.75f;

            var e = Event.current;

            for (var y = 0; y < _currentAsset.size.y; y++)
            for (var x = 0; x < _currentAsset.size.x; x++)
                PlaceIcon(x, hexWidth, y, hexHeight, e);
        }

        private void PlaceIcon(int x, float hexWidth, int y, float hexHeight, Event e)
        {
            var pos = new Vector2(
                x * hexWidth + y % 2 * hexWidth * 0.5f,
                y * hexHeight
            );

            var hexRect = new Rect(pos.x, pos.y + 30, _hexSize, _hexSize);

            var index = y * _currentAsset.size.x + x;
            var isActive = _currentAsset.cells[index];

            GUI.color = isActive ? Color.green : new Color(0.7f, 0.7f, 0.7f);
            GUI.DrawTexture(hexRect, _hexTexture);
            GUI.color = Color.white;

            if (e.type == EventType.MouseDown && hexRect.Contains(e.mousePosition))
            {
                _currentAsset.cells[index] = !_currentAsset.cells[index];
                EditorUtility.SetDirty(_currentAsset);
                Repaint();
                e.Use();
            }
        }


        private void ResizeMap(int width, int height)
        {
            var previousSize = _currentAsset.size;
            var previousMap = _currentAsset.cells;
            _currentAsset.size = new Vector2Int(width, height);
            _currentAsset.cells = new bool[width * height];
            CopyMap(previousMap, previousSize);
            EditorUtility.SetDirty(_currentAsset);
        }

        private void CopyMap(bool[] previousMap, Vector2Int previousSize)
        {
            for (var y = 0; y < _currentAsset.size.y; y++)
            for (var x = 0; x < _currentAsset.size.x; x++)
            {
                var isInside = previousSize.x > x && previousSize.y > y;
                _currentAsset.cells[y * _currentAsset.size.x + x] = isInside && previousMap[y * previousSize.x + x];
            }
        }

        private void ClearMap()
        {
            for (var i = 0; i < _currentAsset.cells.Length; i++) _currentAsset.cells[i] = false;

            EditorUtility.SetDirty(_currentAsset);
        }
    }
}