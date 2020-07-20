using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement speed in units per second
    private float movementSpeed = 5f;
    public GameObject paloma;
    private Touch touch;
    private GameObject[] vocesMuerte = new GameObject[10];
    private float speedModifier = 0.01f;
    private DataBase databaseAcces;
    private bool muertoPigeon = false;

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.tag == "car" && !muertoPigeon)
        {
            muertoPigeon = true;
            var audioInt = Random.Range(0, 9);
            Debug.Log("numero random: " + audioInt);
            //Debug.Log("MUERTE " + collidedObject.tag);
            vocesMuerte[audioInt].transform.position = transform.position;
            vocesMuerte[audioInt].GetComponent<AudioSource>().Play();
            //collidedObject.SendMessage("hitByPlayerBullet", null, SendMessageOptions.DontRequireReceiver);
            Destroy(paloma);
            WaypointPatrol.muerto = true;
            GameStates.resetLvl = true;
        }

    }
    void Start()
    {
        databaseAcces = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DataBase>();
        vocesMuerte[0]= GameObject.Find("vozMuerte1");
        vocesMuerte[1] = GameObject.Find("vozMuerte1 (1)");
        vocesMuerte[2] = GameObject.Find("vozMuerte1 (2)");
        vocesMuerte[3] = GameObject.Find("vozMuerte1 (3)");
        vocesMuerte[4] = GameObject.Find("vozMuerte1 (4)");
        vocesMuerte[5] = GameObject.Find("vozMuerte1 (5)");
        vocesMuerte[6] = GameObject.Find("vozMuerte1 (6)");
        vocesMuerte[7] = GameObject.Find("vozMuerte1 (7)");
        vocesMuerte[8] = GameObject.Find("vozMuerte1 (8)");
        vocesMuerte[9] = GameObject.Find("vozMuerte1 (9)");
    }
    void Update()
    {
        //databaseAcces.GetPaloma();
        if (paloma.transform.position.y < -1 && !muertoPigeon)
        {
            muertoPigeon = true;
            var audioInt = Random.Range(0, 9);
            Debug.Log("numero random: " + audioInt);
            //Debug.Log("MUERTE " + collidedObject.tag);
            vocesMuerte[audioInt].transform.position = transform.position;
            vocesMuerte[audioInt].GetComponent<AudioSource>().Play();
            Destroy(paloma);
            WaypointPatrol.muerto = true;
            GameStates.resetLvl = true;

        }
        if (!GameStates.gameActive) return;
        //get the Input from Horizontal axis
        float horizontalInput = Input.GetAxis("Horizontal");
        //get the Input from Vertical axis
        float verticalInput = Input.GetAxis("Vertical");

        //update the position
        transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);

        //touch movil
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                                                    transform.position.x + touch.deltaPosition.x * speedModifier,
                                                    transform.position.y,
                                                    transform.position.z + touch.deltaPosition.y * speedModifier);
            }
        }
        
        //output to log the position change
        //Debug.Log(transform.position);
    }
}