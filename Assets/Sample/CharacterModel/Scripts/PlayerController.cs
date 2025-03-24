using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject CameraObject;

    public float CameraCurrRotate_Y;
    public Animator animator;// Animator コンポーネント

    //===プレイヤー移動速度系統===
    public float SetMoveSpeed = 5f;//プレイヤーの移動速度入力
    public float MoveCurrSpeed;//プレイヤーの移動速度を保存
    public Vector3 MoveSpeedAxis;//プレイヤーの座標軸移動速度
    public Rigidbody playerRb;//プレイヤーのRigidbody

    [SerializeField] public float TotalSpeedAxis;//アニメーションに適用する移動速度

    public float HP = 100f; //HP
    void Start()
    {
        // アニメーターコンポーネント取得
        animator = GetComponent<Animator>();
        //プレイヤーのRigidbodyを取得
        playerRb = GetComponent<Rigidbody>();
        //最初に代入
        MoveCurrSpeed = SetMoveSpeed;
        // AnimatorControllerが設定されているか確認
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("AnimatorControllerが設定されていません");
        }
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
        MoveSpeedAxis = Vector3.zero;

        //移動入力の省略形
        if (Input.GetKey(KeyCode.W)) MoveSpeedAxis += MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.A)) MoveSpeedAxis -= MoveCurrSpeed * cameraRight;
        if (Input.GetKey(KeyCode.S)) MoveSpeedAxis -= MoveCurrSpeed * cameraForward;
        if (Input.GetKey(KeyCode.D)) MoveSpeedAxis += MoveCurrSpeed * cameraRight;
        //Rキーでダッシュ(Rキーを押している間移動速度を2倍に変更、してない場合通常に変更)
        if (Input.GetKey(KeyCode.R)) MoveCurrSpeed = SetMoveSpeed * 2; else MoveCurrSpeed = SetMoveSpeed;
        //攻撃
        //左クリック(値:0)するたびに攻撃
        if (Input.GetMouseButtonDown(0)) Attack();

        //Moveメソッドで、力加えてもらう
        Move();
    }
    /// <summary>
    /// 移動方向に力を加える（重力対応）
    /// </summary>
    private void Move()
    {
        // 現在のY軸の速度を保存
        float CurrY = playerRb.linearVelocity.y;

        // X/Z軸の移動速度を適用
        playerRb.linearVelocity = new Vector3(MoveSpeedAxis.x, CurrY, MoveSpeedAxis.z);
        // アニメーション用のパラメータ計算
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            float moveAmount = MoveSpeedAxis.magnitude;

            // 動きがある場合（閾値を小さくして感度を上げる）
            if (moveAmount > 0.01f)
            {
                // アニメーション速度を計算
                float Animation_Speed = 1f;
                // スケーリング
                if (MoveCurrSpeed > SetMoveSpeed)
                {
                    Animation_Speed = 2f;
                }
                // BlendTreeに渡すパラメータ値を設定
                animator.SetFloat("Speed_Zero", 0);
                animator.SetFloat("Animation_Speed", Animation_Speed);
            }
            else
            {
                // 動いていない場合は0を設定し、アニメーションを停止させない
                animator.SetFloat("Speed_Zero", 0);
                animator.SetFloat("Animation_Speed", 0);
                animator.speed = 1.0f;  // アニメーション速度を通常に戻す
            }
        }
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
                int Damage = UnityEngine.Random.Range(1, MaxAttackDamage);
                Debug.Log("敵に" + Damage + "/" + MaxAttackDamage + "ダメージを与えた");
                //敵のHPを減らす
                model.GetComponent<EnemyWalkModel>().HP -= Damage;
            }
        }
    }
}

