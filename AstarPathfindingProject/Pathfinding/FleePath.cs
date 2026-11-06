using System;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000139 RID: 313
	public class FleePath : RandomPath
	{
		// Token: 0x06000A42 RID: 2626 RVA: 0x00037247 File Offset: 0x00035447
		public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback = null)
		{
			FleePath path = PathPool.GetPath<FleePath>();
			path.Setup(start, avoid, searchLength, callback);
			return path;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00037258 File Offset: 0x00035458
		protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
		{
			base.Setup(start, searchLength, callback);
			this.aim = start - (avoid - start) * 10f;
		}
	}
}
