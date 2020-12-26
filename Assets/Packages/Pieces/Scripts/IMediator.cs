using System;
using UnityEngine;

public interface IMediator<in T, in W> where T : MonoBehaviour where W : Enum
{
    void SignOn(T sender);
    void Notify(W intFlag);
}