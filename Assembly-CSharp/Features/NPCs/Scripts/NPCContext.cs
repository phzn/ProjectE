using System;
using Features.NPC;
using UnityEngine;

namespace Features.NPCs.Scripts
{
	// Token: 0x02000EC3 RID: 3779
	public class NPCContext : MonoBehaviour
	{
		// Token: 0x06007120 RID: 28960 RVA: 0x002DEE50 File Offset: 0x002DD050
		public void SetWorkplace(Transform workplace)
		{
			this.workplaceRoot = workplace;
			if (NPCDebug.Verbose)
			{
				Debug.Log("[NPCContext] " + base.gameObject.name + " assegnato al workplace: " + workplace.name);
			}
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x002DEE85 File Offset: 0x002DD085
		public bool IsInMyWorkplace(GameObject obj)
		{
			return this.workplaceRoot == null || (!(obj == null) && this.IsInMyWorkplace(obj.transform));
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x002DEEB0 File Offset: 0x002DD0B0
		public bool IsInMyWorkplace(Transform t)
		{
			return this.workplaceRoot == null || (!(t == null) && (t.IsChildOf(this.workplaceRoot) || t == this.workplaceRoot || (this.workplaceRoot.parent != null && t.IsChildOf(this.workplaceRoot.parent))));
		}

		// Token: 0x040073E9 RID: 29673
		[Tooltip("Il Transform radice del punto di interesse (es. Kombini) in cui questo NPC opera.")]
		public Transform workplaceRoot;
	}
}
