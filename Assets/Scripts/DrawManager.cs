using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public static DrawManager Instance;

    public static float Resolution = 0.1f;
    [SerializeField] private Line _linePrefab;

    private Camera _camera;
    public Line CurrentLine { get; set; }
    public Color SelectedColor { get; set; } = Color.black;

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
    }

    private void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            CurrentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity, transform);
            CurrentLine.SetColor(SelectedColor);
        }


        if (Input.GetMouseButton(0))
            CurrentLine.SetPosition(mousePosition);
    }

    public void Clear()
    {
        foreach (Transform line in transform)
            Destroy(line.gameObject);
    }
    
}
