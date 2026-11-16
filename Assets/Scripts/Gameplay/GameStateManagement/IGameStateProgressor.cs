using System;

/// <summary>
/// Interface given and invoked by a select few classes that are given the responsibility to decide when to move to a new state.
/// </summary>
public interface IGameStateProgressor
{
    void InvokeThisWhenGameStateShouldUpdate(GameState newState, Action<GameState> onGameStateChanged);
}