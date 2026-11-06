using System;
using System.Collections.Generic;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000002 RID: 2
	[VisibleToOtherModules]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	internal sealed class AssetFileNameExtensionAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public string preferredExtension { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public IEnumerable<string> otherExtensions { get; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public AssetFileNameExtensionAttribute(string preferredExtension, params string[] otherExtensions)
		{
			this.preferredExtension = preferredExtension;
			this.otherExtensions = otherExtensions;
		}
	}
}
