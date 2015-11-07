using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Function definition for an event raised when a level pack is selected.
/// </summary>
public class LevelPackSelectedEvent : UnityEvent<LevelPack> {}

/// <summary>
/// Function definition for an event raised when a level is selected.
/// </summary>
public class LevelSelectedEvent : UnityEvent<int> {}

public class ScreenRequestBackEvent : UnityEvent<GameScreen> {}
