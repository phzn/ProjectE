using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x02000019 RID: 25
	[Obsolete("GUILayer has been removed.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[ExcludeFromObjectFactory]
	[ExcludeFromPreset]
	public sealed class GUILayer
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x000081C9 File Offset: 0x000063C9
		[Obsolete("GUILayer has been removed.", true)]
		public GUIElement HitTest(Vector3 screenPosition)
		{
			throw new Exception("GUILayer has been removed from Unity.");
		}
	}
}
