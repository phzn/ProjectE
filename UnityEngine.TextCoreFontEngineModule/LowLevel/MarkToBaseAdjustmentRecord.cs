using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x02000023 RID: 35
	[UsedByNativeCode]
	[Serializable]
	internal struct MarkToBaseAdjustmentRecord
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004F40 File Offset: 0x00003140
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00004F58 File Offset: 0x00003158
		public uint baseGlyphID
		{
			get
			{
				return this.m_BaseGlyphID;
			}
			set
			{
				this.m_BaseGlyphID = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004F64 File Offset: 0x00003164
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00004F7C File Offset: 0x0000317C
		public GlyphAnchorPoint baseGlyphAnchorPoint
		{
			get
			{
				return this.m_BaseGlyphAnchorPoint;
			}
			set
			{
				this.m_BaseGlyphAnchorPoint = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00004F88 File Offset: 0x00003188
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00004FA0 File Offset: 0x000031A0
		public uint markGlyphID
		{
			get
			{
				return this.m_MarkGlyphID;
			}
			set
			{
				this.m_MarkGlyphID = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004FAC File Offset: 0x000031AC
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00004FC4 File Offset: 0x000031C4
		public MarkPositionAdjustment markPositionAdjustment
		{
			get
			{
				return this.m_MarkPositionAdjustment;
			}
			set
			{
				this.m_MarkPositionAdjustment = value;
			}
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		[NativeName("baseGlyphID")]
		private uint m_BaseGlyphID;

		// Token: 0x040000C9 RID: 201
		[NativeName("baseAnchor")]
		[SerializeField]
		private GlyphAnchorPoint m_BaseGlyphAnchorPoint;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		[NativeName("markGlyphID")]
		private uint m_MarkGlyphID;

		// Token: 0x040000CB RID: 203
		[NativeName("markPositionAdjustment")]
		[SerializeField]
		private MarkPositionAdjustment m_MarkPositionAdjustment;
	}
}
