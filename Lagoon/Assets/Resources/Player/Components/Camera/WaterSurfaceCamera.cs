using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterSurfaceCamera : MonoBehaviour
{

    [SerializeField] MeshRenderer waterRenderer;
    [SerializeField] Camera playerCamera;
    // Start is called before the first frame update

    Camera thisCamera;
    int renderTextureDepth_ID = Shader.PropertyToID("_Texture_Depth");
    int renderTextureColour_ID = Shader.PropertyToID("_Texture_Colour");

    int mearPlane = Shader.PropertyToID("_NearPlane");
    int farPlane = Shader.PropertyToID("_FarPlane");
    Material waterMat;

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        renderTexture_Depth.Release();
        renderTexture_Colour.Release();
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }


    int screenWidth;
    int screenHeight;
    RenderTexture renderTexture_Depth;
    RenderTexture renderTexture_Colour;
    void Start()
    {

        thisCamera = GetComponent<Camera>();
        thisCamera.forceIntoRenderTexture = true;

        waterMat = waterRenderer.material;
        renderTexture_Depth = new RenderTexture(playerCamera.pixelWidth,playerCamera.pixelHeight, 24, RenderTextureFormat.Depth);
        renderTexture_Colour = new RenderTexture(playerCamera.pixelWidth, playerCamera.pixelHeight, 0, RenderTextureFormat.DefaultHDR);
        waterMat.SetTexture(renderTextureDepth_ID, renderTexture_Depth);
        waterMat.SetTexture(renderTextureColour_ID, renderTexture_Colour);

        screenWidth = playerCamera.pixelWidth;
        screenHeight = playerCamera.pixelHeight;


        thisCamera.depthTextureMode = DepthTextureMode.Depth;


    }



    // Update is called once per frame
    void Update()
    {
        if (playerCamera.pixelWidth != screenWidth || playerCamera.pixelHeight != screenHeight)
        {
            screenHeight = playerCamera.pixelHeight;
            screenWidth = playerCamera.pixelWidth;
            renderTexture_Depth.Release();
            renderTexture_Depth = new RenderTexture(screenWidth, screenHeight, 24, RenderTextureFormat.Depth);
            renderTexture_Colour.Release();
            renderTexture_Colour = new RenderTexture(screenWidth, screenHeight, 0, RenderTextureFormat.DefaultHDR);

            waterMat.SetTexture(renderTextureDepth_ID, renderTexture_Depth);
            waterMat.SetTexture(renderTextureColour_ID, renderTexture_Colour);

        }



        thisCamera.targetTexture = renderTexture_Depth;
        thisCamera.Render();
        thisCamera.targetTexture = renderTexture_Colour;
        thisCamera.Render();
    }

    private void OnPostRender()
    {

    }


}

