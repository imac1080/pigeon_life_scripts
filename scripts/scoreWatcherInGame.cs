using UnityEngine;
using System.Collections;
public class scoreWatcherInGame : MonoBehaviour
{
    public static int currScore = 0;
    public static TextMesh scoreMesh = null;
    void Start()
    {
        scoreMesh = gameObject.GetComponent<TextMesh>();
    }
    void OnEnable()
    {
        WaypointPatrol.enemyDied += addScore;
        //EnemyControllerScript.enemyDied += addScore;
        //BossEventController.bossDied += addScore;
    }
    void OnDisable()
    {
        WaypointPatrol.enemyDied -= addScore;
        //EnemyControllerScript.enemyDied -= addScore;
        //BossEventController.bossDied -= addScore;
    }
    void addScore(int scoreToAdd)
    {
        currScore += scoreToAdd;
        scoreMesh.text = currScore.ToString() + "/" + GameStates.cochesDelvl + "\n" + "Nivel: " + GameStates.lvl;
    }
    public static void updateScorre(int v_score)
    {
        currScore = v_score;
        scoreMesh.text = currScore.ToString() + "/" + GameStates.cochesDelvl + "\n" + "Nivel: " + GameStates.lvl;
    }
}
