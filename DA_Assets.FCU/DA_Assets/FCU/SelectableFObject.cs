using System;
using System.Collections.Generic;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public class SelectableFObject
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00003EBD File Offset: 0x000020BD
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00003EC5 File Offset: 0x000020C5
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00003ECE File Offset: 0x000020CE
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00003ED6 File Offset: 0x000020D6
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00003EDF File Offset: 0x000020DF
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00003EE7 File Offset: 0x000020E7
		public NodeType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00003EF0 File Offset: 0x000020F0
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00003EF8 File Offset: 0x000020F8
		public bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00003F01 File Offset: 0x00002101
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00003F09 File Offset: 0x00002109
		public List<SelectableFObject> Childs
		{
			get
			{
				return this.childs;
			}
			set
			{
				this.childs = value;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00003F14 File Offset: 0x00002114
		public void SetAllSelected(bool value)
		{
			this.selected = value;
			foreach (SelectableFObject selectableFObject in this.childs)
			{
				selectableFObject.SetAllSelected(value);
			}
		}

		// Token: 0x0400035D RID: 861
		[SerializeField]
		private string id;

		// Token: 0x0400035E RID: 862
		[SerializeField]
		private string name;

		// Token: 0x0400035F RID: 863
		[SerializeField]
		private NodeType type;

		// Token: 0x04000360 RID: 864
		[SerializeField]
		private bool selected;

		// Token: 0x04000361 RID: 865
		[SerializeField]
		[SerializeReference]
		private List<SelectableFObject> childs = new List<SelectableFObject>();
	}
}
