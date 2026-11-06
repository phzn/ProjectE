using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.UI
{
	// Token: 0x0200003F RID: 63
	[Obsolete("Use BaseMeshEffect instead", true)]
	public abstract class BaseVertexEffect
	{
		// Token: 0x060004A8 RID: 1192
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use BaseMeshEffect.ModifyMeshes instead", true)]
		public abstract void ModifyVertices(List<UIVertex> vertices);
	}
}
