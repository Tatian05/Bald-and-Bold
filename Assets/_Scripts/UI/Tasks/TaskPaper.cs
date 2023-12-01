using UnityEngine;
public class TaskPaper : MonoBehaviour
{
    [SerializeField] Transform[] _spawns;
    [SerializeField] Animator _animator;

    public Transform[] Spawns { get { return _spawns; } }
    public void PlayNext() => _animator.Play("Next");
    public void PlayBefore() => _animator.Play("Before");
}
