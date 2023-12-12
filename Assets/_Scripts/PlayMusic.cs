using UnityEngine;
public class PlayMusic : MonoBehaviour
{
    [SerializeField] string _musicName;
    [SerializeField] bool _stopMusic;
    void Start()
    {
        if(_stopMusic)
        {
            Helpers.AudioManager.StopMusic();
            return;
        }

        if (_musicName != null) Helpers.AudioManager.PlayMusic(_musicName);
    }
}
