using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixMaterial6 : MonoBehaviour
{ 
    [SerializeField] private Material material;

    void Start()
    {
        material.SetColor("_Color", Color.white); 
        float height = material.GetFloat("_MixValue"); 
    }
}
