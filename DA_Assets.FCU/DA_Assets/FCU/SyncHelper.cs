using System;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200002A RID: 42
	[DisallowMultipleComponent]
	[Serializable]
	public class SyncHelper : MonoBehaviour
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000360C File Offset: 0x0000180C
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003614 File Offset: 0x00001814
		public bool Debug
		{
			get
			{
				return this.debug;
			}
			set
			{
				this.debug = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000361D File Offset: 0x0000181D
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003625 File Offset: 0x00001825
		public SyncData Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003630 File Offset: 0x00001830
		public int HierarchyLevel
		{
			get
			{
				int num = 0;
				Transform transform = base.transform;
				while (transform.parent != null)
				{
					num++;
					transform = transform.parent;
				}
				return num;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003662 File Offset: 0x00001862
		private void OnValidate()
		{
			if (this.data != null)
			{
				this.data.GameObject = base.gameObject;
			}
		}

		// Token: 0x040002FB RID: 763
		[SerializeField]
		private bool debug;

		// Token: 0x040002FC RID: 764
		[SerializeField]
		private SyncData data;
	}
}
