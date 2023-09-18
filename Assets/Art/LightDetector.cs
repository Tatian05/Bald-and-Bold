using UnityEngine;
public class LightDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var shadow = collision.GetComponent<Shadow>();
        if (shadow) shadow.ShadowToggle(true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var shadow = collision.GetComponent<Shadow>();
        if (shadow) shadow.ShadowToggle(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var shadow = collision.GetComponent<Shadow>();
        if (shadow) shadow.ShadowToggle(false);
    }
}
