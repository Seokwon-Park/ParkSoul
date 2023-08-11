using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}