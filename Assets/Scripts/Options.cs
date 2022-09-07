using System.IO;
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

    public void OnUndoClicked()
    {
        if (DrawManager.Instance.UndoHistory.Count == 0)
            return;

        GameObject lastLine = DrawManager.Instance.UndoHistory.Pop();
        lastLine.SetActive(!lastLine.activeInHierarchy);
        DrawManager.Instance.RedoHistory.Push(lastLine);
    }

    public void OnRedoClicked()
    {
        if (DrawManager.Instance.RedoHistory.Count == 0)
            return;

        GameObject lastLine = DrawManager.Instance.RedoHistory.Pop();
        lastLine.SetActive(!lastLine.activeInHierarchy);
        DrawManager.Instance.UndoHistory.Push(lastLine);
    }

    public void OnCloseClicked()
    {
        DrawManager.Instance.SaveBoard();
        Application.Quit();
    }
}
