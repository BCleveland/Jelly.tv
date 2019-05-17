using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T s_instance;

    public static T Instance {
        get {
            if (s_instance == null) {
                s_instance = FindObjectOfType<T>();
                if (s_instance == null) {
                    GameObject go = new GameObject();
                    s_instance = go.AddComponent<T>();
                    go.name = "Singleton " + typeof(T).ToString();
                }
            }
            return s_instance;
        }
    }
}
