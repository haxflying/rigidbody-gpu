﻿using UnityEngine;
using System.Collections;

public class TextureExample : MonoBehaviour
{
    public ComputeShader shader, shaderCopy;

    RenderTexture tex, texCopy;

    private int textureSize = 1024;
    private int threadGroupCount = 32;

    void Start()
    {
        tex = new RenderTexture(textureSize, textureSize, 0);
        tex.enableRandomWrite = true;
        tex.Create();

        texCopy = new RenderTexture(textureSize, textureSize, 0);
        texCopy.enableRandomWrite = true;
        texCopy.Create();

        float value = Random.value;
        Debug.Log(value);
        shader.SetFloat("seed", value);
        shader.SetTexture(0, "tex", tex);
        shader.Dispatch(0, threadGroupCount, threadGroupCount, 1);

        shaderCopy.SetTexture(0, "tex", tex);
        shaderCopy.SetTexture(0, "texCopy", texCopy);
        shaderCopy.Dispatch(0, threadGroupCount, threadGroupCount, 1);
    }

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), texCopy);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        {
            Swap(ref tex, ref texCopy);
            shaderCopy.SetTexture(0, "tex", tex);
            shaderCopy.SetTexture(0, "texCopy", texCopy);
            shaderCopy.Dispatch(0, texCopy.width / 8, texCopy.height / 8, 1);
        }
    }

    void Swap(ref RenderTexture a, ref RenderTexture b)
    {
        RenderTexture tmp = a;
        a = b;
        b = tmp;
    }

    void OnDestroy()
    {
        tex.Release();
        texCopy.Release();
    }
}