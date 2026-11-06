using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E7F RID: 3711
	internal sealed class TripodRecordingAudioScope
	{
		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06006F45 RID: 28485 RVA: 0x002CE8A1 File Offset: 0x002CCAA1
		// (set) Token: 0x06006F46 RID: 28486 RVA: 0x002CE8A9 File Offset: 0x002CCAA9
		public bool IsActive { get; private set; }

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06006F47 RID: 28487 RVA: 0x002CE8B2 File Offset: 0x002CCAB2
		public int AdjustedSourceCount
		{
			get
			{
				return this._snapshots.Count;
			}
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x002CE8C0 File Offset: 0x002CCAC0
		public void Begin(Transform subjectRoot, float minDistance, float maxDistance, Func<AudioSource, bool> exclude = null)
		{
			this.End();
			if (subjectRoot == null)
			{
				return;
			}
			this._subjectRoot = subjectRoot;
			this._exclude = exclude;
			this._minDistance = Mathf.Max(0.05f, minDistance);
			this._maxDistance = Mathf.Max(this._minDistance + 0.1f, maxDistance);
			this.IsActive = true;
			this.Refresh();
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x002CE924 File Offset: 0x002CCB24
		public void Refresh()
		{
			if (!this.IsActive || this._subjectRoot == null)
			{
				return;
			}
			foreach (AudioSource audioSource in this._subjectRoot.GetComponentsInChildren<AudioSource>(true))
			{
				if (!(audioSource == null) && (this._exclude == null || !this._exclude(audioSource)))
				{
					if (!this._snapshots.ContainsKey(audioSource))
					{
						if (audioSource.spatialBlend >= 0.999f)
						{
							goto IL_007F;
						}
						this._snapshots.Add(audioSource, new TripodRecordingAudioScope.SourceSnapshot(audioSource));
					}
					this.ApplyTripodProfile(audioSource);
				}
				IL_007F:;
			}
		}

		// Token: 0x06006F4A RID: 28490 RVA: 0x002CE9BC File Offset: 0x002CCBBC
		public void End()
		{
			foreach (KeyValuePair<AudioSource, TripodRecordingAudioScope.SourceSnapshot> keyValuePair in this._snapshots)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Value.Restore(keyValuePair.Key);
				}
			}
			this._snapshots.Clear();
			this._subjectRoot = null;
			this._exclude = null;
			this.IsActive = false;
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x002CEA50 File Offset: 0x002CCC50
		private void ApplyTripodProfile(AudioSource source)
		{
			source.spatialBlend = 1f;
			source.rolloffMode = AudioRolloffMode.Logarithmic;
			source.minDistance = this._minDistance;
			source.maxDistance = this._maxDistance;
			source.dopplerLevel = 0f;
			source.spread = 0f;
		}

		// Token: 0x0400722D RID: 29229
		private readonly Dictionary<AudioSource, TripodRecordingAudioScope.SourceSnapshot> _snapshots = new Dictionary<AudioSource, TripodRecordingAudioScope.SourceSnapshot>();

		// Token: 0x0400722E RID: 29230
		private Transform _subjectRoot;

		// Token: 0x0400722F RID: 29231
		private Func<AudioSource, bool> _exclude;

		// Token: 0x04007230 RID: 29232
		private float _minDistance;

		// Token: 0x04007231 RID: 29233
		private float _maxDistance;

		// Token: 0x02000E80 RID: 3712
		private struct SourceSnapshot
		{
			// Token: 0x06006F4D RID: 28493 RVA: 0x002CEAB0 File Offset: 0x002CCCB0
			public SourceSnapshot(AudioSource source)
			{
				this.spatialBlend = source.spatialBlend;
				this.rolloffMode = source.rolloffMode;
				this.minDistance = source.minDistance;
				this.maxDistance = source.maxDistance;
				this.dopplerLevel = source.dopplerLevel;
				this.spread = source.spread;
			}

			// Token: 0x06006F4E RID: 28494 RVA: 0x002CEB08 File Offset: 0x002CCD08
			public void Restore(AudioSource source)
			{
				source.spatialBlend = this.spatialBlend;
				source.rolloffMode = this.rolloffMode;
				source.minDistance = this.minDistance;
				source.maxDistance = this.maxDistance;
				source.dopplerLevel = this.dopplerLevel;
				source.spread = this.spread;
			}

			// Token: 0x04007233 RID: 29235
			public float spatialBlend;

			// Token: 0x04007234 RID: 29236
			public AudioRolloffMode rolloffMode;

			// Token: 0x04007235 RID: 29237
			public float minDistance;

			// Token: 0x04007236 RID: 29238
			public float maxDistance;

			// Token: 0x04007237 RID: 29239
			public float dopplerLevel;

			// Token: 0x04007238 RID: 29240
			public float spread;
		}
	}
}
