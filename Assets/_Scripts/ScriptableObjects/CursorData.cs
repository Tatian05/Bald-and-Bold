using UnityEngine;
[CreateAssetMenu(menuName = "Cursor Data/New Cursor Data", fileName = "New Cursor Data")]
public class CursorData : ScriptableObject
{
    public Texture2D cursorTexture;
    public Vector2 cursorHotspot;
}
