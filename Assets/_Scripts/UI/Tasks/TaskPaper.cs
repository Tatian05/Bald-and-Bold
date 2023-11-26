using UnityEngine;
public class TaskPaper : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public void PlayNext() => _animator.Play("Next");
    public void PlayBefore() => _animator.Play("Before");
}
