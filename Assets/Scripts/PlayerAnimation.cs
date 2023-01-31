using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Playerのアニメーション
    private Animator anim;


    private void Start()
    {

        //Playerのアニメーション取得
        anim = this.gameObject.GetComponent<Animator>();
    }


    #region ジャンプアニメーション
    //ジャンプアニメーション開始
    /// <summary>
    /// <para>ジャンプアニメーション開始</para>
    /// </summary>
    public void Jump_True_Animation()
    {

        //ジャンプアニメーションのtrue
        anim.SetBool("Jump", true);
    }

    //ジャンプアニメーション終了
    /// <summary>
    /// <para>ジャンプアニメーション終了</para>
    /// </summary>
    public void Jump_False_Animation()
    {

        //ジャンプアニメーションのtrue
        anim.SetBool("Jump", false);
    }
    #endregion

}
