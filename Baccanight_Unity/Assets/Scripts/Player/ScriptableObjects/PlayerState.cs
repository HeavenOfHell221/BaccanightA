using UnityEngine;

public enum GameState
{
	inMainMenu,
	inPause,
	inGame,
	inDie,
    inLoading
}

[CreateAssetMenu(fileName = "PlayerState", menuName = "AssetProject/PlayerState")]
public class PlayerState : ScriptableObject
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private GameState m_state = GameState.inMainMenu;

#pragma warning restore 0649
    #endregion

    #region Getters / Setters

    public GameState State { get => m_state; set => m_state = value; }

    #endregion

    public void Reset()
	{
		m_state = GameState.inMainMenu;
	}
}


