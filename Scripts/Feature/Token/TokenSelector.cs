using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace Sazboom.WarRoom
{
    public class TokenSelector : NetworkBehaviour
    {
        [SerializeField] private GameObject aalan;
        [SerializeField] private GameObject fayzyre;
        [SerializeField] private GameObject galan;
        [SerializeField] private GameObject kreck;
        [SerializeField] private GameObject liam;
        [SerializeField] private GameObject tbom;
        [SerializeField] private GameObject zizkek;
        [SerializeField] private GameObject selectedToken;

        public GameObject SelectedToken { get { return selectedToken; }}
        
        List<GameObject> tokens = new List<GameObject>();

        public void Awake()
        {
            base.OnStartServer();
            tokens.Add(aalan);
            tokens.Add(fayzyre);
            tokens.Add(galan);
            tokens.Add(kreck);
            tokens.Add(liam);
            tokens.Add(tbom);
            tokens.Add(zizkek);

        }


        public int SelectRandomToken()
        {
            int id = UnityEngine.Random.Range(0, tokens.Count - 1);
            selectedToken = tokens[id];
            return id;
        }

        public GameObject SetToken(int id)
        {
            selectedToken = tokens[id];
            return selectedToken;
        }

        


    }
}


