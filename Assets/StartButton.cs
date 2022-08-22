using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        gameObject.GetComponent<Button>().onClick.AddListener(StartLvl);
    }

    protected void StartLvl()
    {
        Time.timeScale = 1f;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}

