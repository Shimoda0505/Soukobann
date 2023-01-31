using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//製作者 下田祥己
//倉庫番のステージ生成,管理(Script)
public class StageManager : MonoBehaviour
{

 //------------------------------- 宣言部 ------------------------------------

    #region プレイヤー関連
    [SerializeField, Tooltip("プレーヤー")]
    private GameObject _playerObj;

    //プレーヤーの位置を補完
    private Vector2 _playerArrayPos;

    //ステージ更新
    //新しいステージを生成している間,プレイヤーを動かさないためのフラグ
    private bool _isNewStageLoad = true;
    public bool IsNewStageLoad()
    {
        return _isNewStageLoad;
    }

    #endregion


    #region オブジェクト関連
    [SerializeField, Tooltip("オブジェクトプール")]
    private ObjectPool _objectPool;

    //動く箱の位置を補完
    private List<GameObject> _moveBLOCKArrayObjs = new List<GameObject>();

    //生成したオブジェクトを補完
    private GameObject _createObj;

    //MeshFilterのコンポーネント
    private MeshFilter _meshFilterComponent;

    //MeshRendererのコンポーネント
    private MeshRenderer _meshRendererComponent;


    //オブジェクトのメッシュ
    [Header("マテリアルを設定")]
    [SerializeField, Tooltip("壁のマテリアルデータ")]
    private Material[] _materialWallDatas;
    [SerializeField, Tooltip("動く箱のマテリアルデータ")]
    private Material[] _materialBLOCKDatas;
    [SerializeField, Tooltip("ゴールのマテリアルデータ")]
    private Material[] _materialGoalDatas;

    //オブジェクトのマテリアル
    [Header("メッシュを設定")]
    [SerializeField, Tooltip("箱のメッシュデータ")]
    private Mesh[] _meshWallDatas;
    [SerializeField, Tooltip("動く箱のメッシュデータ")]
    private Mesh[] _meshBLOCKDatas;
    [SerializeField, Tooltip("ゴールのメッシュデータ")]
    private Mesh[] _meshGoalDatas;

    //各オブジェクトにマテリアルとメッシュを貼る
    //分岐用Enum
    CubeNumber _cubeNumber;
    enum CubeNumber
    { 
        NULL,
        WALL,
        BLOCK,
        GOAL,
        PLAYER
    }
    #endregion


    #region ステージ関連
    [Header("ステージを設定")]
    [SerializeField, Tooltip("ステージのテキストデータ")]
    private TextAsset[] _stageDatas;

    //行数
    private string[] _stageLines;

    //列数
    private string[] _stageValues;

    //ステージ番号
    private int _stageNumber = 0;


    //縦横配列
    //0 = null , 1 = 壁 , 2 = 移動箱 , 3 = ゴール , 4 = プレーヤー , 5 = ゴールした場所
    private int[,] _stageArrays;

    //ステージ配列内の番号に振られている番号
    //Null
    private const int STAGE_NULL = 0;
    //壁
    private const int STAGE_WALL = 1;
    //移動箱
    private const int STAGE_MOVE_BLOCK = 2;
    //ゴール
    private const int STAGE_GOAL = 3;
    //プレイヤー
    private const int STAGE_PLAYER = 4;
    //ゴールした場所
    private const int STAGE_GOAL_POS = 5;
    #endregion


    #region ゴール関連
    //ゴール数の最大値
    private int _max_goalCount = 0;

    //ゴール数をカウント
    private int _goalCount = 0;

    //ゴールしたかどうか
    private bool _isGoal = false;

    //プレイヤーがゴールを踏んでいるかどうか
    private bool _isGoalStep = false;

    //ゲームクリア
    private bool _isGameClear = false;
    public bool IsGameClear()
    {
        return _isGameClear;
    }
    #endregion


 //------------------------------- 処理部 -------------------------------------

    private void Awake()
    {

        //ステージのテキストデータを読み込み
        StageDataConversion();

        //ステージのオブジェクト生成
        //ステージの生成
        StageCreate();
    }


    //------------------------------- メソッド部 ----------------------------------

    #region ステージのテキストデータを読み込み
    /// <summary>
    /// <para>ステージのテキストデータを読み込み</para>
    /// </summary>
    private void StageDataConversion()
    {

        //1行ごとに読み込み(空白は省略 = RemoveEmptyEntries)
        //'\n', '\r' = 改行して左端に移動
        //_stageLines = 行数(X)
        _stageLines = _stageDatas[_stageNumber].text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //','ごとに1つのstringとする
        //最下段の数だけ分割
        //_stageValues = 列数(Z)
        _stageValues = _stageLines[0].Split(new[] { ',' });

        //ステージ配列の個数を宣言
        _stageArrays = new int[_stageLines.Length, _stageValues.Length];

    }
    #endregion


    #region ステージのオブジェクト生成
    //ステージの生成
    /// <summary>
    /// <para>ステージの生成</para>
    /// </summary>
    private void StageCreate()
    {

        //1行ごとに読み込み
        for (int i = 0; i <= _stageLines.Length - 1; i++)
        {

            //','ごとに1つのstringとする
            //_stageValues = 列数(Z)
            _stageValues = _stageLines[i].Split(new[] { ',' });

            //1列ごとに読み込み
            for (int j = 0; j <= _stageValues.Length - 1; j++)
            {

                //stageTextの[0,0]から順に_stageArrays配列にint型で格納
                //stageTextの[_stageValues,_stageLines]
                _stageArrays[i, j] = int.Parse(_stageValues[j]);

                //_stageArrays[i, j]の中がNULL && プレイヤーではないとき
                if (_stageArrays[i, j] != STAGE_NULL && _stageArrays[i, j] != STAGE_PLAYER)
                {

                    //各オブジェクトにマテリアルとメッシュを貼るためのEnumを変更
                    //int型をEnum型に変更して処理
                    _cubeNumber = (CubeNumber)Enum.ToObject(typeof(CubeNumber), _stageArrays[i, j]);

                    //オブジェクトの呼び出し
                    ObjectCall(i, j);
                }
                else if(_stageArrays[i, j] == STAGE_PLAYER)
                {

                    //プレーヤーを配置
                    _playerObj.transform.position = new Vector3(i, 0, j);

                    //プレーヤーの位置を補完
                    _playerArrayPos = new Vector2((int)i, (int)j);
                }
            }

        }

        //ステージ更新の終了
        _isNewStageLoad = false;

    }


    //オブジェクトの呼び出し
    /// <summary>
    /// <para>オブジェクトの呼び出し</para>
    /// </summary>
    /// <param name="i,j">プレイヤー移動時の方向</param>
    private void ObjectCall(int i, int j)
    {

        //オブジェクトプールからオブジェクトを呼び出し
        //メソッド描画されていないものが返却
        GameObject _createObj_l = _objectPool.GetObj();

        //各オブジェクトにマテリアルとメッシュを貼る
        switch (_cubeNumber)
        {

            //壁
            case CubeNumber.WALL:

                //オブジェクトにメッシュを設定
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshWallDatas[_stageNumber];

                //オブジェクトにマテリアルを貼る
                _createObj_l.GetComponent<MeshRenderer>().material = _materialWallDatas[_stageNumber];

                break;

            //動く箱
            case CubeNumber.BLOCK:

                //オブジェクトにメッシュを設定
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshBLOCKDatas[_stageNumber];

                //オブジェクトにマテリアルを貼る
                _createObj_l.GetComponent<MeshRenderer>().material = _materialBLOCKDatas[_stageNumber];

                //生成したオブジェクトを補完
                _createObj = _createObj_l;

                //動く箱をListに格納
                _moveBLOCKArrayObjs.Add(_createObj);

                break;

            //ゴール
            case CubeNumber.GOAL:

                //オブジェクトにメッシュを設定
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshGoalDatas[_stageNumber];

                //オブジェクトにマテリアルを貼る
                _createObj_l.GetComponent<MeshRenderer>().material = _materialGoalDatas[_stageNumber];

                //ゴール数の最大値をカウント
                _max_goalCount++;

                break;

        }

        //オブジェクトを表示
        _createObj_l.GetComponent<MeshRenderer>().enabled = true;

        //生成したオブジェクトを配置
        _createObj_l.transform.position = new Vector3(i, 0, j);

    }
    #endregion


    #region オブジェクトの情報を初期化(再スタートする時の処理)
    /// <summary>
    /// <para>オブジェクトの情報を初期化(再スタートする時の処理)</para>
    /// </summary>
    private void ObjectReset()
    {

        //ステージ更新
        _isNewStageLoad = true;

        //オブジェクトプールのListを探索
        for (int i = 0; i <= _objectPool.poolObjList.Count - 1; i++)
        {

            //オブジェクトプールのListにあるObjectのMeshを消す
            _objectPool.poolObjList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        //動く箱の位置を補完Listを探索
        //要素をすべて削除
        _moveBLOCKArrayObjs.Clear();

    }
    #endregion


    #region ステージ配列とテキストデータの初期化(再スタートする時の処理)
    /// <summary>
    /// <para>ステージ配列とテキストデータの初期化(再スタートする時の処理)</para>
    /// </summary>
    private void StageReset()
    {

        //ゴール数の最大値を初期化
        _max_goalCount = 0;

        //ゴール数のカウントを初期化
        _goalCount = 0;

        //行数(X)の探索
        for (int i = 0; i <= _stageLines.Length - 1; i++)
        {

            //行を探索して初期化
            _stageLines[i] = null;

            //列数(Z)の探索
            for (int j = 0; j <= _stageValues.Length - 1; j++)
            {

                //ステージの配列をすべて0(null)にする
                _stageArrays[i, j] = 0;

                //列を探索して初期化
                _stageValues[j] = null;

            }

        }

    }
    #endregion


    #region 次のステージを読み込み
    /// <summary>
    /// <para>次のステージを読み込み</para>
    /// </summary>
    public void NextStageLoad()
    {

        //オブジェクトのリセット
        ObjectReset();

        //次のステージへ
        _stageNumber++;

        //ステージの初期化
        //ステージのテキストデータを初期化
        //ステージデータのサイズが変更された時を考慮
        StageReset();

        //ステージのテキストデータを読み込み
        StageDataConversion();

        //ステージの生成
        StageCreate();

        //最終ステージクリアで終了
        if (_stageNumber >= _stageDatas.Length - 1)
        {

            //プレイヤーを消す
            _playerObj.SetActive(false);

            //ゲームクリア
            _isGameClear = true;

            return;
        }

    }

    #endregion


    #region 今のステージを読み込み
    /// <summary>
    /// <para>今のステージを読み込み</para>
    /// </summary>
    public void NowStageLoad()
    {

        //オブジェクトのリセット
        ObjectReset();

        //ステージの初期化
        //ステージのテキストデータを初期化
        //ステージデータのサイズが変更された時を考慮
        StageReset();

        //ステージのテキストデータを読み込み
        StageDataConversion();

        //ステージの生成
        StageCreate();

    }

    #endregion


    #region 移動管理メソッド
    /// <summary>
    /// <para>プレイヤーと移動箱の移動管理メソッド</para>
    /// </summary>
    /// <param name="movePoint">プレイヤーの移動量</param>
    /// <returns>移動可能かどうか</returns>
    public bool IsPlayerMove(Vector2 movePoint)
    {

        //進行方向に壁があるか
        if (MoveOrientationWall(movePoint) == false) { return false; }

        //進行方向に動く箱があるか
        if (MoveOrientationMoveBLOCK(movePoint) == false) { return false; }
 
        //プレーヤーの配列位置 + プレーヤーの移動量
        int pos = _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x, 
                               (int)_playerArrayPos.y + (int)movePoint.y];

        //移動元の番号
        //NUllポイント
        //空白? || 移動箱?
        if (pos == STAGE_NULL || pos == STAGE_MOVE_BLOCK)
        {

            //プレイヤーの位置配列をプレーヤー番号(4)に更新
            _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x,
                         (int)_playerArrayPos.y + (int)movePoint.y] = STAGE_PLAYER;

            //ゴールを踏んでいないなら
            if (!_isGoalStep)
            {

                //プレイヤーの元の位置配列をNull(0)に更新
                _stageArrays[(int)_playerArrayPos.x,
                             (int)_playerArrayPos.y] = STAGE_NULL;
                 
            }

            //ゴールを踏んでいない状態
            _isGoalStep = false;
        }

        //ゴールポイント
        //ゴールした場所? || ゴール?
        else if (pos == STAGE_GOAL_POS || pos == STAGE_GOAL)
        {

            //プレイヤーの位置配列をゴール番号(3)に更新
            _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x,
                         (int)_playerArrayPos.y + (int)movePoint.y] = STAGE_GOAL;

            //ゴールを踏んでいないなら
            if (!_isGoalStep)
            {

                //プレイヤーの元の位置配列をNull(0)に更新
                _stageArrays[(int)_playerArrayPos.x,
                             (int)_playerArrayPos.y] = STAGE_NULL;

            }

            //ゴールを踏んでいる状態
            _isGoalStep = true;

        }


        //プレーヤーの位置を補完(更新)
        _playerArrayPos = new Vector2((int)_playerArrayPos.x + (int)movePoint.x,
                                     (int)_playerArrayPos.y + (int)movePoint.y);

        return true;

    }


    //進行方向に壁があるか
    /// <summary>
    /// <para>進行方向に壁があるか</para>
    /// </summary>
    /// <param name="movePoint_W">プレイヤーの移動量</param>
    /// <returns>移動可能かどうか</returns>
    private bool MoveOrientationWall(Vector2 movePoint_W)
    {

        //プレーヤーの配列位置 + プレーヤーの移動量 == 壁?(1マス前)
        if (_stageArrays[(int)_playerArrayPos.x + (int)movePoint_W.x, (int)_playerArrayPos.y + (int)movePoint_W.y] == STAGE_WALL) { return false; }

        return true;
    }


    //進行方向に動く箱があるか
    /// <summary>
    /// <para>進行方向に動く箱があるか</para>
    /// </summary>
    /// <param name="movePoint_W">プレイヤーの移動量</param>
    /// <returns>移動可能かどうか</returns>
    private bool MoveOrientationMoveBLOCK(Vector2 movePoint_M)
    {

        //プレーヤーの配列位置 + プレーヤーの移動量
        int pos = _stageArrays[(int)_playerArrayPos.x + (int)movePoint_M.x, (int)_playerArrayPos.y + (int)movePoint_M.y];

        //動く箱?　|| ゴール箱?(1マス前)
        if (pos == STAGE_MOVE_BLOCK || pos == STAGE_GOAL_POS)
        {

            //プレーヤーの配列位置 + プレーヤーの移動量 * 2
            int posX2_l = _stageArrays[(int)_playerArrayPos.x + (int)movePoint_M.x * 2, (int)_playerArrayPos.y + (int)movePoint_M.y * 2];

            //動く箱の前が壁? || 動く箱の前が動く箱? || ゴール済み?(2マス前)
            if (posX2_l == STAGE_WALL || posX2_l == STAGE_MOVE_BLOCK || posX2_l == STAGE_GOAL_POS) { return false; }
  
            //動く箱の前がゴール?(2マス前)
            else if (posX2_l == STAGE_GOAL)
            {

                //ゴール
                _isGoal = true;

                //ゴール数をカウントアップ
                _goalCount++;
            }

            //プレーヤーの配列位置 + プレーヤーの移動量 == ゴールした箱?(1マス前)
            if (pos == STAGE_GOAL_POS)
            {

                //ゴール数をカウントダウン
                _goalCount--;

            }

        }

        //動く箱の移動
        //動く箱のListを探索
        for (int i = 0; i <= _moveBLOCKArrayObjs.Count - 1; i++)
        {

            //箱の位置配列(座標) == プレイヤーの1マス前の座標
            if (_moveBLOCKArrayObjs[i].transform.position == new Vector3((int)_playerArrayPos.x + (int)movePoint_M.x, 0, 
                                                                         (int)_playerArrayPos.y + (int)movePoint_M.y))
            {

                //ゴールしてない
                if (!_isGoal)
                {

                    //箱の移動先位置配列を箱番号(2)に更新
                    _stageArrays[(int)_moveBLOCKArrayObjs[i].transform.position.x + (int)movePoint_M.x,
                                 (int)_moveBLOCKArrayObjs[i].transform.position.z + (int)movePoint_M.y] = STAGE_MOVE_BLOCK;

                }

                //ゴールした
                else if (_isGoal)
                {

                    //箱の移動先位置配列を箱番号(5)に更新(ゴール)
                    _stageArrays[(int)_moveBLOCKArrayObjs[i].transform.position.x + (int)movePoint_M.x,
                                 (int)_moveBLOCKArrayObjs[i].transform.position.z + (int)movePoint_M.y] = STAGE_GOAL_POS;

                    //ゴールした数がゴールの数と一致したら
                    if (_goalCount >= _max_goalCount)
                    {

                        //ステージ更新
                        _isNewStageLoad = true;

                        //次のステージを読み込み
                        NextStageLoad();

                        break;
                    }
                }


                //箱の移動
                //プレイヤーの移動量,方向に箱を移動
                _moveBLOCKArrayObjs[i].transform.position = new Vector3(_moveBLOCKArrayObjs[i].transform.position.x + movePoint_M.x, 0,
                                                                        _moveBLOCKArrayObjs[i].transform.position.z + movePoint_M.y);


                break;
            }
        }


        //ゴールフラグの初期化
        _isGoal = false;

        return true;
    }
    #endregion

}
