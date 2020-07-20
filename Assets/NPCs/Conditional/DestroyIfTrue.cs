using System.Collections;
using UnityEngine;

public class DestroyIfTrue : MonoBehaviour
{
    public ReisenGameProgress GameProgress => ReisenGameManager.instance?.gameProgress;

    // Start is called before the first frame update
    void Start()
    {
        CheckAndDestroy();
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        CheckAndDestroy();
    }

    public void CheckAndDestroy()
    {
        if (CheckCondition())
        {
            Destroy(this.gameObject);
        }
    }

    public virtual bool CheckCondition()
    {
        return false;
    }
}
