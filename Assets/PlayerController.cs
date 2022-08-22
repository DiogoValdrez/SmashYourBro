using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //stats
    public float deaths = 0;//they are double  in the square because yes and capsule side
    protected float playerSpeed = 7.5f;
    protected float jumpHeight = 10;
    public int maxl = 6;

    //Keys
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode downKey = KeyCode.S;
    public KeyCode restartKey = KeyCode.R;

    //jump checks
    /*protected bool grounded = true;*/
    protected int jumpLeft = 0;
    private Vector3 validDirection = Vector3.up;  // What you consider to be upwards
    private float contactThreshold = 120;

    //helper vars
    protected float movVelx;
    protected float movVely;
    public Text endText;
    public GameObject endScreen;
    protected float time;
    protected bool canControl = true;

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 0.995f, GetComponent<Rigidbody2D>().velocity.y);
        movVelx = 0;
        movVely = 0;
        if (canControl)
        {
            //Jump
            if (Input.GetKeyDown(jumpKey))
            {
                if (jumpLeft > 0)
                {
                    jumpLeft--;
                    movVely = jumpHeight;
                    if (GetComponent<Rigidbody2D>().velocity.y + movVely <= jumpHeight)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y + movVely);
                    }
                    else if (GetComponent<Rigidbody2D>().velocity.y <= movVely)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movVely);
                    }

                }
            }
            //Go down faster
            if (Input.GetKeyDown(downKey))
            {
                movVely = -playerSpeed;
                if (GetComponent<Rigidbody2D>().velocity.y + movVely >= -jumpHeight)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y + movVely);
                }
                else if (GetComponent<Rigidbody2D>().velocity.y >= movVely)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movVely);
                }
            }

            //Left Right Movement
            if (Input.GetKey(leftKey))
            {
                movVelx = -playerSpeed;
                if (GetComponent<Rigidbody2D>().velocity.x + movVelx >= -playerSpeed)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + movVelx, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (GetComponent<Rigidbody2D>().velocity.x >= movVelx)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(movVelx, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            if (Input.GetKey(rightKey))
            {
                movVelx = playerSpeed;
                if (GetComponent<Rigidbody2D>().velocity.x + movVelx <= playerSpeed)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + movVelx, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (GetComponent<Rigidbody2D>().velocity.x <= movVelx)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(movVelx, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
        }
        
        if (Input.GetKey(restartKey))
        {
            RestartLvl();
        }
    }


    //Reset jumps if touches floor and death
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            if(contact.collider.tag == "Platform" || contact.collider.tag == "Character")
            {
                if (Vector3.Angle(contact.normal, validDirection) <= contactThreshold)
                {
                    jumpLeft = 2;
                    time = 0;
                }
            }else if(contact.collider.tag == "Death")
            {
                deaths++;
                contact.otherCollider.attachedRigidbody.position = new Vector2(0, 0);
                if (deaths >= maxl)
                {
                    endScreen.SetActive(true);
                    Time.timeScale = 0f;
                    endText.text = contact.otherCollider.name + " Lost!";
                }
            }
        }
    }

   //knock out if stay for 3 seconds
    protected void OnCollisionStay2D(Collision2D collision)
    {
        //foreach (ContactPoint2D contact in collision.contacts)
        //{
        if (collision.gameObject.tag == "Character")
        {
            time += Time.deltaTime;//é o dobro acho eu
            Debug.Log(time);

            if (time >= 1.5)
            {
                StartCoroutine(Pusher(collision));
            }
        }
    }

    IEnumerator Pusher(Collision2D collision)
    {
        canControl = false;

        // Calculate Angle Between the collision point and the player
        Vector2 teste = new Vector2(transform.position.x, transform.position.y);
        Vector3 dir = collision.contacts[0].point - teste;
        // We then get the opposite (-Vector3) and normalize it
        dir = -dir.normalized;
        Vector3 teste3 = new Vector3(0, 0.9f, 0);
        dir = dir - teste3;
        // And finally we add force in the direction of dir and multiply it by force. 
        GetComponent<Rigidbody2D>().AddForce(dir * 100, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);

        canControl = true;
    }

    protected void RestartLvl()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

//disable friction when shooting players away(maybe flag also? or seach google)
//falling on the ground disables collision bug