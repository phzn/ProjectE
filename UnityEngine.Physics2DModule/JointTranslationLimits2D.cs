using System;

namespace UnityEngine
{
	// Token: 0x0200001C RID: 28
	public struct JointTranslationLimits2D
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00006CF4 File Offset: 0x00004EF4
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00006D0C File Offset: 0x00004F0C
		public float min
		{
			get
			{
				return this.m_LowerTranslation;
			}
			set
			{
				this.m_LowerTranslation = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00006D18 File Offset: 0x00004F18
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00006D30 File Offset: 0x00004F30
		public float max
		{
			get
			{
				return this.m_UpperTranslation;
			}
			set
			{
				this.m_UpperTranslation = value;
			}
		}

		// Token: 0x04000081 RID: 129
		private float m_LowerTranslation;

		// Token: 0x04000082 RID: 130
		private float m_UpperTranslation;
	}
}
