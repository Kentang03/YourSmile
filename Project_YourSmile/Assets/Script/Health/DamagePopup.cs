using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    //public static DamagePopup Create() 
    //{
    //    Transform damagePopupTransform = Instantiate
    //}

    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void TextSetup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
    }
}
