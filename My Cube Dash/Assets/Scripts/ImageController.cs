using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{

    public Image MyImage;
    public Sprite MySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyImage.enabled = false;
        MyImage.color = Color.red;
        //MyImage.enabled = false;
        //MyImage.color = Color.red;
        //MyImage.color = new Vector4(1, 0, 0, 1);
        MyImage.sprite = MySprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
