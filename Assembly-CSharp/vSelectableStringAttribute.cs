using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class vSelectableStringAttribute : PropertyAttribute
{
	// Token: 0x060009D3 RID: 2515 RVA: 0x0004537A File Offset: 0x0004357A
	public vSelectableStringAttribute(string tittle, string selectableText)
	{
		this.tittle = tittle;
		this.selectableText = selectableText;
	}

	// Token: 0x0400092C RID: 2348
	public string tittle;

	// Token: 0x0400092D RID: 2349
	public string selectableText;
}
