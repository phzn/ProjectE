using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003C3 RID: 963
	internal class UxmlObjectListAttributeDescription<T> : UxmlObjectAttributeDescription<List<T>> where T : new()
	{
		// Token: 0x06001FD4 RID: 8148 RVA: 0x00078EF4 File Offset: 0x000770F4
		public override List<T> GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			VisualTreeAsset visualTreeAsset = cc.visualTreeAsset;
			List<T> list = ((visualTreeAsset != null) ? visualTreeAsset.GetUxmlObjects<T>(bag, cc) : null);
			bool flag = list != null;
			List<T> list3;
			if (flag)
			{
				List<T> list2 = null;
				foreach (T t in list)
				{
					bool flag2 = list2 == null;
					if (flag2)
					{
						list2 = new List<T>();
					}
					list2.Add(t);
				}
				list3 = list2;
			}
			else
			{
				list3 = base.defaultValue;
			}
			return list3;
		}
	}
}
