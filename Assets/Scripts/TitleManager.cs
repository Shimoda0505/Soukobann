using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    private void Update()
    {

        //Enter�L�[���������Ƃ�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            //���C���V�[�������[�h
            SceneManager.LoadScene("MainScene");
        }

    }
}
