using System;
using System.Collections;
using UnityEngine;

namespace Tools
{
    public static class CoHelper
    {
        public static IEnumerator WaitAndDo (float time, Action action) {
            yield return new WaitForSeconds (time);
            action();
        }
    }
}