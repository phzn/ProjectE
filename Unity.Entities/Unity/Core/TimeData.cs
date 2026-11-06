using System;
using System.ComponentModel;
using UnityEngine;

namespace Unity.Core
{
	// Token: 0x020004D1 RID: 1233
	public readonly struct TimeData
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x00067DAD File Offset: 0x00065FAD
		public TimeData(double elapsedTime, float deltaTime)
		{
			this.ElapsedTime = elapsedTime;
			this.DeltaTime = deltaTime;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x00067DBD File Offset: 0x00065FBD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float fixedDeltaTime
		{
			get
			{
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x04000AAC RID: 2732
		public readonly double ElapsedTime;

		// Token: 0x04000AAD RID: 2733
		public readonly float DeltaTime;
	}
}
