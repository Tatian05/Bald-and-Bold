using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(PersistantData))]
public class PersistantDataCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PersistantData persistantData = (PersistantData)target;

        EditorGUILayout.Space(20);
        if (GUILayout.Button("Borrar Progreso de Niveles")) persistantData.BorrarProgresoDeNiveles();
        if (GUILayout.Button("Borrar Ajustes")) persistantData.BorrarAjustes();
        if (GUILayout.Button("Borrar Skins y Weas")) persistantData.BorrarSkinsYWeas();
        if (GUILayout.Button("Borrar Consumibles Activados")) persistantData.BorrarConsumiblesActivados();
        if (GUILayout.Button("Borrar Progreso de Tareas")) persistantData.BorrarProgresoDeTareas();
    }
}
