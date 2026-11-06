using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200029C RID: 668
	public interface ITransform
	{
		// Token: 0x060010AD RID: 4269
		Vector3 Transform(Vector3 position);

		// Token: 0x060010AE RID: 4270
		Vector3 InverseTransform(Vector3 position);
	}
}
