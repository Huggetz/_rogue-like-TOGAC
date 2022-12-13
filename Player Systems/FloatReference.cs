using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatReference
{
    public ValueSO _realValue;
    public float _constantValue;
    public bool _usingConstant;

    public float _value
    {
        get { return _usingConstant ? _constantValue : _realValue.value; }
    }
}
