using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private int sceneindex;
    [SerializeField] private Button button;
    void OnButtonClick()
    {
        SceneManager.LoadScene(sceneindex);
    }

    void Start() {
        button.onClick.AddListener(OnButtonClick);
    }
}
