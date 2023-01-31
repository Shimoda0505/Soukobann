    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//製作者 下田祥己
//オブジェクトの生成(Script)
public class ObjectPool : MonoBehaviour
{

 //----------------------------- 宣言部 ---------------------------------

    [SerializeField,Tooltip("プールに生成したいオブジェクトを代入")]
    private GameObject poolObj;

    //生成したオブジェクトのリスト
    public List<GameObject> poolObjList;



    //---------------------------- メソッド部 -------------------------------

    #region 生成スクリプトから呼び出し
    /// <summary>
    /// <para>GetObj</para>
    /// <para>プールからオブジェクトを呼び出し</para>
    /// </summary>
    /// <returns>オブジェクト</returns>
    public GameObject GetObj()
    {

        // 未使用があれば使用,なければ生成
        // 使用中でないものを探して返す
        //リスト内にあるオブジェクトをobjとして返す
        foreach (GameObject obj in poolObjList)
        {

            //オブジェクトの描画を取得
            bool objrb = obj.GetComponent<MeshRenderer>().enabled;

            //オブジェクトの描画がfalseのものを探す
            if (objrb == false)
            {

                //オブジェクトの描画がfalseのものがあればそれをtrue
                objrb = true;

                //呼び出し元のスクリプトにこのオブジェクトを返す
                return obj;
            }
        }

        // 全て使用中だったら新しく作る
        GameObject newObj = CreateNewObj();

        //リストに保存しておく
        poolObjList.Add(newObj);

        //新しく作ったオブジェクトの描画をそのままtrueにする
        newObj.GetComponent<MeshRenderer>().enabled = true;

        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;
    }
    #endregion


    #region 新しくオブジェクトを作成する処理
    /// <summary>
    /// <para>CreateNewObj</para>
    /// <para>オブジェクトのインスタンティエイト</para>
    /// </summary>
    /// <returns>オブジェクト</returns>
    private GameObject CreateNewObj()
    {

        // 画面外に生成
        Vector3 pos = this.gameObject.transform.position;

        // 新しいオブジェクトを生成(生成したいオブジェクトを,画面外に,親になるオブジェクトと同じ所に)
        GameObject newObj = Instantiate(poolObj, pos, Quaternion.identity);

        //名前に連番付け(リストに追加された順)
        newObj.name = poolObj.name + (poolObjList.Count + 1);

        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;
    }
    #endregion
}

