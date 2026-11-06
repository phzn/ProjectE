using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000083 RID: 131
[AddComponentMenu("")]
public sealed class MapResidencyDependencyAnchor : MonoBehaviour
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0002ABCE File Offset: 0x00028DCE
	public IReadOnlyList<Object> Dependencies
	{
		get
		{
			return this.dependencies;
		}
	}

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	private Object[] dependencies = Array.Empty<Object>();
}
