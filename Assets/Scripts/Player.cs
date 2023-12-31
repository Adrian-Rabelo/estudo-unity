using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public bool isJumping;
    public bool doubleJump;
    private Rigidbody2D rig;
    private Animator anim;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        Move();
        Jump();
        CheckFall();
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;
        CheckDirections();

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                anim.SetBool("jump", true);
            }
            else
            {
                if (doubleJump)
                {
                    anim.SetBool("doubleJump", true);
                    rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    /**
     * Este método verifica quais as direções o jogador está se movendo
     * para poder alterar a direção e a animação do sprite
     */
    private void CheckDirections()
    {
        // * Andando pra direita
        if (Input.GetAxis("Horizontal") > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("walk", true);
        }


        // * Andando pra esquerda
        if (Input.GetAxis("Horizontal") < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("walk", true);
        }

        // * Parado
        if (Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("walk", false);
        }
    }

    /**
     * Este método verifica a velocidade do jogador no eixo Y
     * * Se ela for MENOR que 0, significa que o jogador está caindo, ou seja,
     * * o sprite do player aindo deverá ser chamado
     */
    private void CheckFall()
    {
        if (rig.velocity.y < 0)
        {
            anim.SetBool("doubleJump", false);
            anim.SetBool("fall", true);
        }
        else
        {
            anim.SetBool("fall", false);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = false;
            anim.SetBool("fall", false);
            anim.SetBool("jump", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = true;
        }
    }
}
