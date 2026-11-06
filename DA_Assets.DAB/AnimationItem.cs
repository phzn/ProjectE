using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public struct AnimationItem
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000030FA File Offset: 0x000012FA
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003102 File Offset: 0x00001302
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000310B File Offset: 0x0000130B
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003113 File Offset: 0x00001313
		public bool PingPong
		{
			get
			{
				return this.pingPong;
			}
			set
			{
				this.pingPong = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000311C File Offset: 0x0000131C
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003124 File Offset: 0x00001324
		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000312D File Offset: 0x0000132D
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003135 File Offset: 0x00001335
		public AnimationCurve Curve
		{
			get
			{
				return this.curve;
			}
			set
			{
				this.curve = value;
			}
		}

		// Token: 0x0400003D RID: 61
		[SerializeField]
		private bool enabled;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		private bool pingPong;

		// Token: 0x0400003F RID: 63
		[SerializeField]
		private float duration;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		private AnimationCurve curve;
	}
}
