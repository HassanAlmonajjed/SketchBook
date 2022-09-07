using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawManager : MonoBehaviour
{
    public static DrawManager Instance;
    public static float Resolution = 0.2f;

    [SerializeField] private Line _linePrefab;
    [SerializeField] private LayerMask _lineMask;

    public Line CurrentLine { get; set; }
    public Color SelectedColor { get; set; } = Color.black;
    public DrawState DrawState { get; set; } = DrawState.Draw;
    public int ActiveLineCount
    {
        get 
        {
            int count = 0;
            foreach (Transform line in transform)
            {
                if (line.gameObject.activeSelf)
                    count++;
            }

            return count;
        }
    }
    public Stack<GameObject> UndoHistory { get; set; }
    public Stack<GameObject> RedoHistory { get; set; }

    private Camera _camera;
    private int bitMask;
    private string _dataPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
        bitMask = LayerMask.GetMask("Line");
        UndoHistory = new Stack<GameObject>();
        RedoHistory = new Stack<GameObject>();

        _dataPath = Application.dataPath + "/save.txt";

        LoadBoard();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        switch (DrawState)
        {
            case DrawState.Draw:
                HandleDraw(mousePosition);
                break;
            case DrawState.Erase:
                HandleErase(mousePosition);
                break;
            default:
                break;
        }
        
    }

    public void Clear()
    {
        foreach (Transform line in transform)
            Destroy(line.gameObject);
    }

    private void HandleDraw(Vector2 mousePosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            CurrentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, transform);
            CurrentLine.SetColor(SelectedColor);

            AddToHistory(CurrentLine.gameObject);
        }

        else if (Input.GetMouseButton(0))
        {
            CurrentLine.SetPosition(mousePosition);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (CurrentLine.PointCount <= 1)
                CurrentLine.SetPosition(mousePosition + new Vector2(0.01f,0), true);
        }
    }

    private void HandleErase(Vector2 mousePosition)
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, bitMask);

            if (hit.collider != null)
            {
                GameObject line = hit.collider.transform.parent.gameObject;
                line.SetActive(false);

                AddToHistory(line);
            }
        }
    }

    private void AddToHistory(GameObject currentLine)
    {
        UndoHistory.Push(currentLine);

        foreach (var line in RedoHistory)
            Destroy(line);
        RedoHistory.Clear();
    }

    public void SaveBoard()
    {
        BoardData boardData = new();

        foreach (Transform line in transform)
        {
            if (line.gameObject.activeSelf)
            {
                Line lineObject = line.GetComponent<Line>();
                boardData.Data.Add(lineObject.ExtractData());
            }
        }

        string json = JsonUtility.ToJson(boardData);
        File.WriteAllText(_dataPath, json);
    }

    private void LoadBoard()
    {
        BoardData boardData = JsonUtility.FromJson<BoardData>(File.ReadAllText(_dataPath));

        foreach (var data in boardData.Data)
        {
            var line = Instantiate(_linePrefab, data.Position, Quaternion.identity, transform);

            Vector2[] points = new Vector2[data.Points.Length];
            for (int i = 0; i < points.Length; i++)
                points[i] = data.Points[i];
            line.SetPositions(points);

            line.SetColor(data.Color);
        }
    }
}

public enum DrawState
{
    Draw,
    Erase
}
