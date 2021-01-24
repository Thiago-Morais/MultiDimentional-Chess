using System;
using System.Collections.Generic;
using UnityEngine;

public class Binding
{
    #region -------- FIELDS
    bool isBinded;
    public List<int> binded = new List<int>();
    public Vector3Int signVector;
    #endregion //FIELDS

    #region -------- PROPERTIES
    public bool IsBinded { get => isBinded; private set => isBinded = value; }
    #endregion //PROPERTIES

    #region -------- METHODS
    public Binding SetBindingIgnoringZero(Vector3Int vector)
    {
        IsBinded = true;
        List<int> bindedValues = new List<int>() { vector.x, vector.y, vector.z };
        bindedValues.RemoveAll(i => i == 0);

        if (bindedValues.Count >= 0)
        {
            SetSignVector(vector);
            if (bindedValues.Count >= 1)
                AddBindedValuesAndSetIsBinded(bindedValues);
        }
        return this;
    }
    void SetSignVector(Vector3Int vector) => signVector.Set(Math.Sign(vector.x), Math.Sign(vector.y), Math.Sign(vector.z));
    void AddBindedValuesAndSetIsBinded(List<int> bindedValues)
    {
        for (int i = 0; i < bindedValues.Count - 1; i++)
        {
            bool currentIsDifferentThanNext = Mathf.Abs(bindedValues[i]) != Mathf.Abs(bindedValues[i + 1]);
            if (currentIsDifferentThanNext)
                IsBinded = false;
            else
                binded.Add(bindedValues[i]);
        }

        binded.Add(bindedValues[bindedValues.Count - 1]);
    }
    public static Binding IsBindedIgnoringZero(Vector3Int vector) => new Binding().SetBindingIgnoringZero(vector);
    #endregion //METHODS
}