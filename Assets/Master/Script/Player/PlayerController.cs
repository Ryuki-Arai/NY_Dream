using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
/// <summary>
/// Player�̈ړ��ƃA�C�e���擾���̓�����Ǘ�����R���|�[�l���g
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField, Header("�v���C���[�̔��ēx�����p�l")]
    float _power = 10;
    Rigidbody2D _rb2d;
    Vector2 _pos;
    ReactiveProperty<bool> _isPushed = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsPushed => _isPushed;
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
    /// Player�̈ړ����s���֐� FixedUpdate�œ������悤��
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
        //ItemBase���������ɔ�����Action���Ă�
        if(collision.TryGetComponent<ItemBase>(out var item))
        {
            item.ItemAction();
        }
    }

    void OnDisable()
    {
        _isPushed.Dispose();
    }
}
