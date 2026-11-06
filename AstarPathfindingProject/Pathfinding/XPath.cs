using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013F RID: 319
	[Obsolete("Use an ABPath with the ABPath.endingCondition field instead")]
	public class XPath : ABPath
	{
		// Token: 0x06000A7D RID: 2685 RVA: 0x000385E4 File Offset: 0x000367E4
		[Obsolete("Use ABPath.Construct instead")]
		public new static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			return ABPath.Construct(start, end, callback);
		}
	}
}
