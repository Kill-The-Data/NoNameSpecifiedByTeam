using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalcView : AbstractView
{
    [SerializeField] private LerpScore m_ScoreCalulation = null;
    public LerpScore GetScoreCalc() => m_ScoreCalulation;
}
