using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    RTSController rtsController;
    Transform targetUnit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rtsController = RTSController.Instance;

        StartCoroutine(UpdateTargetCoroutine());
    }

    IEnumerator UpdateTargetCoroutine()
    {
        while (true)
        {
            float nearestDistance = Mathf.Infinity;
            GameObject nearestUnit = null;

            foreach (GameObject unit in rtsController.GetAllUnits())
            {
                if (unit == null)
                {
                    continue; // Skip to the next unit in the list
                }

                float distanceToUnit = Vector3.Distance(transform.position, unit.transform.position);
                if (distanceToUnit < nearestDistance)
                {
                    nearestDistance = distanceToUnit;
                    nearestUnit = unit;
                }
            }

            if (nearestUnit != null && nearestDistance <= 100f) // Adjust the detection range as needed
            {
                targetUnit = nearestUnit.transform;
            }
            else
            {
                targetUnit = null;
            }

            yield return new WaitForSeconds(0.5f); // Adjust the repeat rate as needed
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Unit"))
        {
            Destroy(other.gameObject); // Destroy the unit
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.CompareTag("Unit"))
        {
            Destroy(collision.gameObject); // Destroy the unit
        }
    }

    void Update()
    {
        if (targetUnit != null)
        {
            agent.SetDestination(targetUnit.position);
        }
    }
}