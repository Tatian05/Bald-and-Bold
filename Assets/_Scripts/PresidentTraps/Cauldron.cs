using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Color _colorToChange;
    [SerializeField] Color _startColor;
    [SerializeField] SpriteRenderer _objectToChange;

    private void Update()
    {
        var color = Color.Lerp(_startColor, _colorToChange, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
        _objectToChange.color = color;
    
    }


}
