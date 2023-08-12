using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WorldStateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fantasyVolume;
    [SerializeField]
    private GameObject darkVolume;
    [SerializeField]
    private Camera darkCamera;
    [SerializeField]
    private Camera fantasyCamera;
    [SerializeField]
    private WorldState currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case WorldState.DARK:
                fantasyVolume.gameObject.SetActive(false); 
                darkVolume.gameObject.SetActive(true); 
                darkCamera.gameObject.SetActive(true);
                fantasyCamera.gameObject.SetActive(false);
                break;
            case WorldState.FANTASY:
                fantasyVolume.gameObject.SetActive(true); 
                darkVolume.gameObject.SetActive(false);
                darkCamera.gameObject.SetActive(false);
                fantasyCamera.gameObject.SetActive(true);
                break;
        }
    }
}
public enum WorldState { DARK,FANTASY}