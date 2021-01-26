using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
public class BindingTests
{
    static object[] BindedVector_IsBinded =
    {
        new object[] { new Vector3Int(0, 0, 0), true, },
        new object[] { new Vector3Int(1, 0, 0), true, },
        new object[] { new Vector3Int(-1, 0, 0), true, },
        new object[] { new Vector3Int(0, 2, 2), true, },
        new object[] { new Vector3Int(0, -2, 2), true, },
        new object[] { new Vector3Int(3, 3, 3), true, },
        new object[] { new Vector3Int(3, -3, 3), true, },
        new object[] { new Vector3Int(-3, -3, -3), true, },
        new object[] { new Vector3Int(1, 2, 0), false, },
        new object[] { new Vector3Int(3, 0, 4), false, },
        new object[] { new Vector3Int(5, 6, 7), false, },
        new object[] { new Vector3Int(1, 1, 2), false, },
    };
    static object[] BindedVector_BindedValues =
    {
        new object[] { new Vector3Int(0, 0, 0), new List<int> { }, },
        new object[] { new Vector3Int(1, 0, 0), new List<int> { 1 }, },
        new object[] { new Vector3Int(-1, 0, 0), new List<int> { -1 }, },
        new object[] { new Vector3Int(0, 2, 2), new List<int> { 2, 2 }, },
        new object[] { new Vector3Int(0, -2, 2), new List<int> { -2, 2 }, },
        new object[] { new Vector3Int(3, 3, 3), new List<int> { 3, 3, 3 }, },
        new object[] { new Vector3Int(3, -3, 3), new List<int> { 3, -3, 3 }, },
        new object[] { new Vector3Int(-3, -3, -3), new List<int> { -3, -3, -3 }, },
    };
    static object[] BindedVector_AsVectorSign =
    {
        new object[] { new Vector3Int(0, 0, 0), new Vector3Int(0, 0, 0), },
        new object[] { new Vector3Int(1, 0, 0), new Vector3Int(1, 0, 0), },
        new object[] { new Vector3Int(-1, 0, 0), new Vector3Int(-1, 0, 0), },
        new object[] { new Vector3Int(0, 2, 2), new Vector3Int(0, 1, 1), },
        new object[] { new Vector3Int(0, -2, 2), new Vector3Int(0, -1, 1), },
        new object[] { new Vector3Int(3, 3, 3), new Vector3Int(1, 1, 1), },
        new object[] { new Vector3Int(3, -3, 3), new Vector3Int(1, -1, 1), },
        new object[] { new Vector3Int(-3, -3, -3), new Vector3Int(-1, -1, -1), },
    };
    #region -------- IS BINDED IGNORING ZERO
    [TestCaseSource(nameof(BindedVector_IsBinded))]
    [Test]
    public void IsBindedIgnoringZero_BindedVector_IsBinded(Vector3Int vector, bool expected)
    {
        //SETUP
        //ACT
        Binding binding = Binding.IsBindedIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.IsBinded);
    }
    [TestCaseSource(nameof(BindedVector_BindedValues))]
    [Test]
    public void IsBindedIgnoringZero_BindedVector_BindedValues(Vector3Int vector, List<int> expected)
    {
        //SETUP
        //ACT
        Binding binding = Binding.IsBindedIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.binded);
    }
    [TestCaseSource(nameof(BindedVector_AsVectorSign))]
    [Test]
    public void IsBindedIgnoringZero_BindedVector_AsVectorSign(Vector3Int vector, Vector3Int expected)
    {
        //SETUP
        //ACT
        Binding binding = Binding.IsBindedIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.signVector);
    }
    #endregion //IS BINDED IGNORING ZERO

    [TestCaseSource(nameof(BindedVector_IsBinded))]
    [Test]
    public void SetBindingIgnoringZero_BindedVector_IsBinded(Vector3Int vector, bool expected)
    {
        //SETUP
        Binding binding = new Binding();
        //ACT
        binding = binding.SetBindingIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.IsBinded);
    }
    [TestCaseSource(nameof(BindedVector_BindedValues))]
    [Test]
    public void SetBindingIgnoringZero_BindedVector_BindedValues(Vector3Int vector, List<int> expected)
    {
        //SETUP
        Binding binding = new Binding();
        //ACT
        binding = binding.SetBindingIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.binded);
    }
    [TestCaseSource(nameof(BindedVector_AsVectorSign))]
    [Test]
    public void SetBindedIgnoringZero_BindedVector_AsVectorSign(Vector3Int vector, Vector3Int expected)
    {
        //SETUP
        Binding binding = new Binding();
        //ACT
        binding = binding.SetBindingIgnoringZero(vector);
        //ASSERT
        Assert.AreEqual(expected, binding.signVector);
    }
}
