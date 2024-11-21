using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinArea : MonoBehaviour
{
    [SerializeField] private int SceneID;
    [SerializeField] private GameObject winframe;
    [SerializeField] private float winduration = 2.5f;
 
    float tick = 0;
    bool won = false;
    void Update()
    {
        if (won){
            winframe.SetActive(true);
            tick+=Time.deltaTime;
            if (tick>=winduration){
                SceneManager.LoadScene(SceneID);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.TryGetComponent<Player_Control>(out Player_Control NotUsed) == true){
            won = true;
        }
    }
}
