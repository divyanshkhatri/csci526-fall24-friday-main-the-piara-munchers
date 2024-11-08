using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlagColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void ChangeColorToYellow()
    {
        GetComponent<Renderer>().material.color = Color.green; 
    }
}
