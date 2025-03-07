using UnityEngine;

public class CameraTPS : MonoBehaviour
{
    //メインカメラを取得する変数
    public GameObject cameraObject;
    //キャラクターを取得する変数
    public GameObject MainCharacter;


    /// <summary>
    /// 開始時に読み込まれる
    /// </summary>
    void Start() {
        // メインカメラを取得する
        cameraObject = GameObject.FindWithTag("MainCamera");
        // メインカメラを取得する
        MainCharacter = GameObject.FindWithTag("Phys_Haolan");
    }

    /// <summary>
    /// 0.02sごとに呼び出される
    /// </summary>
    void FixedUpdate()
    {
        // メインカメラの位置をキャラクターの位置に合わせる
        cameraObject.transform.position = MainCharacter.transform.position + new Vector3(0, 1.5f, 0);
    }
}
