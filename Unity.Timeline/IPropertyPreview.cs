using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200004E RID: 78
	public interface IPropertyPreview
	{
		// Token: 0x060002E4 RID: 740
		void GatherProperties(PlayableDirector director, IPropertyCollector driver);
	}
}
