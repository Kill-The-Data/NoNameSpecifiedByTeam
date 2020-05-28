using System;
using UnityEngine;

public class MaximumDebrisCount : MonoBehaviour
{
    [LabelOverride("Max Debris")]
    [SerializeField] private int m_maxDebrisImpl = 2048;

    private static int m_maxDebris = 2048;
    private static int m_currentDebris = 0;

    private void Awake()
    {
        m_maxDebris = m_maxDebrisImpl;
    }


    public static bool AddDebris()
    {
        if (m_currentDebris + 1 > m_maxDebris)
        {
            return false;
        }

        m_currentDebris++;
        return true;
    }

    public static void RemoveDebris()
    {
        m_currentDebris--;
    }

    public static void ClearDebris()
    {
        m_currentDebris = 0;
    }

}
