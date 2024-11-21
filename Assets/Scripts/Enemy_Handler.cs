using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy_Handler : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int maxhp = 5;
    [SerializeField] private int damage = 1;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackrange = 2f;
    [SerializeField] private LayerMask enemymask = 7;
    [SerializeField] private LayerMask allymask = 8;
    int founddir = 0; 
    public int _hp;
    int oldhp;
    float direction = -1;
    bool dead = false;
    float olddir;
    
    int enemymassk;
    void Start()
    {
        enemymassk = enemymask;
        _hp = maxhp;
        oldhp = _hp;
        if (rb==null){
            rb=gameObject.GetComponent<Rigidbody2D>();
        }
        if (animator == null){
            animator=gameObject.GetComponent<Animator>();
        }
        olddir = direction;
    } 
    void OnPlayerFound(int Dir)
    {
        direction = 0;
        founddir = Dir;
        gameObject.GetComponent<SpriteRenderer>().flipX = Dir > 0;
        animator.SetTrigger("Attack");
    }
    void FixedUpdate(){
        if (!dead){ 
            leftcheck = Physics2D.Raycast(transform.position,-transform.right*attackrange,attackrange,~allymask);
            rightcheck = Physics2D.Raycast(transform.position,transform.right*attackrange,attackrange,~allymask);
            if (leftcheck.collider != null){
                if (leftcheck.collider.gameObject.layer == 7){
                    OnPlayerFound(-1);
                }
            } 
            if (rightcheck.collider != null){
                if (rightcheck.collider.gameObject.layer == 7)
                {
                    OnPlayerFound(1);
                }
            } 
            rb.velocity = new Vector2(direction*speed,rb.velocity.y);
        }
    }

    RaycastHit2D leftcheck;
    RaycastHit2D rightcheck;
    void CheckStatus()
    {
        animator.SetBool("Walking", direction!=0);
        gameObject.GetComponent<SpriteRenderer>().flipX = direction == 1;
    }
    void Update()
    {
        if (_hp<oldhp) {
            OnHit();
            }
        if (!dead){
            CheckStatus();
        }
        oldhp = _hp;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("AI_Turn") == true){
            direction = -direction;
            olddir = direction;
        }
    }
    void OnHit(){
        direction = 0;
        if (_hp > 0){
            animator.SetTrigger("Hurt");
        }else {
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            direction = 0;
            animator.SetTrigger("Dying");
            dead = true;
            Destroy(gameObject,2);
        }
    }
    RaycastHit2D hit;
    public void AtackTrigger(){
            hit = Physics2D.Raycast(transform.position,founddir*transform.right*attackrange,attackrange,~allymask);
            if (hit.collider != null){
                if (hit.collider.gameObject.TryGetComponent<Player_Control>(out Player_Control Player_Control)){
                    Player_Control._hp -= damage;
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(10*founddir,10);
                }
            }
    }
    public void ReturnSpeed(){
        direction=olddir;
    }
}
