using UnityEngine;
using UnityEngine.UI;

public class LayoutElementController : MonoBehaviour
{

    private LayoutElement layoutElement;
    private static float layoutElementMinHeight_1_7; // X
    private static float layoutElementMinHeight_1_6; // Y
    private static float layoutElementMinHeight_2_3; // Z
    private static float aspectRatio_1_7 = 1.777778f; // X
    private static float aspectRatio_1_6 = 1.6f; // Y
    private static float aspectRatio_2_3 = 2.37037f; // Z


    private float lastAspectRatio;

    public RectTransform scrollViewRect;

    public RectTransform targetUI; // Drag and drop UI object (misalnya Scroll View)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per framea
    void Update()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Cek apakah aspect ratio berubah
        if (!Mathf.Approximately(currentAspectRatio, lastAspectRatio))
        {
            lastAspectRatio = currentAspectRatio;
            Debug.Log("Screen Aspect Ratio: " + currentAspectRatio);

            // Ketika aspect ratio = 1.6, maka minHeight dari layout element = Y
            if (currentAspectRatio >= 1.59f && currentAspectRatio <= 1.61f)
            {
                Debug.Log("INI aspectRatio_1_6 " + aspectRatio_1_6);

                SetHeight(350);

                float width = targetUI.rect.width;
                Debug.Log("INI WIDTH " + width);

                float height = targetUI.rect.height;
                Debug.Log("INI HEIGHT " + height);

            }

            // Ketika aspect ratio = 1.7, maka minHeight dari layout element = X
            else if (currentAspectRatio >= 1.77f && currentAspectRatio <= 1.79f)
            {
                Debug.Log("INI aspectRatio_1_7 " + aspectRatio_1_7);

                SetHeight(300);

                float width = targetUI.rect.width;
                Debug.Log("INI WIDTH " + width);

                float height = targetUI.rect.height;
                Debug.Log("INI HEIGHT " + height);
            }

            // Ketika aspect ratio = 2.3, maka minHeight dari layout element = Y
            else if (currentAspectRatio >= 2.36f && currentAspectRatio <= 2.38f)
            {
                Debug.Log("INI aspectRatio_2_3 " + aspectRatio_2_3);

                SetHeight(185);

                float width = targetUI.rect.width;
                Debug.Log("INI WIDTH " + width);

                float height = targetUI.rect.height;
                Debug.Log("INI HEIGHT " + height);

            }
            else
            {
                Debug.Log("INI else ");
            }
        }
    }

    // Method untuk mengubah height
    public void SetHeight(float newHeight)
    {
        Vector2 size = scrollViewRect.sizeDelta;
        size.y = newHeight;
        scrollViewRect.sizeDelta = size;
    }


}
