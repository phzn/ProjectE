using System;
using System.Diagnostics;

// Token: 0x0200005D RID: 93
public sealed class DeferredStreamingFrameBudget
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00025A42 File Offset: 0x00023C42
	// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00025A4A File Offset: 0x00023C4A
	public double LimitMilliseconds { get; private set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060004BA RID: 1210 RVA: 0x00025A53 File Offset: 0x00023C53
	// (set) Token: 0x060004BB RID: 1211 RVA: 0x00025A5B File Offset: 0x00023C5B
	public double ElapsedMilliseconds { get; private set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060004BC RID: 1212 RVA: 0x00025A64 File Offset: 0x00023C64
	// (set) Token: 0x060004BD RID: 1213 RVA: 0x00025A6C File Offset: 0x00023C6C
	public int DeferredQueueCount { get; private set; }

	// Token: 0x060004BE RID: 1214 RVA: 0x00025A75 File Offset: 0x00023C75
	public DeferredStreamingFrameBudget()
		: this(new Func<double>(DeferredStreamingFrameBudget.CurrentTimeMilliseconds))
	{
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00025A89 File Offset: 0x00023C89
	public DeferredStreamingFrameBudget(Func<double> nowMilliseconds)
	{
		if (nowMilliseconds == null)
		{
			throw new ArgumentNullException("nowMilliseconds");
		}
		this._nowMilliseconds = nowMilliseconds;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00025AA7 File Offset: 0x00023CA7
	public static double CurrentTimeMilliseconds()
	{
		return (double)Stopwatch.GetTimestamp() * (1000.0 / (double)Stopwatch.Frequency);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00025AC0 File Offset: 0x00023CC0
	public void BeginFrame(double limitMilliseconds, bool enforceSharedLimit)
	{
		this.LimitMilliseconds = limitMilliseconds;
		this.ElapsedMilliseconds = 0.0;
		this.DeferredQueueCount = 0;
		this._enforceSharedLimit = enforceSharedLimit;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00025AE6 File Offset: 0x00023CE6
	public double BeginQueue()
	{
		this._queueDeferred = false;
		return this._nowMilliseconds();
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00025AFC File Offset: 0x00023CFC
	public bool CanStartOperation(double queueStartMilliseconds, double queueLimitMilliseconds)
	{
		double num = Math.Max(0.0, this._nowMilliseconds() - queueStartMilliseconds);
		if (num >= queueLimitMilliseconds)
		{
			return false;
		}
		if (this._enforceSharedLimit && this.ElapsedMilliseconds + num >= this.LimitMilliseconds)
		{
			if (!this._queueDeferred)
			{
				int deferredQueueCount = this.DeferredQueueCount;
				this.DeferredQueueCount = deferredQueueCount + 1;
				this._queueDeferred = true;
			}
			return false;
		}
		return true;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00025B65 File Offset: 0x00023D65
	public void EndQueue(double queueStartMilliseconds)
	{
		this.ElapsedMilliseconds += Math.Max(0.0, this._nowMilliseconds() - queueStartMilliseconds);
	}

	// Token: 0x04000397 RID: 919
	private readonly Func<double> _nowMilliseconds;

	// Token: 0x04000398 RID: 920
	private bool _enforceSharedLimit;

	// Token: 0x04000399 RID: 921
	private bool _queueDeferred;
}
