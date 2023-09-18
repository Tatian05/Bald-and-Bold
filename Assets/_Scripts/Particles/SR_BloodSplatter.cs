using UnityEngine;
public class SR_BloodSplatter : MonoBehaviour
{
    public SR_BloodSplatter SetPosition(Vector3 pos)
    {
        Vector3 fixedPos = new Vector3(pos.x, pos.y, 2);
        transform.position = fixedPos;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        return this;
    }

    private void Reset()
    {
    }

    public static void TurnOn(SR_BloodSplatter b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(SR_BloodSplatter b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        FRY_EnemyBloodSplatter.Instance.ReturnObject(this);
    }
}
