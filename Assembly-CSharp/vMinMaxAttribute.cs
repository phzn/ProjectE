using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class vMinMaxAttribute : PropertyAttribute
{
	// Token: 0x06000979 RID: 2425 RVA: 0x00043E35 File Offset: 0x00042035
	public vMinMaxAttribute()
	{
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00043E48 File Offset: 0x00042048
	public vMinMaxAttribute(float min, float max)
	{
		this.minLimit = min;
		this.maxLimit = max;
	}

	// Token: 0x040008B7 RID: 2231
	public float minLimit;

	// Token: 0x040008B8 RID: 2232
	public float maxLimit = 1f;
}
