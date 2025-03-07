using UnityEngine;

public class ModelWalk : MonoBehaviour
{
    public float Speed;
    float x, z;
    Rigidbody rb;
    Vector3 moving;
    /// <summary>
    /// 開始時に読み込まれる
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// フレームごとに呼び出される
    /// </summary>
    void Update()
    {
        // キャラクターの移動
        //Input.GetAxis("Horizontal")はキーの入力を取得する
        //縦
        x = Input.GetAxis("Horizontal");
        //横
        z = Input.GetAxis("Vertical");

        moving = new Vector3(x, 0, z);
        //normalized : 斜めのベクトルを縦/横のベクトルと長さを同じにする
        moving = moving.normalized * Speed;
        //移動
        rb.linearVelocity = moving;
    }
}
