using UnityEngine;

public class SavePointSceneFunctions : MonoBehaviour
{
    public void School_ReleaseEntranceBlocks()
    {
        Destroy(GameObject.Find("WestEntranceBlockUntilSavePoint"));
        Destroy(GameObject.Find("EastEntranceBlockUntilSavePoint"));
    }
}
