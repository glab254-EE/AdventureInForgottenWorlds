using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        if (rb==null) rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.TryGetComponent<Player_Control>(out Player_Control player) == true){
            player._hp = 0;
        }
    }
}
