using System.Security;
using UnityEngine;
using System.Collections;

public class EnemyWalkModel : MonoBehaviour
{
    public Transform target; // プレイヤーのTransform
    public float MoveSpeed = 3f; // 速度
    private Rigidbody enemyRb;
    public float HP = 100f; //HP
    private bool isAttacking = false; // 攻撃中かどうかのフラグ
    public Vector3 CurrSpeedAxis ; //敵の座標軸移動速度
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        HP = Random.Range(50, 200);
    }

    void Update()
    {
        if (target == null) return;

        // プレイヤーの方向を計算（高さは無視）
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y軸の変化をなくす（地面に沿って移動）

        // 移動
        CurrSpeedAxis = direction * MoveSpeed;
        enemyRb.linearVelocity = new Vector3(CurrSpeedAxis.x, enemyRb.linearVelocity.y, CurrSpeedAxis.z);

        // プレイヤーの方向を向く
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        // HPが0以下になったら消滅
        if (HP <= 0)
        {
            //将来的に、ラグドールを導入予定
            //https://github.com/medakoro0321/ChronosFall/issues/14 [#14]
            Destroy(gameObject);
        }

        // 周辺3m以内にプレイヤーがいたら攻撃
        if (Vector3.Distance(target.position, transform.position) < 3f && !isAttacking)
        {
            //Cotroutineを使ってAttack()を呼び出す
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private IEnumerator Attack()
    {
        // 攻撃中フラグを立てる
        isAttacking = true;
        // 1~5秒待つ（秒単位に変更）
        int waitTime = Random.Range(1, 5);
        yield return new WaitForSeconds(waitTime);
        // 途中でプレイヤーが消えた場合は中断
        if (target == null) yield break;

        GameObject main_character = GameObject.Find("example_character");
        float AttackDistance = Vector3.Distance(transform.position, main_character.transform.position);
        int Damage = (int)(50 - AttackDistance * 10);

        //キャラにダメージを与える
        target.GetComponent<PlayerController>().HP -= Damage;
        Debug.Log("敵が " + Damage + " ダメージを与えた！");
        // 攻撃終了フラグを解除
        isAttacking = false;
    }

}
