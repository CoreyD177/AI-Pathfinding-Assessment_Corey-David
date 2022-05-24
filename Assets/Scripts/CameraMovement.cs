using UnityEngine; //Required for Unity connection
//Require a CharacterController to be attached to the Camera
[RequireComponent(typeof(CharacterController))]
public class CameraMovement : MonoBehaviour
{
    #region Variables
    //Vector2 Variable to store inputs for movement & Vector3 to transform the cameras position
    private Vector2 _camMovement;
    private Vector3 _camDirection;
    //CharacterController variable to handle movement based off _camDirection value
    private CharacterController _charC;
    //Speed of movement
    private float _speed = 10f;
    //Pause panel game object
    [Header("Game Objects")]
    [Tooltip("Add the pause panel here")]
    [SerializeField] private GameObject _pausePanel;
    #endregion
    void Start()
    {
        //Start game with time paused as menu will be overlayed
        Time.timeScale = 0;
        //Retrieve the CharacterController component from the Camera
        _charC = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Run pause function if we press the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        //Rotate the camera based off of mouse movement
        if (Input.GetAxis("Mouse X") !=0 || Input.GetAxis("Mouse Y") != 0)
        {
            transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
        //Reset Z Rotation so camera will not roll upside down on the z axis
        float z = transform.eulerAngles.z;
        transform.Rotate(0, 0, -z);
        //Store keyboard input to be used for movement
        _camMovement.x = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        _camMovement.y = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        //Transform input to a Vector3 for direction
        _camDirection = transform.TransformDirection(new Vector3(_camMovement.x, 0, _camMovement.y));
        //Move according to our Vector3 direction
        _charC.Move(_camDirection);
    }
    #region Pause/Exit
    //Pause function to be activated either by escape button or button on pause menu
    public void Pause()
    {
        //If time is already paused disable the pause panel, lock and hide cursor and resume time
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        //Else pause time, display pause panel, and show unlocked cursor
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    //EndGame function to be activated by button on pause menu
    public void EndGame()
    {
        //If we are playing in Unity Editor hitting exit will end play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //Quit the game
        Application.Quit();
    }
    #endregion
}
