using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy Data", menuName = "New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Transform playerPivot;
    public float lostTime = 3f;
}
