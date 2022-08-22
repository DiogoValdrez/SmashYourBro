using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{
    public Text txtObject;
    public GameObject playerObject;
    private PlayerController playerController;
    private void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }
    private void Update()
    {
        txtObject.text = ((playerController.maxl- playerController.deaths)/2).ToString();//divide by 2 because bug
    }
    //adicionar ecra de inicio e fim, deixar as vidas ligadas no ecra de fim, apenas por um ecra meio transparente escuro.
}
