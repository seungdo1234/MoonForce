using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumber : MonoBehaviour
{
    public int magicNumber;

    public Vector3 resetScale;
    public bool isSizeUp;
    public void AnimationEnd()
    {
        gameObject.SetActive(false); 
    }

    public void CamaraShake(float shakeTime)
    {
        GameManager.instance.cameraShake.ShakeCamera(shakeTime, 5, 5);
    }
}
