using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using MongoDB.Bson;
using System.Text;

public class DataBase : MonoBehaviour
{
    private string baseHeroku = "https://pigeonlife.herokuapp.com";
    public Animator transition;
    public GameObject inputFieldUserRegister;
    public GameObject inputFieldpasswordRegister;
    public TextMeshProUGUI textDisplayRegister;
    public GameObject inputFieldUserLogin;
    public GameObject inputFieldUserLoginParent;
    public GameObject inputFieldpasswordLogin;
    public Transform contentPanel;
    public TextMeshProUGUI textDisplayLogin;
    public TextMeshProUGUI textDisplayUsuarioLoginOK;
    public TextMeshProUGUI textDisplayCrearCuenta;
    public GameStates stateManager = null;
    public Button prefabBoton;
    public GameObject BtnLogout;
    public GameObject BtnLogin;
    public GameObject BtnRegister;
    public TextMesh recordMesh;
    public String usuarioSave = "unknown";
    public String passwordSave = "";
    public int recordSave = 0;
    public Sprite Image1Login;
    public Sprite Image2Login;
    public Button v_boton_loginUI;
    // Start is called before the first frame update
    void Start()
    {

        PlayerData data = SaveSystem.LoadPLayer();
        if(data != null)
        {
            Debug.Log(data.username+" rank: "+ data.ranking);
            usuarioSave = data.username;
            recordSave = data.ranking;

            if (usuarioSave == "unknown")
            {
                BtnLogout.active = false;
                textDisplayUsuarioLoginOK.enabled = false;
                v_boton_loginUI.image.overrideSprite = Image1Login;
            }
            else
            {
                inputFieldpasswordLogin.active = false;
                inputFieldUserLoginParent.active = false;
                BtnLogin.active = false;
                textDisplayCrearCuenta.enabled = false;
                BtnRegister.active = false;
                textDisplayLogin.enabled=false;
                v_boton_loginUI.image.overrideSprite = Image2Login;
                textDisplayUsuarioLoginOK.text = "Bienvenido " + usuarioSave;
            }
        }
        else
        {
            BtnLogout.active = false;
            textDisplayUsuarioLoginOK.enabled = false;
        }


        StartCoroutine(RunLaterCoroutine(2));

    }
    public IEnumerator RunLaterCoroutine( float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        SaveDataToFileAndMongo(recordSave);
    }

    public IEnumerator GetScoresFromDataBaseHeroku()
    {
        var request = new UnityWebRequest(baseHeroku + "/Ranking", "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        JSONNode RankingInfo = JSON.Parse(request.downloadHandler.text);

        Debug.Log("listaRanking "+ contentPanel.childCount);
        var contadorPuesto = 1;
        for (int i = 0; i < RankingInfo.Count; i++)
        {
            JSONNode playerData = (JSONNode)RankingInfo[i]["username"].Value;

            Button newButton = Instantiate(prefabBoton) as Button;
            newButton.transform.SetParent(contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.transform.FindChild("TextUsuario").GetComponentInChildren<TMP_Text>().text = RankingInfo[i]["username"].Value;
            newButton.transform.FindChild("TextRanking").GetComponentInChildren<TMP_Text>().text = RankingInfo[i]["ranking"].Value;
            if (contadorPuesto == 1)
            {
                newButton.transform.FindChild("Image").GetComponentInChildren<Image>().sprite = newButton.GetComponentInChildren<botonLista>().oro;
                newButton.transform.FindChild("TextRankingPos").GetComponentInChildren<TMP_Text>().text = "";
            }
            else if (contadorPuesto == 2)
            {
                newButton.transform.FindChild("Image").GetComponentInChildren<Image>().sprite = newButton.GetComponentInChildren<botonLista>().plata;
                newButton.transform.FindChild("TextRankingPos").GetComponentInChildren<TMP_Text>().text = "";
            }
            else if (contadorPuesto == 3)
            {
                newButton.transform.FindChild("Image").GetComponentInChildren<Image>().sprite = newButton.GetComponentInChildren<botonLista>().bronce;
                newButton.transform.FindChild("TextRankingPos").GetComponentInChildren<TMP_Text>().text = "";
            }
            else
            {
                newButton.transform.FindChild("TextRankingPos").GetComponentInChildren<TMP_Text>().text = "#" + contadorPuesto;
            }

            if (contadorPuesto > 3)
            {
                botonLista btnlista = newButton.GetComponent<botonLista>();
                btnlista.Setup();
            }

            contadorPuesto++;
        }
    }

    private Ranking Deserialize(string rawJson)
    {
        Debug.Log(rawJson);
        JSONNode pojInfo = JSON.Parse(rawJson);
        Debug.Log(pojInfo["username"].ToString());
        var ranking = new Ranking();
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),")+4);
        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}") - stringWithoutID.IndexOf(":") - 3);
        ranking.UserName = username;
        ranking.Score = Convert.ToInt32(score);
        return ranking;
    }

    public async void registrarUsuario()
    {
        StartCoroutine(registrarUsuarioHeroku());
    }

    public IEnumerator registrarUsuarioHeroku()
    {

        var document1 = new BsonDocument { { "username", inputFieldUserRegister.GetComponent<Text>().text } };
        var request1 = new UnityWebRequest(baseHeroku + "/getUser", "POST");
        byte[] bodyRaw1 = Encoding.UTF8.GetBytes(document1.ToString());
        request1.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw1);
        request1.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request1.SetRequestHeader("Content-Type", "application/json");
        yield return request1.SendWebRequest();

        if (request1.isNetworkError || request1.isHttpError)
        {
            Debug.LogError(request1.error);
            yield break;
        }

        bool usuarioInfo1 = JSON.Parse(request1.downloadHandler.text);
        if (usuarioInfo1 == false)
        {
            if (inputFieldUserRegister.GetComponent<Text>().text != "" && inputFieldpasswordRegister.GetComponent<InputField>().text != "")
            {
                String passwordHash = SecureHelper.Hash(inputFieldpasswordRegister.GetComponent<InputField>().text);
                var document = new BsonDocument { { "username", inputFieldUserRegister.GetComponent<Text>().text }, { "password", passwordHash } };
                var request = new UnityWebRequest(baseHeroku + "/newUser", "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(document.ToString());
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError(request.error);
                    yield break;
                }

                JSONNode usuarioInfo = JSON.Parse(request.downloadHandler.text);
                Debug.Log(usuarioInfo);
                textDisplayRegister.text = "Registro completado";
                inputFieldUserLoginParent.GetComponent<InputField>().text = inputFieldUserRegister.GetComponent<Text>().text;
                inputFieldpasswordLogin.GetComponent<InputField>().text = inputFieldpasswordRegister.GetComponent<InputField>().text;
                LoginUsuario();
                stateManager.loginGame();
            }
            else
            {
                textDisplayRegister.text = "¡No dejes campos vacíos!";
            }

        }
        else
        {
            textDisplayRegister.text = "¡El usuario ya existe!";
        }

    }

    

        public async void LoginUsuario()
    {
        StartCoroutine(LoginUsuarioHeroku());
    }

    public IEnumerator LoginUsuarioHeroku()
    {
        if (inputFieldUserLogin.GetComponent<Text>().text != "" && inputFieldpasswordLogin.GetComponent<InputField>().text != "")
        {
            String passwordHash = SecureHelper.Hash(inputFieldpasswordLogin.GetComponent<InputField>().text);
            var document1 = new BsonDocument { { "username", inputFieldUserLogin.GetComponent<Text>().text }, { "password", passwordHash } };
            var request1 = new UnityWebRequest(baseHeroku + "/LoginUser", "POST");
            byte[] bodyRaw1 = Encoding.UTF8.GetBytes(document1.ToString());
            request1.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw1);
            request1.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request1.SetRequestHeader("Content-Type", "application/json");
            yield return request1.SendWebRequest();

            if (request1.isNetworkError || request1.isHttpError)
            {
                Debug.LogError(request1.error);
                yield break;
            }

            JSONNode usuarioInfo1 = JSON.Parse(request1.downloadHandler.text);
            string opciones = request1.downloadHandler.text;
            if (opciones != "null")
            {
                if (opciones == "ok")
                {
                    textDisplayLogin.text = "Este nombre de usuario existe.";
                    usuarioSave = inputFieldUserLogin.GetComponent<Text>().text;
                    textDisplayLogin.text = "";
                    textDisplayUsuarioLoginOK.text = "Bienvenido " + usuarioSave;
                    inputFieldUserLoginParent.GetComponent<InputField>().text = "";
                    inputFieldpasswordLogin.GetComponent<InputField>().text = "";
                    SaveDataToFileAndMongo(0);
                    inputFieldpasswordLogin.active = false;
                    inputFieldUserLoginParent.active = false;
                    BtnLogout.active = true;
                    textDisplayUsuarioLoginOK.enabled = true;
                    BtnLogin.active = false;
                    textDisplayCrearCuenta.enabled = false;
                    BtnRegister.active = false;
                    textDisplayLogin.enabled = false;
                    v_boton_loginUI.image.overrideSprite = Image2Login;
                }
                else
                {
                    textDisplayLogin.text = "La contraseña es incorrecta. Inténtalo de nuevo.";
                }              

            }
            else
            {
                textDisplayLogin.text = "Este nombre de usuario no existe.";
            }
        }
        else
        {
            textDisplayLogin.text = "¡No dejes campos vacíos!";
        }

    }

    public async void LogoutUsuario()
    {
        usuarioSave = "unknown";
        v_boton_loginUI.image.overrideSprite = Image1Login;
        SaveDataToFileAndMongo(0);
        inputFieldpasswordLogin.active = true;
        inputFieldUserLoginParent.active = true;
        BtnLogout.active = false;
        textDisplayUsuarioLoginOK.enabled = false;
        BtnLogin.active = true;
        textDisplayCrearCuenta.enabled = true;
        BtnRegister.active = true;
        textDisplayLogin.enabled = true;
    }
    


    public async void SaveDataToFileAndMongo(int lvl)
    {
        recordSave = lvl;
        SaveSystem.SavePlayer(new Player(recordSave, usuarioSave));
        if (recordSave == 0)
        {
            recordMesh.text = "";
        }
        else
        {
            recordMesh.text = "Récord: lvl " + recordSave;
        }
        StartCoroutine(SaveDataToFileAndMongoHeroku());

    }

    public IEnumerator SaveDataToFileAndMongoHeroku()
    {
        ////mongo   
        Debug.Log("subiendo a mongo...");

        var document1 = new BsonDocument { { "username", usuarioSave } };
        var request1 = new UnityWebRequest(baseHeroku + "/GetRankingUser", "POST");
        byte[] bodyRaw1 = Encoding.UTF8.GetBytes(document1.ToString());
        request1.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw1);
        request1.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request1.SetRequestHeader("Content-Type", "application/json");
        yield return request1.SendWebRequest();

        if (request1.isNetworkError || request1.isHttpError)
        {
            Debug.LogError(request1.error);
            yield break;
        }

        JSONNode usuarioInfo1 = JSON.Parse(request1.downloadHandler.text);
        if (usuarioInfo1 == "")
        {
            var document2 = new BsonDocument { { "username", usuarioSave }, { "ranking", recordSave } };
            var request2 = new UnityWebRequest(baseHeroku + "/InsertRanking", "POST");
            byte[] bodyRaw2 = Encoding.UTF8.GetBytes(document2.ToString());
            request2.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw2);
            request2.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request2.SetRequestHeader("Content-Type", "application/json");
            yield return request2.SendWebRequest();

            if (request2.isNetworkError || request2.isHttpError)
            {
                Debug.LogError(request2.error);
                yield break;
            }
            Debug.Log("Insert ranking subido a mongo");
        }
        else
        {
            if (int.Parse(usuarioInfo1["ranking"].Value) < recordSave)
            {
                var document3 = new BsonDocument { { "username", usuarioSave }, { "ranking", recordSave } };
                var request3 = new UnityWebRequest(baseHeroku + "/UpdateRankingUser", "POST");
                byte[] bodyRaw3 = Encoding.UTF8.GetBytes(document3.ToString());
                request3.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw3);
                request3.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request3.SetRequestHeader("Content-Type", "application/json");
                yield return request3.SendWebRequest();

                if (request3.isNetworkError || request3.isHttpError)
                {
                    Debug.LogError(request3.error);
                    yield break;
                }
                Debug.Log("Update ranking subido a mongo");
            }
            else
            {
                if (usuarioSave != "unknown" && int.Parse(usuarioInfo1["ranking"].Value) > recordSave)
                {
                    SaveDataToFileAndMongo(int.Parse(usuarioInfo1["ranking"].Value));
                    Debug.Log("Ranking no subido, menor o igual que en mongo USUARIO: " + usuarioSave + " RECOGIDO DE MONGO");
                    recordMesh.text = "Récord: lvl " + int.Parse(usuarioInfo1["ranking"].Value);
                }
                else
                {
                    Debug.Log("Ranking no subido, menor o igual que en mongo USUARIO: " + usuarioSave);
                }
            }
        }
    }

    public async void PonerListaRanking()
    {
        await EliminarBotonesRanking();
        StartCoroutine(GetScoresFromDataBaseHeroku());
    }

    public async Task EliminarBotonesRanking()
    {
        GameObject[] botonesRanking = GameObject.FindGameObjectsWithTag("BotonRanking");
        foreach (GameObject bontonranking in botonesRanking)
             GameObject.Destroy(bontonranking);
    }

    public async void PlayOnline()
    {
        StartCoroutine(PlayOnlineCountine());
    }

    public IEnumerator PlayOnlineCountine()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("salaEspera");
    }
}

public class Ranking
{
    public string UserName { get; set; }

    public int Score { get; set; }
}



