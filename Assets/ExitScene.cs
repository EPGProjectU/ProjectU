using ProjectU.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public string nextSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject._CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }     
}
