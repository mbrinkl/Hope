using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_0 : MonoBehaviour
{
    public Text helpText;

    // Start is called before the first frame update
    void Start()
    {
        helpText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Collision with a soul
        if (collision.gameObject.tag == "Interactable")
        {
            // Collision with the soul that changes scenes
            if (collision.gameObject.name == "Soul")
            {
                SceneManager.LoadScene("_Scene_1");
            }

            // Collision with Marah's soul
            else
            {
                helpText.text = "Proceed to the next soul";
                helpText.gameObject.SetActive(true);
            }
        }
    }
}
