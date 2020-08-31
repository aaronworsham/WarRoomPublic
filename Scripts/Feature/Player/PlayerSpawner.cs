using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Sazboom.WarRoom
{

}

public class PlayerSpawner : MonoBehaviour
{
    #region Types
    [System.Serializable]
    private class CharacterTokenPrefab
    {
        [Tooltip("Prefab Spawn for the character type")]
        [SerializeField] private GameObject _prefab;

        public GameObject PreFab { get { return _prefab; } }

        [Tooltip("Character Type the prefab is for")]
        [SerializeField] private CharacterToken _characterToken;

        public CharacterToken CharacterToken { get { return _characterToken; } }
    }
    #endregion

    [SerializeField]
    private List<CharacterToken> _charactertokens = new List<CharacterToken>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
