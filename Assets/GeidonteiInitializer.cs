using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int shardCount = ReisenGameManager.instance.gameProgress.Player.ShardsAcquired.Sum(x => x.ShardValue);

        GeidonteiSign.SetActive(shardCount >= 5);


        if (0 <= shardCount && shardCount < 10)
        {
            GeidonteiBase.SetActive(true);
            GeidonteiPartial.SetActive(false);
            GeidonteiFull.SetActive(false);
        }
        else if (10 <= shardCount && shardCount < 15)
        {
            GeidonteiBase.SetActive(false);
            GeidonteiPartial.SetActive(true);
            GeidonteiFull.SetActive(false);
        }
        else if (15 <= shardCount)
        {
            GeidonteiBase.SetActive(false);
            GeidonteiPartial.SetActive(false);
            GeidonteiFull.SetActive(true);
        }
    }
}
