using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [Header("Shader variables")]
    [SerializeField] float windSpeed;

    [Header("Materials")]
    [SerializeField] Material[] materials;

    void Update()
    {
        foreach (Material material in materials)
        {
            if (material.HasFloat("_WindSpeed"))
            {
                material.SetFloat("_WindSpeed", windSpeed);
            }
        }
    }
}
