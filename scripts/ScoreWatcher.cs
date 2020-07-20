using UnityEngine;
using System.Collections;
public class ScoreWatcher : MonoBehaviour
{
    public static int currScore = 0;
    public static TextMesh scoreMesh = null;
    void Start()
    {
        scoreMesh = gameObject.GetComponent<TextMesh>();
        scoreMesh.text = "0";

        PlayerData data = SaveSystem.LoadPLayer();
        if (data != null && data.ranking != 0)
        {
           scoreMesh.text = "Récord: lvl " + data.ranking;
        }
        else
        {
            scoreMesh.text = "";
        }
    }
}
