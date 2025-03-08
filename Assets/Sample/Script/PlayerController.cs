using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeedIn;//プレイヤーの移動速度を入力


    Rigidbody playerRb;//プレイヤーのRigidbody

    Vector3 moveSpeed;//プレイヤーの移動速度

    Vector3 currentPos;//プレイヤーの現在の位置
    Vector3 pastPos;//プレイヤーの過去の位置

    Vector3 delta;//プレイヤーの移動量

    Quaternion playerRot;//プレイヤーの進行方向を向くクォータニオン

    float currentAngularVelocity;//現在の回転各速度

    [SerializeField]
    float maxAngularVelocity = Mathf.Infinity;//最大の回転角速度[deg/s]

    [SerializeField]
    float smoothTime = 0.1f;//進行方向にかかるおおよその時間[s]

    float diffAngle;//現在の向きと進行方向の角度

    float rotAngle;//現在の回転する角度

    Quaternion nextRot;//どんくらい回転するか
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        pastPos = transform.position;
    }

    void Update()
    {
        //------プレイヤーの移動------

        //カメラに対して前と右を取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        //moveVelocityを0で初期化する
        moveSpeed = Vector3.zero;

        //移動入力の省略形
        if (Input.GetKey(KeyCode.W)) moveSpeed += moveSpeedIn * cameraForward;
        if (Input.GetKey(KeyCode.A)) moveSpeed -= moveSpeedIn * cameraRight;
        if (Input.GetKey(KeyCode.S)) moveSpeed -= moveSpeedIn * cameraForward;
        if (Input.GetKey(KeyCode.D)) moveSpeed += moveSpeedIn * cameraRight;

        //Moveメソッドで、力加えてもらう
        Move();


        //------プレイヤーの回転------
        //現在の位置
        currentPos = transform.position;

        //移動量計算
        delta = currentPos - pastPos;
        delta.y = 0;

        //過去の位置の更新
        pastPos = currentPos;


        // もし静止していたら、カメラの向きに揃える
        if (delta == Vector3.zero) {
        transform.rotation = Quaternion.LookRotation(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)), Vector3.up);
        return;
        }
        // 移動しているときは、移動方向を向く
        /*Quaternion targetRotation = Quaternion.LookRotation(moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);*/
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

