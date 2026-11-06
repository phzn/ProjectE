using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FE RID: 254
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class vCheckPropertyAttribute : PropertyAttribute
{
	// Token: 0x06000975 RID: 2421 RVA: 0x00043D94 File Offset: 0x00041F94
	public vCheckPropertyAttribute(string propertyNames, params object[] values)
	{
		this.checkValues.Clear();
		string[] array = propertyNames.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			try
			{
				this.checkValues.Add(new vCheckPropertyAttribute.CheckValue(array[i], values[i]));
			}
			catch
			{
				break;
			}
		}
	}

	// Token: 0x040008AB RID: 2219
	public List<vCheckPropertyAttribute.CheckValue> checkValues = new List<vCheckPropertyAttribute.CheckValue>();

	// Token: 0x040008AC RID: 2220
	public bool hideInInspector;

	// Token: 0x040008AD RID: 2221
	public bool invertResult;

	// Token: 0x020000FF RID: 255
	[Serializable]
	public struct CheckValue
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00043E04 File Offset: 0x00042004
		public bool isValid
		{
			get
			{
				return this.value != null;
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00043E0F File Offset: 0x0004200F
		public CheckValue(string property, object value)
		{
			this.property = property;
			this.value = value;
		}

		// Token: 0x040008AE RID: 2222
		public string property;

		// Token: 0x040008AF RID: 2223
		public object value;
	}
}
