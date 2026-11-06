using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000073 RID: 115
	public struct RaycastResult
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x0001BCE8 File Offset: 0x00019EE8
		public GameObject gameObject
		{
			get
			{
				return this.m_GameObject;
			}
			set
			{
				this.m_GameObject = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001BCF1 File Offset: 0x00019EF1
		public bool isValid
		{
			get
			{
				return this.module != null && this.gameObject != null;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001BD10 File Offset: 0x00019F10
		public void Clear()
		{
			this.gameObject = null;
			this.module = null;
			this.distance = 0f;
			this.index = 0f;
			this.depth = 0;
			this.sortingLayer = 0;
			this.sortingOrder = 0;
			this.worldNormal = Vector3.up;
			this.worldPosition = Vector3.zero;
			this.screenPosition = Vector3.zero;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001BD7C File Offset: 0x00019F7C
		public override string ToString()
		{
			if (!this.isValid)
			{
				return "";
			}
			string[] array = new string[24];
			array[0] = "Name: ";
			int num = 1;
			GameObject gameObject = this.gameObject;
			array[num] = ((gameObject != null) ? gameObject.ToString() : null);
			array[2] = "\nmodule: ";
			int num2 = 3;
			BaseRaycaster baseRaycaster = this.module;
			array[num2] = ((baseRaycaster != null) ? baseRaycaster.ToString() : null);
			array[4] = "\ndistance: ";
			array[5] = this.distance.ToString();
			array[6] = "\nindex: ";
			array[7] = this.index.ToString();
			array[8] = "\ndepth: ";
			array[9] = this.depth.ToString();
			array[10] = "\nworldNormal: ";
			int num3 = 11;
			Vector3 vector = this.worldNormal;
			array[num3] = vector.ToString();
			array[12] = "\nworldPosition: ";
			int num4 = 13;
			vector = this.worldPosition;
			array[num4] = vector.ToString();
			array[14] = "\nscreenPosition: ";
			int num5 = 15;
			Vector2 vector2 = this.screenPosition;
			array[num5] = vector2.ToString();
			array[16] = "\nmodule.sortOrderPriority: ";
			array[17] = this.module.sortOrderPriority.ToString();
			array[18] = "\nmodule.renderOrderPriority: ";
			array[19] = this.module.renderOrderPriority.ToString();
			array[20] = "\nsortingLayer: ";
			array[21] = this.sortingLayer.ToString();
			array[22] = "\nsortingOrder: ";
			array[23] = this.sortingOrder.ToString();
			return string.Concat(array);
		}

		// Token: 0x04000238 RID: 568
		private GameObject m_GameObject;

		// Token: 0x04000239 RID: 569
		public BaseRaycaster module;

		// Token: 0x0400023A RID: 570
		public float distance;

		// Token: 0x0400023B RID: 571
		public float index;

		// Token: 0x0400023C RID: 572
		public int depth;

		// Token: 0x0400023D RID: 573
		public int sortingGroupID;

		// Token: 0x0400023E RID: 574
		public int sortingGroupOrder;

		// Token: 0x0400023F RID: 575
		public int sortingLayer;

		// Token: 0x04000240 RID: 576
		public int sortingOrder;

		// Token: 0x04000241 RID: 577
		public Vector3 worldPosition;

		// Token: 0x04000242 RID: 578
		public Vector3 worldNormal;

		// Token: 0x04000243 RID: 579
		public Vector2 screenPosition;

		// Token: 0x04000244 RID: 580
		public int displayIndex;
	}
}
