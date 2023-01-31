    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//����� ���c�ˌ�
//�I�u�W�F�N�g�̐���(Script)
public class ObjectPool : MonoBehaviour
{

 //----------------------------- �錾�� ---------------------------------

    [SerializeField,Tooltip("�v�[���ɐ����������I�u�W�F�N�g����")]
    private GameObject poolObj;

    //���������I�u�W�F�N�g�̃��X�g
    public List<GameObject> poolObjList;



    //---------------------------- ���\�b�h�� -------------------------------

    #region �����X�N���v�g����Ăяo��
    /// <summary>
    /// <para>GetObj</para>
    /// <para>�v�[������I�u�W�F�N�g���Ăяo��</para>
    /// </summary>
    /// <returns>�I�u�W�F�N�g</returns>
    public GameObject GetObj()
    {

        // ���g�p������Ύg�p,�Ȃ���ΐ���
        // �g�p���łȂ����̂�T���ĕԂ�
        //���X�g���ɂ���I�u�W�F�N�g��obj�Ƃ��ĕԂ�
        foreach (GameObject obj in poolObjList)
        {

            //�I�u�W�F�N�g�̕`����擾
            bool objrb = obj.GetComponent<MeshRenderer>().enabled;

            //�I�u�W�F�N�g�̕`�悪false�̂��̂�T��
            if (objrb == false)
            {

                //�I�u�W�F�N�g�̕`�悪false�̂��̂�����΂����true
                objrb = true;

                //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
                return obj;
            }
        }

        // �S�Ďg�p����������V�������
        GameObject newObj = CreateNewObj();

        //���X�g�ɕۑ����Ă���
        poolObjList.Add(newObj);

        //�V����������I�u�W�F�N�g�̕`������̂܂�true�ɂ���
        newObj.GetComponent<MeshRenderer>().enabled = true;

        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }
    #endregion


    #region �V�����I�u�W�F�N�g���쐬���鏈��
    /// <summary>
    /// <para>CreateNewObj</para>
    /// <para>�I�u�W�F�N�g�̃C���X�^���e�B�G�C�g</para>
    /// </summary>
    /// <returns>�I�u�W�F�N�g</returns>
    private GameObject CreateNewObj()
    {

        // ��ʊO�ɐ���
        Vector3 pos = this.gameObject.transform.position;

        // �V�����I�u�W�F�N�g�𐶐�(�����������I�u�W�F�N�g��,��ʊO��,�e�ɂȂ�I�u�W�F�N�g�Ɠ�������)
        GameObject newObj = Instantiate(poolObj, pos, Quaternion.identity);

        //���O�ɘA�ԕt��(���X�g�ɒǉ����ꂽ��)
        newObj.name = poolObj.name + (poolObjList.Count + 1);

        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }
    #endregion
}

