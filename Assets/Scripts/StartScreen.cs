using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartScreen : MonoBehaviour
{
    public Button btnStart;

    // Start is called before the first frame update
    void Start()
    {
        btnStart.onClick.AddListener(OnStartClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStartClicked()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
