using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x020000B3 RID: 179
	[Serializable]
	public class Beat
	{
		// Token: 0x06000383 RID: 899 RVA: 0x0001194C File Offset: 0x0000FB4C
		public void InitializeIfNeeded(int id, int bandID)
		{
			if (!this._initialized)
			{
				this.Mode = Beat.Modes.Normalized;
				this.BeatValueMode = Beat.BeatValueModes.Remapped;
				this.Name = "Beat " + id.ToString();
				this.BeatColor = MMColors.RandomColor();
				this.BandID = bandID;
				this.Threshold = 0.3f + (float)id * 0.02f;
				if (this.Threshold > 0.6f)
				{
					this.Threshold -= 0.5f;
				}
				this.Threshold %= 1f;
				this.MinimumTimeBetweenBeats = 0.25f + (float)id * 0.02f;
				this.RemappedAttack = 0.05f + (float)id * 0.01f;
				this.RemappedDecay = 0.2f + (float)id * 0.01f;
				this._initialized = true;
			}
		}

		// Token: 0x040004E4 RID: 1252
		public string Name = "Beat";

		// Token: 0x040004E5 RID: 1253
		public Beat.Modes Mode = Beat.Modes.BufferedNormalized;

		// Token: 0x040004E6 RID: 1254
		public Beat.BeatValueModes BeatValueMode;

		// Token: 0x040004E7 RID: 1255
		[MMEnumCondition("Mode", new int[] { 0, 1, 2, 3 })]
		public Color BeatColor = Color.cyan;

		// Token: 0x040004E8 RID: 1256
		public int BandID;

		// Token: 0x040004E9 RID: 1257
		public float Threshold = 0.5f;

		// Token: 0x040004EA RID: 1258
		public float MinimumTimeBetweenBeats = 0.25f;

		// Token: 0x040004EB RID: 1259
		[MMEnumCondition("BeatValueMode", new int[] { 0 })]
		public float RemappedAttack = 0.05f;

		// Token: 0x040004EC RID: 1260
		[MMEnumCondition("BeatValueMode", new int[] { 0 })]
		public float RemappedDecay = 0.2f;

		// Token: 0x040004ED RID: 1261
		[MMReadOnly]
		public bool BeatThisFrame;

		// Token: 0x040004EE RID: 1262
		[MMReadOnly]
		public float CurrentValue;

		// Token: 0x040004EF RID: 1263
		[HideInInspector]
		public float _previousValue;

		// Token: 0x040004F0 RID: 1264
		[HideInInspector]
		public float _lastBeatAt;

		// Token: 0x040004F1 RID: 1265
		[HideInInspector]
		public float _lastBeatValue;

		// Token: 0x040004F2 RID: 1266
		[HideInInspector]
		public bool _initialized;

		// Token: 0x040004F3 RID: 1267
		public UnityEvent OnBeat;

		// Token: 0x020003F9 RID: 1017
		public enum Modes
		{
			// Token: 0x04001EAC RID: 7852
			Raw,
			// Token: 0x04001EAD RID: 7853
			Normalized,
			// Token: 0x04001EAE RID: 7854
			BufferedRaw,
			// Token: 0x04001EAF RID: 7855
			BufferedNormalized,
			// Token: 0x04001EB0 RID: 7856
			Amplitude,
			// Token: 0x04001EB1 RID: 7857
			NormalizedAmplitude,
			// Token: 0x04001EB2 RID: 7858
			AmplitudeBuffered,
			// Token: 0x04001EB3 RID: 7859
			NormalizedAmplitudeBuffered
		}

		// Token: 0x020003FA RID: 1018
		public enum BeatValueModes
		{
			// Token: 0x04001EB5 RID: 7861
			Remapped,
			// Token: 0x04001EB6 RID: 7862
			Live
		}
	}
}
