using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000014 RID: 20
	public class MMToggleActive : MonoBehaviour
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002E79 File Offset: 0x00001079
		protected virtual void Awake()
		{
			if (this.TargetGameObject == null)
			{
				this.TargetGameObject = base.gameObject;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E95 File Offset: 0x00001095
		public virtual void ToggleActive()
		{
			this.TargetGameObject.SetActive(!this.TargetGameObject.activeInHierarchy);
		}

		// Token: 0x0400005B RID: 91
		[Header("Target - leave empty for self")]
		public GameObject TargetGameObject;

		// Token: 0x0400005C RID: 92
		[MMInspectorButton("ToggleActive")]
		public bool ToggleActiveButton;
	}
}
