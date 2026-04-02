using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // скорость движения персонажа
    [SerializeField] private float jumpForce = 10f; // сила прыжка
    private bool isGrounded = false; // переменная для проверки, на земле ли персонаж

    private Rigidbody2D rb; // компонент Rigidbody2D для работы с физикой
    private Animator anim; // компонент Animator для управления анимациями
    private SpriteRenderer sprite; // компонент SpriteRenderer для отображения спрайтов

    // В инспекторе можно выбрать, какие слои будут землей
    public LayerMask whatIsGround;
    // Позиция для проверки касания с землей
    public Transform groundCheck;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>(); // Получаем компонент SpriteRenderer для отображения спрайта
        anim = GetComponent<Animator>(); // Получаем компонент Animator для управления анимациями
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D для работы с физикой
    }

    private void Run()
    {
        float dir = Input.GetAxis("Horizontal"); // Получаем значение направления движения по оси X

        // Двигаем персонажа в сторону, в зависимости от направления
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y); // Используем velocity для физики

        // Поворачиваем спрайт в зависимости от направления движения
        if (dir != 0f)
        {
            sprite.flipX = dir < 0f; // Поворот спрайта: влево или вправо
        }
    }

    private void Update()
    {
        // Проверка состояния покоя
        if (isGrounded) State = States.idle; // Если персонаж на земле, состояние "покоя"

        // Если нажата кнопка перемещения по оси X, выполняем движение
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0f)
        {
            Run();
        }

        // Проверка состояния бега
        if (isGrounded) State = States.run; // Если персонаж на земле, состояние "бега"

        // Если нажата кнопка прыжка и персонаж находится на земле, выполняем прыжок
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        CheckGround(); // Проверяем, находится ли персонаж на земле
    }

    private void Jump()
    {
        // Применяем силу вверх для прыжка
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Останавливаем вертикальную скорость перед прыжком
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); // Прыжок
    }

    private void CheckGround()
    {
        // В инспекторе можно выбрать, какие слои будут землей
        // Проверяем, находится ли персонаж на земле с помощью области groundCheck
        isGrounded = Physics2D.OverlapPoint(groundCheck.position, whatIsGround);

        // Дополнительный способ проверки касания земли (по кругу):
        // Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        // isGrounded = collider.Length > 1;

        // Если персонаж не на земле, значит он в воздухе
        if (!isGrounded) State = States.jump; // Если не на земле, состояние "прыжок"
    }

    private void SetState(int value)
    {
        // С помощью этого метода обновляется состояние в аниматоре
        anim.SetInteger("state", value); // Обновляем состояние анимации
    }

    // Перечисление состояний
    public enum States
    {
        idle, // состояние покоя
        run,  // состояние бега
        jump  // состояние прыжка
    }

    private States State
    {
        get { return (States)anim.GetInteger("state"); } // Получаем значение состояния из аниматора
        set { anim.SetInteger("state", (int)value); } // Устанавливаем состояние в аниматоре
    }
}