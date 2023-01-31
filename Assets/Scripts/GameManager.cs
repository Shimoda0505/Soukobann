using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField, Tooltip("ステージマネージャー")]
    private StageManager _stageManager;

    [Header("テキスト")]
    [SerializeField, Tooltip("リスタートText")]
    private Text _reStartText;

    [SerializeField, Tooltip("タイトルロードText")]
    private Text _titleLoadText;

    private void Start()
    {

        //リスタートTextを表示
        _reStartText.GetComponent<Text>().enabled = true;

        //タイトルロードTextを非表示
        _titleLoadText.GetComponent<Text>().enabled = false;

    }

    private void Update()
    {

        //ゲームクリア前
        if (!_stageManager.IsGameClear()) { return; }


        //タイトルロードTextを表示
        _titleLoadText.GetComponent<Text>().enabled = true;

        //リスタートTextを非表示
        _reStartText.GetComponent<Text>().enabled = false;

        //Enterキーを押したとき
        if (Input.GetKeyDown(KeyCode.Return))
        {

            //現在のステージをside呼び出し
            SceneManager.LoadScene("TitleScene");
        }

    }

}

