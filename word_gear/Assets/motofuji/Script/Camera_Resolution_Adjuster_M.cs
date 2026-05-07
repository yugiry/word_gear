using UnityEngine;

public class Camera_Resolution_Adjuster_M : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera; //対象とするカメラ
    [SerializeField]
    private Vector2 aspectVec; //目的解像度

    void Update()
    {
        AdjustCamera();
    }

    //画像サイズを調整する
    void AdjustCamera()
    {
        float targetAspect = aspectVec.x / aspectVec.y;
        float currentAspect = (float)Screen.width / Screen.height;

        var magRate = targetAspect / currentAspect;
        var viewportRect = new Rect(0, 0, 1, 1);
        if (magRate < 1)
        {
            viewportRect.width = magRate; //使用する横幅を変更
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
        }
        else
        {
            viewportRect.height = 1 / magRate; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央寄せ
        }

        targetCamera.rect = viewportRect; //カメラのViewportに適用

        //float baseOrthoSize = referenceHeight / 200f;

        //Camera cam = Camera.main;

        //if (currentAspect >= targetAspect)
        //{
        //    // 横が広い → 縦を合わせる
        //    float scale = targetAspect / currentAspect;
        //    cam.orthographicSize = baseOrthoSize * scale;
        //}
        //else
        //{
        //    // 縦が長い → 横を合わせる
        //    float scale = targetAspect / currentAspect;
        //    cam.orthographicSize = baseOrthoSize * scale;
        //}
    }
}
