using System.Security;
using UnityEngine;

public class EnemyWalkModel : MonoBehaviour
{
    public Transform target; // プレイヤーのTransform
    public float moveSpeed = 3f; // 速度
    private Rigidbody enemyRb;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target == null) return;

        // プレイヤーの方向を計算（高さは無視）
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y軸の変化をなくす（地面に沿って移動）

        // 移動
        enemyRb.linearVelocity = direction * moveSpeed;

        // プレイヤーの方向を向く
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }
}
