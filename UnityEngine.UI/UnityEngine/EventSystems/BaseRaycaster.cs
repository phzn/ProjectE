using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000070 RID: 112
	public abstract class BaseRaycaster : UIBehaviour
	{
		// Token: 0x0600066F RID: 1647
		public abstract void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList);

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000670 RID: 1648
		public abstract Camera eventCamera { get; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x0001B5CE File Offset: 0x000197CE
		[Obsolete("Please use sortOrderPriority and renderOrderPriority", false)]
		public virtual int priority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001B5D1 File Offset: 0x000197D1
		public virtual int sortOrderPriority
		{
			get
			{
				return int.MinValue;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001B5D8 File Offset: 0x000197D8
		public virtual int renderOrderPriority
		{
			get
			{
				return int.MinValue;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001B5E0 File Offset: 0x000197E0
		public BaseRaycaster rootRaycaster
		{
			get
			{
				if (this.m_RootRaycaster == null)
				{
					BaseRaycaster[] componentsInParent = base.GetComponentsInParent<BaseRaycaster>();
					if (componentsInParent.Length != 0)
					{
						this.m_RootRaycaster = componentsInParent[componentsInParent.Length - 1];
					}
				}
				return this.m_RootRaycaster;
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001B61C File Offset: 0x0001981C
		public override string ToString()
		{
			string[] array = new string[8];
			array[0] = "Name: ";
			int num = 1;
			GameObject gameObject = base.gameObject;
			array[num] = ((gameObject != null) ? gameObject.ToString() : null);
			array[2] = "\neventCamera: ";
			int num2 = 3;
			Camera eventCamera = this.eventCamera;
			array[num2] = ((eventCamera != null) ? eventCamera.ToString() : null);
			array[4] = "\nsortOrderPriority: ";
			array[5] = this.sortOrderPriority.ToString();
			array[6] = "\nrenderOrderPriority: ";
			array[7] = this.renderOrderPriority.ToString();
			return string.Concat(array);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001B6A0 File Offset: 0x000198A0
		protected override void OnEnable()
		{
			base.OnEnable();
			RaycasterManager.AddRaycaster(this);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001B6AE File Offset: 0x000198AE
		protected override void OnDisable()
		{
			RaycasterManager.RemoveRaycasters(this);
			base.OnDisable();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001B6BC File Offset: 0x000198BC
		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			this.m_RootRaycaster = null;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001B6CB File Offset: 0x000198CB
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_RootRaycaster = null;
		}

		// Token: 0x04000230 RID: 560
		private BaseRaycaster m_RootRaycaster;
	}
}
