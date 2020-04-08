using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "AssetProject/PlayerHealth")]
public class PlayerHealth : ScriptableObject
{
	#region Inspector
#pragma warning disable 0649
	[SerializeField]
	private int m_maxHealth = 50;

	[SerializeField]
	private int m_currentHealth;

	[SerializeField]
	private bool m_isDead = false;

	[SerializeField]
	private bool m_isInvincible = false;

#pragma warning restore 0649
	#endregion

	#region Getters / Setters

	public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }
	public int MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }
	public bool IsDead { get => m_isDead; set => m_isDead = value; }
	public bool IsInvincible { get => m_isInvincible; set => m_isInvincible = value; }

	public float GetRatio() => CurrentHealth / MaxHealth;

	#endregion

	public void Reset()
	{
		m_maxHealth = 50;
		m_currentHealth = m_maxHealth;
		m_isDead = false;
		m_isInvincible = false;
	}
}