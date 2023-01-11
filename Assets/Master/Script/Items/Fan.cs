using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : ItemBase
{
    [SerializeField] float _gageScore = 1;
    public override void ItemAction()
    {
        GameManager.Instance.AddFanValue(_gageScore);
        Destroy(gameObject);
        //GameManagerのゲージ加算関数を呼び、引数に自身が持つ値セット。
    }
}
