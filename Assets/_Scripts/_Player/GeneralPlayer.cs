using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralPlayer : MonoBehaviour
{
    protected bool _canMove = false;

    public abstract void PausePlayer();
    public abstract void UnPausePlayer();
}
