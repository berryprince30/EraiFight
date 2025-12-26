using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject PortalOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TelePortal(GameObject gameObject)
    {
        gameObject.transform.position = PortalOut.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Body")
        {
            TelePortal(other.gameObject);
        }
    }
}
