using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersist : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
