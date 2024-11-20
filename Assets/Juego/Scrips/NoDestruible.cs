using UnityEngine;
using System.Collections;

public class NoDestruible : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}