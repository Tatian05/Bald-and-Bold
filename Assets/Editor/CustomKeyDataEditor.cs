using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(KeyData))]
public class CustomKeyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        KeyData keyData = (KeyData)target;

        keyData.keySprite = (Sprite)EditorGUILayout.ObjectField("Key Sprite", keyData.keySprite, typeof(Sprite), false);
        keyData.pressedKey = (Sprite)EditorGUILayout.ObjectField("Pressed Key", keyData.pressedKey, typeof(Sprite), false);

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(keyData);

    }
}
