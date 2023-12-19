using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CosmeticData))]
public class CustomCosmeticDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();
        CosmeticData cosmetic = (CosmeticData)target;

        if (cosmetic.cosmeticType == CosmeticType.Player)
        {
            EditorGUILayout.LabelField("Player Dash Texture");
            cosmetic.playerDashTexture = (Sprite)EditorGUILayout.ObjectField(cosmetic.playerDashTexture, typeof(Sprite), true, GUILayout.MaxWidth(500));
        }

        EditorGUILayout.EndVertical();
    }
}
