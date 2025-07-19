using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class GuardAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 20f; // Jarak maksimum untuk mencari waypoint
    public float waypointTolerance = 1f; // Jarak toleransi untuk mencapai waypoint
    public float waitTimeAtWaypoint = 2f; // Waktu tunggu di setiap waypoint (detik)
    public bool patrolOnStart = true; // Apakah patroli langsung dimulai saat game dimulai?
    public string waypointTag = "Waypoint"; // Tag untuk GameObject waypoint

    [Header("Gizmos")]
    public bool showGizmos = true; // Tampilkan gizmos di Scene view
    public Color patrolAreaColor = new Color(0, 1, 0, 0.1f); // Warna area patroli
    public Color waypointColor = Color.yellow; // Warna waypoint yang tersedia
    public Color activeWaypointColor = Color.green; // Warna waypoint tujuan

    [Header("Interaction Settings")]
    public float doorCheckDistance = 2f; // Jarak untuk mendeteksi pintu
    public LayerMask doorLayer; // Layer khusus untuk pintu

    private NavMeshAgent agent;
    private Transform[] allWaypoints;
    private Transform currentWaypoint;
    private Transform previousWaypoint;
    private bool isPatrolling = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is required on " + gameObject.name);
            enabled = false;
            return;
        }

        // Cari semua waypoint berdasarkan tag
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        allWaypoints = waypointObjects.Select(go => go.transform).ToArray();

        if (allWaypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints found with tag '" + waypointTag + "'. Patrolling will be disabled.");
            patrolOnStart = false;
        }

        if (patrolOnStart)
        {
            StartPatrolling();
        }
    }

    private void Update()
    {
        // Pastikan agent valid sebelum menjalankan logika apapun
        if (agent == null || !isPatrolling) return;

        if (isWaiting)
        {
            // Tunggu di waypoint saat ini
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetNewWaypoint();
            }
        }
        // Cek jika sudah sampai di waypoint (dan agent tidak sedang menghitung path)
        else if (currentWaypoint != null && !agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            // Sampai di waypoint, tunggu sebentar
            WaitAtWaypoint();
        }
        // Jika agent tidak bisa mencapai tujuan (misal, terhalang pintu)
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
        // 1. Dapatkan semua waypoint dalam radius patroli
        List<Transform> availableWaypoints = allWaypoints.Where(wp => Vector3.Distance(transform.position, wp.position) <= patrolRadius).ToList();

        // 2. Hapus waypoint sebelumnya jika ada lebih dari satu pilihan
        if (availableWaypoints.Count > 1 && previousWaypoint != null)
        {
            availableWaypoints.Remove(previousWaypoint);
        }

        if (availableWaypoints.Count == 0)
        {
            Debug.LogWarning(gameObject.name + ": No new waypoints found in radius. Stopping patrol.");
            StopPatrolling();
            return;
        }

        // 3. Pilih waypoint baru secara acak dan set sebagai tujuan
        previousWaypoint = currentWaypoint;
        currentWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];
        agent.SetDestination(currentWaypoint.position);
    }

    private void WaitAtWaypoint()
    {
        isWaiting = true;
        waitTimer = waitTimeAtWaypoint;
    }

    private void HandleBlockedPath()
    {
        // Lakukan raycast ke depan untuk mendeteksi pintu
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, doorCheckDistance, doorLayer))
        {
            Door door = hit.collider.GetComponentInParent<Door>();
            // Jika pintu ditemukan dan sedang tertutup, buka pintu
            if (door != null && !door.IsOpen)
            {
                Debug.Log("[GuardAI] Pintu terdeteksi, mencoba membuka.");
                door.Interact(transform);
            }
        }
    }

    // Menggambar gizmos di Scene view
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        // Gambar area patroli
        Gizmos.color = patrolAreaColor;
        Gizmos.DrawSphere(transform.position, patrolRadius);

        // Gambar waypoint yang tersedia dan waypoint tujuan
        if (allWaypoints != null)
        {
            foreach (Transform waypoint in allWaypoints)
            {
                if (Vector3.Distance(transform.position, waypoint.position) <= patrolRadius)
                {
                    if (Application.isPlaying && waypoint == currentWaypoint)
                    {
                        Gizmos.color = activeWaypointColor;
                        Gizmos.DrawWireSphere(waypoint.position, 1f);
                        Gizmos.DrawLine(transform.position, waypoint.position);
                    }
                    else
                    {
                        Gizmos.color = waypointColor;
                        Gizmos.DrawWireSphere(waypoint.position, 0.5f);
                    }
                }
            }
        }
    }
}