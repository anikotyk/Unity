
public class PockeballEgg : PickUpObject
{
    protected override void ActionOnPickUp()
    {
        PockeballEggsController.Instance.IncreaseCount(1);
    }
}
