using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCameraSlave : MonoBehaviour
{
    // Start is called before the first frame update


    Coroutine renderRoutine;
    IEnumerator enumeratorRender;
    Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        enumeratorRender = RenderCamera();
        StartCoroutine(enumeratorRender);
    }


    IEnumerator RenderCamera()
    {
        while (true)
        {
            camera.Render();
            yield return new WaitForEndOfFrame();
        }
    }

}
