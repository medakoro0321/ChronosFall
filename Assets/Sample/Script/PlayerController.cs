using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject CameraObject;
    
    public float CameraCurrRotate_Y;
    void Start()
    {
       
    }

    void Update()
    {
        //カメラの向いてる向きを取得
        CameraCurrRotate_Y = CameraObject.transform.eulerAngles.y;
        //モデルにカメラの向いてる向きをY座標軸のみ同期
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, CameraCurrRotate_Y, transform.eulerAngles.z);
    }
}

