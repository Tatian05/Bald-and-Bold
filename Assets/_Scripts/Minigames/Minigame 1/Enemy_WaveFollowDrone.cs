using UnityEngine;
using UnityEngine.AI;
public class Enemy_WaveFollowDrone : Enemy_FollowDrone
{
    [SerializeField] int _coinsDrop;
    public override void Die()
    {
        base.Die();
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + _coinsDrop);
        FRY_CoinFeedback.Instance.pool.GetObject().SetPosition(transform.position);
    }
    public override void ActionOnPlayerDead(params object[] param) { }
    public override void ReturnObject()
    {
        base.ReturnObject();
        gameManager.EnemyManager.RemoveEnemy(this);
    }
}

