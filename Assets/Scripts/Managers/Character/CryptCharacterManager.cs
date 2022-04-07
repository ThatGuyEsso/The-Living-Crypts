using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptCharacterManager : MonoBehaviour
{
    public System.Action<CryptCharacterManager> OnCharacterRemoved;
    public System.Action<List<CryptCharacterManager>> OnCharactersAdded;
    public System.Action<CryptCharacterManager> OnAddCharacter;


    public void RemoveSelf()
    {
        OnCharacterRemoved?.Invoke(this);
    }


    public void AddNewCharacter(CryptCharacterManager character)
    {
        OnAddCharacter?.Invoke(character);
    }
}
