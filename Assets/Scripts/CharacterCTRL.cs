using UnityEngine;
using System.Collections;

public class CharacterCTRL : MonoBehaviour {
    public int speed;
    public bool flip;
    public int hitPoints;
    private Animator charaAnim;
    private Rigidbody2D rigidBody2D;
    bool moving;
    bool facingRight;
    public enum PlayerFacing
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public PlayerFacing currDir;
	// Use this for initialization
	void Start () {
        charaAnim = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 currScale = transform.localScale;
        currScale.x *= -1;
        transform.localScale = currScale;
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            switch (currDir)
            {
                case PlayerFacing.DOWN:
                    charaAnim.Play("ATTACK DOWN");
                    break;
                case PlayerFacing.LEFT:
                    charaAnim.Play("ATTACK LEFT");
                    break;
                case PlayerFacing.RIGHT:
                    charaAnim.Play("ATTACK RIGHT");
                    break;
                case PlayerFacing.UP:
                    charaAnim.Play("ATTACK UP");
                    break;

            }
            
        }
            
    }
	
	// Update is called once per frame
	void LateUpdate () {
        float _x = Input.GetAxis("Horizontal");
        float _y = Input.GetAxis("Vertical");

        //Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 mousePosition = new Vector3(ray.origin.x, ray.origin.y, 0);

        Vector2 thisTransform = transform.position;
        Vector2 mouseHeading = thisTransform - mousePosition;
        float distanceFromMouse = mouseHeading.magnitude;
        Vector2 mouseDir = mouseHeading / distanceFromMouse;
        //Debug.Log(mouseDir);
        float lookDirX = mouseDir.x;
        float lookDirY = mouseDir.y;

        if (!moving)
        {
            if (flip)
            {
                if (lookDirX > 0 && !facingRight)
                {
                    FlipSprite();
                }
                else if (lookDirX < 0 && facingRight)
                {
                    FlipSprite();
                }
            }
            
            if (lookDirX > 0.5f)
            {
                if (flip)
                {
                    //FlipSprite();
                    currDir = PlayerFacing.LEFT;
                    charaAnim.SetInteger("_state", 6);
                }
                else
                {
                    charaAnim.SetInteger("_state", 5);
                    currDir = PlayerFacing.RIGHT;
                }

            }
            else if (lookDirX < -0.5f)
            {
                if (flip)
                {
                    //FlipSprite();
                    currDir = PlayerFacing.RIGHT;
                    charaAnim.SetInteger("_state", 6);
                }
                else
                {
                    charaAnim.SetInteger("_state", 5);
                    currDir = PlayerFacing.LEFT;
                }
                
            }
            else if (lookDirY > 0.5f)
            {
                currDir = PlayerFacing.DOWN;
                charaAnim.SetInteger("_state", 0);
            }
            else if (lookDirY < -0.5f)
            {
                currDir = PlayerFacing.UP;
                charaAnim.SetInteger("_state", 7);
            }
        }
        

        if (_x < 0.5f && _x > -0.5f)
        {
            _x = 0;
            //charaAnim.SetInteger("_state", 0);
        }
        if (flip)
        {
            if (_x > 0 && !facingRight)
            {
                //FlipSprite();
            }
            else if (_x < 0 && facingRight)
            {
                //FlipSprite();
            }
        }



        if (_y < 0.5f && _y > -0.5f)
        {
            _y = 0;
            //charaAnim.SetInteger("_state", 0);
        }
        if(_x < -0.5f)
        {
            
            if (flip)
            {
                currDir = PlayerFacing.LEFT;
                charaAnim.SetInteger("_state", 2);
                if(transform.localScale.x > 0)
                {
                    FlipSprite();
                }
                
            }
            else
            {
                currDir = PlayerFacing.RIGHT;
                charaAnim.SetInteger("_state", 4);
            }
        }
        else if (_x > 0.5f)
        {
            if (flip)
            {
                currDir = PlayerFacing.RIGHT;
                charaAnim.SetInteger("_state", 2);
                if (transform.localScale.x < 0)
                {
                    FlipSprite();
                }
                //FlipSprite();
            }
            else
            {
                currDir = PlayerFacing.RIGHT;
                charaAnim.SetInteger("_state", 4);
            }
            
        }
        if (_y < -0.5f)
        {
            currDir = PlayerFacing.DOWN;
            charaAnim.SetInteger("_state", 1);
        }
        else if (_y > 0.5f)
        {
            currDir = PlayerFacing.UP;
            charaAnim.SetInteger("_state", 3);
        }
        if(rigidBody2D.velocity.x == 0 && rigidBody2D.velocity.y == 0)
        {
            moving = false;
        }
        Vector2 move = new Vector2(_x, _y);
        rigidBody2D.velocity = move * speed ;
    }
}
