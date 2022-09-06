using UnityEngine;


public class ColorSelector : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _selecteIndicator;
    
    public void OnColorSelected()
    {
        DrawManager.Instance.SelectedColor = _color;

        ResetIndicators();
        _selecteIndicator.SetActive(true);
    }

    private void ResetIndicators()
    {
        var indicators = FindObjectsOfType<ColorSelector>();
        foreach (var indicator in indicators)
        {
            indicator._selecteIndicator.SetActive(false);
        }
    }
}
