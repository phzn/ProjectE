using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200000E RID: 14
	internal interface ICurvesOwner
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000094 RID: 148
		AnimationClip curves { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000095 RID: 149
		bool hasCurves { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000096 RID: 150
		double duration { get; }

		// Token: 0x06000097 RID: 151
		void CreateCurves(string curvesClipName);

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000098 RID: 152
		string defaultCurvesName { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000099 RID: 153
		Object asset { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009A RID: 154
		Object assetOwner { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009B RID: 155
		TrackAsset targetTrack { get; }
	}
}
