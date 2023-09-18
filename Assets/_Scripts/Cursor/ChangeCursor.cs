using UnityEngine;
public class ChangeCursor : MonoBehaviour
{
    [SerializeField] CursorData _cursor;

    CursorData _defaultCursor;
    bool _hasExit;
    void Awake()
    {
        _defaultCursor = Resources.Load<CursorData>("DefaultCursor");   
    }
    private void OnMouseEnter()
    {
        SetCursor();
        _hasExit = false;
    }
    private void OnMouseExit()
    {
        SetDefaultCursor();
        _hasExit = true;
    }
    private void OnDisable()
    {
        if (!_hasExit) SetDefaultCursor();
    }
    void SetDefaultCursor()
    {
        Cursor.SetCursor(_defaultCursor.cursorTexture, _defaultCursor.cursorHotspot, CursorMode.ForceSoftware);
    }
    void SetCursor()
    {
        Cursor.SetCursor(_cursor.cursorTexture, _cursor.cursorHotspot, CursorMode.ForceSoftware);
    }
}
