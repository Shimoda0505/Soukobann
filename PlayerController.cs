using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����� ���c�ˌ�
//�v���C���[�̈ړ�(Script)
public class PlayerController : MonoBehaviour
{

 //----------------------------- �錾�� ------------------------------------

    [SerializeField, Tooltip("StageManager�̃X�N���v�g")]
    private StageManager stageManager;

    [SerializeField, Tooltip("PlayerAnimation�̃X�N���v�g")]
    private PlayerAnimation playerAnimation;

    #region ���Ԋ֘A
    [SerializeField, Tooltip("�ҋ@����")]
    private float intervalCount;

    //�ҋ@���Ԃ̌v��
    private float intervalTime = 0;
    #endregion


    #region �v���C���[�֘A
    //�v���C���[�̈ړ��ʂƈړ�����
    private Vector2 playerMoveVolume;

    //�v���C���[�̊p�x
    private float playerAngle;
    #endregion


    #region�@�ړ�Enum
    MoveDirection moveDirection = MoveDirection.Wait;
    enum MoveDirection
    {
        Wait,//�ҋ@
        Up,//��ړ�
        Down,//���ړ�
        Left,//���ړ�
        Right,//�E�ړ�
        Interval//�ҋ@�ڍs

    }
    #endregion



 //------------------------------- ������ -------------------------------------

    private void Update()
    {

        //�X�e�[�W�X�V���͈ړ��ł��Ȃ�
        if(stageManager.IsNewStageLoad())
        {
            return;
        }

        //Enter�L�[���������Ƃ�
        if (Input.GetKeyDown(KeyCode.Return))
        {

            //���݂̃X�e�[�W��side�Ăяo��
            stageManager.NowStageLoad();
        }


        #region ���͈ړ�
        switch (moveDirection)
        {

            //�ҋ@
            case MoveDirection.Wait:

                // ���͈ړ�
                //��ړ�
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    moveDirection = MoveDirection.Up;
                }

                //���ړ�
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    moveDirection = MoveDirection.Down;
                }

                //���ړ�
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    moveDirection = MoveDirection.Left;
                }

                //�E�ړ�
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

        //�ړ�Enum
        switch (moveDirection)
        {

            #region �ړ�����
            //��ړ�
            case MoveDirection.Up:

                //�v���C���[�̈ړ��ʂƈړ�����
                playerMoveVolume = new Vector2(0, 1);

                //�v���C���[�̊p�x
                playerAngle = 0;

                //�v���C���[�̈ړ����\�b�h
                PlayerMove();

                break;


            //���ړ�
            case MoveDirection.Down:

                //�v���C���[�̈ړ��ʂƈړ�����
                playerMoveVolume = new Vector2(0, -1);

                //�v���C���[�̊p�x
                playerAngle = 180;

                //�v���C���[�̈ړ����\�b�h
                PlayerMove();

                break;


            //�E�ړ�
            case MoveDirection.Right:

                //�v���C���[�̈ړ��ʂƈړ�����
                playerMoveVolume = new Vector2(1, 0);

                //�v���C���[�̊p�x
                playerAngle = 90;

                //�v���C���[�̈ړ����\�b�h
                PlayerMove();

                break;


            //���ړ�
            case MoveDirection.Left:

                //�v���C���[�̈ړ��ʂƈړ�����
                playerMoveVolume = new Vector2(-1, 0);

                //�v���C���[�̊p�x
                playerAngle = 270;

                //�v���C���[�̈ړ����\�b�h
                PlayerMove();

                break;


            #endregion


            #region �ړ���C���^�[�o��
            //�ҋ@�ڍs
            //�ړ���̃C���^�[�o��
            case MoveDirection.Interval:

                //�v��
                intervalTime += Time.deltaTime;

                //�v����
                if(intervalTime >= intervalCount)
                {
                    intervalTime = 0;

                    //�ҋ@Enum�Ɉڍs
                    moveDirection = MoveDirection.Wait;
                }

                break;
                #endregion

        }

    }


    //------------------------------- ���\�b�h�� ----------------------------------

    #region �ړ����\�b�h
    //�v���C���[�̈ړ����\�b�h
    /// <summary>
    /// <para>�v���C���[�̈ړ����\�b�h</para>
    /// </summary>
    private void PlayerMove()
    {
        
        //�ړ������̃I�u�W�F�N�g��T��
        //�ړ������ɏ�Q�����Ȃ��Ȃ�ړ����ł���
        if (stageManager.IsPlayerMove(playerMoveVolume) == true)
        {

            //�ړ��A�j���[�V�����J�n
            playerAnimation.Jump_True_Animation();

            //�ړ�
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + playerMoveVolume.x, 0,
                                                             this.gameObject.transform.position.z + playerMoveVolume.y);

        }

        //�p�x
        this.gameObject.transform.eulerAngles = new Vector3(this.gameObject.transform.rotation.x, playerAngle, this.gameObject.transform.rotation.z);

        //�ҋ@�ڍsEnum�ɑJ��
        moveDirection = MoveDirection.Interval;

    }
    #endregion

}
