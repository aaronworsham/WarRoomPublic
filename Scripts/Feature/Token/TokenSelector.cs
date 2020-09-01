using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sazboom.WarRoom
{
    public class TokenSelector : MonoBehaviour
    {
        [SerializeField] private GameObject aalan;
        [SerializeField] private GameObject fayzyre;
        [SerializeField] private GameObject galan;
        [SerializeField] private GameObject kreck;
        [SerializeField] private GameObject liam;
        [SerializeField] private GameObject tbom;
        [SerializeField] private GameObject zizkek;
        [SerializeField] private GameObject selectedToken;
        [SerializeField] private string selectedTokenString;

        public GameObject SelectedToken { get { return selectedToken; }}
        public string SelectedTokenString { get { return selectedTokenString; }}

        Dictionary<string, GameObject> tokenLookup = new Dictionary<string, GameObject>();


        public void Awake()
        {
            tokenLookup.Add("aalan", aalan);
            tokenLookup.Add("fayzyre", fayzyre);
            tokenLookup.Add("galan", galan);
            tokenLookup.Add("kreck", kreck);
            tokenLookup.Add("liam", liam);
            tokenLookup.Add("tbom", tbom);
            tokenLookup.Add("zizkek", zizkek);

            LoadUI.OnCharacterSelection += HandleCharacterSelection;

        }

        public void HandleCharacterSelection(string name)
        {
            selectedTokenString = name;
            selectedToken = tokenLookup[name];
            
        }

        //public int SelectRandomToken()
        //{
        //    int index = UnityEngine.Random.Range(0, tokens.Count - 1);
        //    selectedTokenIndex = id;
        //    selectedToken = tokens[id];
        //    return id;
        //}

        public GameObject SetToken(string name)
        {
            selectedToken = tokenLookup[name];
            return selectedToken;
        }

        public GameObject GetTokenFromName(string name)
        {
            return tokenLookup[name];
        }


        


    }
}


