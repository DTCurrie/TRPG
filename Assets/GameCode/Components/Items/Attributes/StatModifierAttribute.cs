using UnityEngine;

public class StatModifierAttribute : MonoBehaviour, IAttribute
{
    private GameObject _owner;
    private GameObject _target;

    public StatTypes Stat;
    public int Modifier;

    private Stats Stats => _target.GetComponent<Stats>();

    public GameObject Owner => _owner;
    public GameObject Target => _target;

    public void Activate(GameObject target, bool use = false)
    {
        if (_target == null)
        {
            _target = target;
            OnActivate();
            if (use) _target = null;
        }
    }

    public void Deactivate()
    {
        if (_target != null)
        {
            OnDeactivate();
            _target = null;
        }
    }

    public void OnActivate() => Stats[Stat] += Modifier;
    public void OnDeactivate() => Stats[Stat] -= Modifier;
}
