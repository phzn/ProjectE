using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000348 RID: 840
	public struct StylePropertyName : IEquatable<StylePropertyName>
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x0006E51F File Offset: 0x0006C71F
		internal readonly StylePropertyId id { get; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x0006E527 File Offset: 0x0006C727
		private readonly string name { get; }

		// Token: 0x06001C55 RID: 7253 RVA: 0x0006E530 File Offset: 0x0006C730
		internal static StylePropertyId StylePropertyIdFromString(string name)
		{
			StylePropertyId stylePropertyId;
			bool flag = StylePropertyUtil.s_NameToId.TryGetValue(name, out stylePropertyId);
			StylePropertyId stylePropertyId2;
			if (flag)
			{
				stylePropertyId2 = stylePropertyId;
			}
			else
			{
				stylePropertyId2 = StylePropertyId.Unknown;
			}
			return stylePropertyId2;
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x0006E55C File Offset: 0x0006C75C
		internal StylePropertyName(StylePropertyId stylePropertyId)
		{
			this.id = stylePropertyId;
			this.name = null;
			string text;
			bool flag = StylePropertyUtil.s_IdToName.TryGetValue(stylePropertyId, out text);
			if (flag)
			{
				this.name = text;
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0006E594 File Offset: 0x0006C794
		public StylePropertyName(string name)
		{
			this.id = StylePropertyName.StylePropertyIdFromString(name);
			this.name = null;
			bool flag = this.id > StylePropertyId.Unknown;
			if (flag)
			{
				this.name = name;
			}
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x0006E5CC File Offset: 0x0006C7CC
		public static bool IsNullOrEmpty(StylePropertyName propertyName)
		{
			return propertyName.id == StylePropertyId.Unknown;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x0006E5E8 File Offset: 0x0006C7E8
		public static bool operator ==(StylePropertyName lhs, StylePropertyName rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x0006E60C File Offset: 0x0006C80C
		public static bool operator !=(StylePropertyName lhs, StylePropertyName rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x0006E634 File Offset: 0x0006C834
		public static implicit operator StylePropertyName(string name)
		{
			return new StylePropertyName(name);
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x0006E64C File Offset: 0x0006C84C
		public override int GetHashCode()
		{
			return (int)this.id;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x0006E664 File Offset: 0x0006C864
		public override bool Equals(object other)
		{
			return other is StylePropertyName && this.Equals((StylePropertyName)other);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0006E690 File Offset: 0x0006C890
		public bool Equals(StylePropertyName other)
		{
			return this == other;
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0006E6B0 File Offset: 0x0006C8B0
		public override string ToString()
		{
			return this.name;
		}
	}
}
