using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastScrips : MonoBehaviour
{
    PersistantData _persistantData;
    private void Awake()
    {
        _persistantData = Helpers.PersistantData;
    }

    public void Buttonsound()
    {
        Helpers.AudioManager.PlaySFX("ButtonSound");
    }
}
