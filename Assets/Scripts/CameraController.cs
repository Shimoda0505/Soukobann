using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//����� ���c�ˌ�
//�J�����R���g���[���[
public class CameraController : MonoBehaviour
{

 //----------------------------- �錾�� ------------------------------------

    [SerializeField, Tooltip("StageManager�̃X�N���v�g")]
    private StageManager stageManager;


    #region �J�����̕ύX�֘A
    [Header("�ʒu")]
    [SerializeField, Tooltip("�J�����ʒu")]
    private Vector3 cameraPos;

    [SerializeField, Tooltip("�J�����̈ړ����x")]
    private float cameraPosSpeed;


    [Header("�p�x")]
    [SerializeField, Tooltip("�J�����p�x")]
    private Quaternion cameraAngle;

    [SerializeField, Tooltip("�J�����̊p�x�ύX���x")]
    private float cameraAngleSpeed;
    #endregion


    //----------------------------- ������ -------------------------------------

    private void FixedUpdate()
    {
        //�Q�[���N���A�O��return
        if(!stageManager.IsGameClear())
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScene");
        }

        //�Q�[���N���A���̏���
        //�J�����ʒu���ړ�
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, cameraPos, cameraPosSpeed);

        //�J�����p�x�̕ύX
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, cameraAngle, cameraAngleSpeed);
    }
}
