using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class vHelpBoxAttribute : PropertyAttribute
{
	// Token: 0x06000978 RID: 2424 RVA: 0x00043E1F File Offset: 0x0004201F
	public vHelpBoxAttribute(string text, vHelpBoxAttribute.MessageType messageType = vHelpBoxAttribute.MessageType.None)
	{
		this.text = text;
		this.messageType = messageType;
	}

	// Token: 0x040008B0 RID: 2224
	public string text;

	// Token: 0x040008B1 RID: 2225
	public int lineSpace;

	// Token: 0x040008B2 RID: 2226
	public vHelpBoxAttribute.MessageType messageType;

	// Token: 0x02000101 RID: 257
	public enum MessageType
	{
		// Token: 0x040008B4 RID: 2228
		None,
		// Token: 0x040008B5 RID: 2229
		Info,
		// Token: 0x040008B6 RID: 2230
		Warning
	}
}
