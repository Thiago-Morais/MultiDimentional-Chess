using System;

public interface IMediator<in W> where W : Enum
{
    void SignOn();
    void Notify(W intFlag);
}