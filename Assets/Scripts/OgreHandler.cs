using System.Collections; //To allow the use of IEnumerators
using UnityEngine; //Required for Unity connection
using UnityEngine.AI; //To allow use of AI features such as NavMesh Agents

public class OgreHandler : MonoBehaviour
{
    #region Variables
    //Variables for Ogre Character
    private NavMeshAgent _ogreAgent;
    private Animator _ogreAnim;
    private string _ogreState = "Heist";
    //Variable for human character to be used as a proximity check
    [Header("Game Objects")]
    [Tooltip("Add the Human character object here")]
    [SerializeField] private GameObject _human;
    //Variables for navpoints
    [Tooltip("Add the RaidGate object here")]
    [SerializeField] private GameObject _heistPoint;
    [Tooltip("Add the FleePoint object here")]
    [SerializeField] private GameObject _fleePoint;
    #endregion
    void Start()
    {
        //Get components for Agent and Animator
        _ogreAnim = GetComponent<Animator>(); 
        _ogreAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //Change animation based on speed of movement
        if (_ogreAgent.velocity.magnitude < 0.01f)
        {
            _ogreAnim.SetBool("isWalking", false);
            _ogreAnim.SetBool("isRunning", false);
        }
        else if (_ogreAgent.velocity.magnitude < 5f)
        {
            _ogreAnim.SetBool("isWalking", true);
            _ogreAnim.SetBool("isRunning", false);

        }
        else
        {
            _ogreAnim.SetBool("isWalking", false);
            _ogreAnim.SetBool("isRunning", true);
        }
    }
    #region Ogre States
    //Select state based off _ogreState value. First activation will be from MutantHandler class
    public void SelectState(string state)
    {
        switch (state)
        {
            case "Heist":
                StartCoroutine(Heisting());
                break;
            case "Flee":
                StartCoroutine(Fleeing());
                break;
            case "Escaped":
                StartCoroutine(Escaped());
                break;
                default:
                Debug.Log("Case Not Set");
                break;
        }
    }
    IEnumerator Heisting()
    {
        //Set destination to point near gate guarding crypt
        _ogreAgent.SetDestination(_heistPoint.transform.position);
        //Set speed to walking speed
        _ogreAgent.speed = 4f;
        //While we are heisting, check for proximity of human character and switch to flee state if he is close
        while (_ogreState == "Heist")
        {
            if (Vector3.Distance(transform.position, _human.transform.position) < 10f)
            {
                _ogreState = "Flee";
            }
            yield return null;
        }
        //Activate SelectState function with current value of _ogreState
        SelectState(_ogreState);
    }
    IEnumerator Fleeing()
    {
        //Set destination to escape point
        _ogreAgent.SetDestination(_fleePoint.transform.position);
        //Set speed to run speed
        _ogreAgent.speed = 6f;
        //While we are fleeing, check if we have reached escape point and change state to escaped when we have
        while (_ogreState == "Flee")
        {
            if (Vector3.Distance(_fleePoint.transform.position, transform.position) < 0.05f)
            {
                _ogreState = "Escaped";
            }
            yield return null;
        }
        //Activate SelectState function with current value of _ogreState
        SelectState(_ogreState);
    }
    IEnumerator Escaped()
    {
        //Set animation to dancing to rub in the fact he escaped
        _ogreAnim.SetBool("isDancing", true);
        _ogreAnim.SetBool("isWalking", false);
        _ogreAnim.SetBool("isRunning", false);
        yield return null;
    }
    #endregion
}
