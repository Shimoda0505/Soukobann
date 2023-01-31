using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//����� ���c�ˌ�
//�q�ɔԂ̃X�e�[�W����,�Ǘ�(Script)
public class StageManager : MonoBehaviour
{

 //------------------------------- �錾�� ------------------------------------

    #region �v���C���[�֘A
    [SerializeField, Tooltip("�v���[���[")]
    private GameObject _playerObj;

    //�v���[���[�̈ʒu��⊮
    private Vector2 _playerArrayPos;

    //�X�e�[�W�X�V
    //�V�����X�e�[�W�𐶐����Ă����,�v���C���[�𓮂����Ȃ����߂̃t���O
    private bool _isNewStageLoad = true;
    public bool IsNewStageLoad()
    {
        return _isNewStageLoad;
    }

    #endregion


    #region �I�u�W�F�N�g�֘A
    [SerializeField, Tooltip("�I�u�W�F�N�g�v�[��")]
    private ObjectPool _objectPool;

    //�������̈ʒu��⊮
    private List<GameObject> _moveBLOCKArrayObjs = new List<GameObject>();

    //���������I�u�W�F�N�g��⊮
    private GameObject _createObj;

    //MeshFilter�̃R���|�[�l���g
    private MeshFilter _meshFilterComponent;

    //MeshRenderer�̃R���|�[�l���g
    private MeshRenderer _meshRendererComponent;


    //�I�u�W�F�N�g�̃��b�V��
    [Header("�}�e���A����ݒ�")]
    [SerializeField, Tooltip("�ǂ̃}�e���A���f�[�^")]
    private Material[] _materialWallDatas;
    [SerializeField, Tooltip("�������̃}�e���A���f�[�^")]
    private Material[] _materialBLOCKDatas;
    [SerializeField, Tooltip("�S�[���̃}�e���A���f�[�^")]
    private Material[] _materialGoalDatas;

    //�I�u�W�F�N�g�̃}�e���A��
    [Header("���b�V����ݒ�")]
    [SerializeField, Tooltip("���̃��b�V���f�[�^")]
    private Mesh[] _meshWallDatas;
    [SerializeField, Tooltip("�������̃��b�V���f�[�^")]
    private Mesh[] _meshBLOCKDatas;
    [SerializeField, Tooltip("�S�[���̃��b�V���f�[�^")]
    private Mesh[] _meshGoalDatas;

    //�e�I�u�W�F�N�g�Ƀ}�e���A���ƃ��b�V����\��
    //����pEnum
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


    #region �X�e�[�W�֘A
    [Header("�X�e�[�W��ݒ�")]
    [SerializeField, Tooltip("�X�e�[�W�̃e�L�X�g�f�[�^")]
    private TextAsset[] _stageDatas;

    //�s��
    private string[] _stageLines;

    //��
    private string[] _stageValues;

    //�X�e�[�W�ԍ�
    private int _stageNumber = 0;


    //�c���z��
    //0 = null , 1 = �� , 2 = �ړ��� , 3 = �S�[�� , 4 = �v���[���[ , 5 = �S�[�������ꏊ
    private int[,] _stageArrays;

    //�X�e�[�W�z����̔ԍ��ɐU���Ă���ԍ�
    //Null
    private const int STAGE_NULL = 0;
    //��
    private const int STAGE_WALL = 1;
    //�ړ���
    private const int STAGE_MOVE_BLOCK = 2;
    //�S�[��
    private const int STAGE_GOAL = 3;
    //�v���C���[
    private const int STAGE_PLAYER = 4;
    //�S�[�������ꏊ
    private const int STAGE_GOAL_POS = 5;
    #endregion


    #region �S�[���֘A
    //�S�[�����̍ő�l
    private int _max_goalCount = 0;

    //�S�[�������J�E���g
    private int _goalCount = 0;

    //�S�[���������ǂ���
    private bool _isGoal = false;

    //�v���C���[���S�[���𓥂�ł��邩�ǂ���
    private bool _isGoalStep = false;

    //�Q�[���N���A
    private bool _isGameClear = false;
    public bool IsGameClear()
    {
        return _isGameClear;
    }
    #endregion


 //------------------------------- ������ -------------------------------------

    private void Awake()
    {

        //�X�e�[�W�̃e�L�X�g�f�[�^��ǂݍ���
        StageDataConversion();

        //�X�e�[�W�̃I�u�W�F�N�g����
        //�X�e�[�W�̐���
        StageCreate();
    }


    //------------------------------- ���\�b�h�� ----------------------------------

    #region �X�e�[�W�̃e�L�X�g�f�[�^��ǂݍ���
    /// <summary>
    /// <para>�X�e�[�W�̃e�L�X�g�f�[�^��ǂݍ���</para>
    /// </summary>
    private void StageDataConversion()
    {

        //1�s���Ƃɓǂݍ���(�󔒂͏ȗ� = RemoveEmptyEntries)
        //'\n', '\r' = ���s���č��[�Ɉړ�
        //_stageLines = �s��(X)
        _stageLines = _stageDatas[_stageNumber].text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //','���Ƃ�1��string�Ƃ���
        //�ŉ��i�̐���������
        //_stageValues = ��(Z)
        _stageValues = _stageLines[0].Split(new[] { ',' });

        //�X�e�[�W�z��̌���錾
        _stageArrays = new int[_stageLines.Length, _stageValues.Length];

    }
    #endregion


    #region �X�e�[�W�̃I�u�W�F�N�g����
    //�X�e�[�W�̐���
    /// <summary>
    /// <para>�X�e�[�W�̐���</para>
    /// </summary>
    private void StageCreate()
    {

        //1�s���Ƃɓǂݍ���
        for (int i = 0; i <= _stageLines.Length - 1; i++)
        {

            //','���Ƃ�1��string�Ƃ���
            //_stageValues = ��(Z)
            _stageValues = _stageLines[i].Split(new[] { ',' });

            //1�񂲂Ƃɓǂݍ���
            for (int j = 0; j <= _stageValues.Length - 1; j++)
            {

                //stageText��[0,0]���珇��_stageArrays�z���int�^�Ŋi�[
                //stageText��[_stageValues,_stageLines]
                _stageArrays[i, j] = int.Parse(_stageValues[j]);

                //_stageArrays[i, j]�̒���NULL && �v���C���[�ł͂Ȃ��Ƃ�
                if (_stageArrays[i, j] != STAGE_NULL && _stageArrays[i, j] != STAGE_PLAYER)
                {

                    //�e�I�u�W�F�N�g�Ƀ}�e���A���ƃ��b�V����\�邽�߂�Enum��ύX
                    //int�^��Enum�^�ɕύX���ď���
                    _cubeNumber = (CubeNumber)Enum.ToObject(typeof(CubeNumber), _stageArrays[i, j]);

                    //�I�u�W�F�N�g�̌Ăяo��
                    ObjectCall(i, j);
                }
                else if(_stageArrays[i, j] == STAGE_PLAYER)
                {

                    //�v���[���[��z�u
                    _playerObj.transform.position = new Vector3(i, 0, j);

                    //�v���[���[�̈ʒu��⊮
                    _playerArrayPos = new Vector2((int)i, (int)j);
                }
            }

        }

        //�X�e�[�W�X�V�̏I��
        _isNewStageLoad = false;

    }


    //�I�u�W�F�N�g�̌Ăяo��
    /// <summary>
    /// <para>�I�u�W�F�N�g�̌Ăяo��</para>
    /// </summary>
    /// <param name="i,j">�v���C���[�ړ����̕���</param>
    private void ObjectCall(int i, int j)
    {

        //�I�u�W�F�N�g�v�[������I�u�W�F�N�g���Ăяo��
        //���\�b�h�`�悳��Ă��Ȃ����̂��ԋp
        GameObject _createObj_l = _objectPool.GetObj();

        //�e�I�u�W�F�N�g�Ƀ}�e���A���ƃ��b�V����\��
        switch (_cubeNumber)
        {

            //��
            case CubeNumber.WALL:

                //�I�u�W�F�N�g�Ƀ��b�V����ݒ�
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshWallDatas[_stageNumber];

                //�I�u�W�F�N�g�Ƀ}�e���A����\��
                _createObj_l.GetComponent<MeshRenderer>().material = _materialWallDatas[_stageNumber];

                break;

            //������
            case CubeNumber.BLOCK:

                //�I�u�W�F�N�g�Ƀ��b�V����ݒ�
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshBLOCKDatas[_stageNumber];

                //�I�u�W�F�N�g�Ƀ}�e���A����\��
                _createObj_l.GetComponent<MeshRenderer>().material = _materialBLOCKDatas[_stageNumber];

                //���������I�u�W�F�N�g��⊮
                _createObj = _createObj_l;

                //��������List�Ɋi�[
                _moveBLOCKArrayObjs.Add(_createObj);

                break;

            //�S�[��
            case CubeNumber.GOAL:

                //�I�u�W�F�N�g�Ƀ��b�V����ݒ�
                _createObj_l.GetComponent<MeshFilter>().mesh = _meshGoalDatas[_stageNumber];

                //�I�u�W�F�N�g�Ƀ}�e���A����\��
                _createObj_l.GetComponent<MeshRenderer>().material = _materialGoalDatas[_stageNumber];

                //�S�[�����̍ő�l���J�E���g
                _max_goalCount++;

                break;

        }

        //�I�u�W�F�N�g��\��
        _createObj_l.GetComponent<MeshRenderer>().enabled = true;

        //���������I�u�W�F�N�g��z�u
        _createObj_l.transform.position = new Vector3(i, 0, j);

    }
    #endregion


    #region �I�u�W�F�N�g�̏���������(�ăX�^�[�g���鎞�̏���)
    /// <summary>
    /// <para>�I�u�W�F�N�g�̏���������(�ăX�^�[�g���鎞�̏���)</para>
    /// </summary>
    private void ObjectReset()
    {

        //�X�e�[�W�X�V
        _isNewStageLoad = true;

        //�I�u�W�F�N�g�v�[����List��T��
        for (int i = 0; i <= _objectPool.poolObjList.Count - 1; i++)
        {

            //�I�u�W�F�N�g�v�[����List�ɂ���Object��Mesh������
            _objectPool.poolObjList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        //�������̈ʒu��⊮List��T��
        //�v�f�����ׂč폜
        _moveBLOCKArrayObjs.Clear();

    }
    #endregion


    #region �X�e�[�W�z��ƃe�L�X�g�f�[�^�̏�����(�ăX�^�[�g���鎞�̏���)
    /// <summary>
    /// <para>�X�e�[�W�z��ƃe�L�X�g�f�[�^�̏�����(�ăX�^�[�g���鎞�̏���)</para>
    /// </summary>
    private void StageReset()
    {

        //�S�[�����̍ő�l��������
        _max_goalCount = 0;

        //�S�[�����̃J�E���g��������
        _goalCount = 0;

        //�s��(X)�̒T��
        for (int i = 0; i <= _stageLines.Length - 1; i++)
        {

            //�s��T�����ď�����
            _stageLines[i] = null;

            //��(Z)�̒T��
            for (int j = 0; j <= _stageValues.Length - 1; j++)
            {

                //�X�e�[�W�̔z������ׂ�0(null)�ɂ���
                _stageArrays[i, j] = 0;

                //���T�����ď�����
                _stageValues[j] = null;

            }

        }

    }
    #endregion


    #region ���̃X�e�[�W��ǂݍ���
    /// <summary>
    /// <para>���̃X�e�[�W��ǂݍ���</para>
    /// </summary>
    public void NextStageLoad()
    {

        //�I�u�W�F�N�g�̃��Z�b�g
        ObjectReset();

        //���̃X�e�[�W��
        _stageNumber++;

        //�X�e�[�W�̏�����
        //�X�e�[�W�̃e�L�X�g�f�[�^��������
        //�X�e�[�W�f�[�^�̃T�C�Y���ύX���ꂽ�����l��
        StageReset();

        //�X�e�[�W�̃e�L�X�g�f�[�^��ǂݍ���
        StageDataConversion();

        //�X�e�[�W�̐���
        StageCreate();

        //�ŏI�X�e�[�W�N���A�ŏI��
        if (_stageNumber >= _stageDatas.Length - 1)
        {

            //�v���C���[������
            _playerObj.SetActive(false);

            //�Q�[���N���A
            _isGameClear = true;

            return;
        }

    }

    #endregion


    #region ���̃X�e�[�W��ǂݍ���
    /// <summary>
    /// <para>���̃X�e�[�W��ǂݍ���</para>
    /// </summary>
    public void NowStageLoad()
    {

        //�I�u�W�F�N�g�̃��Z�b�g
        ObjectReset();

        //�X�e�[�W�̏�����
        //�X�e�[�W�̃e�L�X�g�f�[�^��������
        //�X�e�[�W�f�[�^�̃T�C�Y���ύX���ꂽ�����l��
        StageReset();

        //�X�e�[�W�̃e�L�X�g�f�[�^��ǂݍ���
        StageDataConversion();

        //�X�e�[�W�̐���
        StageCreate();

    }

    #endregion


    #region �ړ��Ǘ����\�b�h
    /// <summary>
    /// <para>�v���C���[�ƈړ����̈ړ��Ǘ����\�b�h</para>
    /// </summary>
    /// <param name="movePoint">�v���C���[�̈ړ���</param>
    /// <returns>�ړ��\���ǂ���</returns>
    public bool IsPlayerMove(Vector2 movePoint)
    {

        //�i�s�����ɕǂ����邩
        if (MoveOrientationWall(movePoint) == false) { return false; }

        //�i�s�����ɓ����������邩
        if (MoveOrientationMoveBLOCK(movePoint) == false) { return false; }
 
        //�v���[���[�̔z��ʒu + �v���[���[�̈ړ���
        int pos = _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x, 
                               (int)_playerArrayPos.y + (int)movePoint.y];

        //�ړ����̔ԍ�
        //NUll�|�C���g
        //��? || �ړ���?
        if (pos == STAGE_NULL || pos == STAGE_MOVE_BLOCK)
        {

            //�v���C���[�̈ʒu�z����v���[���[�ԍ�(4)�ɍX�V
            _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x,
                         (int)_playerArrayPos.y + (int)movePoint.y] = STAGE_PLAYER;

            //�S�[���𓥂�ł��Ȃ��Ȃ�
            if (!_isGoalStep)
            {

                //�v���C���[�̌��̈ʒu�z���Null(0)�ɍX�V
                _stageArrays[(int)_playerArrayPos.x,
                             (int)_playerArrayPos.y] = STAGE_NULL;
                 
            }

            //�S�[���𓥂�ł��Ȃ����
            _isGoalStep = false;
        }

        //�S�[���|�C���g
        //�S�[�������ꏊ? || �S�[��?
        else if (pos == STAGE_GOAL_POS || pos == STAGE_GOAL)
        {

            //�v���C���[�̈ʒu�z����S�[���ԍ�(3)�ɍX�V
            _stageArrays[(int)_playerArrayPos.x + (int)movePoint.x,
                         (int)_playerArrayPos.y + (int)movePoint.y] = STAGE_GOAL;

            //�S�[���𓥂�ł��Ȃ��Ȃ�
            if (!_isGoalStep)
            {

                //�v���C���[�̌��̈ʒu�z���Null(0)�ɍX�V
                _stageArrays[(int)_playerArrayPos.x,
                             (int)_playerArrayPos.y] = STAGE_NULL;

            }

            //�S�[���𓥂�ł�����
            _isGoalStep = true;

        }


        //�v���[���[�̈ʒu��⊮(�X�V)
        _playerArrayPos = new Vector2((int)_playerArrayPos.x + (int)movePoint.x,
                                     (int)_playerArrayPos.y + (int)movePoint.y);

        return true;

    }


    //�i�s�����ɕǂ����邩
    /// <summary>
    /// <para>�i�s�����ɕǂ����邩</para>
    /// </summary>
    /// <param name="movePoint_W">�v���C���[�̈ړ���</param>
    /// <returns>�ړ��\���ǂ���</returns>
    private bool MoveOrientationWall(Vector2 movePoint_W)
    {

        //�v���[���[�̔z��ʒu + �v���[���[�̈ړ��� == ��?(1�}�X�O)
        if (_stageArrays[(int)_playerArrayPos.x + (int)movePoint_W.x, (int)_playerArrayPos.y + (int)movePoint_W.y] == STAGE_WALL) { return false; }

        return true;
    }


    //�i�s�����ɓ����������邩
    /// <summary>
    /// <para>�i�s�����ɓ����������邩</para>
    /// </summary>
    /// <param name="movePoint_W">�v���C���[�̈ړ���</param>
    /// <returns>�ړ��\���ǂ���</returns>
    private bool MoveOrientationMoveBLOCK(Vector2 movePoint_M)
    {

        //�v���[���[�̔z��ʒu + �v���[���[�̈ړ���
        int pos = _stageArrays[(int)_playerArrayPos.x + (int)movePoint_M.x, (int)_playerArrayPos.y + (int)movePoint_M.y];

        //������?�@|| �S�[����?(1�}�X�O)
        if (pos == STAGE_MOVE_BLOCK || pos == STAGE_GOAL_POS)
        {

            //�v���[���[�̔z��ʒu + �v���[���[�̈ړ��� * 2
            int posX2_l = _stageArrays[(int)_playerArrayPos.x + (int)movePoint_M.x * 2, (int)_playerArrayPos.y + (int)movePoint_M.y * 2];

            //�������̑O����? || �������̑O��������? || �S�[���ς�?(2�}�X�O)
            if (posX2_l == STAGE_WALL || posX2_l == STAGE_MOVE_BLOCK || posX2_l == STAGE_GOAL_POS) { return false; }
  
            //�������̑O���S�[��?(2�}�X�O)
            else if (posX2_l == STAGE_GOAL)
            {

                //�S�[��
                _isGoal = true;

                //�S�[�������J�E���g�A�b�v
                _goalCount++;
            }

            //�v���[���[�̔z��ʒu + �v���[���[�̈ړ��� == �S�[��������?(1�}�X�O)
            if (pos == STAGE_GOAL_POS)
            {

                //�S�[�������J�E���g�_�E��
                _goalCount--;

            }

        }

        //�������̈ړ�
        //��������List��T��
        for (int i = 0; i <= _moveBLOCKArrayObjs.Count - 1; i++)
        {

            //���̈ʒu�z��(���W) == �v���C���[��1�}�X�O�̍��W
            if (_moveBLOCKArrayObjs[i].transform.position == new Vector3((int)_playerArrayPos.x + (int)movePoint_M.x, 0, 
                                                                         (int)_playerArrayPos.y + (int)movePoint_M.y))
            {

                //�S�[�����ĂȂ�
                if (!_isGoal)
                {

                    //���̈ړ���ʒu�z��𔠔ԍ�(2)�ɍX�V
                    _stageArrays[(int)_moveBLOCKArrayObjs[i].transform.position.x + (int)movePoint_M.x,
                                 (int)_moveBLOCKArrayObjs[i].transform.position.z + (int)movePoint_M.y] = STAGE_MOVE_BLOCK;

                }

                //�S�[������
                else if (_isGoal)
                {

                    //���̈ړ���ʒu�z��𔠔ԍ�(5)�ɍX�V(�S�[��)
                    _stageArrays[(int)_moveBLOCKArrayObjs[i].transform.position.x + (int)movePoint_M.x,
                                 (int)_moveBLOCKArrayObjs[i].transform.position.z + (int)movePoint_M.y] = STAGE_GOAL_POS;

                    //�S�[�����������S�[���̐��ƈ�v������
                    if (_goalCount >= _max_goalCount)
                    {

                        //�X�e�[�W�X�V
                        _isNewStageLoad = true;

                        //���̃X�e�[�W��ǂݍ���
                        NextStageLoad();

                        break;
                    }
                }


                //���̈ړ�
                //�v���C���[�̈ړ���,�����ɔ����ړ�
                _moveBLOCKArrayObjs[i].transform.position = new Vector3(_moveBLOCKArrayObjs[i].transform.position.x + movePoint_M.x, 0,
                                                                        _moveBLOCKArrayObjs[i].transform.position.z + movePoint_M.y);


                break;
            }
        }


        //�S�[���t���O�̏�����
        _isGoal = false;

        return true;
    }
    #endregion

}
