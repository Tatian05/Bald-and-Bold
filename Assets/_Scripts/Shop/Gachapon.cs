using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class Gachapon : MonoBehaviour
{

    [SerializeField] int _gachaCost = 1;
    [SerializeField] GameObject _uiCostGO;
    [SerializeField] TextMeshProUGUI _gachaCostTxt;
    [SerializeField] GachaBallAnimationScrip _gachaBall;

    [Header("Lists")]
    [SerializeField] ShoppableSO[] _allGachaShoppables;
    [SerializeField] List<ShoppableSO> _normalQualityShoppables, _rareQualityShoppables, _epicQualityShoppables;

    Dictionary<List<ShoppableSO>, int> _gachaDictionary = new Dictionary<List<ShoppableSO>, int>();
    PersistantData _persistantData;
    Animator _animator;
    ShoppableSO _gachaShoppable;

    public bool CanGacha => _persistantData.persistantDataSaved.goldenBaldCoins >= _gachaCost;
    private void Start()
    {
        _persistantData = Helpers.PersistantData;
        _animator = GetComponent<Animator>();
        _allGachaShoppables = _persistantData.allShoppables;
        UpdateLists();
        ShopWindow.UpdateCollList += UpdateListOnShop;
        _gachaCostTxt.text = _gachaCost.ToString();
    }
    void UpdateListOnShop() => Invoke(nameof(UpdateLists), .1f);
    private void OnDestroy()
    {
        ShopWindow.UpdateCollList -= UpdateListOnShop;
    }
    public void Gacha()
    {
        _gachaShoppable = RWS(_gachaDictionary);
        Debug.Log($"{_gachaShoppable.name} - {_gachaShoppable.shoppableQuality}");
        if (_gachaShoppable is CosmeticData)
        {
            var cosmetic = _gachaShoppable as CosmeticData;
            _persistantData.AddShoppableToCollection(cosmetic);
        }
        else if(_gachaShoppable is ConsumableData)
        {
            var consumable = _gachaShoppable as ConsumableData;
            _persistantData.AddShoppableToCollection(consumable);
        }
        else if(_gachaShoppable is WeaponSkinData)
        {
            var consumable = _gachaShoppable as WeaponSkinData;
            _persistantData.AddShoppableToCollection(consumable);
        }
        else
        {
            var bullet = _gachaShoppable as BulletData;
            _persistantData.AddShoppableToCollection(bullet);
        }
        _gachaShoppable.OnStart();

        _persistantData.persistantDataSaved.Gacha(_gachaCost);
        _animator.Play("GachaAnim");
        UpdateLists();
        EventManager.TriggerEvent(Contains.SHOP_ON_BUY, _persistantData.persistantDataSaved.presiCoins, _persistantData.persistantDataSaved.goldenBaldCoins);
    }
    void UpdateLists()
    {
        _gachaDictionary = new Dictionary<List<ShoppableSO>, int>();
        var filterColl = _allGachaShoppables.Except(_persistantData.CosmeticsInCollection);
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

    //LO LLAMO POR ANIMACION
    public void PlayGachaBall()
    {
        _gachaBall.gameObject.SetActive(true);
        _gachaBall.PlayGachaBallAnim(_gachaShoppable);
    }
}
