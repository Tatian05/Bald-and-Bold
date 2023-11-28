using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Gachapon : MonoBehaviour
{
    [SerializeField] ShoppableSO[] _allGachaShoppables;
    [SerializeField] List<ShoppableSO> _normalQualityShoppables, _rareQualityShoppables, _epicQualityShoppables;

    Dictionary<List<ShoppableSO>, int> _gachaDictionary = new Dictionary<List<ShoppableSO>, int>();
    PersistantDataSaved _persistantDataSaved;
    private void Start()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        UpdateLists();
        ShopWindow.UpdateCollList += UpdateListOnShop;
    }
    void UpdateListOnShop() => Invoke(nameof(UpdateLists), .1f);
    private void OnDestroy()
    {
        ShopWindow.UpdateCollList -= UpdateListOnShop;
    }
    public void Gacha()
    {
        var shoppableGacha = RWS(_gachaDictionary);
        if (shoppableGacha is CosmeticData)
        {
            var cosmetic = shoppableGacha as CosmeticData;
            _persistantDataSaved.AddCosmetic(cosmetic.cosmeticType, cosmetic);
        }
        else if(shoppableGacha is ConsumableData)
        {
            var consumable = shoppableGacha as ConsumableData;
            _persistantDataSaved.AddConsumable(consumable);
        }
        else
        {
            var bullet = shoppableGacha as BulletData;
            _persistantDataSaved.AddBullet(bullet);
        }
        shoppableGacha.OnStart();

        Debug.Log($"{shoppableGacha.name} - {shoppableGacha.shoppableQuality}");
        UpdateLists();
    }
    void UpdateLists()
    {
        _persistantDataSaved.LoadCosmeticsCollection();
        _gachaDictionary = new Dictionary<List<ShoppableSO>, int>();
        var filterColl = _allGachaShoppables.Except(_persistantDataSaved.cosmeticsInCollection);
        _normalQualityShoppables = filterColl.Where(x => x.shoppableQuality == ShoppableQuality.Normal).ToList();
        _rareQualityShoppables = filterColl.Where(x => x.shoppableQuality == ShoppableQuality.Rare).ToList();
        _epicQualityShoppables = filterColl.Where(x => x.shoppableQuality == ShoppableQuality.Epic).ToList();

        _gachaDictionary.Add(_normalQualityShoppables, 60);
        _gachaDictionary.Add(_rareQualityShoppables, 25);
        _gachaDictionary.Add(_epicQualityShoppables, 15);
    }
    ShoppableSO RWS(Dictionary<List<ShoppableSO>, int> values)
    {
        float sum = 0;
        foreach (var item in values)
            sum += item.Value;

        float random = Random.Range(0f, 1f);
        float count = 0;
        foreach (var item in values)
        {
            count += item.Value / sum;
            if (count >= random)
            {
                int randomInColl = (int)Random.Range(0f, item.Key.Count);
                return item.Key[randomInColl];
            }
        }
        return default;
    }
}
