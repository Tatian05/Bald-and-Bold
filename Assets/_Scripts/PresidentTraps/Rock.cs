using UnityEngine;
public class Rock : MonoBehaviour
{
    [SerializeField] ParticleSystem _presidentCD_PS;
    [SerializeField] GameObject _presidentGO;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<President_PartyWhsitle>())
        {
            _presidentCD_PS.Play();
            _presidentGO.SetActive(false);
        }
    }
}
