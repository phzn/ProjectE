using System;
using Features.CielVoice;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x0200103D RID: 4157
	public sealed class IntroMenuMusic : MonoBehaviour
	{
		// Token: 0x06007E32 RID: 32306 RVA: 0x00339BC9 File Offset: 0x00337DC9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			IntroMenuMusic._instance = null;
		}

		// Token: 0x06007E33 RID: 32307 RVA: 0x00339BD4 File Offset: 0x00337DD4
		public static void Begin(AudioClip clip, float volume, float start, float end, float overlap)
		{
			if (clip == null || IntroMenuMusic._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Intro Menu Music");
			Object.DontDestroyOnLoad(gameObject);
			IntroMenuMusic introMenuMusic = gameObject.AddComponent<IntroMenuMusic>();
			IntroMenuMusic._instance = introMenuMusic;
			introMenuMusic._clip = clip;
			introMenuMusic._volume = volume;
			introMenuMusic._loop = new CrossfadeLoopPlayer(introMenuMusic.CreateSource(), introMenuMusic.CreateSource(), clip, overlap, start, end);
		}

		// Token: 0x06007E34 RID: 32308 RVA: 0x00339C3E File Offset: 0x00337E3E
		private AudioSource CreateSource()
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.spatialBlend = 0f;
			audioSource.dopplerLevel = 0f;
			audioSource.ignoreListenerVolume = true;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Music);
			return audioSource;
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x00339C78 File Offset: 0x00337E78
		private void Update()
		{
			if (this._loop == null || this._clip == null)
			{
				return;
			}
			if (!this._loop.IsPlaying && this._clip.loadState == AudioDataLoadState.Loaded)
			{
				this._loop.Play();
			}
			if (!this._loop.IsPlaying)
			{
				return;
			}
			this._elapsed += Time.unscaledDeltaTime;
			this._loop.Tick(this._volume * Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this._elapsed / 1.2f)));
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x00339D14 File Offset: 0x00337F14
		public static bool TryClaim(Transform owner, AudioClip clip, out CrossfadeLoopPlayer loop)
		{
			loop = null;
			if (IntroMenuMusic._instance == null || IntroMenuMusic._instance._clip != clip)
			{
				return false;
			}
			IntroMenuMusic instance = IntroMenuMusic._instance;
			loop = instance._loop;
			instance._loop = null;
			IntroMenuMusic._instance = null;
			instance.transform.SetParent(owner, false);
			Object.Destroy(instance);
			return true;
		}

		// Token: 0x06007E37 RID: 32311 RVA: 0x00339D74 File Offset: 0x00337F74
		public static void CancelUnclaimed()
		{
			if (IntroMenuMusic._instance != null)
			{
				Object.Destroy(IntroMenuMusic._instance.gameObject);
			}
		}

		// Token: 0x06007E38 RID: 32312 RVA: 0x00339D92 File Offset: 0x00337F92
		private void OnDestroy()
		{
			CrossfadeLoopPlayer loop = this._loop;
			if (loop != null)
			{
				loop.Stop();
			}
			if (IntroMenuMusic._instance == this)
			{
				IntroMenuMusic._instance = null;
			}
		}

		// Token: 0x040081D1 RID: 33233
		private static IntroMenuMusic _instance;

		// Token: 0x040081D2 RID: 33234
		private CrossfadeLoopPlayer _loop;

		// Token: 0x040081D3 RID: 33235
		private AudioClip _clip;

		// Token: 0x040081D4 RID: 33236
		private float _volume;

		// Token: 0x040081D5 RID: 33237
		private float _elapsed;
	}
}
