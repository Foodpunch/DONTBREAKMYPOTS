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
    protected float aggroRange = 15f;                   //Range before AI detects player
    protected Transform _player;                        //Transform of the player
    protected enum StateMachine                         //Enum containing all possible AI States
    {
        IDLE,
        ATTACK,
        DEATH,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    protected StateMachine AIStates;                   //Enum states for AI
    // COMPONENTS IN AI
    protected Transform _AITransform;             //transform of the AI
    protected Animator _anim;                     //Animator for AI
    protected Rigidbody _rigidBody;               //RigidBody for AI
    
    
    //HP bar class?


    //ABSTRACT FUNCTIONS
    protected abstract void SpecialAbility();


    // Use this for initialization
    protected virtual void Awake () {
        //Instantiate classes first
        attack = new DamageClass(PAttack, EAttack);
        defense = new DamageClass(PDefense, EDefense);
        element = new Element(_element);
	}

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();     //gets transform of player
        AIStates = StateMachine.IDLE;                                                       //sets AI to idle first
    }
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
    protected virtual void AIPathFinding()
    {
        switch(AIStates)
        {
            //AI Action States
            case StateMachine.IDLE:     //AI's state upon init (plays down Anim)
                break;
            case StateMachine.ATTACK:
                break;
            case StateMachine.DEATH:
                break;

            //AI Movement States
            case StateMachine.UP:
                break;
            case StateMachine.DOWN:
                break;
            case StateMachine.LEFT:
                break;
            case StateMachine.RIGHT:
                break;
        }
    }


    protected virtual float DistanceBetween(Vector2 A, Vector2 B) //function to calculate distance between 2 Vector2's
    {
        return Vector2.Distance(A, B);
    }
    protected virtual float DistanceBetween(Vector3 A, Vector3 B) //overloaded functon to calculate distance between 2 Vector3's
    {
        return Vector3.Distance(A, B);
    }
}
