using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//製作者 下田祥己
//プレイヤーの移動(Script)
public class PlayerController : MonoBehaviour
{

 //----------------------------- 宣言部 ------------------------------------

    [SerializeField, Tooltip("StageManagerのスクリプト")]
    private StageManager stageManager;

    [SerializeField, Tooltip("PlayerAnimationのスクリプト")]
    private PlayerAnimation playerAnimation;

    #region 時間関連
    [SerializeField, Tooltip("待機時間")]
    private float intervalCount;

    //待機時間の計測
    private float intervalTime = 0;
    #endregion


    #region プレイヤー関連
    //プレイヤーの移動量と移動方向
    private Vector2 playerMoveVolume;

    //プレイヤーの角度
    private float playerAngle;
    #endregion


    #region　移動Enum
    MoveDirection moveDirection = MoveDirection.Wait;
    enum MoveDirection
    {
        Wait,//待機
        Up,//上移動
        Down,//下移動
        Left,//左移動
        Right,//右移動
        Interval//待機移行

    }
    #endregion



 //------------------------------- 処理部 -------------------------------------

    private void Update()
    {

        //ステージ更新中は移動できない
        if(stageManager.IsNewStageLoad())
        {
            return;
        }

        //Enterキーを押したとき
        if (Input.GetKeyDown(KeyCode.Return))
        {

            //現在のステージをside呼び出し
            stageManager.NowStageLoad();
        }


        #region 入力移動
        switch (moveDirection)
        {

            //待機
            case MoveDirection.Wait:

                // 入力移動
                //上移動
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    moveDirection = MoveDirection.Up;
                }

                //下移動
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    moveDirection = MoveDirection.Down;
                }

                //左移動
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    moveDirection = MoveDirection.Left;
                }

                //右移動
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    moveDirection = MoveDirection.Right;
                }

                break;
        
        }
        #endregion

    }

    private void FixedUpdate()
    {

        //移動Enum
        switch (moveDirection)
        {

            #region 移動処理
            //上移動
            case MoveDirection.Up:

                //プレイヤーの移動量と移動方向
                playerMoveVolume = new Vector2(0, 1);

                //プレイヤーの角度
                playerAngle = 0;

                //プレイヤーの移動メソッド
                PlayerMove();

                break;


            //下移動
            case MoveDirection.Down:

                //プレイヤーの移動量と移動方向
                playerMoveVolume = new Vector2(0, -1);

                //プレイヤーの角度
                playerAngle = 180;

                //プレイヤーの移動メソッド
                PlayerMove();

                break;


            //右移動
            case MoveDirection.Right:

                //プレイヤーの移動量と移動方向
                playerMoveVolume = new Vector2(1, 0);

                //プレイヤーの角度
                playerAngle = 90;

                //プレイヤーの移動メソッド
                PlayerMove();

                break;


            //左移動
            case MoveDirection.Left:

                //プレイヤーの移動量と移動方向
                playerMoveVolume = new Vector2(-1, 0);

                //プレイヤーの角度
                playerAngle = 270;

                //プレイヤーの移動メソッド
                PlayerMove();

                break;


            #endregion


            #region 移動後インターバル
            //待機移行
            //移動後のインターバル
            case MoveDirection.Interval:

                //計測
                intervalTime += Time.deltaTime;

                //計測後
                if(intervalTime >= intervalCount)
                {
                    intervalTime = 0;

                    //待機Enumに移行
                    moveDirection = MoveDirection.Wait;
                }

                break;
                #endregion

        }

    }


    //------------------------------- メソッド部 ----------------------------------

    #region 移動メソッド
    //プレイヤーの移動メソッド
    /// <summary>
    /// <para>プレイヤーの移動メソッド</para>
    /// </summary>
    private void PlayerMove()
    {
        
        //移動方向のオブジェクトを探索
        //移動方向に障害物がないなら移動ができる
        if (stageManager.IsPlayerMove(playerMoveVolume) == true)
        {

            //移動アニメーション開始
            playerAnimation.Jump_True_Animation();

            //移動
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + playerMoveVolume.x, 0,
                                                             this.gameObject.transform.position.z + playerMoveVolume.y);

        }

        //角度
        this.gameObject.transform.eulerAngles = new Vector3(this.gameObject.transform.rotation.x, playerAngle, this.gameObject.transform.rotation.z);

        //待機移行Enumに遷移
        moveDirection = MoveDirection.Interval;

    }
    #endregion

}
