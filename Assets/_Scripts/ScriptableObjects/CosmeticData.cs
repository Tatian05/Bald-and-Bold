using UnityEngine;
[CreateAssetMenu(fileName = "New CosmeticData", menuName = "New Cosmetic")]
public class CosmeticData : ScriptableObject
{
    public string cosmeticName;
    public int cost;
    public Sprite headSprite, torsoSprite, rightLegSprite, leftLegSprite, rightHandSprite, leftHandSprite, tailSprite;
}
