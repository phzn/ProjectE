using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public struct AnimatedGradient
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003040 File Offset: 0x00001240
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003048 File Offset: 0x00001248
		public Gradient Gradient
		{
			get
			{
				return this._gradient;
			}
			set
			{
				this._gradient = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003051 File Offset: 0x00001251
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003059 File Offset: 0x00001259
		public float Angle
		{
			get
			{
				return this._angle;
			}
			set
			{
				this._angle = value;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003062 File Offset: 0x00001262
		public AnimatedGradient(Gradient gradient, float angle)
		{
			this._gradient = gradient;
			this._angle = angle;
		}

		// Token: 0x04000027 RID: 39
		[SerializeField]
		private Gradient _gradient;

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private float _angle;
	}
}
