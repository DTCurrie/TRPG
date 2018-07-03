public interface IState
{
    void Enter();
    void Exit();
    void OnDestroy();
    void AddListeners();
    void RemoveListeners();
}