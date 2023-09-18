public class PS_EnemyBloodExplode : Particle
{
    public override void ReturnObject()
    {
        FRY_EnemyBloodExplodeParticle.Instance.ReturnObject(this);
    }
}
