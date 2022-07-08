
using UnityEngine;

public class PockeballEggsController : CountableObjectsController<PockeballEggsController>
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
