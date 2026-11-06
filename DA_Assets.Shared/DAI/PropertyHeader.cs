using System;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x0200003E RID: 62
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public abstract class PropertyHeader : PropertyAttribute
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000062DD File Offset: 0x000044DD
		// (set) Token: 0x06000132 RID: 306 RVA: 0x000062E5 File Offset: 0x000044E5
		public GUIContent HeaderLabel { get; protected set; }

		// Token: 0x06000133 RID: 307 RVA: 0x000062EE File Offset: 0x000044EE
		protected PropertyHeader(GUIContent headerLabel)
		{
			this.HeaderLabel = headerLabel;
		}
	}
}
