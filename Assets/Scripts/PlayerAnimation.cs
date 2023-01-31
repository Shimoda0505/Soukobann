using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Player�̃A�j���[�V����
    private Animator anim;


    private void Start()
    {

        //Player�̃A�j���[�V�����擾
        anim = this.gameObject.GetComponent<Animator>();
    }


    #region �W�����v�A�j���[�V����
    //�W�����v�A�j���[�V�����J�n
    /// <summary>
    /// <para>�W�����v�A�j���[�V�����J�n</para>
    /// </summary>
    public void Jump_True_Animation()
    {

        //�W�����v�A�j���[�V������true
        anim.SetBool("Jump", true);
    }

    //�W�����v�A�j���[�V�����I��
    /// <summary>
    /// <para>�W�����v�A�j���[�V�����I��</para>
    /// </summary>
    public void Jump_False_Animation()
    {

        //�W�����v�A�j���[�V������true
        anim.SetBool("Jump", false);
    }
    #endregion

}
