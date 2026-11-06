using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005C RID: 92
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Setup/Rig")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/RiggingWorkflow.html#rig-component")]
	public class Rig : MonoBehaviour, IRigEffectorHolder
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00007C61 File Offset: 0x00005E61
		// (set) Token: 0x0600025C RID: 604 RVA: 0x00007C69 File Offset: 0x00005E69
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
			set
			{
				this.m_Weight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x040000FD RID: 253
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Weight = 1f;

		// Token: 0x040000FE RID: 254
		[SerializeField]
		private List<RigEffectorData> m_Effectors = new List<RigEffectorData>();
	}
}
