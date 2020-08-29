using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherPressureplate : MonoBehaviour
{
    public LauncherScript attachedLauncher;

    bool canLaunch = true;

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.layer == 11 || other.gameObject.layer == 8) && canLaunch)
        {
            StartCoroutine(StartLaunch());
        }
    }

    IEnumerator StartLaunch()
    {
        transform.position -= Vector3.up * 0.4f;
        canLaunch = false;
        yield return new WaitForSeconds(0.25f);
        attachedLauncher.LaunchObject();

        yield return new WaitForSeconds(0.75f);
        transform.position += Vector3.up * 0.4f;
        yield return new WaitForSeconds(2f);
        canLaunch = true;
    }
}
