using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class waypointPersona : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints2;
    private Animator buttonAnim;
    private int vuelta = 1;
    private int orientacion =0;


    void Start()
    {
        buttonAnim = GetComponent<Animator>();
        navMeshAgent.SetDestination(waypoints2[0].position);
        if (name == "SportyGirlCorrer")
        {
            buttonAnim.SetFloat("Speed", 1f);
        }
    }
    void Update()
    {
        if (navMeshAgent.enabled)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                if (vuelta % 2 == 0)
                {
                    navMeshAgent.SetDestination(waypoints2[0].position);
                    vuelta++;
                    orientacion = 0;
                }
                else
                {
                    navMeshAgent.SetDestination(waypoints2[1].position);
                    vuelta++;
                    orientacion = 1;
                }
            }
        }
       
    }
    public void setOrientacion()
    {
        //Debug.Log("destination inicial "+ orientacion);
        navMeshAgent.SetDestination(waypoints2[orientacion].position);
    }
}