using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003AA RID: 938
	public abstract class TypedUxmlAttributeDescription<T> : UxmlAttributeDescription
	{
		// Token: 0x06001F61 RID: 8033
		public abstract T GetValueFromBag(IUxmlAttributes bag, CreationContext cc);

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x00077F17 File Offset: 0x00076117
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x00077F1F File Offset: 0x0007611F
		public T defaultValue { get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x00077F28 File Offset: 0x00076128
		public override string defaultValueAsString
		{
			get
			{
				T defaultValue = this.defaultValue;
				return defaultValue.ToString();
			}
		}
	}
}
