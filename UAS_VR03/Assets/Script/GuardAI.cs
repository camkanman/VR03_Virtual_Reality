using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class GuardAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 20f;
    public float waypointTolerance = 1f;
    public float waitTimeAtWaypoint = 2f;
    public bool patrolOnStart = true;
    public string waypointTag = "Waypoint";

    [Header("Interaction Settings")]
    public float doorCheckDistance = 2f;
    public LayerMask doorLayer;

    // --- Deteksi Player ---
    [Header("Detection Settings")]
    public float detectionRadius = 8f; // Radius deteksi Guard ke Player
    public float chaseSpeed = 8f;      // Speed saat ngejar Player
    public float patrolSpeed = 4f;     // Speed patrol biasa

    // --- Internal Variables ---
    private NavMeshAgent agent;
    private Transform[] allWaypoints;
    private Transform currentWaypoint;
    private Transform previousWaypoint;
    private bool isPatrolling = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private Door waitingDoor;
    private Transform playerTarget;
    private bool isChasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is required on " + gameObject.name);
            enabled = false;
            return;
        }

        // Ambil semua waypoint
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        allWaypoints = waypointObjects.Select(go => go.transform).ToArray();

        // Cari Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) playerTarget = playerObj.transform;

        // Tambahkan SphereCollider sebagai deteksi (isTrigger)
        SphereCollider col = GetComponent<SphereCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = detectionRadius;
        }
        else
        {
            col.isTrigger = true;
            col.radius = detectionRadius;
        }

        if (patrolOnStart && allWaypoints.Length > 0)
        {
            StartPatrolling();
        }
    }

    private void Update()
    {
        if (agent == null || !isPatrolling) return;

        // Jika sedang ngejar Player
        if (isChasing && playerTarget != null)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTarget.position);
            return;
        }
        else
        {
            agent.speed = patrolSpeed;
        }

        // Patrol Logic
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetNewWaypoint();
            }
        }
        else if (currentWaypoint != null && !agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            WaitAtWaypoint();
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            HandleBlockedPath();
        }
    }

    public void StartPatrolling()
    {
        if (isPatrolling || allWaypoints.Length == 0) return;
        isPatrolling = true;
        agent.isStopped = false;
        SetNewWaypoint();
    }

    public void StopPatrolling()
    {
        isPatrolling = false;
        isWaiting = false;
        agent.isStopped = true;
        currentWaypoint = null;
    }

    private void SetNewWaypoint()
    {
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        Transform[] allWaypoints = waypointObjects.Select(go => go.transform).ToArray();
        var availableWaypoints = allWaypoints
            .Where(wp => Vector3.Distance(transform.position, wp.position) <= patrolRadius)
            .Where(wp => wp != previousWaypoint)
            .ToList();

        if (availableWaypoints.Count == 0)
            availableWaypoints = allWaypoints.Where(wp => wp != previousWaypoint).ToList();
        if (availableWaypoints.Count == 0)
            availableWaypoints = allWaypoints.ToList();

        previousWaypoint = currentWaypoint;
        currentWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];
        agent.SetDestination(currentWaypoint.position);
    }

    private void WaitAtWaypoint()
    {
        isWaiting = true;
        waitTimer = waitTimeAtWaypoint;
        // Tutup pintu jika sudah cukup jauh dari pintu
        if (waitingDoor != null && waitingDoor.IsOpen)
        {
            float dist = Vector3.Distance(transform.position, waitingDoor.transform.position);
            if (dist > 2f)
            {
                waitingDoor.CloseDoor();
                waitingDoor = null;
            }
        }
    }

    private void HandleBlockedPath()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 toWaypoint = (currentWaypoint.position - transform.position).normalized;

        if (Physics.Raycast(rayOrigin, toWaypoint, out hit, doorCheckDistance, doorLayer, QueryTriggerInteraction.Collide))
        {
            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null && !door.IsOpen)
            {
                Debug.Log("[GuardAI] Pintu terdeteksi di jalur waypoint, mencoba membuka.");
                door.Interact(transform);
                waitingDoor = door;
            }
        }
    }

    // --- Area Deteksi Player: Ngejar kalau Player masuk, patrol kalau keluar ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log($"{gameObject.name} mulai mengejar Player!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            Debug.Log($"{gameObject.name} berhenti mengejar, kembali patrol.");
            SetNewWaypoint();
        }
    }
}