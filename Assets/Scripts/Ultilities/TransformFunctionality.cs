using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Currently not being used
public static class TransformFunctionality
{
    public static void MakeTwoTransformEqual(Vector3 _sourcePos, Transform _constraintTransform)
    {
        _constraintTransform.position = _sourcePos;
    }

    public static void MakeTwoTransformEqual(Transform _sourceTransform, Transform _constraintTransform)
    {
        _constraintTransform.position = _sourceTransform.position;
    }
}
