using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class vToggleOptionAttribute : PropertyAttribute
{
	// Token: 0x0600097E RID: 2430 RVA: 0x00043ECC File Offset: 0x000420CC
	public vToggleOptionAttribute(string label = "", string falseValue = "No", string trueValue = "Yes")
	{
		this.label = label;
		this.falseValue = falseValue;
		this.trueValue = trueValue;
	}

	// Token: 0x040008BD RID: 2237
	public string label;

	// Token: 0x040008BE RID: 2238
	public string falseValue;

	// Token: 0x040008BF RID: 2239
	public string trueValue;
}
