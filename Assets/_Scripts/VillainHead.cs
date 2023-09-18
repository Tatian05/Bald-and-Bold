using UnityEngine;
using System.Linq;
public class VillainHead : MonoBehaviour
{
    [SerializeField] Transform _eyesParent;
    [SerializeField] Transform[] _eyes;

    Transform _player;
    Transform[] _iris;
    Vector3[] _initialPos = new Vector3[2];
    private void Start()
    {
        _iris = _eyes.Select(x => x.GetChild(0)).ToArray();
        _player = Helpers.GameManager.Player.transform;

        for (int i = 0; i < _initialPos.Length; i++) _initialPos[i] = _iris[i].localPosition;
    }

    private void LateUpdate()
    {
        Vector3 dist = _player.position - _eyesParent.position;

        for (int i = 0; i < _eyes.Length; i++)
        {
            _eyes[i].right = dist;
            _iris[i].eulerAngles = new Vector3(0, 0, _iris[i].rotation.z - _eyes[i].rotation.z);
        }
    }
}
