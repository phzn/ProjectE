using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000469 RID: 1129
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class vHideInInspectorAttribute : PropertyAttribute
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x000AF797 File Offset: 0x000AD997
		// (set) Token: 0x06001BF1 RID: 7153 RVA: 0x000AF79F File Offset: 0x000AD99F
		public bool hideProperty { get; set; }

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000AF7A8 File Offset: 0x000AD9A8
		public vHideInInspectorAttribute(string refbooleanProperty, bool invertValue = false)
		{
			this.refbooleanProperty = refbooleanProperty;
			this.invertValue = invertValue;
		}

		// Token: 0x04001D55 RID: 7509
		public string refbooleanProperty;

		// Token: 0x04001D56 RID: 7510
		public bool invertValue;
	}
}
