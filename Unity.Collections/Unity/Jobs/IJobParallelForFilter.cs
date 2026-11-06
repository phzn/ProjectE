using System;

namespace Unity.Jobs
{
	// Token: 0x02000009 RID: 9
	[Obsolete("'IJobParallelForFilter' has been deprecated; use 'IJobFilter' instead. (UnityUpgradable) -> IJobFilter")]
	public interface IJobParallelForFilter
	{
		// Token: 0x06000010 RID: 16
		bool Execute(int index);
	}
}
