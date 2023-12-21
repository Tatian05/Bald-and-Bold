using System;
using System.Threading.Tasks;
using UnityEngine;
public class GachaBallAnimationScrip : MonoBehaviour
{
    [SerializeField] ShopItem _gachaItem;
    [SerializeField] Animator _animator;
    public static event Action OnGachaEnd;
    public void PlayGachaBallAnim(ShoppableSO shoppable)
    {
        _animator.Play("GachaBall");
        _gachaItem.SetCosmeticData(shoppable, true);
    }

    //LO LLAMO POR ANIMACION
    void CloseGachaBall()
    {
        GetComponent<WindowsAnimation>().OnWindowClose();
        OnGachaEnd();
        _gachaItem.ResetGachaItemShop();
    }
}
