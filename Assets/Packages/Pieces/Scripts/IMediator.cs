using System;
using UnityEngine;

public interface IMediator<in W> where W : Enum
{
    void SignOn();
    void Notify(W intFlag);
}
// public interface IMediatorInstance<T, W> where T : MonoBehaviour where W : Enum
// {
//     MediatorConcrete<T, W> Mediator { get; set; }
//     void SignOn();
//     void Notify(W intFlag);
// }