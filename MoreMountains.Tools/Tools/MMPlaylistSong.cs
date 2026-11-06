using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public class MMPlaylistSong
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x00012810 File Offset: 0x00010A10
		public virtual void Initialization()
		{
			if (this._initialized)
			{
				return;
			}
			this.Volume = new Vector2(0f, 1f);
			this.InitialDelay = Vector2.zero;
			this.CrossFadeDuration = new Vector2(2f, 2f);
			this.Pitch = Vector2.one;
			this.StereoPan = 0f;
			this.SpatialBlend = 0f;
			this.Loop = false;
			this._initialized = true;
		}

		// Token: 0x0400051E RID: 1310
		public AudioSource TargetAudioSource;

		// Token: 0x0400051F RID: 1311
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Volume = new Vector2(0f, 1f);

		// Token: 0x04000520 RID: 1312
		[MMVector(new string[] { "RMin", "RMax" })]
		public Vector2 InitialDelay = Vector2.zero;

		// Token: 0x04000521 RID: 1313
		[MMVector(new string[] { "RMin", "RMax" })]
		public Vector2 CrossFadeDuration = new Vector2(2f, 2f);

		// Token: 0x04000522 RID: 1314
		[MMVector(new string[] { "RMin", "RMax" })]
		public Vector2 Pitch = Vector2.one;

		// Token: 0x04000523 RID: 1315
		[Range(-1f, 1f)]
		public float StereoPan;

		// Token: 0x04000524 RID: 1316
		[Range(0f, 1f)]
		public float SpatialBlend;

		// Token: 0x04000525 RID: 1317
		public bool Loop;

		// Token: 0x04000526 RID: 1318
		[MMReadOnly]
		public bool Playing;

		// Token: 0x04000527 RID: 1319
		[MMReadOnly]
		public bool Fading;

		// Token: 0x04000528 RID: 1320
		[MMHidden]
		public bool _initialized;
	}
}
