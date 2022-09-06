using UnityEngine;


public class ColorSelector : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _selecteIndicator;
    
    public void OnColorSelected()
    {
        DrawManager.Instance.SelectedColor = _color;
        _selecteIndicator.SetActive(true);
    }
}
