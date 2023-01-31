using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//製作者 下田祥己
//カメラコントローラー
public class CameraController : MonoBehaviour
{

 //----------------------------- 宣言部 ------------------------------------

    [SerializeField, Tooltip("StageManagerのスクリプト")]
    private StageManager stageManager;


    #region カメラの変更関連
    [Header("位置")]
    [SerializeField, Tooltip("カメラ位置")]
    private Vector3 cameraPos;

    [SerializeField, Tooltip("カメラの移動速度")]
    private float cameraPosSpeed;


    [Header("角度")]
    [SerializeField, Tooltip("カメラ角度")]
    private Quaternion cameraAngle;

    [SerializeField, Tooltip("カメラの角度変更速度")]
    private float cameraAngleSpeed;
    #endregion


    //----------------------------- 処理部 -------------------------------------

    private void FixedUpdate()
    {
        //ゲームクリア前はreturn
        if(!stageManager.IsGameClear())
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScene");
        }

        //ゲームクリア時の処理
        //カメラ位置を移動
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, cameraPos, cameraPosSpeed);

        //カメラ角度の変更
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, cameraAngle, cameraAngleSpeed);
    }
}
