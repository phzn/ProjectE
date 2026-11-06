using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000122 RID: 290
	public class UnityTimeSpan
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x000301D2 File Offset: 0x0002E3D2
		public UnityTimeSpan(string name)
		{
			this.name = name;
			this.stime = Time.realtimeSinceStartup;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000301F7 File Offset: 0x0002E3F7
		public void Finish()
		{
			if (!this.isFinish)
			{
				this.etime = Time.realtimeSinceStartup;
				this.isFinish = true;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00030213 File Offset: 0x0002E413
		public float TotalSeconds()
		{
			this.Finish();
			return this.etime - this.stime;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00030228 File Offset: 0x0002E428
		public float TotalMilliSeconds()
		{
			this.Finish();
			return (this.etime - this.stime) * 1000f;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00030243 File Offset: 0x0002E443
		public override string ToString()
		{
			return string.Format("UnityTimeSpan [{0}] : {1}(ms)", this.name, this.TotalMilliSeconds());
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00030260 File Offset: 0x0002E460
		public void DebugLog()
		{
			Debug.Log(this);
		}

		// Token: 0x040008BD RID: 2237
		private string name = string.Empty;

		// Token: 0x040008BE RID: 2238
		private float stime;

		// Token: 0x040008BF RID: 2239
		private float etime;

		// Token: 0x040008C0 RID: 2240
		private bool isFinish;
	}
}
