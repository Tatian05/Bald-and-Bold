using UnityEngine;
using DG.Tweening;
public class WindowsAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBack);       
    }
    public void OnWindowClose()
    {
        transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InBack).OnComplete(()=>gameObject.SetActive(false));
    }
}
