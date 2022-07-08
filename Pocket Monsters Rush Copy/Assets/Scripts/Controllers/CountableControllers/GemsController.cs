using UnityEngine;

public class GemsController : CountableObjectsController<GemsController>
{
    public override int GetCount()
    {
        return PlayerPrefs.GetInt("Gems");
    }

    protected override void SetCount(int count)
    {
        PlayerPrefs.SetInt("Gems", count);
        ShowCount();
    }
}
