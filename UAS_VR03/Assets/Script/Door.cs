using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f; // Sudut rotasi saat pintu terbuka
    public float openSpeed = 2f; // Kecepatan rotasi pintu
    
    private Transform pivot; // Titik pivot rotasi (parent object)
    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isPlayerInRange = false;
    private Transform playerTransform; // Referensi ke transform pemain

    private void Start()
    {
        // Dapatkan transform parent sebagai pivot
        pivot = transform.parent;
        if (pivot == null)
        {
            Debug.LogError("Door must be a child of a pivot object!");
            return;
        }
        
        // Dapatkan referensi ke pemain
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        initialRotation = pivot.localRotation;
        targetRotation = initialRotation;
    }

    private void Update()
    {
        // Smoothly rotate the door around the pivot
        if (pivot != null)
        {
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, targetRotation, openSpeed * Time.deltaTime);
        }
        
        // Check for player interaction
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Pemain berinteraksi dengan pintu
            Interact(playerTransform);
        }
    }

    // Fungsi ini dipanggil oleh pemain
    public void ToggleDoor()
    {
        Interact(playerTransform);
    }

    // Fungsi interaksi umum yang bisa dipanggil oleh siapa saja (Pemain, Guard, dll)
    public void Interact(Transform interactor)
    {
        if (interactor == null) return;
        
        // Tentukan arah buka berdasarkan posisi interactor
        Vector3 toInteractor = interactor.position - pivot.position;
        
        // Hitung dot product antara vektor hadap pintu dan vektor ke interactor
        float dot = Vector3.Dot(pivot.forward, toInteractor.normalized);
        
        // Tentukan arah putaran berdasarkan posisi interactor
        bool shouldOpenInwards = dot > 0; // Jika interactor di depan pintu
        
        // Tentukan arah putaran (positif atau negatif)
        float angle = shouldOpenInwards ? -openAngle : openAngle;
        
        // Terapkan rotasi
        if (!isOpen)
        { 
            targetRotation = initialRotation * Quaternion.Euler(0, angle, 0);
            isOpen = true;
        }
        else
        {
            targetRotation = initialRotation;
            isOpen = false;
        }
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
    
    // Public property to access isOpen
    public bool IsOpen 
    { 
        get { return isOpen; } 
        private set { isOpen = value; }
    }
}
