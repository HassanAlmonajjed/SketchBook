using UnityEngine;

public class Options : MonoBehaviour
{
    public void OnPencilClicked()
    {
        DrawManager.Instance.DrawState = DrawState.Draw;
    }

    public void OnRubberClicked()
    {
        DrawManager.Instance.DrawState = DrawState.Erase;
    }
}
