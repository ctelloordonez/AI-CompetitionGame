using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAmmo : MonoBehaviour
{

    public GameObject tankShell;
    public GameObject tankBullet;
    // Start is called before the first frame update
    void Start()
    {
        tankBullet.SetActive(true);
        tankShell.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            tankBullet.SetActive(true);
            tankShell.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            tankBullet.SetActive(false);
            tankShell.SetActive(true);
        }
    }
}
