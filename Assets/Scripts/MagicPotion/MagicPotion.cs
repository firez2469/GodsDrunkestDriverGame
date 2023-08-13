using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : MonoBehaviour
{
    [SerializeField] private GameObject magicPotion;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrinkPotion();
    }

    void DrinkPotion()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            magicPotion.SetActive(true);
        }

    }
}
