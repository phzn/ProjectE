using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D7 RID: 983
	public abstract class UxmlTypeRestriction : IEquatable<UxmlTypeRestriction>
	{
		// Token: 0x06002039 RID: 8249 RVA: 0x00079D38 File Offset: 0x00077F38
		public virtual bool Equals(UxmlTypeRestriction other)
		{
			return this == other;
		}
	}
}
