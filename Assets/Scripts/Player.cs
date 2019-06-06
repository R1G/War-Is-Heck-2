using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void CreatePlayer(GameObject go);
    public static event CreatePlayer OnCreatePlayer;

    void Start() {
        OnCreatePlayer?.Invoke(gameObject);
    }
}
