using UnityEngine;

public class SpriteOrderInLayerZ : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = (int)(this.transform.position.z * 100) * -1;
    }
}
