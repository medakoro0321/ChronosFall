using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject CameraObject;

    public float CameraCurrRotate_Y;
    //===プレイヤー移動速度系統===
    public float SetMoveSpeed;//プレイヤーの移動速度入力
    public float MoveCurrSpeed;//プレイヤーの移動速度を保存
    public Vector3 moveSpeed;//プレイヤーの座標軸移動速度
    public Rigidbody playerRb;//プレイヤーのRigidbody

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //カメラの向いてる向きを取得
        CameraCurrRotate_Y = CameraObject.transform.eulerAngles.y;
        //モデルにカメラの向いてる向きをY座標軸のみ同期
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, CameraCurrRotate_Y, transform.eulerAngles.z);

        //------プレイヤーの移動------

        //カメラに対して前と右を取得
        Vector3 cameraForward = Vector3.Scale(CameraObject.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(CameraObject.transform.right, new Vector3(1, 0, 1)).normalized;

        //moveVelocityを0で初期化する
        moveSpeed = Vector3.zero;

        //移動入力の省略形
        if (Input.GetKey(KeyCode.W)) moveSpeed += MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.A)) moveSpeed -= MoveCurrSpeed * cameraRight;
        if (Input.GetKey(KeyCode.S)) moveSpeed -= MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.D)) moveSpeed += MoveCurrSpeed * cameraRight;
        //Rキーでダッシュ(Rキーを押している間移動速度を2倍に変更、してない場合通常に変更)
        if (Input.GetKey(KeyCode.R)) MoveCurrSpeed = SetMoveSpeed * 2; else MoveCurrSpeed = SetMoveSpeed;

        //Moveメソッドで、力加えてもらう
        Move();
    }
    /// <summary>
    /// 移動方向に力を加える
    /// </summary>
    private void Move()
    {
        //playerRb.AddForce(moveSpeed, ForceMode.Force);

        playerRb.linearVelocity = moveSpeed;
    }
}

