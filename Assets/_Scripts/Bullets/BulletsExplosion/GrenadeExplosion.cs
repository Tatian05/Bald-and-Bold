using System.Collections;
using UnityEngine;
public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] float _timeToReturn;

    Vector2 _initialScale = new Vector2 { x = 2, y = 2 };
    void OnEnable()
    {
        StartCoroutine(DisableObject());
    }
    private void OnDisable()
    {
        StopCoroutine(DisableObject());
    }
    IEnumerator DisableObject()
    {
        float timer = 0;
        while (timer <= _timeToReturn)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        FRY_GrenadeExplosion.Instance.ReturnObject(this);
    }
    public static void TurnOn(GrenadeExplosion g)
    {
        g.gameObject.SetActive(true);
    }
    public static void TurnOff(GrenadeExplosion g)
    {
        g.transform.localScale = g._initialScale;
        g.gameObject.SetActive(false);
    }

    #region BUILDER
    public GrenadeExplosion SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }
    public GrenadeExplosion SetScale(float scale)
    {
        transform.localScale = _initialScale * scale;
        return this;
    }

    #endregion
}
