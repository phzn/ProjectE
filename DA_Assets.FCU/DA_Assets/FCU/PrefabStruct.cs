using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public struct PrefabStruct
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00003CE8 File Offset: 0x00001EE8
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00003CF0 File Offset: 0x00001EF0
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00003CF9 File Offset: 0x00001EF9
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00003D01 File Offset: 0x00001F01
		public int Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00003D0A File Offset: 0x00001F0A
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00003D12 File Offset: 0x00001F12
		public int Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00003D1B File Offset: 0x00001F1B
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00003D23 File Offset: 0x00001F23
		public List<int> Childs
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00003D2C File Offset: 0x00001F2C
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00003D34 File Offset: 0x00001F34
		public int SiblingIndex
		{
			get
			{
				return this.siblingIndex;
			}
			set
			{
				this.siblingIndex = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00003D3D File Offset: 0x00001F3D
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00003D45 File Offset: 0x00001F45
		public int Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00003D4E File Offset: 0x00001F4E
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00003D56 File Offset: 0x00001F56
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00003D5F File Offset: 0x00001F5F
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00003D67 File Offset: 0x00001F67
		public int PrefabNumber
		{
			get
			{
				return this.prefabNumber;
			}
			set
			{
				this.prefabNumber = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00003D70 File Offset: 0x00001F70
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00003D78 File Offset: 0x00001F78
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00003D81 File Offset: 0x00001F81
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00003D89 File Offset: 0x00001F89
		public UguiTransformData UguiTransformData
		{
			get
			{
				return this.uguiTransformData;
			}
			set
			{
				this.uguiTransformData = value;
			}
		}

		// Token: 0x04000347 RID: 839
		[SerializeField]
		private string name;

		// Token: 0x04000348 RID: 840
		[SerializeField]
		private int current;

		// Token: 0x04000349 RID: 841
		[SerializeField]
		private int parent;

		// Token: 0x0400034A RID: 842
		[Space]
		[SerializeField]
		private List<int> childs;

		// Token: 0x0400034B RID: 843
		[Space]
		[SerializeField]
		private int siblingIndex;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		private int hash;

		// Token: 0x0400034D RID: 845
		[SerializeField]
		private string id;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		private int prefabNumber;

		// Token: 0x0400034F RID: 847
		[SerializeField]
		private string text;

		// Token: 0x04000350 RID: 848
		[SerializeField]
		private UguiTransformData uguiTransformData;
	}
}
