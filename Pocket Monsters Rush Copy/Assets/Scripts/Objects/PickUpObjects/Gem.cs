
public class Gem : PickUpObject
{
    protected override void ActionOnPickUp()
    {
        LevelGemsController.Instance.IncreaseCount(1);
    }
}
