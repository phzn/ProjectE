using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.UI
{
	// Token: 0x02000041 RID: 65
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use IMeshModifier instead", true)]
	public interface IVertexModifier
	{
		// Token: 0x060004B1 RID: 1201
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("use IMeshModifier.ModifyMesh (VertexHelper verts)  instead", true)]
		void ModifyVertices(List<UIVertex> verts);
	}
}
