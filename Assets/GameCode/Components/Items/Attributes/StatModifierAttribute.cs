using UnityEngine;

public class StatModifierAttribute : MonoBehaviour, IAttribute
{
    public StatTypes Stat;
    public int Modifier;

    private Stats Stats => Target.GetComponent<Stats>();

    public GameObject Owner { get; private set; }
    public GameObject Target { get; private set; }

    public void Activate(GameObject target, bool use = false)
    {
        if (Target == null)
        {
            Target = target;
            OnActivate();
            if (use) Target = null;
        }
    }

    public void Deactivate()
    {
        if (Target != null)
        {
            OnDeactivate();
            Target = null;
        }
    }

    public void OnActivate() => Stats[Stat] += Modifier;
    public void OnDeactivate() => Stats[Stat] -= Modifier;
}
