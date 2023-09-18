using UnityEngine;
using PickUps;
public class AmmoPickUp : PickUp
{
    [SerializeField] int amount;
    protected override void Start()
    {
        base.Start();
        amount = Random.Range(10, 30);
        _uiText = "x" + amount;
    }
}
