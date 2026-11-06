using System;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000016 RID: 22
	[RequiredByNativeCode]
	[Serializable]
	public abstract class VFXSpawnerCallbacks : ScriptableObject
	{
		// Token: 0x06000086 RID: 134
		public abstract void OnPlay(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent);

		// Token: 0x06000087 RID: 135
		public abstract void OnUpdate(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent);

		// Token: 0x06000088 RID: 136
		public abstract void OnStop(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent);
	}
}
