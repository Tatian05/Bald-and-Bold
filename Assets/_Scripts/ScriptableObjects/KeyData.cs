using UnityEngine;

[CreateAssetMenu(fileName = "New KeyData", menuName = "New KeyData")]
public class KeyData : ScriptableObject
{
    [HideInInspector] public Sprite keySprite, pressedKey;
    public KeyCode input;
}
