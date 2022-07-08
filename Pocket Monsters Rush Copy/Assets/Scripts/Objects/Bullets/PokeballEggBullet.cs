
public class PokeballEggBullet : Bullet
{
    protected override void OnReachedTarget()
    {
        base.OnReachedTarget();
        _target.GetComponent<Pokemon>().GetDamage(_bulletDamage);
    }
}
