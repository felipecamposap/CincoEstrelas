using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReplaceShader : MonoBehaviour
{
    [SerializeField] private Camera targetcamera;
    // Start is called before the first frame update
    void Start()
    {
        targetcamera.SetReplacementShader(Shader.Find("Unlit/Color"),"RenderType");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
