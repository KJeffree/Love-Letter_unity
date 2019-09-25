using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int value;
    public int count;


    [SerializeField] Sprite front;

    [SerializeField] Sprite back;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }

    public Sprite GetBackImage()
    {
        return this.back;
    }

    public Sprite GetFrontImage()
    {
        return this.front;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
