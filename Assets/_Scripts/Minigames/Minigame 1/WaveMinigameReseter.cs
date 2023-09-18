using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMinigameReseter : MonoBehaviour
{
    private void Start()
    {
        Helpers.GameManager.SaveDataManager.SaveInt("WavesCurrentDeaths", 0);
        Helpers.GameManager.SaveDataManager.SaveBool("LevelsSetted", false);
    }
}
