using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class vBarDisplayAttribute : PropertyAttribute
{
	// Token: 0x06000974 RID: 2420 RVA: 0x00043D7B File Offset: 0x00041F7B
	public vBarDisplayAttribute(string maxValueProperty, bool showJuntInPlayMode = false)
	{
		this.maxValueProperty = maxValueProperty;
		this.showJuntInPlayMode = showJuntInPlayMode;
	}

	// Token: 0x040008A9 RID: 2217
	public readonly string maxValueProperty;

	// Token: 0x040008AA RID: 2218
	public readonly bool showJuntInPlayMode;
}
