using UnityEngine;

public enum GamePlayerState
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
	private GamePlayerState m_state = GamePlayerState.inMainMenu;

#pragma warning restore 0649
    #endregion

    #region Getters / Setters

    public GamePlayerState State { get => m_state; set => m_state = value; }

    #endregion

    public void Reset()
	{
		m_state = GamePlayerState.inMainMenu;
	}
}


