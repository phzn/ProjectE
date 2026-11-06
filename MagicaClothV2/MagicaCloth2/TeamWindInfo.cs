using System;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000B9 RID: 185
	public struct TeamWindInfo : IValid
	{
		// Token: 0x06000331 RID: 817 RVA: 0x00021DA1 File Offset: 0x0001FFA1
		public bool IsValid()
		{
			return this.main > 1E-06f;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00021DB0 File Offset: 0x0001FFB0
		public override string ToString()
		{
			return string.Format("windId:{0}, time:{1}, main:{2}, direction:{3}", new object[] { this.windId, this.time, this.main, this.direction });
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00021E05 File Offset: 0x00020005
		public void DebugLog()
		{
			Debug.Log(this.ToString());
		}

		// Token: 0x04000709 RID: 1801
		public int windId;

		// Token: 0x0400070A RID: 1802
		public float time;

		// Token: 0x0400070B RID: 1803
		public float main;

		// Token: 0x0400070C RID: 1804
		public float3 direction;
	}
}
