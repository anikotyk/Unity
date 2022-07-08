
public class BattlePokemonBullet : Bullet
{
    protected override void OnReachedTarget()
    {
        base.OnReachedTarget();
        _target.GetComponent<PokemonBattle>().GetDamage(_bulletDamage);
    }
}
