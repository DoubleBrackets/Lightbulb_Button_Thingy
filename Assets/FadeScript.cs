using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public static FadeScript fadeScript;

    Animator anim;

    private void Awake()
    {
        fadeScript = this;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        anim.SetBool("FadeIn", true);
        anim.SetBool("FadeOut", false);
    }
    public void FadeOut()
    {
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", true);
    }
}
