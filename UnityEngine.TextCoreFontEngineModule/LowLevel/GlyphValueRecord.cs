using System;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel
{
	// Token: 0x0200001E RID: 30
	[UsedByNativeCode]
	[Serializable]
	public struct GlyphValueRecord : IEquatable<GlyphValueRecord>
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000049F0 File Offset: 0x00002BF0
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00004A08 File Offset: 0x00002C08
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00004A14 File Offset: 0x00002C14
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00004A2C File Offset: 0x00002C2C
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00004A38 File Offset: 0x00002C38
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00004A50 File Offset: 0x00002C50
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004A5C File Offset: 0x00002C5C
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00004A74 File Offset: 0x00002C74
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

		// Token: 0x06000126 RID: 294 RVA: 0x00004A7E File Offset: 0x00002C7E
		public GlyphValueRecord(float xPlacement, float yPlacement, float xAdvance, float yAdvance)
		{
			this.m_XPlacement = xPlacement;
			this.m_YPlacement = yPlacement;
			this.m_XAdvance = xAdvance;
			this.m_YAdvance = yAdvance;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public static GlyphValueRecord operator +(GlyphValueRecord a, GlyphValueRecord b)
		{
			GlyphValueRecord glyphValueRecord;
			glyphValueRecord.m_XPlacement = a.xPlacement + b.xPlacement;
			glyphValueRecord.m_YPlacement = a.yPlacement + b.yPlacement;
			glyphValueRecord.m_XAdvance = a.xAdvance + b.xAdvance;
			glyphValueRecord.m_YAdvance = a.yAdvance + b.yAdvance;
			return glyphValueRecord;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004B0C File Offset: 0x00002D0C
		[ExcludeFromDocs]
		public static GlyphValueRecord operator *(GlyphValueRecord a, float emScale)
		{
			a.m_XPlacement = a.xPlacement * emScale;
			a.m_YPlacement = a.yPlacement * emScale;
			a.m_XAdvance = a.xAdvance * emScale;
			a.m_YAdvance = a.yAdvance * emScale;
			return a;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004B60 File Offset: 0x00002D60
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004B84 File Offset: 0x00002D84
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public bool Equals(GlyphValueRecord other)
		{
			return base.Equals(other);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004BD0 File Offset: 0x00002DD0
		public static bool operator ==(GlyphValueRecord lhs, GlyphValueRecord rhs)
		{
			return lhs.m_XPlacement == rhs.m_XPlacement && lhs.m_YPlacement == rhs.m_YPlacement && lhs.m_XAdvance == rhs.m_XAdvance && lhs.m_YAdvance == rhs.m_YAdvance;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004C20 File Offset: 0x00002E20
		public static bool operator !=(GlyphValueRecord lhs, GlyphValueRecord rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040000BB RID: 187
		[SerializeField]
		[NativeName("xPlacement")]
		private float m_XPlacement;

		// Token: 0x040000BC RID: 188
		[SerializeField]
		[NativeName("yPlacement")]
		private float m_YPlacement;

		// Token: 0x040000BD RID: 189
		[SerializeField]
		[NativeName("xAdvance")]
		private float m_XAdvance;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		[NativeName("yAdvance")]
		private float m_YAdvance;
	}
}
