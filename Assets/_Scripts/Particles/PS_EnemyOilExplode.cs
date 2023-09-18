public class PS_EnemyOilExplode : Particle
{
    public override void ReturnObject()
    {
        FRY_EnemyOilExplodeParticle.Instance.ReturnObject(this);
    }
}
