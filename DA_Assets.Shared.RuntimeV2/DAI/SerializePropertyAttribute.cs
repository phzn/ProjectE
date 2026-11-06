using System;

namespace DA_Assets.DAI
{
	// Token: 0x0200000B RID: 11
	public class SerializePropertyAttribute : Attribute
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000026BA File Offset: 0x000008BA
		public SerializePropertyAttribute(string fieldName)
		{
			this.fieldName = fieldName;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000026C9 File Offset: 0x000008C9
		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}

		// Token: 0x0400000B RID: 11
		private string fieldName;
	}
}
