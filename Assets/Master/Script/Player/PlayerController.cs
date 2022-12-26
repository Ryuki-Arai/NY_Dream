using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Playerの移動とアイテム取得時の動作を管理するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("プレイヤーの飛翔度調整用値")]
    float _power = 10;
    Rigidbody2D _rb2d;
    Vector2 _pos;
    bool _isPushed;

    public bool IsPushed => _isPushed;
    public Vector2 Pos { get => _pos;}

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        PlayerMove();
    }
    /// <summary>
    /// Playerの移動を行う関数 FixedUpdateで動かすように
    /// </summary>
    void PlayerMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(transform.up * _power, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ItemBase持ちだけに反応しActionを呼ぶ
        if(collision.TryGetComponent<ItemBase>(out var item))
        {
            item.ItemAction();
        }
    }
}
