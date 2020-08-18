using System;
using System.Collections;
using System.Collections.Generic;
using GameServer;
using GameServer.SharedData;
using UnityEngine;

public class MarioCharacterController : MonoBehaviour {

    public float motionSpeed;
    public float jumpForce;

    bool isMoving;


    public GameObject box;
    
    
    public GameObject curBox;

    private bool isBoxMoving;
    
    public RoomClientSimulator client;

    public bool isMainRole;
    
    public Vector2 launchDir = Vector2.right;

    private float input_V;
    private float input_H;
    public float angle =0;


    public float leftTime ;

    private bool canClick;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        
        isMoving = false;

        // Horizontal Motion

            if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.LeftArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    angle -= 3f;
                }

                 if (Input.GetKey(KeyCode.LeftArrow))
                 {
                     angle += 3f;
                 }
                
                 if (angle >180)
                     angle = angle-360;
                 
                 if(angle<-180)
                     angle = 360 + angle;
                 //Vector3 targetDir = launchDir ;
                 //float angle = Vector3.Angle( targetDir, transform.forward );
                 GameManager.Instance.arrow.transform.rotation = Quaternion.Euler (0f, 0f, angle);
            }
            
       

      
            if(Input.GetKey(KeyCode.D)) {
                isMoving = true;
                this.transform.Translate(Vector3.right * motionSpeed);
                this.GetComponent<SpriteRenderer>().flipX = true;
            }

            if(Input.GetKey(KeyCode.A)) {
                isMoving = true;
                this.transform.Translate(Vector3.left * motionSpeed);
                this.GetComponent<SpriteRenderer>().flipX = false;
            }

            this.GetComponent<Animator>().SetBool("MarioIsMoving", isMoving);
            // Jump
            if(Input.GetKeyDown(KeyCode.Space)&&canClick)
            {
                canClick = false;
               leftTime = Time.time;
                this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                this.GetComponent<Animator>().SetBool("MarioIsOnFloor", false);
            }
       

        if (Time.time - leftTime>2)
        {
            canClick = true;
        }


         
            // 抛掷
            if (Input.GetKeyUp(KeyCode.J))
            {
                if (isBoxMoving == false)
                {
                    if (GameManager.Instance.coinBoxNum > 1)
                    {
                        GameManager.Instance.coinBoxNum -= 1;
                    }
                    curBox = Instantiate(box) as GameObject;
                    curBox.transform.localPosition =
                        new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
                    curBox.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)Math.Cos(angle*3.1415/180), (float)Math.Sin(angle*3.1415/180)).normalized * 7, ForceMode2D.Impulse);
                    var boxPos = new GameBox();
                    boxPos.id = GameManager.Instance.boxs.Count;
                    boxPos.x = curBox.transform.position.x;
                    boxPos.y = curBox.transform.position.y;
                    GameManager.Instance.boxs.Add(boxPos);
                    GameManager.Instance.boxsDic.Add(  boxPos.id,curBox);
                    
                    
                    isBoxMoving = true;
                }else if(isBoxMoving == true)
                {
                    curBox.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    curBox.GetComponent<Box>().StartTime();
                    isBoxMoving = false;
                }
            }
            
            if (GameManager.Instance.boxs != null )
            {
                for (int i = 0; i <  GameManager.Instance.boxs.Count ; i++)
                {
                    var curBox = GameManager.Instance.boxs[i];
                    if (curBox != null)
                    {
                        GameObject curBoxObj;
//                        if (GameManager.Instance.boxsDic.TryGetValue(curBox.id, out curBoxObj))
//                        {
//                            curBox.x = curBoxObj.transform.position.x;
//                            curBox.y = curBoxObj.transform.position.y;
//
//                        }
                    }
                   
                }
            }
        
        
        
        
       

        //client.SetRotation (input.x, input.y);
        //移动指令
        if (client != null)
        {
            if (GameManager.Instance.roleType == GameManager.RoleType.move)
            {
                //client.Move(transform.position.x, transform.position.y);
                
            }

            if (GameManager.Instance.roleType == GameManager.RoleType.launch)
            {
                if (GameManager.Instance.boxs != null&&GameManager.Instance.boxs.Count>0)
                {
                    //client.OnBoxsPos(GameManager.Instance.boxs,GameManager.Instance.coinBoxNum);
                   
                }
                client.LaunchDir(angle);
                
                GameManager.Instance.txt_coinBoxNum.text = GameManager.Instance.coinBoxNum.ToString();
            }
        }

        if (transform.position.y < -50)
        {
            GameManager.Instance.gameOver.SetActive(true);
        }
       
        
        
        //CubeController.Instance.UpdatePosition(transform.position.x,transform.position.y,transform.position.z) ;
    }

    public void SetPosition(float x, float y)
    {
        if(GameManager.Instance.roleType!= GameManager.RoleType.move)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        this.GetComponent<Animator>().SetBool("MarioIsOnFloor", true);
        if (other.collider.tag == "fire")
        {
            GameManager.Instance.gameOver.SetActive(true);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Floor")
        {
             isBoxMoving = false;
             if (curBox!=null)
             {
                 curBox.GetComponent<Box>().StartTime();
             }
        }

        if (other.tag=="CoinBox")
        {
            Destroy(other);
        }

        if (other.tag == "door")
        {
            GameManager.Instance.winPanel.SetActive(true);
        }
        
        if (other.tag == "fire")
        {
            Debug.Log("  fire     ");
            GameManager.Instance.gameOver.SetActive(true);
        }
        
    }
}
