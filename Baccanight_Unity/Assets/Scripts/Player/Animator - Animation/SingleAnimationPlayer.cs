using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class SingleAnimationPlayer : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private UnityEvent m_AfterAnimation;

	[SerializeField]
	private bool m_playOnce;

#pragma warning restore 0649
	#endregion

	#region Variables

	private Animator anim;
	private bool isPlayingAnimation = false;
	bool have_played = false;

	#endregion

	void Start()
	{
		anim = GetComponent<Animator>();
		anim.enabled = false;
	}

	public void Play(string stateAnimatorToPlay)
	{
		if (have_played && m_playOnce)
			return;

		if (!isPlayingAnimation)
			StartCoroutine(_Play(stateAnimatorToPlay));
	}

	IEnumerator _Play(string stateAnimatorToPlay)
	{
		isPlayingAnimation = true;
		anim.enabled = true;
		anim.Play(stateAnimatorToPlay, 0, 0f);
		yield return new WaitForSeconds(GetLengthTime());
		anim.enabled = false;
		m_AfterAnimation.Invoke();
		have_played = true;
		isPlayingAnimation = false;
	}

	public float GetLengthTime()
	{
		return anim.GetCurrentAnimatorStateInfo(0).length;
	}
}
