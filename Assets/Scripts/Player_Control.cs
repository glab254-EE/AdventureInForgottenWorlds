using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Control : MonoBehaviour
{
    [SerializeField] private float plrspeed = 5;
    [SerializeField] private float jumpforce = 2;
    [SerializeField] private float koyotetime = 0.1f;
    [SerializeField] private int maxjumpammount = 1;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private GameObject detectorgo;
    [SerializeField]  private int plrmaxhp = 1;
    public int _hp;
    private Rigidbody2D rb;
    private Animator animator;
    bool touchinggrnd = false;
    int jumpammount;
    float defaultmovespeed;
    bool dead = false;
    int oldhp;
    void Start()
    {
        jumpammount = maxjumpammount;
        _hp = plrmaxhp;
        defaultmovespeed = plrspeed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        oldhp = _hp;
    }

    float cspeed;
    float koyotetimer;
    bool IsGrounded() {
        Vector2 position = detectorgo.transform.position;
        Vector2 direction = Vector2.right;
        float lenght = 0.07f;
        
        RaycastHit2D hit = Physics2D.Raycast(position + new Vector2(-lenght, 0), direction, lenght, groundmask+LayerMask.GetMask("MovableObj"));
        if (hit.collider != null) {
            if (koyotetimer>0){
                koyotetimer = 0;
            }
            animator.SetBool("Jumping",false);
            jumpammount = maxjumpammount;
            return true;
        }
        
        return false;
    }
    void Jump()
    {
        jumpammount--;
        animator.SetBool("Jumping", true);
        rb.velocity = new Vector2(cspeed * plrspeed, jumpforce * 5);
    }
    void OnHurt()
    {
        animator.SetTrigger("Hurt");
        oldhp = _hp;
        if (_hp <= 0)
        {
            dead = true;
            DestroyImmediate(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void Update(){
        if (dead) return;
        touchinggrnd = IsGrounded();
        if (!touchinggrnd&&jumpammount>0){
            koyotetimer+=Time.deltaTime;
            if (koyotetimer>=koyotetime){
                jumpammount = maxjumpammount-1;
                koyotetimer = 0;
            }
        }
        if (cspeed >= .25 || cspeed <= -.25){
            animator.SetBool("Walking",true);
            if (cspeed<0){
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        } else {    
            animator.SetBool("Walking",false);
        }
        if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.W))&&jumpammount>0){
            Jump();
        }
        if (_hp<oldhp){
            OnHurt();
        }
    }
    void FixedUpdate(){
        if (dead) return;
        cspeed = Input.GetAxisRaw("Horizontal");
        RaycastHit2D hitright = Physics2D.Raycast(transform.position+new Vector3(0,0.1f),transform.right,0.6f,groundmask);
        RaycastHit2D hitleft = Physics2D.Raycast(transform.position+new Vector3(0,0.1f),-transform.right,0.6f,groundmask);    
        if ((hitright.collider && cspeed > 0)||(hitleft.collider && cspeed < 0)){
            cspeed = 0;
        }
        rb.velocity = new Vector2(cspeed*plrspeed,rb.velocity.y);
    }

}
