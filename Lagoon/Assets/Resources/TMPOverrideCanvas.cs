using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class TMPOverrideCanvas : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;


    private void Awake()
    {
   //     RenderPipelineManager.beginCameraRendering += onRender;
     //   RenderPipelineManager.endCameraRendering += onRender;
    }
    private void onRender(ScriptableRenderContext t, Camera c)
    {
    //    canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
    }
}
