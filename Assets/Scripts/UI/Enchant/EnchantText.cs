using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantText : MonoBehaviour
{
    public Enchant enchant;
    public void NegativeText()
    {
        gameObject.SetActive(false);
        enchant.EnchantSelectTextOn(0);
    }
}
