using UnityEngine;
using System.Collections;

public class InflictPoisonAbilityEffect : InflictAbilityEffect<PoisonStatusEffect>
{
    private void Start() => Duration = 5;
}
