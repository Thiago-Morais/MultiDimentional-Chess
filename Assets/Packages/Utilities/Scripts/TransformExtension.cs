using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class TransformExtension
    {
        public static void LerpTransform(this Transform to, Transform from, float time)
        {
        }
        public static void SetTransform(this Transform to, Transform from, float time)
        {
            to.position = from.position;
        }
    }
}