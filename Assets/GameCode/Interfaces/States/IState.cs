public interface IState : IListener
{
    void Enter();
    void Exit();
    void OnDestroy();
}