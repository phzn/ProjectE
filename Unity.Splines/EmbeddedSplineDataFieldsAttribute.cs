using System;

namespace UnityEngine.Splines
{
	// Token: 0x0200001A RID: 26
	public class EmbeddedSplineDataFieldsAttribute : PropertyAttribute
	{
		// Token: 0x0600009E RID: 158 RVA: 0x0000440F File Offset: 0x0000260F
		public EmbeddedSplineDataFieldsAttribute(EmbeddedSplineDataField fields)
		{
			this.Fields = fields;
		}

		// Token: 0x0400003F RID: 63
		public readonly EmbeddedSplineDataField Fields;
	}
}
