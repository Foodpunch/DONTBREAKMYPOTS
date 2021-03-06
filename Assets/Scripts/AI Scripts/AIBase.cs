﻿using UnityEngine;
using System.Collections;


public abstract class AIBase : MonoBehaviour {
    //This script contains all the BASE variables ALL AI will have
    //Short makes the CPU have overhead because it needs to mask the first 16bits. But short saves memory at the expense of CPU

    // PRIVATE CLASSES FOR CHILD CLASS TO DEFINE
    private DamageClass attack;                 //Struct for Attack (Physical, Elemental)
    private DamageClass defense;                //Struct for Defense (Physical, Elemental)
    private Element element;                    //Elemental Damage AI does

    // VARIABLES FOR AI (stats)
    protected int health = 5;                               //Health AI has
    protected int PAttack = 5;                              //Physical Attack
    protected int EAttack = 1;                              //Elemental Attack
    protected int PDefense = 5;                             //Physical Defense
    protected int EDefense = 1;                             //Elemental Defense
    protected float attackSpeed = 2f;                       //Rate of which AI attacks (hits per sec)
    protected int moveSpeed = 3;                            //Rate of which AI moves
    protected Element.Type _element = Element.Type.NULL;    //Type of element AI has


    // VARIABLES FOR AI LOGIC (Logic)
    protected float stateTimer = 0f;                    //Timer for the statemachine
    protected float idleWaitTimer = 3f;                 //how long the idle animation should wait for
    protected short moveCount;                          //Counts how many directions AI has moved (to make it more realistic)
    protected short moveLimit;                          //Limit check to randomise how much AI can move
    protected float aggroRange = 15f;                   //Range before AI detects player
    [SerializeField] protected float attackRange = 1f;                   //Range before AI Attacks (not an actual AI stat, but more for checking)
    protected Transform _player;                        //Transform of the player
    protected enum StateMachine                         //Enum containing all possible AI States
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        IDLE,
        ATTACK,
        DEATH
    }
   [SerializeField] protected StateMachine AIStates;                   //Enum states for AI
   protected StateMachine prevState;                                   //Stores previous state for attack animation stuff
   [SerializeField] protected bool hasToFlipAnim = true;               //if the AI has to flip its animation
   [SerializeField] protected bool isFacingRight;                      //Checks if the AI is facing right or left
    
    // COMPONENTS IN AI
    protected Transform _AITransform;                //transform of the AI
    protected Animator _anim;                        //Animator for AI
    protected SpriteRenderer _spriteRenderer;        //Sprite of the AI
    [SerializeField] protected string[] animNames;   //Names for animations (0:Up 1:Down 2:Left 3:Right 4: Idle 5:Death 6:Extra)
    [SerializeField] protected string[] attAnims;    //Names for the attack animations (0:up 1:down 2:left 3:right)
    protected Rigidbody2D _rigidBody;                //RigidBody for AI


    //HP bar class?


    //ABSTRACT FUNCTIONS
    protected abstract void AttackAbility();
    protected abstract void SpecialAbility();


    // Use this for initialization
    protected virtual void Awake () {
        //Instantiate classes first
        attack = new DamageClass(PAttack, EAttack);
        defense = new DamageClass(PDefense, EDefense);
        element = new Element(_element);
        //Get the components
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _AITransform = GetComponent<Transform>();
	}

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();     //gets transform of player
        AIStates = StateMachine.IDLE;                                                       //sets AI to idle first
        moveLimit = (short)Random.Range(1, 4);
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        PlayAnim(AIStates);
        AIPathFinding();
	}
    protected virtual void AIPathFinding()
    {
        stateTimer += Time.deltaTime;

        switch (AIStates)
        {
            //AI Action States
            case StateMachine.IDLE:     //AI's state upon init (plays down Anim)
                //AI idles for 3 seconds before moving randomly if player not in range
                //if the AI can attack (which means he is close enough to the AI, he attacks after waiting)
                if(canAttack)
                {
                    if (stateTimer > idleWaitTimer)
                    {
                        AIStates = StateMachine.ATTACK;
                        stateTimer = 0f;
                    }
                }
                if(stateTimer > idleWaitTimer && !isAggro && !canAttack)
                {
                    AIStates = (StateMachine)Random.Range(0, 3); //picks a random state from up/down/left/right
                    stateTimer = 0;
                }
                else if(isAggro && !canAttack)          
                {
                    AIChaseLogic();
                }
                break;
            case StateMachine.ATTACK:   //Contains Attack Logic (play Attack Anim)
                if(stateTimer > attackSpeed && isAggro && canAttack)  
                {
                    AIStates = StateMachine.IDLE;
                    stateTimer = 0f;
                }
                else if (!canAttack || !isAggro) //if player is out of range of attack or aggro
                {
                    AIStates = StateMachine.IDLE; //it's really hard for the player to immediately exit aggro range o
                    stateTimer = 0f;              //resets statetimer for idle (it'll most probably just continue chasing the player anyway)
                   
                }
                
             
                break;
            case StateMachine.DEATH:    //Contains Death Logic  (play Death Anim)
               
                break;

            //AI Movement States
            case StateMachine.UP:
            if(isAggro)
                {
                    //Check if within range of attack
                    if(canAttack)
                    {
                        //Store the previous movement state before it switches. Then
                        prevState = AIStates;
                        //if the AI is close enough to the player, ATTACK!
                        AIStates = StateMachine.ATTACK;
                       stateTimer = 0f;
                    }
                    //pathfinding logic
                    float yDiff = _player.position.y- _AITransform.position.y;
                    if(yDiff < 1f && yDiff > -1f) //how close AI has to be to the player before it changes state
                    {
                        //Check for left right
                        if(_AITransform.position.x < _player.position.x) //if AI is to the left of player
                        {
                            AIStates = StateMachine.RIGHT; //move right
                        }
                        else //if not
                        {
                            AIStates = StateMachine.LEFT; //move left
                        }
                    }
                    else
                    {
                        transform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    }
                }
            else
                {
                    //normal movement logic
                    transform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    ChangeState((int)AIStates);
                }
                break;
            case StateMachine.DOWN:
                if (isAggro)
                {
                    //Check if within range of attack
                    if (canAttack)
                    {
                        //Store the previous movement state before it switches. Then
                        prevState = AIStates;
                        //if the AI is close enough to the player, ATTACK!
                        AIStates = StateMachine.ATTACK;
                       stateTimer = 0f;
                    }
                    //pathfinding logic
                    float yDiff = _player.position.y - _AITransform.position.y;
                    if (yDiff < 1f && yDiff > -1f) //how close AI has to be to the player before it changes state
                    {
                        if (_AITransform.position.x < _player.position.x) //horizontal check if player is to left or right of AI
                        {
                            AIStates = StateMachine.RIGHT; //if AI is to the left of player, move Ai right
                        }
                        else 
                        {
                            AIStates = StateMachine.LEFT; //if AI is to the right of the player, move AI left
                        }
                    }
                    else 
                    {
                        transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    }
                }
                else
                {
                    //normal movement logic
                    transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    ChangeState((int)AIStates);
                }
                break;
            case StateMachine.LEFT:
                isFacingRight = false;
                if (isAggro)
                {
                    //Check if within range of attack
                    if (canAttack)
                    {
                        //Store the previous movement state before it switches. Then
                        prevState = AIStates;
                        //if the AI is close enough to the player, ATTACK!
                        AIStates = StateMachine.ATTACK;
                        stateTimer = 0f;
                    }
                    //pathfinding logic
                    float xDiff = _player.transform.position.x- _AITransform.position.x;
                    if(xDiff < 1f && xDiff > -1f)           //if AI is close enough to player horizontally
                    {
                        AIChaseLogic(); //checks for updown again
                    }
                    else
                    {
                        transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    }
                }
                else
                {
                    //normal movement logic
                    transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    ChangeState((int)AIStates);
                }

                break;
            case StateMachine.RIGHT:
                //Animation for the sprite needs to be flipped when it walks to the right
                isFacingRight = true; //flips the sprite
                if (isAggro)
                {
                    //Check if within range of attack
                    if (canAttack)
                    {
                        //Store the previous movement state before it switches. Then
                        prevState = AIStates;
                        //if the AI is close enough to the player, ATTACK!
                        AIStates = StateMachine.ATTACK;
                        stateTimer = 0f;
                    }
                    //pathfinding logic
                    float xDiff = _AITransform.position.x - _player.transform.position.x;
                    if (xDiff < 1f && xDiff > -1f)           //if AI is close enough to player horizontally
                    {
                        AIChaseLogic(); //checks for updown again
                    }
                    else
                    {
                        transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    }
                }
                else
                {
                    //normal movement logic
                    transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    ChangeState((int)AIStates);
                }
                break;
        }
        if(moveCount > moveLimit)       //move limiter to reset the movement back to idle 
        {
            moveLimit = (short)Random.Range(1, 4);      //picks new move limit
            moveCount = 0;                              //resets move counter
            stateTimer = 0;                             //resets state timer since state is changed
            AIStates = StateMachine.IDLE;               //sets state to idle
        }
    }
    protected virtual void AIChaseLogic()
    {
        if (isAggro)
        {    
            if (_AITransform.position.y < _player.position.y)
            {
                AIStates = StateMachine.UP;
            }
            else
            {
                AIStates = StateMachine.DOWN;
            }

        }
        else //reset the statemachine
        {
            stateTimer = 0;
            moveCount = 0;
            AIStates = StateMachine.IDLE;
        }
        
    }
    protected void ChangeState(int currState)
    {
        float randWaitTime = Random.Range(1, idleWaitTimer-1);
        if(stateTimer > randWaitTime)
        {
            stateTimer = 0;
            moveCount++;
            AIStates = (StateMachine)((Random.Range(1, 3) + currState) % 4); //makes sure it doesn't pick itself again
        }
    }

    protected virtual void PlayAnim(StateMachine _state)  //plays appropriate animations
    {
       if(_state != StateMachine.ATTACK)        //if current state is not attack state
        {
            _anim.Play(animNames[(int)_state]); //play NON ATTACK animations
        }
       else                                     //else if it's in attack state
        {
            _anim.Play(attAnims[(int)prevState]);   //play attack animations
        }
        if(hasToFlipAnim)                                       //if the animation has to be flipped
        {
            _spriteRenderer.flipX = isFacingRight;              //flip the animation!
        }
    }
    protected bool CheckCollision
    {
        get {
            if (Physics2D.Raycast(transform.position, Vector2.up * 2))
                return true;
            if (Physics2D.Raycast(transform.position, Vector2.down * 2))
                return true;
            else return false;



        }
    }
    protected bool isAggro
    {
      get { return DistanceBetween(_player.position, _AITransform.position) < aggroRange ? true : false; }
    }
    protected bool canAttack
    {
        get { return DistanceBetween(_AITransform.position, _player.transform.position) < attackRange ? true : false; }
    }

    protected float DistanceBetween(Vector2 A, Vector2 B) //function to calculate distance between 2 Vector2's
    {
        return Vector2.Distance(A, B);
    }
    protected float DistanceBetween(Vector3 A, Vector3 B) //overloaded functon to calculate distance between 2 Vector3's
    {
        return Vector3.Distance(A, B);
    }
}
