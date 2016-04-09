using UnityEngine;
using System.Collections;


public abstract class AIBase : MonoBehaviour {
    //This script contains all the BASE variables ALL AI will have

    // PRIVATE CLASSES FOR CHILD CLASS TO DEFINE
    private DamageClass attack;                 //Struct for Attack (Physical, Elemental)
    private DamageClass defense;                //Struct for Defense (Physical, Elemental)
    private Element element;                   //Elemental Damage AI does

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
    protected float aggroRange = 15f;                   //Range before AI detects player
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
    protected StateMachine AIStates;                   //Enum states for AI
    
    // COMPONENTS IN AI
    protected Transform _AITransform;             //transform of the AI
    protected Animator _anim;                     //Animator for AI
    [SerializeField] protected string[] animNames;//Names for animations (0:Up 1:Down 2:Left 3:Right 4: Idle 5:Attack 6:Death)
    protected Rigidbody2D _rigidBody;               //RigidBody for AI
    
    
    //HP bar class?


    //ABSTRACT FUNCTIONS
    protected abstract void SpecialAbility();


    // Use this for initialization
    protected virtual void Awake () {
        //Instantiate classes first
        attack = new DamageClass(PAttack, EAttack);
        defense = new DamageClass(PDefense, EDefense);
        element = new Element(_element);
        //Get the components
        _anim = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _AITransform = GetComponent<Transform>();
	}

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();     //gets transform of player
        AIStates = StateMachine.IDLE;                                                       //sets AI to idle first
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        PlayAnim(AIStates);
        AIPathFinding();
	}
    protected virtual void AIPathFinding()
    {
       // float playerRange = DistanceBetween(_player.position, _AITransform.position);
        stateTimer += Time.deltaTime;
        switch (AIStates)
        {
            //AI Action States
            case StateMachine.IDLE:     //AI's state upon init (plays down Anim)
                //AI idles for 3 seconds before moving randomly if player not in range
                if(stateTimer > idleWaitTimer && !isAggro())
                {
                    AIStates = (StateMachine)Random.Range(0, 3); //picks a random state from up/down/left/right
                    stateTimer = 0;
                }
                break;
            case StateMachine.ATTACK:   //Contains Attack Logic (play Attack Anim)
             
                break;
            case StateMachine.DEATH:    //Contains Death Logic  (play Death Anim)
               
                break;

            //AI Movement States
            case StateMachine.UP:
            if(isAggro())
                {
                    //pathfinding logic
                }
            else
                {
                    float randWaitTime = Random.Range(1, idleWaitTimer);
                    Debug.Log(randWaitTime);
                    transform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    if(stateTimer > randWaitTime)
                    {
                        stateTimer = 0;
                        AIStates = (StateMachine)Random.Range(1, 3); //makes sure it doesn't pick itself again
                    }
                }
                break;
            case StateMachine.DOWN:
                if (isAggro())
                {
                    //pathfinding logic
                }
                else
                {
                    float randWaitTime = Random.Range(1, idleWaitTimer);
                    transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    if (stateTimer > randWaitTime)
                    {
                        stateTimer = 0;
                        AIStates = (StateMachine)((Random.Range(1, 3)+1)%4); //makes sure it doesn't pick itself again
                    }
                }
                break;
            case StateMachine.LEFT:
                if (isAggro())
                {
                    //pathfinding logic
                }
                else
                {
                    float randWaitTime = Random.Range(1, idleWaitTimer);
                    transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    if (stateTimer > randWaitTime)
                    {
                        stateTimer = 0;
                        AIStates = (StateMachine)((Random.Range(1, 3) + 2) % 4); //makes sure it doesn't pick itself again
                    }
                }

                break;
            case StateMachine.RIGHT:
                if (isAggro())
                {
                    //pathfinding logic
                }
                else
                {
                    float randWaitTime = Random.Range(1, idleWaitTimer);
                    transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime; //fixex delta for physics shenanigans
                    if (stateTimer > randWaitTime)
                    {
                        stateTimer = 0;
                        AIStates = (StateMachine)((Random.Range(1, 3) + 3) % 4); //makes sure it doesn't pick itself again
                    }
                }
                break;
        }
    }
    protected virtual void AIMovementLogic()
    {

    }

    protected virtual void PlayAnim(StateMachine _state)
    {
        Debug.Log((int)_state);
        _anim.Play(animNames[(int)_state]);
    }
    protected bool isAggro()
    {
      return DistanceBetween(_player.position, _AITransform.position) < aggroRange ? true : false;
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
