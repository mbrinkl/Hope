using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_1 : MonoBehaviour
{
    public Text fragmentsText;
    public GameObject gate;

    private int fragmentsCount = 0;
    private int maxFragments = 6;

    // Start is called before the first frame update
    void Start()
    {
        fragmentsText.text = $"0 of {maxFragments}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Collision with a fragment
        if (collision.gameObject.tag == "Collectable")
        {
            // Destroy the fragment
            Destroy(collision.gameObject);

            // Update the fragment count UI
            fragmentsCount++;
            fragmentsText.text =  $"{fragmentsCount} of {maxFragments}";

            // Destroy the gate if all fragments are collected
            if (fragmentsCount == maxFragments)
                Destroy(gate);
        }

        // Collision with the soul (for now using a soul behind the gate as an
        //      entrypoint to a new scene for the bossfight)
        else if (collision.gameObject.tag == "Interactable")
        {
            SceneManager.LoadScene("_Scene_2");
        }
    }
}
