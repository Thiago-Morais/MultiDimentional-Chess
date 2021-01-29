using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    [Serializable]
    public sealed class Name
    {
        [SerializeField] GameObject gameObject;
        [SerializeField] int value;
        CanvasVisibility setBool;

        public GameObject GameObject { get => gameObject; set => gameObject = value; }
        public int Value { get => value; set => this.value = value; }
    }
    public void Log(string message) => Debug.Log($"{message}", gameObject);
    public void Test(Name value) { }
    public void Test0(AnimationEvent value) { }
}
 