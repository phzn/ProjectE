using System;

namespace MagicaCloth2
{
	// Token: 0x02000121 RID: 289
	public class TimeSpan
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x000300D4 File Offset: 0x0002E2D4
		public TimeSpan()
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000300E7 File Offset: 0x0002E2E7
		public TimeSpan(string name)
		{
			this.name = name;
			this.stime = DateTime.Now;
			this.isFinish = false;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00030113 File Offset: 0x0002E313
		public void Start()
		{
			this.stime = DateTime.Now;
			this.isFinish = false;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00030127 File Offset: 0x0002E327
		public void Finish()
		{
			if (!this.isFinish)
			{
				this.etime = DateTime.Now;
				this.isFinish = true;
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00030144 File Offset: 0x0002E344
		public double TotalSeconds()
		{
			this.Finish();
			return (this.etime - this.stime).TotalSeconds;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00030170 File Offset: 0x0002E370
		public double TotalMilliSeconds()
		{
			this.Finish();
			return (this.etime - this.stime).TotalMilliseconds;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0003019C File Offset: 0x0002E39C
		public override string ToString()
		{
			return string.Format("TimeSpan [{0}] : {1}(ms)", this.name, this.TotalMilliSeconds());
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000706D File Offset: 0x0000526D
		public void DebugLog()
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000301BC File Offset: 0x0002E3BC
		public void Log()
		{
			object obj = this;
			Develop.Log(in obj);
		}

		// Token: 0x040008B9 RID: 2233
		private string name = string.Empty;

		// Token: 0x040008BA RID: 2234
		private DateTime stime;

		// Token: 0x040008BB RID: 2235
		private DateTime etime;

		// Token: 0x040008BC RID: 2236
		private bool isFinish;
	}
}
