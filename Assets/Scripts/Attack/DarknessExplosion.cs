using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessExplosion : MonoBehaviour
{
    public void AnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
