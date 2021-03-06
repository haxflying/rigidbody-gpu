﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Conway's Game Of Life.
/// </summary>
public class TextureExample : MonoBehaviour
{
    public ComputeShader shader, shaderCopy;

    private RenderTexture tex, texCopy;

    private int textureSize = 1024;
    private int threadGroupCount = 128;

    private void Start()
    {
        tex = new RenderTexture(textureSize, textureSize, 0);
        tex.enableRandomWrite = true;
        tex.Create();

        texCopy = new RenderTexture(textureSize, textureSize, 0);
        texCopy.enableRandomWrite = true;
        texCopy.Create();

        shader.SetTexture(0, "tex", tex);
        shader.Dispatch(0, threadGroupCount, threadGroupCount, 1);

        shaderCopy.SetTexture(0, "tex", tex);
        shaderCopy.SetTexture(0, "texCopy", texCopy);
        shaderCopy.Dispatch(0, threadGroupCount, threadGroupCount, 1);
    }

    private void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), texCopy);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        {
            Swap(ref tex, ref texCopy);
            shaderCopy.SetTexture(0, "tex", tex);
            shaderCopy.SetTexture(0, "texCopy", texCopy);
            shaderCopy.Dispatch(0, texCopy.width / 8, texCopy.height / 8, 1);
        }
    }

    private void Swap(ref RenderTexture a, ref RenderTexture b)
    {
        RenderTexture tmp = a;
        a = b;
        b = tmp;
    }

    private void OnDestroy()
    {
        tex.Release();
        texCopy.Release();
    }
}