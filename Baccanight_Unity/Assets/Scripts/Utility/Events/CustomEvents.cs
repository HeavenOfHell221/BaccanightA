using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A Vector2 unity event.
/// </summary>
[System.Serializable]
public class Vector2Event : UnityEvent<Vector2> { }

[System.Serializable]
public class BossStateEvent : UnityEvent<BossActionType> { }

/// <summary>
/// A boolean unity event.
/// </summary>
[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

/// <summary>
/// A GameObject unity event.
/// </summary>
[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }

/// <summary>
/// Float and GameObject unity event.
/// </summary>
[System.Serializable]
public class LifeEvent : UnityEvent<float, GameObject> { }

/// <summary>
/// A float unity event.
/// </summary>
[System.Serializable]
public class FloatEvent : UnityEvent<float> { }