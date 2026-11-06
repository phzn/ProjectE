using System;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000E01 RID: 3585
	public static class TaserSfx
	{
		// Token: 0x06006ACD RID: 27341 RVA: 0x002ADA24 File Offset: 0x002ABC24
		public static void RegisterElectroshockClip(AudioClip clip)
		{
			if (clip != null)
			{
				TaserSfx._electroshockClip = clip;
			}
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x002ADA35 File Offset: 0x002ABC35
		public static void PlayShockOn(GameObject target)
		{
			if (target == null || TaserSfx._electroshockClip == null)
			{
				return;
			}
			TaserSfx.EnsureChildAudio(target).Play(TaserSfx._electroshockClip);
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x002ADA60 File Offset: 0x002ABC60
		private static TaserShockAudio EnsureChildAudio(GameObject target)
		{
			Transform transform = target.transform.Find("TaserShockAudio");
			if (transform != null)
			{
				TaserShockAudio component = transform.GetComponent<TaserShockAudio>();
				if (component != null)
				{
					return component;
				}
			}
			GameObject gameObject = new GameObject("TaserShockAudio");
			gameObject.transform.SetParent(target.transform, false);
			gameObject.transform.localPosition = new Vector3(0f, 1.2f, 0f);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 1f;
			audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
			audioSource.minDistance = 1.5f;
			audioSource.maxDistance = 18f;
			audioSource.dopplerLevel = 0f;
			audioSource.spread = 50f;
			audioSource.priority = 110;
			TaserShockAudio taserShockAudio = gameObject.AddComponent<TaserShockAudio>();
			taserShockAudio.Bind(audioSource);
			return taserShockAudio;
		}

		// Token: 0x04006D82 RID: 28034
		private static AudioClip _electroshockClip;

		// Token: 0x04006D83 RID: 28035
		private const string AudioChildName = "TaserShockAudio";
	}
}
