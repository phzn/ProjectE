using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x020000D4 RID: 212
	[Serializable]
	public class MMSoundManagerAudioPool
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00015A44 File Offset: 0x00013C44
		public virtual void FillAudioSourcePool(int poolSize, Transform parent)
		{
			if (this._pool == null)
			{
				this._pool = new List<AudioSource>();
			}
			if (poolSize <= 0 || this._pool.Count >= poolSize)
			{
				return;
			}
			foreach (AudioSource audioSource in this._pool)
			{
				Object.Destroy(audioSource.gameObject);
			}
			for (int i = 0; i < poolSize; i++)
			{
				GameObject gameObject = new GameObject("MMAudioSourcePool_" + i.ToString());
				SceneManager.MoveGameObjectToScene(gameObject.gameObject, parent.gameObject.scene);
				AudioSource audioSource2 = gameObject.AddComponent<AudioSource>();
				MMFollowTarget mmfollowTarget = gameObject.AddComponent<MMFollowTarget>();
				mmfollowTarget.enabled = false;
				mmfollowTarget.DisableSelfOnSetActiveFalse = true;
				gameObject.transform.SetParent(parent);
				gameObject.SetActive(false);
				this._pool.Add(audioSource2);
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00015B30 File Offset: 0x00013D30
		public virtual IEnumerator AutoDisableAudioSource(float duration, AudioSource source, AudioClip clip, bool doNotAutoRecycleIfNotDonePlaying, float playbackTime, float playbackDuration)
		{
			while (source.time == 0f && source.isPlaying)
			{
				yield return null;
			}
			float num = ((playbackDuration > 0f) ? playbackDuration : duration);
			yield return MMCoroutine.WaitForUnscaled(num);
			if (source.clip != clip)
			{
				yield break;
			}
			if (doNotAutoRecycleIfNotDonePlaying)
			{
				float maxTime = ((playbackDuration > 0f) ? (playbackTime + playbackDuration) : source.clip.length);
				while (source.time != 0f && source.time <= maxTime)
				{
					yield return null;
				}
			}
			source.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00015B68 File Offset: 0x00013D68
		public virtual AudioSource GetAvailableAudioSource(bool poolCanExpand, Transform parent)
		{
			foreach (AudioSource audioSource in this._pool)
			{
				if (!audioSource.gameObject.activeInHierarchy)
				{
					audioSource.gameObject.SetActive(true);
					return audioSource;
				}
			}
			if (poolCanExpand)
			{
				GameObject gameObject = new GameObject("MMAudioSourcePool_" + this._pool.Count.ToString());
				SceneManager.MoveGameObjectToScene(gameObject.gameObject, parent.gameObject.scene);
				AudioSource audioSource2 = gameObject.AddComponent<AudioSource>();
				gameObject.transform.SetParent(parent);
				gameObject.SetActive(true);
				this._pool.Add(audioSource2);
				return audioSource2;
			}
			return null;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00015C38 File Offset: 0x00013E38
		public virtual bool FreeSound(AudioSource sourceToStop)
		{
			foreach (AudioSource audioSource in this._pool)
			{
				if (audioSource == sourceToStop)
				{
					audioSource.Stop();
					audioSource.gameObject.SetActive(false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040005D2 RID: 1490
		protected List<AudioSource> _pool;
	}
}
