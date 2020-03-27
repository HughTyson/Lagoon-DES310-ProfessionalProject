using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class HDRP_Unlit_ManualAnimator : MonoBehaviour
{


    HDRP_SpriteSheet spriteSheet;
    HDRP_SpriteSheet.AnimationFrames frames;

    Material myMaterial;
    MeshRenderer myRenderer;
    int meshTextureID = Shader.PropertyToID("_UnlitColorMap");

    
    public int TotalFrames
    {
        get
        {
            if (frames != null)
            {
                return frames.FrameIndexes.Count;
            }
            return 0;
        }
    
    }



    Material MyMaterial
    {
        get
        {
            if (myMaterial == null)
            {
                myMaterial = MyRenderer.material;

            }
            return myMaterial;
        }
    }
    MeshRenderer MyRenderer
    {
        get
        {
            if (myRenderer == null)
            {
                myRenderer = GetComponent<MeshRenderer>();

            }
            return myRenderer;
        }
    }



    void SetSpriteSheet(HDRP_SpriteSheet sprite_sheet)
    {
        spriteSheet = sprite_sheet;
        MyMaterial.SetTexture(meshTextureID, spriteSheet.SpriteSheetTexture);
        MyMaterial.SetTextureScale(meshTextureID, spriteSheet.Offset);
    }


    bool flipX = false;
    bool flipY = false;

    int currentFrame = 0;
    public int CurrentFrame => currentFrame;
    public void SetAnimation(HDRP_SpriteSheet.AnimationFrames frames_)
    {
        frames = frames_;
        if (frames.SpriteSheet != spriteSheet)
            SetSpriteSheet(frames.SpriteSheet);
        SetFrame(0);
        Show();
    }

    public bool FlipX
    {
        get 
        {
            return flipX;
        }
        set
        {
            flipX = value;
            SetFrame(currentFrame);
        }
    }
    public bool FlipY
    {
        get
        {
            return flipY;
        }
        set
        {
            flipY = value;
            SetFrame(currentFrame);
        }
    }

    public void SetFrame(int frameIndex)
    {
        currentFrame = frameIndex;
        Vector2 offset = (spriteSheet.Offset * (frames.FrameIndexes[frameIndex]));

        if (flipX)
            offset.x += 1;
        if (flipY)
            offset.y += 1;

        MyMaterial.SetTextureOffset(meshTextureID, offset);
    }


    public void Hide()
    {
        MyRenderer.enabled = false;
    }
    public void Show()
    {
        MyRenderer.enabled = true;
    }
}
