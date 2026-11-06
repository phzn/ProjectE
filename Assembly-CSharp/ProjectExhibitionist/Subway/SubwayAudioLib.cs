using System;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200091E RID: 2334
	public static class SubwayAudioLib
	{
		// Token: 0x06003F3F RID: 16191 RVA: 0x00177521 File Offset: 0x00175721
		public static void WarmAll()
		{
			AudioClip rumbleLoop = SubwayAudioLib.RumbleLoop;
			AudioClip doorHiss = SubwayAudioLib.DoorHiss;
			AudioClip doorThunk = SubwayAudioLib.DoorThunk;
			AudioClip chime = SubwayAudioLib.Chime;
			AudioClip railClack = SubwayAudioLib.RailClack;
			AudioClip brake = SubwayAudioLib.Brake;
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00177547 File Offset: 0x00175747
		public static AudioClip RumbleLoop
		{
			get
			{
				if (SubwayAudioLib._rumble == null)
				{
					SubwayAudioLib._rumble = SubwayAudioLib.CreateRumbleLoop(6f);
				}
				return SubwayAudioLib._rumble;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x0017756A File Offset: 0x0017576A
		public static AudioClip DoorHiss
		{
			get
			{
				if (SubwayAudioLib._hiss == null)
				{
					SubwayAudioLib._hiss = SubwayAudioLib.CreateHiss(1.4f);
				}
				return SubwayAudioLib._hiss;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x0017758D File Offset: 0x0017578D
		public static AudioClip DoorThunk
		{
			get
			{
				if (SubwayAudioLib._thunk == null)
				{
					SubwayAudioLib._thunk = SubwayAudioLib.CreateThunk();
				}
				return SubwayAudioLib._thunk;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003F43 RID: 16195 RVA: 0x001775AB File Offset: 0x001757AB
		public static AudioClip Chime
		{
			get
			{
				if (SubwayAudioLib._chime == null)
				{
					SubwayAudioLib._chime = SubwayAudioLib.CreateChime();
				}
				return SubwayAudioLib._chime;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x001775C9 File Offset: 0x001757C9
		public static AudioClip RailClack
		{
			get
			{
				if (SubwayAudioLib._clack == null)
				{
					SubwayAudioLib._clack = SubwayAudioLib.CreateClack();
				}
				return SubwayAudioLib._clack;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003F45 RID: 16197 RVA: 0x001775E7 File Offset: 0x001757E7
		public static AudioClip Brake
		{
			get
			{
				if (SubwayAudioLib._brake == null)
				{
					SubwayAudioLib._brake = SubwayAudioLib.CreateBrake(3f);
				}
				return SubwayAudioLib._brake;
			}
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0017760A File Offset: 0x0017580A
		private static AudioClip Bake(string name, float[] samples)
		{
			AudioClip audioClip = AudioClip.Create(name, samples.Length, 1, 22050, false);
			audioClip.SetData(samples, 0);
			return audioClip;
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00177628 File Offset: 0x00175828
		private static AudioClip CreateRumbleLoop(float seconds)
		{
			int num = Mathf.RoundToInt(seconds * 22050f);
			float[] array = new float[num];
			Random random = new Random(1234);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num3 = (float)(random.NextDouble() * 2.0 - 1.0);
				num2 = (num2 + 0.02f * num3) / 1.02f;
				array[i] = num2 * 3.2f;
			}
			SubwayAudioLib.Lowpass(array, 0.06f);
			SubwayAudioLib.Lowpass(array, 0.09f);
			SubwayAudioLib.Lowpass(array, 0.13f);
			for (int j = 0; j < num; j++)
			{
				float num4 = (float)j / (float)num;
				float num5 = 0.94f + 0.06f * Mathf.Sin(num4 * 2f * 3.1415927f * 2f);
				array[j] *= num5;
			}
			SubwayAudioLib.MakeSeamless(array, Mathf.RoundToInt(8820f));
			SubwayAudioLib.Normalize(array, 0.85f);
			return SubwayAudioLib.Bake("SubwayRumbleLoop", array);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x00177738 File Offset: 0x00175938
		private static AudioClip CreateHiss(float seconds)
		{
			int num = Mathf.RoundToInt(seconds * 22050f);
			float[] array = new float[num];
			Random random = new Random(99);
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / (float)num;
				float num3 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num2 / 0.18f));
				float num4 = Mathf.Exp(-2.2f * Mathf.Max(0f, num2 - 0.18f));
				array[i] = (float)(random.NextDouble() * 2.0 - 1.0) * num3 * num4;
			}
			SubwayAudioLib.Lowpass(array, 0.3f);
			SubwayAudioLib.Lowpass(array, 0.5f);
			SubwayAudioLib.FadeEdges(array, 0.05f, 0.35f);
			SubwayAudioLib.Normalize(array, 0.42f);
			return SubwayAudioLib.Bake("SubwayDoorHiss", array);
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x00177814 File Offset: 0x00175A14
		private static AudioClip CreateThunk()
		{
			int num = Mathf.RoundToInt(4851f);
			float[] array = new float[num];
			Random random = new Random(7);
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)i / 22050f;
				float num3 = Mathf.Exp(-22f * num2);
				float num4 = Mathf.Sin(427.25662f * num2) * num3;
				float num5 = (float)(random.NextDouble() * 2.0 - 1.0) * Mathf.Exp(-90f * num2);
				array[i] = num4 * 0.9f + num5 * 0.4f;
			}
			SubwayAudioLib.Lowpass(array, 0.4f);
			SubwayAudioLib.FadeEdges(array, 0.003f, 0.06f);
			SubwayAudioLib.Normalize(array, 0.7f);
			return SubwayAudioLib.Bake("SubwayDoorThunk", array);
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x001778E4 File Offset: 0x00175AE4
		private static AudioClip CreateChime()
		{
			float[] array = new float[] { 830f, 623f };
			float[] array2 = new float[] { 0f, 0.42f };
			int num = Mathf.RoundToInt(2.2f * 22050f);
			float[] array3 = new float[num];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Mathf.RoundToInt(array2[i] * 22050f);
				for (int j = num2; j < num; j++)
				{
					float num3 = (float)(j - num2) / 22050f;
					float num4 = Mathf.Exp(-2.1f * num3) * Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num3 / 0.025f));
					float num5 = Mathf.Sin(6.2831855f * array[i] * num3) + 0.28f * Mathf.Sin(6.2831855f * array[i] * 2f * num3);
					array3[j] += num5 * num4 * 0.5f;
				}
			}
			SubwayAudioLib.FadeEdges(array3, 0.01f, 0.25f);
			SubwayAudioLib.Normalize(array3, 0.6f);
			return SubwayAudioLib.Bake("SubwayChime", array3);
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00177A1C File Offset: 0x00175C1C
		private static AudioClip CreateClack()
		{
			int num = Mathf.RoundToInt(7717.5f);
			float[] array = new float[num];
			Random random = new Random(42);
			float[] array2 = new float[] { 0f, 0.13f };
			for (int i = 0; i < array2.Length; i++)
			{
				int num2 = Mathf.RoundToInt(array2[i] * 22050f);
				for (int j = num2; j < num; j++)
				{
					float num3 = (float)(j - num2) / 22050f;
					float num4 = Mathf.Clamp01(num3 / 0.012f) * Mathf.Exp(-38f * num3);
					if (num3 > 0.012f && num4 < 0.001f)
					{
						break;
					}
					float num5 = (float)(random.NextDouble() * 2.0 - 1.0);
					float num6 = Mathf.Sin(439.823f * num3);
					array[j] += (num5 * 0.3f + num6 * 0.9f) * num4;
				}
			}
			SubwayAudioLib.Lowpass(array, 0.18f);
			SubwayAudioLib.Lowpass(array, 0.35f);
			SubwayAudioLib.FadeEdges(array, 0.004f, 0.08f);
			SubwayAudioLib.Normalize(array, 0.5f);
			return SubwayAudioLib.Bake("SubwayRailClack", array);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00177B58 File Offset: 0x00175D58
		private static AudioClip CreateBrake(float seconds)
		{
			int num = Mathf.RoundToInt(seconds * 22050f);
			float[] array = new float[num];
			Random random = new Random(555);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num3 = (float)i / (float)num;
				float num4 = Mathf.Lerp(0.5f, 0.06f, num3);
				float num5 = (float)(random.NextDouble() * 2.0 - 1.0);
				num2 += num4 * (num5 - num2);
				float num6 = Mathf.Sin(num3 * 3.1415927f);
				float num7 = Mathf.Lerp(2100f, 1400f, num3);
				float num8 = Mathf.Sin(6.2831855f * num7 * ((float)i / 22050f)) * 0.06f * Mathf.Clamp01(1.2f - num3);
				array[i] = (num2 * 1.6f + num8) * num6;
			}
			SubwayAudioLib.FadeEdges(array, 0.25f, 0.6f);
			SubwayAudioLib.Normalize(array, 0.55f);
			return SubwayAudioLib.Bake("SubwayBrake", array);
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00177C68 File Offset: 0x00175E68
		private static void FadeEdges(float[] s, float inSeconds, float outSeconds)
		{
			int num = Mathf.Min(s.Length / 2, Mathf.RoundToInt(inSeconds * 22050f));
			int num2 = Mathf.Min(s.Length / 2, Mathf.RoundToInt(outSeconds * 22050f));
			for (int i = 0; i < num; i++)
			{
				s[i] *= Mathf.SmoothStep(0f, 1f, (float)i / (float)num);
			}
			for (int j = 0; j < num2; j++)
			{
				float num3 = Mathf.SmoothStep(0f, 1f, (float)j / (float)num2);
				s[s.Length - 1 - j] *= num3;
			}
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x00177D04 File Offset: 0x00175F04
		private static void Lowpass(float[] s, float a)
		{
			float num = 0f;
			for (int i = 0; i < s.Length; i++)
			{
				num += a * (s[i] - num);
				s[i] = num;
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00177D34 File Offset: 0x00175F34
		private static void MakeSeamless(float[] s, int fade)
		{
			int num = s.Length;
			for (int i = 0; i < fade; i++)
			{
				float num2 = (float)i / (float)fade;
				s[i] = s[i] * num2 + s[num - fade + i] * (1f - num2);
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x00177D70 File Offset: 0x00175F70
		private static void Normalize(float[] s, float peak)
		{
			float num = 0f;
			for (int i = 0; i < s.Length; i++)
			{
				num = Mathf.Max(num, Mathf.Abs(s[i]));
			}
			if (num < 1E-05f)
			{
				return;
			}
			float num2 = peak / num;
			for (int j = 0; j < s.Length; j++)
			{
				s[j] *= num2;
			}
		}

		// Token: 0x04003F22 RID: 16162
		private const int SampleRate = 22050;

		// Token: 0x04003F23 RID: 16163
		private static AudioClip _rumble;

		// Token: 0x04003F24 RID: 16164
		private static AudioClip _hiss;

		// Token: 0x04003F25 RID: 16165
		private static AudioClip _thunk;

		// Token: 0x04003F26 RID: 16166
		private static AudioClip _chime;

		// Token: 0x04003F27 RID: 16167
		private static AudioClip _clack;

		// Token: 0x04003F28 RID: 16168
		private static AudioClip _brake;
	}
}
