using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000045 RID: 69
	public class SelectableObject<T>
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00003F7F File Offset: 0x0000217F
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00003F87 File Offset: 0x00002187
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00003F90 File Offset: 0x00002190
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00003F98 File Offset: 0x00002198
		public List<SelectableObject<T>> Childs
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00003FA1 File Offset: 0x000021A1
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00003FA9 File Offset: 0x000021A9
		public T Object { get; set; }

		// Token: 0x06000189 RID: 393 RVA: 0x00003FB4 File Offset: 0x000021B4
		public void SetAllSelected(bool value)
		{
			this.selected = value;
			foreach (SelectableObject<T> selectableObject in this.childs)
			{
				selectableObject.SetAllSelected(value);
			}
		}

		// Token: 0x04000362 RID: 866
		[SerializeField]
		private bool selected;

		// Token: 0x04000363 RID: 867
		[SerializeField]
		private List<SelectableObject<T>> childs = new List<SelectableObject<T>>();
	}
}
