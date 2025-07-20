using UnityEngine;
using System; // Untuk event Action<Door>
using System.Collections;

/// <summary>
/// Script pintu interaktif untuk game.
/// Fitur: buka/tutup pintu, auto-close, interaksi player/guard, event terbuka.
/// </summary>
public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;
    public float autoCloseDelay = 4f;

    private Transform pivot;
    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isPlayerInRange = false;
    private Transform playerTransform;
    private UnityEngine.AI.NavMeshObstacle navObstacle;
    private Coroutine autoCloseCoroutine;

    public event Action<Door> OnDoorFullyOpened;

    private Collider doorCollider;

    private void Start()
    {
        pivot = transform.parent;
        if (pivot == null)
        {
            Debug.LogError("Door must be a child of a pivot object!");
            return;
        }

        navObstacle = pivot.GetComponent<UnityEngine.AI.NavMeshObstacle>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        initialRotation = pivot.localRotation;
        targetRotation = initialRotation;

        // Ambil collider pintu (bisa BoxCollider di object pintu)
        doorCollider = GetComponent<Collider>();
        if (doorCollider == null)
        {
            // Optional: log warning supaya dev aware
            Debug.LogWarning("Door collider not found! Pastikan pintu punya Collider (BoxCollider/other)");
        }
    }

    private void Update()
    {
        if (pivot != null)
        {
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, targetRotation, openSpeed * Time.deltaTime);

            if (isOpen && Quaternion.Angle(pivot.localRotation, targetRotation) < 1f)
            {
                if (OnDoorFullyOpened != null)
                {
                    OnDoorFullyOpened.Invoke(this);
                    OnDoorFullyOpened = null;
                }
            }
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact(playerTransform);
        }
    }

    public void ToggleDoor()
    {
        Interact(playerTransform);
    }

    public void Interact(Transform interactor)
    {
        if (interactor == null) return;

        Vector3 toInteractor = interactor.position - pivot.position;
        float dot = Vector3.Dot(pivot.forward, toInteractor.normalized);
        bool shouldOpenInwards = dot > 0;

        float angle = shouldOpenInwards ? -openAngle : openAngle;

        if (!isOpen)
        {
            targetRotation = initialRotation * Quaternion.Euler(0, angle, 0);
            isOpen = true;

            // Nonaktifkan NavMeshObstacle agar AI bisa lewat
            if (navObstacle != null) navObstacle.carving = false;

            // Nonaktifkan collider pintu agar player/guard bisa lewat
            if (doorCollider != null) doorCollider.enabled = false;

            // Jika interaktor adalah Guard, jangan pakai auto-close
        }
        else
        {
            targetRotation = initialRotation;
            isOpen = false;

            // Aktifkan NavMeshObstacle agar AI tidak bisa lewat
            if (navObstacle != null) navObstacle.carving = true;

            // Aktifkan collider pintu agar tidak bisa lewat
            if (doorCollider != null) doorCollider.enabled = true;

            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = null;
            }
        }
    }

    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        targetRotation = initialRotation;
        isOpen = false;

        if (navObstacle != null) navObstacle.carving = true;

        // Aktifkan collider pintu
        if (doorCollider != null) doorCollider.enabled = true;

        autoCloseCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
        private set { isOpen = value; }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            targetRotation = initialRotation;
            isOpen = false;
            if (navObstacle != null) navObstacle.carving = true;

            // Aktifkan collider pintu
            if (doorCollider != null) doorCollider.enabled = true;

            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = null;
            }
        }
    }
}