using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggplant : ItemBase
{
    [SerializeField]
    float _score = 0f;
    [SerializeField]
    int _feverScore = 0;
    public override void ItemAction()
    {
        GameManager.Instance.AddScore(_score);
        GameManager.Instance.AddFevarValue(_feverScore);
        Destroy(gameObject);
        //GameManagerのスコア加算関数を呼び、引数に自身が持つ値セット。
    }
}
