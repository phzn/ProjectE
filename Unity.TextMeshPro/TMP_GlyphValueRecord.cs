using System;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public struct TMP_GlyphValueRecord
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0001CE56 File Offset: 0x0001B056
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0001CE5E File Offset: 0x0001B05E
		public float xPlacement
		{
			get
			{
				return this.m_XPlacement;
			}
			set
			{
				this.m_XPlacement = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0001CE67 File Offset: 0x0001B067
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0001CE6F File Offset: 0x0001B06F
		public float yPlacement
		{
			get
			{
				return this.m_YPlacement;
			}
			set
			{
				this.m_YPlacement = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0001CE78 File Offset: 0x0001B078
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0001CE80 File Offset: 0x0001B080
		public float xAdvance
		{
			get
			{
				return this.m_XAdvance;
			}
			set
			{
				this.m_XAdvance = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0001CE89 File Offset: 0x0001B089
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0001CE91 File Offset: 0x0001B091
		public float yAdvance
		{
			get
			{
				return this.m_YAdvance;
			}
			set
			{
				this.m_YAdvance = value;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001CE9A File Offset: 0x0001B09A
		public TMP_GlyphValueRecord(float xPlacement, float yPlacement, float xAdvance, float yAdvance)
		{
			this.m_XPlacement = xPlacement;
			this.m_YPlacement = yPlacement;
			this.m_XAdvance = xAdvance;
			this.m_YAdvance = yAdvance;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001CEB9 File Offset: 0x0001B0B9
		internal TMP_GlyphValueRecord(GlyphValueRecord_Legacy valueRecord)
		{
			this.m_XPlacement = valueRecord.xPlacement;
			this.m_YPlacement = valueRecord.yPlacement;
			this.m_XAdvance = valueRecord.xAdvance;
			this.m_YAdvance = valueRecord.yAdvance;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001CEEB File Offset: 0x0001B0EB
		internal TMP_GlyphValueRecord(GlyphValueRecord valueRecord)
		{
			this.m_XPlacement = valueRecord.xPlacement;
			this.m_YPlacement = valueRecord.yPlacement;
			this.m_XAdvance = valueRecord.xAdvance;
			this.m_YAdvance = valueRecord.yAdvance;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0001CF24 File Offset: 0x0001B124
		public static TMP_GlyphValueRecord operator +(TMP_GlyphValueRecord a, TMP_GlyphValueRecord b)
		{
			TMP_GlyphValueRecord tmp_GlyphValueRecord;
			tmp_GlyphValueRecord.m_XPlacement = a.xPlacement + b.xPlacement;
			tmp_GlyphValueRecord.m_YPlacement = a.yPlacement + b.yPlacement;
			tmp_GlyphValueRecord.m_XAdvance = a.xAdvance + b.xAdvance;
			tmp_GlyphValueRecord.m_YAdvance = a.yAdvance + b.yAdvance;
			return tmp_GlyphValueRecord;
		}

		// Token: 0x040001F2 RID: 498
		[SerializeField]
		internal float m_XPlacement;

		// Token: 0x040001F3 RID: 499
		[SerializeField]
		internal float m_YPlacement;

		// Token: 0x040001F4 RID: 500
		[SerializeField]
		internal float m_XAdvance;

		// Token: 0x040001F5 RID: 501
		[SerializeField]
		internal float m_YAdvance;
	}
}
