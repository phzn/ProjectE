using System;
using Features.MainMenu;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009A2 RID: 2466
	internal static class MiniMapAudio
	{
		// Token: 0x060044D4 RID: 17620 RVA: 0x0019C1B8 File Offset: 0x0019A3B8
		public static void Play(MiniMapSound sound)
		{
			AudioClip audioClip = MiniMapAudio.Resolve(sound);
			if (audioClip == null)
			{
				return;
			}
			AudioSource audioSource = MiniMapAudio.EnsureSource();
			if (audioSource != null)
			{
				audioSource.PlayOneShot(audioClip, 0.55f);
			}
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x0019C1F4 File Offset: 0x0019A3F4
		private static AudioSource EnsureSource()
		{
			if (MiniMapAudio._source != null)
			{
				return MiniMapAudio._source;
			}
			GameObject gameObject = new GameObject("[MiniMap Audio]");
			gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
			Object.DontDestroyOnLoad(gameObject);
			MiniMapAudio._source = gameObject.AddComponent<AudioSource>();
			MiniMapAudio._source.playOnAwake = false;
			MiniMapAudio._source.spatialBlend = 0f;
			MiniMapAudio._source.ignoreListenerPause = true;
			MiniMapAudio._source.bypassReverbZones = true;
			GameAudioRouter.Route(MiniMapAudio._source, GameAudioRouter.Channel.Sfx);
			return MiniMapAudio._source;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x0019C278 File Offset: 0x0019A478
		private static AudioClip Resolve(MiniMapSound sound)
		{
			if (MiniMapAudio._resolved[(int)sound])
			{
				return MiniMapAudio._clips[(int)sound];
			}
			MiniMapAudio._resolved[(int)sound] = true;
			string text;
			switch (sound)
			{
			case MiniMapSound.MapOpen:
				text = "MapOpen";
				break;
			case MiniMapSound.MapClose:
				text = "MapClose";
				break;
			case MiniMapSound.PinPlace:
				text = "Pin";
				break;
			default:
				text = "Unpin";
				break;
			}
			AudioClip audioClip = Resources.Load<AudioClip>("Minimap/Sfx/" + text);
			MiniMapAudio._clips[(int)sound] = ((audioClip != null) ? audioClip : MiniMapAudio.Synthesize(sound));
			return MiniMapAudio._clips[(int)sound];
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x0019C304 File Offset: 0x0019A504
		private static AudioClip Synthesize(MiniMapSound sound)
		{
			switch (sound)
			{
			case MiniMapSound.MapOpen:
				return MiniMapAudio.BuildTwoTone("MiniMap Open", 520f, 780f, 0.09f, 0.14f, 0.16f);
			case MiniMapSound.MapClose:
				return MiniMapAudio.BuildTwoTone("MiniMap Close", 720f, 460f, 0.08f, 0.12f, 0.14f);
			case MiniMapSound.PinPlace:
				return MiniMapAudio.BuildPing("MiniMap Pin", 880f, 1320f, 0.32f, 0.55f);
			default:
				return MiniMapAudio.BuildPing("MiniMap Unpin", 560f, 740f, 0.18f, 0.4f);
			}
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x0019C3AC File Offset: 0x0019A5AC
		private static AudioClip BuildTwoTone(string name, float firstHz, float secondHz, float firstLength, float secondLength, float gain)
		{
			int num = Mathf.RoundToInt(firstLength * 44100f);
			int num2 = Mathf.RoundToInt(secondLength * 44100f);
			int num3 = num + num2;
			float[] array = new float[num3];
			for (int i = 0; i < num3; i++)
			{
				bool flag = i >= num;
				int num4 = (flag ? (i - num) : i);
				int num5 = (flag ? num2 : num);
				float num6 = (flag ? secondHz : firstHz);
				float num7 = (float)num4 / 44100f;
				float num8 = (float)num4 / (float)num5;
				float num9 = Mathf.Min(1f, num8 * 12f) * (1f - num8 * num8);
				float num10 = 1f + 0.004f * Mathf.Sin(num7 * 41f);
				float num11 = Mathf.Sin(6.2831855f * num6 * num10 * num7) + 0.25f * Mathf.Sin(6.2831855f * num6 * 2f * num7);
				array[i] = num11 * num9 * gain;
			}
			return MiniMapAudio.Create(name, array);
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x0019C4B0 File Offset: 0x0019A6B0
		private static AudioClip BuildPing(string name, float hz, float harmonicHz, float length, float gain)
		{
			int num = Mathf.RoundToInt(length * 44100f);
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 44100f;
				float num3 = (float)i / (float)num;
				float num4 = Mathf.Min(1f, num3 * 40f);
				float num5 = Mathf.Exp(-num3 * 6.5f);
				float num6 = Mathf.Sin(6.2831855f * hz * num2) + 0.45f * Mathf.Sin(6.2831855f * harmonicHz * num2) * Mathf.Exp(-num3 * 10f);
				array[i] = num6 * num4 * num5 * gain;
			}
			return MiniMapAudio.Create(name, array);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x0019C55A File Offset: 0x0019A75A
		private static AudioClip Create(string name, float[] data)
		{
			AudioClip audioClip = AudioClip.Create(name, data.Length, 1, 44100, false);
			audioClip.SetData(data, 0);
			audioClip.hideFlags = HideFlags.HideAndDontSave;
			return audioClip;
		}

		// Token: 0x04004477 RID: 17527
		private const string ResourceFolder = "Minimap/Sfx/";

		// Token: 0x04004478 RID: 17528
		private const int SampleRate = 44100;

		// Token: 0x04004479 RID: 17529
		private const float Volume = 0.55f;

		// Token: 0x0400447A RID: 17530
		private static AudioSource _source;

		// Token: 0x0400447B RID: 17531
		private static readonly AudioClip[] _clips = new AudioClip[4];

		// Token: 0x0400447C RID: 17532
		private static readonly bool[] _resolved = new bool[4];
	}
}
