using UnityEngine;
using System.Linq;
using TMPro;
public class MiniGamesSelectorDoor : MonoBehaviour
{
    [SerializeField] string _miniGameSceneName;
    [SerializeField] int _miniGameZone;
    [SerializeField] TextMeshProUGUI _levelIndexTxt;

    Animator _anim;
    Collider2D _colliderNoTrigger;
    private void Start()
    {

        if (_miniGameZone > Helpers.PersistantData.gameData.currentLevel) return;
        _anim = GetComponentInChildren<Animator>();
        _colliderNoTrigger = GetComponents<Collider2D>().Where(x => !x.isTrigger).FirstOrDefault();

        _levelIndexTxt.text = _miniGameZone.ToString();


        ShowExit();
    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _colliderNoTrigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Helpers.GameManager.LoadSceneManager.LoadLevel(_miniGameSceneName);
            Helpers.GameManager.Player.PausePlayer();
        }
    }
}
