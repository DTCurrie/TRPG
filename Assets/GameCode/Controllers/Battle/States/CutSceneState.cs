using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CutSceneState : MonoBehaviour, IBattleState
{
    private ConversationController _controller;
    private ConversationData _conversation;

    public BattleController Controller { get; private set; }

    public AbilityMenuController AbilityMenuController => Controller.AbilityMenuController;
    public StatPanelController StatPanelController => Controller.StatPanelController;

    public TurnData Turn => Controller.Turn;
    public List<Unit> Units => Controller.Units;

    private void OnCompleteConversation(object sender, EventArgs e) =>
        Controller.StateMachine.ChangeState<SelectUnitState_PROTOTYPE>();

    private void Awake()
    {
        _controller = GetComponentInChildren<ConversationController>();
        _conversation = Resources.Load<ConversationData>("Conversations/IntroScene");
        Controller = GetComponent<BattleController>();
    }

    public void Enter()
    {
        AddListeners();
        _controller.Show(_conversation);
    }

    public void Exit() => RemoveListeners();

    public void OnDestroy()
    {
        RemoveListeners();
        if (_conversation) Resources.UnloadAsset(_conversation);
    }

    public void AddListeners()
    {
        this.ToggleListeners(true);
        ConversationController.CompleteEvent += OnCompleteConversation;
    }

    public void RemoveListeners()
    {
        this.ToggleListeners(false);
        ConversationController.CompleteEvent -= OnCompleteConversation;
    }

    public void OnMove(object sender, DataEventArgs<float2> e) { }

    public void OnFire(object sender, DataEventArgs<int> e) => _controller.Next();
}
