using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniGameSystem : MonoBehaviour
{
    public Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
}
