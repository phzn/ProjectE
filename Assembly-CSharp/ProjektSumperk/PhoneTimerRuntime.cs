using System;
using Features.MainMenu;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x02000409 RID: 1033
	internal static class PhoneTimerRuntime
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x000A66E4 File Offset: 0x000A48E4
		public static float Duration
		{
			get
			{
				return PhoneTimerRuntime._duration;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x000A66EB File Offset: 0x000A48EB
		public static bool IsRunning
		{
			get
			{
				return PhoneTimerRuntime._running;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000A66F2 File Offset: 0x000A48F2
		public static bool DidFinish
		{
			get
			{
				return PhoneTimerRuntime._didFinish;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x000A66F9 File Offset: 0x000A48F9
		public static float Remaining
		{
			get
			{
				if (PhoneTimerRuntime._running)
				{
					return Mathf.Max(0f, (float)(PhoneTimerRuntime._deadline - Time.realtimeSinceStartupAsDouble));
				}
				if (!PhoneTimerRuntime._didFinish)
				{
					return PhoneTimerRuntime._duration;
				}
				return 0f;
			}
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000A672B File Offset: 0x000A492B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			PhoneTimerRuntime._runner = null;
			PhoneTimerRuntime._deadline = 0.0;
			PhoneTimerRuntime._duration = 300f;
			PhoneTimerRuntime._running = false;
			PhoneTimerRuntime._didFinish = false;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000A6758 File Offset: 0x000A4958
		public static void Ensure()
		{
			if (PhoneTimerRuntime._runner != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Phone Timer Runtime");
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			PhoneTimerRuntime._runner = gameObject.AddComponent<PhoneTimerRuntime.Runner>();
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x000A6796 File Offset: 0x000A4996
		public static void SetDuration(float seconds)
		{
			if (PhoneTimerRuntime._running)
			{
				return;
			}
			PhoneTimerRuntime.Ensure();
			PhoneTimerRuntime._duration = Mathf.Clamp(seconds, 1f, 359999f);
			PhoneTimerRuntime._didFinish = false;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000A67C0 File Offset: 0x000A49C0
		public static void Start(float seconds)
		{
			PhoneTimerRuntime.Ensure();
			PhoneTimerRuntime._duration = Mathf.Clamp(seconds, 1f, 359999f);
			PhoneTimerRuntime._deadline = Time.realtimeSinceStartupAsDouble + (double)PhoneTimerRuntime._duration;
			PhoneTimerRuntime._didFinish = false;
			PhoneTimerRuntime._running = true;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000A67F9 File Offset: 0x000A49F9
		public static void Reset(float seconds)
		{
			PhoneTimerRuntime._running = false;
			PhoneTimerRuntime.SetDuration(seconds);
			PhoneTimerRuntime._didFinish = false;
			if (PhoneTimerRuntime._runner != null)
			{
				PhoneTimerRuntime._runner.StopAlarm();
			}
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x000A6824 File Offset: 0x000A4A24
		private static void Tick()
		{
			if (!PhoneTimerRuntime._running || Time.realtimeSinceStartupAsDouble < PhoneTimerRuntime._deadline)
			{
				return;
			}
			PhoneTimerRuntime._running = false;
			PhoneTimerRuntime._didFinish = true;
			if (PhoneTimerRuntime._runner != null)
			{
				PhoneTimerRuntime._runner.PlayAlarm();
			}
		}

		// Token: 0x04001B5B RID: 7003
		private const float DefaultDuration = 300f;

		// Token: 0x04001B5C RID: 7004
		public const float MaximumDuration = 359999f;

		// Token: 0x04001B5D RID: 7005
		private static PhoneTimerRuntime.Runner _runner;

		// Token: 0x04001B5E RID: 7006
		private static double _deadline;

		// Token: 0x04001B5F RID: 7007
		private static float _duration = 300f;

		// Token: 0x04001B60 RID: 7008
		private static bool _running;

		// Token: 0x04001B61 RID: 7009
		private static bool _didFinish;

		// Token: 0x0200040A RID: 1034
		private sealed class Runner : MonoBehaviour
		{
			// Token: 0x06001A4C RID: 6732 RVA: 0x000A686C File Offset: 0x000A4A6C
			private void Awake()
			{
				this._source = base.gameObject.AddComponent<AudioSource>();
				this._source.playOnAwake = false;
				this._source.spatialBlend = 0f;
				GameAudioRouter.Route(this._source, GameAudioRouter.Channel.Sfx);
				this._clip = PhoneTimerRuntime.Runner.BuildAlarmClip();
			}

			// Token: 0x06001A4D RID: 6733 RVA: 0x000A68BD File Offset: 0x000A4ABD
			private void Update()
			{
				PhoneTimerRuntime.Tick();
			}

			// Token: 0x06001A4E RID: 6734 RVA: 0x000A68C4 File Offset: 0x000A4AC4
			public void PlayAlarm()
			{
				if (this._source != null && this._clip != null)
				{
					this._source.PlayOneShot(this._clip);
				}
			}

			// Token: 0x06001A4F RID: 6735 RVA: 0x000A68F3 File Offset: 0x000A4AF3
			public void StopAlarm()
			{
				if (this._source != null)
				{
					this._source.Stop();
				}
			}

			// Token: 0x06001A50 RID: 6736 RVA: 0x000A6910 File Offset: 0x000A4B10
			private static AudioClip BuildAlarmClip()
			{
				float[] array = new float[Mathf.CeilToInt(27562.5f)];
				for (int i = 0; i < array.Length; i++)
				{
					float num = (float)i / 22050f;
					float num2 = num % 0.4f;
					if (num2 <= 0.18f && num <= 1f)
					{
						float num3 = Mathf.Sin(3.1415927f * num2 / 0.18f);
						array[i] = Mathf.Sin(5529.203f * num) * num3 * 0.32f;
					}
				}
				AudioClip audioClip = AudioClip.Create("Phone Timer Alarm", array.Length, 1, 22050, false);
				audioClip.SetData(array, 0);
				return audioClip;
			}

			// Token: 0x06001A51 RID: 6737 RVA: 0x000A69A5 File Offset: 0x000A4BA5
			private void OnDestroy()
			{
				if (this._clip != null)
				{
					Object.Destroy(this._clip);
				}
				if (PhoneTimerRuntime._runner == this)
				{
					PhoneTimerRuntime._runner = null;
				}
			}

			// Token: 0x04001B62 RID: 7010
			private AudioSource _source;

			// Token: 0x04001B63 RID: 7011
			private AudioClip _clip;
		}
	}
}
