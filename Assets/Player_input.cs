using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro; //texto
using UnityEngine.UI;
using Unity.VisualScripting; //imagem

public class Player_input : MonoBehaviour
{
    public FixedJoystick joystick;
    public GameObject painelVencer;
    public TMP_Text coinsText;
    public GameObject painel;
    private Rigidbody2D rbplayer;
    public GameObject light;
    public float speed = 6;
    public static int coinsCont;
    public static int contPerguntas = 10;
    float beSpeed;


    public Animator anim;
    Vector2 lookdirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {

        Screen.autorotateToLandscapeLeft = true;
      
        
        
        Screen.autorotateToLandscapeRight = false;


        rbplayer = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {

        if (painelVencer.activeSelf)
        {
            rbplayer.velocity = Vector2.zero;
            return; // Retorna para não executar o resto do código
        }

        if (!painel.activeSelf && speed == 0)
        {
            speed = beSpeed;
        }

        
    }

    void MovePlayer()
    {
        //float verticalMoviment = Input.GetAxis("Vertical");
        // float horizontalMoviment = Input.GetAxis("Horizontal");
        float verticalMoviment = joystick.Vertical;
        float horizontalMoviment = joystick.Horizontal;

        Vector2 direction = new Vector2(horizontalMoviment, verticalMoviment);

        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
        anim.SetFloat("Speed", direction.magnitude);
        if (horizontalMoviment != 0)

        {
            anim.SetFloat("ultimaDirecao", horizontalMoviment); // 1 para direita, -1 para esquerda
        }

        

        if (!Mathf.Approximately(direction.x , 0f)|| !Mathf.Approximately(direction.y, 0f))
        {
            lookdirection.Set(direction.x, direction.y);
            
        }

        if (coinsCont != contPerguntas)
        {
            rbplayer.velocity = new Vector2(horizontalMoviment * speed, rbplayer.velocity.y);
            if (horizontalMoviment != 0 && verticalMoviment != 0)
            {
                rbplayer.velocity = new Vector2(rbplayer.velocity.x, verticalMoviment * (speed / 1.2f));
            }
            else if (horizontalMoviment == 0)
            {
                rbplayer.velocity = new Vector2(rbplayer.velocity.x, verticalMoviment * speed);
            }
        }
        else
        {
            rbplayer.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coins"))
        {
            light.SetActive(false);
            painel.SetActive(true);
            Destroy(other.gameObject);
            beSpeed = speed;
            speed = 0;
        }
    }
}
