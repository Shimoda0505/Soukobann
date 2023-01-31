using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    private void Update()
    {

        //Enterキーを押したとき
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            //メインシーンをロード
            SceneManager.LoadScene("MainScene");
        }

    }
}
