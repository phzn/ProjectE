using System;
using System.Collections;
using Invector;
using UnityEngine;

// Token: 0x0200010D RID: 269
[RequireComponent(typeof(AudioSource))]
public class PlaySoundLoop : MonoBehaviour
{
	// Token: 0x06000991 RID: 2449 RVA: 0x00044312 File Offset: 0x00042512
	public void ChangeCurveTime(float curveTime)
	{
		this.currentCurveTime = curveTime;
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0004431B File Offset: 0x0004251B
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		if (this.source != null && this.playOnAwake)
		{
			this.Play();
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x00044345 File Offset: 0x00042545
	public void Play()
	{
		this.isPlaying = true;
		base.StopAllCoroutines();
		if (base.gameObject.activeInHierarchy)
		{
			this.currentCurveTime = 0f;
			base.StartCoroutine(this.PlayRoutine());
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00044379 File Offset: 0x00042579
	public void Stop()
	{
		this.isPlaying = false;
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00044382 File Offset: 0x00042582
	private IEnumerator PlayRoutine()
	{
		float time = 0f;
		while (this.isPlaying)
		{
			float num = this.frequencyCurve.Evaluate(this.currentCurveTime);
			if (time < num)
			{
				time += Time.deltaTime;
			}
			else
			{
				time = 0f;
				this.PlayOnShot();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00044394 File Offset: 0x00042594
	protected virtual void PlayOnShot()
	{
		if (this.source)
		{
			this.frequencyCurve.Evaluate(this.currentCurveTime);
			float num = this.volumeCurve.Evaluate(this.currentCurveTime);
			float num2 = this.pitchCurve.Evaluate(this.currentCurveTime);
			AudioClip audioClip = this.clips[Random.Range(0, this.clips.Length)];
			if (audioClip != null)
			{
				this.source.volume = num;
				this.source.pitch = num2;
				this.source.PlayOneShot(audioClip);
			}
		}
	}

	// Token: 0x040008DA RID: 2266
	public AudioClip[] clips;

	// Token: 0x040008DB RID: 2267
	public bool playOnAwake;

	// Token: 0x040008DC RID: 2268
	public AnimationCurve volumeCurve;

	// Token: 0x040008DD RID: 2269
	public AnimationCurve pitchCurve;

	// Token: 0x040008DE RID: 2270
	public AnimationCurve frequencyCurve;

	// Token: 0x040008DF RID: 2271
	protected AudioSource source;

	// Token: 0x040008E0 RID: 2272
	[vReadOnly(true)]
	public float currentCurveTime;

	// Token: 0x040008E1 RID: 2273
	private bool isPlaying;
}
