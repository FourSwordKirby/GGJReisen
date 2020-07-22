using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeidonteiInitializer : MonoBehaviour
{
    public GameObject GeidonteiSign;
    public GameObject GeidonteiBase;
    public GameObject GeidonteiPartial;
    public GameObject GeidonteiFull;

    // Start is called before the first frame update
    void Start()
    {
        int shardCount = ReisenGameManager.instance.gameProgress.Player.ShardsAcquired.Count;

        if (shardCount >= 5)
        {
            GeidonteiSign.SetActive(true);
        }

        if (0 <= shardCount && shardCount < 10)
        {
            GeidonteiBase.SetActive(true);
            GeidonteiPartial.SetActive(false);
            GeidonteiFull.SetActive(false);
        }
        else if (10 <= shardCount && shardCount < 20)
        {
            GeidonteiBase.SetActive(false);
            GeidonteiPartial.SetActive(true);
            GeidonteiFull.SetActive(false);
        }
        else if (20 <= shardCount)
        {
            GeidonteiBase.SetActive(false);
            GeidonteiPartial.SetActive(false);
            GeidonteiFull.SetActive(true);
        }
    }
}
