using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aceraAbajo : MonoBehaviour
{
    bool yourVar;
    public waypointPersona personaWalking;
    public waypointPersona personaWalking2;
    private bool puedeAnimarMujerCorriendo = true;
    private float x1, z1, x2, z2;
    public static bool CologarParaCambiarLvl = false;
    bool recolocarAcera1vez = false;
    bool recolocarAcera1vezCambioLvl = false;
    public Material material;
    public Collider col2;
    public GameObject OtraCarretera;
    public List<GameObject> objetosCambiarLvl = new List<GameObject>();
    public static List<Collider> cols = new List<Collider>();
    //public static Collider col;
    //public GameObject carretera;
    //public GameObject camera;
    //public GameObject paredes;

    Renderer rend;
    int frames=0;

        public float speed = 1.0f;
    public Color startColor;
    public Color endColor;
    public bool repeatable = false;
    float startTime;
    int segundos=0;
    int segundosBakcup=0;

    void Start()
    {
        //col = col2;
        //personaWalking = GameObject.Find("SportyGirl").GetComponent<waypointPersona>();
        cols.Add(col2);
        rend = GetComponent<Renderer>();
        material.color = Color.white;
        startTime = Time.time;

        // Use the Specular shader on the material
        //rend.material.shader = Shader.Find("NormalMap");
        // rend.material.EnableKeyword("_NORMALMAP");
        //rend.material.EnableKeyword("_METALLICGLOSSMAP");

        //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        // rend.material.SetTexture("_MainTex", m_MainTexture);
        //Set the Normal map using the Texture you assign in the Inspector
        // rend.material.SetTexture("_BumpMap", m_Normal);
        //Set the Metallic Texture as a Texture you assign in the Inspector
        // rend.material.SetTexture("_MetallicGlossMap", m_Metal);
    }

    void OnTriggerEnter(Collider Colider2)
    {
        if (Colider2.tag == "Player")
        {
            //Debug.Log("Col.lisio amb DeathTrigger. Tag: " + col.tag);
            //collidedObject.SendMessage("hitByPlayerBullet", null, SendMessageOptions.DontRequireReceiver);
            yourVar = true;
            frames = 3;
        }
       
    }
    void OnTriggerExit(Collider Colider2)
    {
        if (Colider2.tag == "Player")
        {
            //Debug.Log("Col.lisio amb DeathTrigger. Tag: " + col.tag);
            segundosBakcup = segundos;
            col2.enabled = true;
            recolocarAcera1vez = true;
            Debug.Log("Collider.enabled = " + col2.enabled);
            //Debug.Log("Collider.enabled = " + cols[0].enabled);
            //Debug.Log("Collider.enabled = " + cols[1].enabled);
            //collidedObject.SendMessage("hitByPlayerBullet", null, SendMessageOptions.DontRequireReceiver);
            yourVar = false;
        }
    }

    void Update()
    {
        if (CologarParaCambiarLvl)
        {
            

            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //OtraCarretera.transform.position = new Vector3(OtraCarretera.transform.position.x, 0, OtraCarretera.transform.position.z);
            segundos = 0;
            //float t = (Time.time - startTime) * speed;
            //GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
            recolocarAcera1vez = false;
            CologarParaCambiarLvl = false;
            //Debug.Log("Colocar ");
        }
        if (GameStates.SwitchingLvl && transform.position.y > -23)
        {
            if (personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled)
            {
                puedeAnimarMujerCorriendo = false;
                personaWalking.GetComponent<waypointPersona>().navMeshAgent.Stop();
                personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled = false;
                personaWalking2.GetComponent<waypointPersona>().navMeshAgent.Stop();
                personaWalking2.GetComponent<waypointPersona>().navMeshAgent.enabled = false;
            }
            cols[0].enabled = true;
            cols[1].enabled = true;
            col2.enabled = true;
            float t = (Time.time - startTime) * speed;
                //carretera.transform.position = carretera.transform.position + new Vector3(0, +1f * Time.deltaTime, 0);
                //camera.transform.position = camera.transform.position + new Vector3(0, +1f * Time.deltaTime, 0);
                //paredes.transform.position = paredes.transform.position + new Vector3(0, +1f * Time.deltaTime, 0);
                transform.position = transform.position + new Vector3(0, -10f * Time.deltaTime, 0);
        }else if(GameStates.SwitchingLvl && transform.position.y <= -23)
        {
            //Debug.Log("Colocar2");
            transform.position = new Vector3(transform.position.x, 17.3f, transform.position.z);
           OtraCarretera.transform.position = new Vector3(OtraCarretera.transform.position.x, 17.3f, OtraCarretera.transform.position.z);
            personaWalking.transform.position = new Vector3(3.45f, 17.3f, 6.3f);
            personaWalking2.transform.position = new Vector3(6.39f, 17.3f, 2.32f);
            int i=0;
            for (i = 0; i < objetosCambiarLvl.Count; i++)
            {
                objetosCambiarLvl[i].transform.position = new Vector3(objetosCambiarLvl[i].transform.position.x, 19, objetosCambiarLvl[i].transform.position.z);
            }
            GameStates.SwitchingLvl = false;
            recolocarAcera1vezCambioLvl = true;
        }
        else if (!GameStates.SwitchingLvl && transform.position.y > 0 && !recolocarAcera1vez && recolocarAcera1vezCambioLvl)
        {
            //Debug.Log("Colocar3");
            float t = (Time.time - startTime) * speed;
            transform.position = transform.position + new Vector3(0, -10f * Time.deltaTime, 0);
            OtraCarretera.transform.position = OtraCarretera.transform.position + new Vector3(0, -10f * Time.deltaTime, 0);

            //Debug.Log("aaaaaaaaa ");

        }else if (recolocarAcera1vezCambioLvl)
        {
            recolocarAcera1vezCambioLvl = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            OtraCarretera.transform.position = new Vector3(OtraCarretera.transform.position.x, 0, OtraCarretera.transform.position.z);
            //Debug.Log("aaaaaaaaa 22222");
            GameStates.coches++;
            GameStates.lvl++;
            GameStates.cochesDelvl = GameStates.cochesDelvl + 2;
            StartCoroutine(GameStates.ExampleCoroutine());
            scoreWatcherInGame.updateScorre(0);
            int i = 0;
            for (i = 0; i < cols.Count; i++)
            {
                cols[i].enabled = false;
            }

            personaWalking.transform.position = new Vector3(3.45f, 0, 6.3f);
            personaWalking2.transform.position = new Vector3(6.39f, 0, 2.32f);
            personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled = true;
            personaWalking.GetComponent<waypointPersona>().setOrientacion();
            personaWalking.GetComponent<waypointPersona>().navMeshAgent.Resume();
            personaWalking2.GetComponent<waypointPersona>().navMeshAgent.enabled = true;
            personaWalking2.GetComponent<waypointPersona>().setOrientacion();
            personaWalking2.GetComponent<waypointPersona>().navMeshAgent.Resume();
            puedeAnimarMujerCorriendo = true;


        }
        /*if (!repeatable)
        {
            float t = (Time.time - startTime) * speed;
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
        }
        else
        {
            float t = (Mathf.Sin(Time.time - startTime) * speed);
            GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
        }*/

        if (yourVar)
        {
            segundos++;
            //transform.position = transform.position + new Vector3(0, -5f * Time.deltaTime, 0);
            //rend.material.SetTexture("_MainTex", m_MainTexture);
            //rend.material.SetColor("_SpecColor", Color.red);
            //material.color = new Color(233, 79, 55);
        } 
        else {
            //material.color = Color.black;
            
            if (segundos != 0)
            {
                if (personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled && !GameStates.SwitchingLvl && puedeAnimarMujerCorriendo)
                {
                    x1 = personaWalking.transform.position.x;
                    z1 = personaWalking.transform.position.z;
                    x2 = personaWalking2.transform.position.x;
                    z2 = personaWalking2.transform.position.z;
                    personaWalking.GetComponent<waypointPersona>().navMeshAgent.Stop();
                    personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled = false;
                    personaWalking2.GetComponent<waypointPersona>().navMeshAgent.Stop();
                    personaWalking2.GetComponent<waypointPersona>().navMeshAgent.enabled = false;
                }
                float t = (Time.time - startTime) * speed;
                GetComponent<Renderer>().material.color = Color.Lerp(endColor, startColor, t);
                transform.position = transform.position + new Vector3(0, -5f * Time.deltaTime, 0);
                segundos--;
            }
            else
            {
                if (transform.position.y < 0 && !GameStates.SwitchingLvl && !recolocarAcera1vezCambioLvl)
                {
                    transform.position = transform.position + new Vector3(0, 5f * Time.deltaTime, 0);
                    
                }else if (transform.position.y > 0 && recolocarAcera1vez && !GameStates.SwitchingLvl)
                {
                    float t = (Time.time - startTime) * speed;
                    GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
                    col2.enabled = false;
                    recolocarAcera1vez = false;
                    //cols[0].enabled = false;
                    //cols[1].enabled = false;
                    Debug.Log("Col disabled ");
                    personaWalking.transform.position = new Vector3(x1, 0, z1);
                    personaWalking2.transform.position = new Vector3(x2, 0, z2);
                    personaWalking.GetComponent<waypointPersona>().navMeshAgent.enabled = true;
                    personaWalking.GetComponent<waypointPersona>().setOrientacion();
                    personaWalking.GetComponent<waypointPersona>().navMeshAgent.Resume();
                    personaWalking2.GetComponent<waypointPersona>().navMeshAgent.enabled = true;
                    personaWalking2.GetComponent<waypointPersona>().setOrientacion();
                    personaWalking2.GetComponent<waypointPersona>().navMeshAgent.Resume();
                }
            }
        }

        // Animate the Shininess value
        //float shininess = Mathf.PingPong(Time.time, 1.0f);
        //rend.material.SetFloat("_Shininess", shininess);
      
    }
    
    
}
