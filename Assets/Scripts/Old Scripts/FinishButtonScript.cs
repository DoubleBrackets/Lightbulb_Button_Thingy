using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishButtonScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MoveableObject"))
        {
            gameObject.transform.localScale = new Vector3(1, 1, 0.5f);
            LightbulbEffectScript.lightbulbEffectScript.LevelComplete();
        }
    }
}
