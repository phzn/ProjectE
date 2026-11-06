using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003C2 RID: 962
	internal class UxmlObjectAttributeDescription<T> where T : new()
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00078E67 File Offset: 0x00077067
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x00078E6F File Offset: 0x0007706F
		public T defaultValue { get; set; }

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00078E78 File Offset: 0x00077078
		public virtual T GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			VisualTreeAsset visualTreeAsset = cc.visualTreeAsset;
			List<T> list = ((visualTreeAsset != null) ? visualTreeAsset.GetUxmlObjects<T>(bag, cc) : null);
			bool flag = list != null;
			if (flag)
			{
				using (List<T>.Enumerator enumerator = list.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			return this.defaultValue;
		}
	}
}
