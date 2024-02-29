using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject[] walkPointGOs;

    //Patrolling
    private GameObject currentWalkPoint;
    bool walkPointSet;

    private Animator animator;

    private void Awake()
    {
        agent =this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        Patrolling();   
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(currentWalkPoint.transform.position);

        //Walkpoint reached
        if (Vector3.Distance(transform.position, currentWalkPoint.transform.position) < 1f)
        {
            walkPointSet = false;
            animator.SetBool("Walk", false);
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
              
        while(waitTime < 2 && !walkPointSet)
        {
            waitTime += Time.deltaTime;
        }

        if(waitTime >= 2)
        {
            walkPointSet =true;
            int index = Random.Range(0, walkPointGOs.Length);
            currentWalkPoint = walkPointGOs[index];
            animator.SetBool("Walk", true);
            waitTime = 0;
        }
    }

    private float waitTime = 0;

}
