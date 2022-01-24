using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMainMenu : MonoBehaviour
{

    float timer = 0f;

    // Update is called once per frame

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 10)
        {

            SceneManager.LoadScene(1);

        }

    }


}
