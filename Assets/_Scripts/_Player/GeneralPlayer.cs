using UnityEngine;
public abstract class GeneralPlayer : MonoBehaviour
{
    protected bool _canMove = false;
    [SerializeField] protected Transform _centerPivot;
    public Transform CenterPivot { get { return _centerPivot; } private set { } }
    public abstract void PausePlayer();
    public abstract void UnPausePlayer();
}
