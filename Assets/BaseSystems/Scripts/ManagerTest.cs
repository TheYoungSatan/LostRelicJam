using System.Collections;
using UnityEngine;

public class ManagerTest : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        BaseMainManager.INSTANCE.PlaySFX("Error", true);
        BaseMainManager.INSTANCE.PlaySFX("Environment");
        yield return new WaitForSeconds(5f);
        BaseMainManager.INSTANCE.PlaySFX("Confirm", true);
        BaseMainManager.INSTANCE.Pause();
        yield return new WaitForSeconds(2f);
        BaseMainManager.INSTANCE.UnPause();
    }
}
