using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private bool _transitioning;

    public IState CurrentState { get => _currentState; set => Transition(value); }

    private void Transition(IState state)
    {
        if (_currentState == state || _transitioning) return;
        _transitioning = true;

        if (_currentState != null) _currentState.Exit();
        _currentState = state;

        if (_currentState != null) _currentState.Enter();
        _transitioning = false;
    }

    public T GetState<T>() where T : Component, IState
    {
        var target = GetComponent<T>();
        if (target == null)
        {
            target = gameObject.AddComponent<T>();
        }
        return target;
    }

    public void ChangeState<T>() where T : Component, IState
    {
        CurrentState = GetState<T>();
    }
}
