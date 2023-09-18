using UnityEngine;
public class SR_OilSplat : MonoBehaviour
{
    public SR_OilSplat SetPosition(Vector3 pos)
    {
        Vector3 fixedPos = new Vector3(pos.x, pos.y, 2);
        transform.position = fixedPos;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        return this;
    }

    private void Reset()
    {
    }

    public static void TurnOn(SR_OilSplat b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(SR_OilSplat b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        FRY_EnemyOilSplat.Instance.ReturnObject(this);
    }
}
