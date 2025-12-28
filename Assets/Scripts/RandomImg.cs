using UnityEngine;

public class RandomImg : MonoBehaviour
{
    public GameObject[] Imgs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int indexx = Random.Range(0,5);
        Imgs[indexx].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
