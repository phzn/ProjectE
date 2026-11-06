using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003C RID: 60
	[AttributeUsage(AttributeTargets.Class)]
	[Obsolete("TrackMediaType has been deprecated. It is no longer required, and will be removed in a future release.", false)]
	public class TrackMediaType : Attribute
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x00009A30 File Offset: 0x00007C30
		public TrackMediaType(TimelineAsset.MediaType mt)
		{
			this.m_MediaType = mt;
		}

		// Token: 0x040000E7 RID: 231
		public readonly TimelineAsset.MediaType m_MediaType;
	}
}
