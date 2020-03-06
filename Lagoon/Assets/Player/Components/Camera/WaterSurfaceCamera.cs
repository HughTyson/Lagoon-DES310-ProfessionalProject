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
    int renderTextureID = Shader.PropertyToID("_Texture_SurfaceDepth");
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
        renderTexture.Release();
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }


    int screenWidth;
    int screenHeight;
    RenderTexture renderTexture;
    void Start()
    {
        waterMat = waterRenderer.material;
        renderTexture = new RenderTexture(playerCamera.pixelWidth,playerCamera.pixelHeight, 24, RenderTextureFormat.Depth);
        screenWidth = playerCamera.pixelWidth;
        screenHeight = playerCamera.pixelHeight;



        thisCamera = GetComponent<Camera>();
        thisCamera.targetTexture = renderTexture;
        thisCamera.depthTextureMode = DepthTextureMode.Depth;
     

    }



    // Update is called once per frame
    void Update()
    {
        if (playerCamera.pixelWidth != screenWidth || playerCamera.pixelHeight != screenHeight)
        {
            screenHeight = playerCamera.pixelHeight;
            screenWidth = playerCamera.pixelWidth;
            renderTexture.Release();
            renderTexture = new RenderTexture(screenWidth, screenHeight, 24, RenderTextureFormat.Depth);
            thisCamera.targetTexture = renderTexture;
        }
    }

    private void OnPostRender()
    {
        waterMat.SetTexture(renderTextureID, renderTexture);
        waterMat.SetFloat(farPlane, thisCamera.farClipPlane);
        waterMat.SetFloat(mearPlane, thisCamera.nearClipPlane);

    }
}

