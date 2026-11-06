using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000035 RID: 53
	public interface ITimeControl
	{
		// Token: 0x06000286 RID: 646
		void SetTime(double time);

		// Token: 0x06000287 RID: 647
		void OnControlTimeStart();

		// Token: 0x06000288 RID: 648
		void OnControlTimeStop();
	}
}
