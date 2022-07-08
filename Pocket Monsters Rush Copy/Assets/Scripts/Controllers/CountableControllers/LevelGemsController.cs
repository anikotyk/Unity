
using UnityEngine;

public class LevelGemsController : CountableObjectsController<LevelGemsController>
{
    private void OnEnable()
    {
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetController;
    }

    private void ResetController()
    {
        SetCount(0);
    }
}
