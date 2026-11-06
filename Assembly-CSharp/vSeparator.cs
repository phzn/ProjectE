using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class vSeparator : PropertyAttribute
{
	// Token: 0x0600097B RID: 2427 RVA: 0x00043E69 File Offset: 0x00042069
	public vSeparator()
	{
		this.fontSize = 15;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00043E81 File Offset: 0x00042081
	public vSeparator(string label, string tooltip = "")
	{
		this.label = label;
		this.tooltip = tooltip;
		this.fontSize = 15;
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00043EA7 File Offset: 0x000420A7
	public vSeparator(string label, int fontSize, string tooltip = "")
	{
		this.label = label;
		this.tooltip = tooltip;
		this.fontSize = fontSize;
	}

	// Token: 0x040008B9 RID: 2233
	public string label;

	// Token: 0x040008BA RID: 2234
	public string tooltip;

	// Token: 0x040008BB RID: 2235
	public string style;

	// Token: 0x040008BC RID: 2236
	public int fontSize = 10;
}
