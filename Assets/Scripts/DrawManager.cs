using System.Collections.Generic;
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
    public Stack<GameObject> UndoHistory { get; set; }
    public Stack<GameObject> RedoHistory { get; set; }

    private Camera _camera;
    private int bitMask;

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
    }

    private void Update()
    {
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CurrentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, transform);
            CurrentLine.SetColor(SelectedColor);

            AddToHistory(CurrentLine.gameObject);
        }

        if (Input.GetMouseButton(0))
            CurrentLine.SetPosition(mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentLine.PointCount <= 1)
            {
                CurrentLine.SetPosition(mousePosition + new Vector2(0.01f,0), true);
            }
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
}

public enum DrawState
{
    Draw,
    Erase
}
