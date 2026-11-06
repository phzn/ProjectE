using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000091 RID: 145
	[AddComponentMenu("More Mountains/Tools/Utilities/MMDebugController")]
	public class MMDebugController : MonoBehaviour
	{
		// Token: 0x0600032C RID: 812 RVA: 0x000102BB File Offset: 0x0000E4BB
		protected virtual void Awake()
		{
			MMDebug.SetDebugLogsEnabled(this.DebugLogsEnabled);
			MMDebug.SetDebugDrawEnabled(this.DebugDrawEnabled);
		}

		// Token: 0x0400045C RID: 1116
		public bool DebugLogsEnabled = true;

		// Token: 0x0400045D RID: 1117
		public bool DebugDrawEnabled = true;
	}
}
