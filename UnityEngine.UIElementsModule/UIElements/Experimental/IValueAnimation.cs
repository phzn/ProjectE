using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004C1 RID: 1217
	public interface IValueAnimation
	{
		// Token: 0x060025F9 RID: 9721
		void Start();

		// Token: 0x060025FA RID: 9722
		void Stop();

		// Token: 0x060025FB RID: 9723
		void Recycle();

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060025FC RID: 9724
		bool isRunning { get; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060025FD RID: 9725
		// (set) Token: 0x060025FE RID: 9726
		int durationMs { get; set; }
	}
}
