using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3f; // Jarak maksimum untuk berinteraksi
    public LayerMask interactableLayer; // Layer untuk objek yang bisa diinteraksi
    public Vector3 raycastOffset = new Vector3(0, 1.5f, 0); // Offset dari posisi player
    public KeyCode interactKey = KeyCode.E; // Added back the interact key
    
    [Header("UI Elements")]
    public Text interactText; // UI Text untuk menampilkan instruksi
    
    private Camera playerCamera;
    private Door currentDoor;
    private StarterAssets.StarterAssetsInputs inputs;

    private void Start()
    {
        playerCamera = Camera.main;
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        
        // Sembunyikan teks interaksi di awal
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Reset interaksi sebelumnya
        if (currentDoor != null)
        {
            currentDoor = null;
            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
            }
        }
        
        // Dapatkan arah hadap karakter (forward vector)
        Vector3 direction = transform.forward;
        Vector3 rayOrigin = transform.position + raycastOffset;
        
        // Lakukan raycast dari posisi karakter (dengan offset) ke arah hadap karakter
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, direction, out hit, interactDistance, interactableLayer))
        {
            // Cek jika objek yang terkena raycast adalah pintu
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                currentDoor = door;
                
                // Tampilkan teks interaksi
                if (interactText != null)
                {
                    interactText.text = "Tekan 'E' untuk " + (door.IsOpen ? "menutup" : "membuka") + " pintu";
                    interactText.gameObject.SetActive(true);
                }
                
                // Cek input interaksi
                if (Input.GetKeyDown(interactKey))
                {
                    door.ToggleDoor();
                }
            }
        }
        
        // Debug draw ray
        Debug.DrawRay(rayOrigin, direction * interactDistance, Color.red);
    }
}
