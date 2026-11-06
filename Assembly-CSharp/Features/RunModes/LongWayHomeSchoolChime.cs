using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CFA RID: 3322
	public static class LongWayHomeSchoolChime
	{
		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x002656DE File Offset: 0x002638DE
		public static AudioClip Clip
		{
			get
			{
				if (LongWayHomeSchoolChime._clip == null)
				{
					LongWayHomeSchoolChime._clip = LongWayHomeSchoolChime.Create();
				}
				return LongWayHomeSchoolChime._clip;
			}
		}

		// Token: 0x060060B0 RID: 24752 RVA: 0x002656FC File Offset: 0x002638FC
		public static float NoteStart(int index)
		{
			return (float)index * 0.62f + ((index >= 4) ? 0.55f : 0f);
		}

		// Token: 0x060060B1 RID: 24753 RVA: 0x00265718 File Offset: 0x00263918
		private static AudioClip Create()
		{
			int num = Mathf.RoundToInt((LongWayHomeSchoolChime.NoteStart(LongWayHomeSchoolChime.Notes.Length - 1) + 3.2f) * 44100f);
			float[] array = new float[num];
			for (int i = 0; i < LongWayHomeSchoolChime.Notes.Length; i++)
			{
				int num2 = Mathf.RoundToInt(LongWayHomeSchoolChime.NoteStart(i) * 44100f);
				bool flag = i == 3 || i == LongWayHomeSchoolChime.Notes.Length - 1;
				float num3 = (flag ? 3.2f : 2.4f);
				int num4 = Mathf.Min(num, num2 + Mathf.RoundToInt(num3 * 44100f));
				for (int j = num2; j < num4; j++)
				{
					float num5 = (float)(j - num2) / 44100f;
					float num6 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num5 / 0.012f));
					float num7 = 0f;
					for (int k = 0; k < LongWayHomeSchoolChime.Partials.Length; k++)
					{
						float num8 = LongWayHomeSchoolChime.Notes[i] * LongWayHomeSchoolChime.Partials[k] * ((k == 0) ? 1.0015f : 1f);
						num7 += LongWayHomeSchoolChime.Gains[k] * Mathf.Exp(-LongWayHomeSchoolChime.Decays[k] * num5) * Mathf.Sin(6.2831855f * num8 * num5);
					}
					array[j] += num7 * num6 * (flag ? 0.85f : 1f);
				}
			}
			LongWayHomeSchoolChime.Normalize(array, 0.62f);
			AudioClip audioClip = AudioClip.Create("LongWayHome School Chime", num, 1, 44100, false);
			audioClip.SetData(array, 0);
			audioClip.hideFlags = HideFlags.DontSave;
			return audioClip;
		}

		// Token: 0x060060B2 RID: 24754 RVA: 0x002658B8 File Offset: 0x00263AB8
		private static void Normalize(float[] samples, float peak)
		{
			float num = 0f;
			foreach (float num2 in samples)
			{
				num = Mathf.Max(num, Mathf.Abs(num2));
			}
			if (num <= 0f)
			{
				return;
			}
			float num3 = peak / num;
			for (int j = 0; j < samples.Length; j++)
			{
				samples[j] *= num3;
			}
		}

		// Token: 0x04006247 RID: 25159
		private const int SampleRate = 44100;

		// Token: 0x04006248 RID: 25160
		private const float NoteSpacing = 0.62f;

		// Token: 0x04006249 RID: 25161
		private const float PhraseGap = 0.55f;

		// Token: 0x0400624A RID: 25162
		private static AudioClip _clip;

		// Token: 0x0400624B RID: 25163
		private static readonly float[] Notes = new float[] { 329.63f, 261.63f, 293.66f, 196f, 196f, 293.66f, 329.63f, 261.63f };

		// Token: 0x0400624C RID: 25164
		private static readonly float[] Partials = new float[] { 1f, 2f, 3.01f, 4.17f, 5.43f };

		// Token: 0x0400624D RID: 25165
		private static readonly float[] Gains = new float[] { 1f, 0.42f, 0.24f, 0.12f, 0.06f };

		// Token: 0x0400624E RID: 25166
		private static readonly float[] Decays = new float[] { 1.15f, 1.9f, 2.7f, 3.6f, 4.8f };
	}
}
