using System;
using UnityEngine;
public class GachaBallAnimationScrip : MonoBehaviour
{
    [SerializeField] private ParticleSystem _psA;
    [SerializeField] private ParticleSystem _psB;
    [SerializeField] ShopItem _gachaItem;
    Animator _animator;
    public static event Action OnGachaEnd;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void ActiveParticles()
    {
        _psA.Play();
        _psB.Play();
    }
    public void PlayGachaBallAnim(ShoppableSO shoppable)
    {
        _animator.Play("GachaBall");
        _gachaItem.SetCosmeticData(shoppable);
    }

    //LO LLAMO POR ANIMACION
    public void CloseGachaBall()
    {
        GetComponent<WindowsAnimation>().OnWindowClose();
        OnGachaEnd();
    }
}
