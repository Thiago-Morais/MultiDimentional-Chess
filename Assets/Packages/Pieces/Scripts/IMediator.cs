using System;
using UnityEngine;

internal interface IMediator<T, W> where T : MonoBehaviour where W : Enum
{
    void SignOn(T sender);
    void Notify(T sender, W intFlag);
}