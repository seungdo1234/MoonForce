using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumber : MonoBehaviour
{
    public int magicNumber;

    public void AnimationEnd()
    {
        gameObject.SetActive(false); 
    }
}
