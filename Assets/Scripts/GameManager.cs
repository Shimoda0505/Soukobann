using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField, Tooltip("�X�e�[�W�}�l�[�W���[")]
    private StageManager _stageManager;

    [Header("�e�L�X�g")]
    [SerializeField, Tooltip("���X�^�[�gText")]
    private Text _reStartText;

    [SerializeField, Tooltip("�^�C�g�����[�hText")]
    private Text _titleLoadText;

    private void Start()
    {

        //���X�^�[�gText��\��
        _reStartText.GetComponent<Text>().enabled = true;

        //�^�C�g�����[�hText���\��
        _titleLoadText.GetComponent<Text>().enabled = false;

    }

    private void Update()
    {

        //�Q�[���N���A�O
        if (!_stageManager.IsGameClear()) { return; }


        //�^�C�g�����[�hText��\��
        _titleLoadText.GetComponent<Text>().enabled = true;

        //���X�^�[�gText���\��
        _reStartText.GetComponent<Text>().enabled = false;

        //Enter�L�[���������Ƃ�
        if (Input.GetKeyDown(KeyCode.Return))
        {

            //���݂̃X�e�[�W��side�Ăяo��
            SceneManager.LoadScene("TitleScene");
        }

    }

}

