using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000013 RID: 19
	// (Invoke) Token: 0x06000089 RID: 137
	public delegate void RenderFunc<PassData>(PassData data, RenderGraphContext renderGraphContext) where PassData : class, new();
}
