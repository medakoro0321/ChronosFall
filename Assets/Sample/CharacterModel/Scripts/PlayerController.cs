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
    public float SetMoveSpeed = 5f;//プレイヤーの移動速度入力
    public float MoveCurrSpeed;//プレイヤーの移動速度を保存
    public Vector3 MoveSpeed;//プレイヤーの座標軸移動速度
    public Rigidbody playerRb;//プレイヤーのRigidbody

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //最初に代入
        MoveCurrSpeed = SetMoveSpeed;
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
        MoveSpeed = Vector3.zero;

        //移動入力の省略形
        if (Input.GetKey(KeyCode.W)) MoveSpeed += MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.A)) MoveSpeed -= MoveCurrSpeed * cameraRight;
        if (Input.GetKey(KeyCode.S)) MoveSpeed -= MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.D)) MoveSpeed += MoveCurrSpeed * cameraRight;
        //Rキーでダッシュ(Rキーを押している間移動速度を2倍に変更、してない場合通常に変更)
        if (Input.GetKey(KeyCode.R)) MoveCurrSpeed = SetMoveSpeed * 2; else MoveCurrSpeed = SetMoveSpeed;
        //攻撃
        //左クリック(値:0)するたびに攻撃
        if (Input.GetMouseButtonDown(0)) Attack();

        //Moveメソッドで、力加えてもらう
        Move();
    }
    /// <summary>
    /// 移動方向に力を加える
    /// </summary>
    private void Move()
    {
        //playerRb.AddForce(MoveSpeed, ForceMode.Force);

        playerRb.linearVelocity = MoveSpeed;
    }
    /// <summary>
    /// 左クリックするたびに呼び出し
    /// </summary>
    private void Attack()
    {
        GameObject[] allModels = GameObject.FindGameObjectsWithTag("Models");
        foreach (GameObject model in allModels)
        {
            float AttackDistance = Vector3.Distance(transform.position, model.transform.position);

            //直線距離で3m以内に敵がいるかどうか
            if (AttackDistance <= 3f)
            {
                //50-攻撃距離*10の範囲でダメージを与える
                int MaxAttackDamage = (int)(50 - AttackDistance * 10);
                int Damage = Random.Range(1, MaxAttackDamage);
                Debug.Log("敵に" + Damage + "/" + MaxAttackDamage + "ダメージを与えた");
            }
        }
    }
}

