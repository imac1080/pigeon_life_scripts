using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace tutoriales.multiplayer
{
    public class PlayerMovPhoton : Photon.Pun.MonoBehaviourPun
    {
        //movement speed in units per second
        public TextMesh NameTag;
        private DataBaseMuli databaseAcces;
        public PalomasCountJugadoresTransfer listaPalomas;
        public Material materialLila;
        public Material materialVerde;
        public Material materialPaloma;
        public Material materialAzul;
        public Material colorPaloma;
        public Material materialRojo;
        private GameObject[] cubos = new GameObject[5];
        private GameObject[] vocesMuerte = new GameObject[10];
        public PalomasListas PalomasListasControler;
        public GameObject cuerpoPaloma ;
        public bool Active = true;
        public bool Player = false;
        private float movementSpeed = 5f;
        private int NumeroJugador;
        public GameObject paloma;
        private Touch touch;
        private float speedModifier = 0.01f;

        void OnTriggerEnter(Collider collidedObject)
        {
            if (collidedObject.tag == "car")
              PalomasListasControler = GameObject.FindGameObjectWithTag("revancha").GetComponent<PalomasListas>();
            if (collidedObject.tag == "car" && paloma.GetPhotonView().IsMine)
            {
                muerteAudioPaloma();
                Debug.Log("MUERTE " + collidedObject.tag);

                //collidedObject.SendMessage("hitByPlayerBullet", null, SendMessageOptions.DontRequireReceiver);
                //if (!deletedPigeon)
                //{
                    //if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null/* && SceneManager.GetActiveScene().buildIndex == 1*/)
                    //{
                        gavitiFalse();
                    //}
                    
                       // Photon.Pun.PhotonNetwork.Destroy(paloma);
                    //Destroy(paloma);
                    //deletedPigeon = true;
                    PalomasListasControler.MuertePalomaMetodo();
                //}
                
                    
                
            }else if (collidedObject.tag == "CuboLila")
            {
                cuerpoPaloma.GetComponent<Renderer>().material = materialLila;
                cubos[0].GetComponent<girarCubo>().GrirarMas1(4);
                colorPaloma = materialLila;
            }
            else if (collidedObject.tag == "CuboVerde")
            {
                cuerpoPaloma.GetComponent<Renderer>().material = materialVerde;
                cubos[1].GetComponent<girarCubo>().GrirarMas1(4);
                colorPaloma = materialVerde;
            }
            else if (collidedObject.tag == "CuboPaloma")
            {
                cuerpoPaloma.GetComponent<Renderer>().material = materialPaloma;
                cubos[2].GetComponent<girarCubo>().GrirarMas1(4);
                colorPaloma = materialPaloma;
            }
            else if (collidedObject.tag == "CuboAzul")
            {
                cuerpoPaloma.GetComponent<Renderer>().material = materialAzul;
                cubos[3].GetComponent<girarCubo>().GrirarMas1(4);
                colorPaloma = materialAzul;
            }
            else if (collidedObject.tag == "CuboRojo")
            {
                cuerpoPaloma.GetComponent<Renderer>().material = materialRojo;
                cubos[4].GetComponent<girarCubo>().GrirarMas1(4);
                colorPaloma = materialRojo;
            }
        }
        void Start()
        {
            databaseAcces = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DataBaseMuli>();
            listaPalomas = GameObject.Find("Palomas").GetComponent<PalomasCountJugadoresTransfer>();
            cubos[0] = GameObject.Find("CubeLila");
            cubos[1] = GameObject.Find("CubeVerde");
            cubos[2] = GameObject.Find("CubePaloma");
            cubos[3] = GameObject.Find("CubeAzul");
            cubos[4] = GameObject.Find("CubeRojo");
            //PalomasListasControler = Button.Find("¡Revancha!").GetComponent<PalomasListas>();
            PalomasListasControler =GameObject.FindGameObjectWithTag("revancha").GetComponent<PalomasListas>();
            //databaseAcces = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DataBase>();
            listaPalomas.AddPaloma(gameObject);
            if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null && SceneManager.GetActiveScene().buildIndex == 1)
            {
                NumeroJugador = PhotonNetwork.CurrentRoom.PlayerCount;
                photonView.RPC("SetName2", Photon.Pun.RpcTarget.All, "caca", NumeroJugador);
            }
            //NameTag.text = databaseAcces.usuarioSave;
        }
        void FixedUpdate()
        {
            if (Player)
            {
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
            }
            else
            {
                Player = photonView.IsMine;

            }
            //databaseAcces.GetPaloma();
            if (paloma.transform.position.y < -1)
            {
                if(SceneManager.GetActiveScene().buildIndex == 1)
                {
                    muerteAudioPaloma();
                    resetPosForLVL(new Vector3(-0.07120486f, 7, -0.1657899f));
                }
                else
                {
                        PalomasListasControler = GameObject.FindGameObjectWithTag("revancha").GetComponent<PalomasListas>();
                        PalomasListasControler.MuertePalomaMetodo();
                        if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
                        {
                            StartCoroutine(GravityFalseithSecond());
                        }
                    

                }
               
                //Photon.Pun.PhotonNetwork.Destroy(paloma);
                //Destroy(paloma);
                //photonView.RPC("muertoNumero", Photon.Pun.RpcTarget.All);
            }
            //if (!GameStates.gameActive) return;

            
            //output to log the position change
            //Debug.Log(transform.position);

        }

        public void muerteAudioPaloma()
        {
            vocesMuerte[0] = GameObject.Find("vozMuerte1");
            vocesMuerte[1] = GameObject.Find("vozMuerte1 (1)");
            vocesMuerte[2] = GameObject.Find("vozMuerte1 (2)");
            vocesMuerte[3] = GameObject.Find("vozMuerte1 (3)");
            vocesMuerte[4] = GameObject.Find("vozMuerte1 (4)");
            vocesMuerte[5] = GameObject.Find("vozMuerte1 (5)");
            vocesMuerte[6] = GameObject.Find("vozMuerte1 (6)");
            vocesMuerte[7] = GameObject.Find("vozMuerte1 (7)");
            vocesMuerte[8] = GameObject.Find("vozMuerte1 (8)");
            vocesMuerte[9] = GameObject.Find("vozMuerte1 (9)");
            var audioInt = Random.Range(0, 9);
            Debug.Log("numero random: " + audioInt);
            vocesMuerte[audioInt].transform.position = paloma.transform.position;
            vocesMuerte[audioInt].GetComponent<AudioSource>().Play();
        }

        public IEnumerator GravityFalseithSecond()
        {
            yield return new WaitForSeconds(3f);
            muerteAudioPaloma();
            gavitiFalse();

        }



        public void resetPosForLVL(Vector3 pos)
        {
            paloma.transform.position = pos;
        }

        public void gavitiFalse()
        {
            photonView.RPC("gavitiFalse2", Photon.Pun.RpcTarget.All);
        }

        public void gavitiTrue()
        {
            photonView.RPC("gavitiTrue2", Photon.Pun.RpcTarget.All);
        }

        public void Setname( string nombre)
        {
                photonView.RPC("SetName2", Photon.Pun.RpcTarget.All, nombre);

        }

        [Photon.Pun.PunRPC]
        void SetName2(string namePaloma, int NumeroJugador2)
        {
            //GetComponent<PhotonView>().name = PhotonNetwork.NickName + " " + NumeroJugador2;
            //GetComponent<PhotonView>().name = namePaloma;
            //if (Photon.Pun.PhotonNetwork.IsMasterClient && Photon.Pun.PhotonNetwork.CurrentRoom != null)
                PalomasListasControler.SetNombresMaster();
        }

        [Photon.Pun.PunRPC]
        void gavitiFalse2()
        {
            paloma.SetActive(false);
        }

        [Photon.Pun.PunRPC]
        void gavitiTrue2()
        {
            paloma.SetActive(true);
            paloma.transform.position = new Vector3(0.008051243f, 3, 5.851913f);
        }


    }

}
