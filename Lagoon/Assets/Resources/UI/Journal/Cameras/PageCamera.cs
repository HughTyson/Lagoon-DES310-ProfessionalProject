using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCamera : MonoBehaviour
{
    //[SerializeField] RenderTexture renderTextureToCopyFrom;

    //[SerializeField] GameObject pageCaameraSlavePrefab;
    //public class PageRendering
    //{
    //     RenderTexture renderTexture;
    //     GameObject gameObjectWithPageMesh;
    //     GameObject gameObjectOfCamera;
    //     Camera camera;
    //    //  static readonly int shaderBaseColourTextureID = Shader.PropertyToID("_RenderTexture");
    //      static readonly int shaderBaseColourTextureID = Shader.PropertyToID("_BaseColorMap");
    //    bool IsActive = false;
    //    public PageRendering(RenderTexture renderTexture_, GameObject gameObjectOfCamera_, GameObject gameObjectWithPageMesh_)
    //    {
    //        renderTexture = renderTexture_;
    //        gameObjectOfCamera = gameObjectOfCamera_;
    //        camera = gameObjectOfCamera.GetComponent<Camera>();

    //        camera.enabled = false;
    //        camera.forceIntoRenderTexture = true;
    //        camera.targetTexture = renderTexture;


    //        gameObjectWithPageMesh = gameObjectWithPageMesh_;

    //        gameObjectWithPageMesh.GetComponent<MeshRenderer>().material.SetTexture(shaderBaseColourTextureID, renderTexture);

    //        gameObjectWithPageMesh.SetActive(false);
    //        gameObjectOfCamera.SetActive(false);
    //    }

    //    public Vector3 CameraPosition => gameObjectOfCamera.transform.position;
    //    public Quaternion CameraRotation => gameObjectOfCamera.transform.rotation;
    //    public void Activate(Vector3 cameraPosition, Quaternion cameraRotation)
    //    {
    //        IsActive = true;
    //        gameObjectWithPageMesh.SetActive(true);
    //        gameObjectOfCamera.SetActive(true);

    //        gameObjectOfCamera.transform.rotation = cameraRotation;
    //        gameObjectOfCamera.transform.position = cameraPosition;
    //    }
    //    public void Show()
    //    {
    //        IsActive = true;
    //        gameObjectWithPageMesh.SetActive(true);
    //        gameObjectOfCamera.SetActive(true);
    //    }

    //    public void Deactivate()
    //    {
    //        IsActive = false;
    //        gameObjectWithPageMesh.SetActive(false);
    //        gameObjectOfCamera.SetActive(false);
    //    }
    //}

    //Dictionary<object, PageRendering> allRenderings = new Dictionary<object, PageRendering>();



    //public void CreatePageRendering(object key, GameObject gameObjectWithPageMesh)
    //{

    //    GameObject new_gameObject = Instantiate<GameObject>(pageCaameraSlavePrefab, gameObject.transform);

    //    PageRendering new_page_rendering = new PageRendering(new RenderTexture(renderTextureToCopyFrom), new_gameObject, gameObjectWithPageMesh);
    //    allRenderings.Add(key, new_page_rendering);
       

    //}

    //public void StartRenderingPage(object key, BasePagePair.PageInfo page)
    //{
    //    allRenderings[key].Activate(page.DefaultPosition - new Vector3(0,0,0.5f), page.DefaultRotation);
    //}

    //public void ShowRenderingPage(object key)
    //{
    //    allRenderings[key].Show();
    //}

    //public void StartRenderingPage(object to_render_key, object reference_key)
    //{
    //    PageRendering reference = allRenderings[reference_key];
    //    allRenderings[to_render_key].Activate(reference.CameraPosition, reference.CameraRotation);
    //}
    //public void StopRenderingPage(object key)
    //{
    //    allRenderings[key].Deactivate();
    //}
    //public void StopRenderingAll()
    //{
    //    foreach (KeyValuePair<object, PageRendering> entry in allRenderings)
    //    {
    //        entry.Value.Deactivate();
    //    }
    //}
}
