using UnityEngine;
using TMPro;
using System.Threading.Tasks;
public class ShopUpdateCoins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _goldenBaldCoinsTxt, _presiCoinsTxt;
    async void Start()
    {
        await Task.Yield();
        EventManager.SubscribeToEvent(Contains.SHOP_ON_BUY, UptadeCoinsShowed);

        PersistantData persistantData = Helpers.PersistantData;
        EventManager.TriggerEvent(Contains.SHOP_ON_BUY, persistantData.persistantDataSaved.presiCoins, persistantData.persistantDataSaved.goldenBaldCoins);
    }
    private void OnDestroy()
    {       
        EventManager.UnSubscribeToEvent(Contains.SHOP_ON_BUY, UptadeCoinsShowed);
    }
    void UptadeCoinsShowed(params object[] param)
    {
        _presiCoinsTxt.text = ((int)param[0]).ToString();
        _goldenBaldCoinsTxt.text = ((int)param[1]).ToString();
    }
}
