using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x02000024 RID: 36
	[UsedByNativeCode]
	[Serializable]
	internal struct MarkToMarkAdjustmentRecord
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00004FD0 File Offset: 0x000031D0
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00004FE8 File Offset: 0x000031E8
		public uint baseMarkGlyphID
		{
			get
			{
				return this.m_BaseMarkGlyphID;
			}
			set
			{
				this.m_BaseMarkGlyphID = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00004FF4 File Offset: 0x000031F4
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000500C File Offset: 0x0000320C
		public GlyphAnchorPoint baseMarkGlyphAnchorPoint
		{
			get
			{
				return this.m_BaseMarkGlyphAnchorPoint;
			}
			set
			{
				this.m_BaseMarkGlyphAnchorPoint = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00005018 File Offset: 0x00003218
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00005030 File Offset: 0x00003230
		public uint combiningMarkGlyphID
		{
			get
			{
				return this.m_CombiningMarkGlyphID;
			}
			set
			{
				this.m_CombiningMarkGlyphID = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000503C File Offset: 0x0000323C
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00005054 File Offset: 0x00003254
		public MarkPositionAdjustment combiningMarkPositionAdjustment
		{
			get
			{
				return this.m_CombiningMarkPositionAdjustment;
			}
			set
			{
				this.m_CombiningMarkPositionAdjustment = value;
			}
		}

		// Token: 0x040000CC RID: 204
		[SerializeField]
		[NativeName("baseMarkGlyphID")]
		private uint m_BaseMarkGlyphID;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		[NativeName("baseMarkAnchor")]
		private GlyphAnchorPoint m_BaseMarkGlyphAnchorPoint;

		// Token: 0x040000CE RID: 206
		[NativeName("combiningMarkGlyphID")]
		[SerializeField]
		private uint m_CombiningMarkGlyphID;

		// Token: 0x040000CF RID: 207
		[NativeName("combiningMarkPositionAdjustment")]
		[SerializeField]
		private MarkPositionAdjustment m_CombiningMarkPositionAdjustment;
	}
}
