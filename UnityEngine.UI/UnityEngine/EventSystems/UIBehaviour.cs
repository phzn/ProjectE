using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000074 RID: 116
	public abstract class UIBehaviour : MonoBehaviour
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0001BEF3 File Offset: 0x0001A0F3
		protected virtual void Awake()
		{
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001BEF5 File Offset: 0x0001A0F5
		protected virtual void OnEnable()
		{
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001BEF7 File Offset: 0x0001A0F7
		protected virtual void Start()
		{
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001BEF9 File Offset: 0x0001A0F9
		protected virtual void OnDisable()
		{
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001BEFB File Offset: 0x0001A0FB
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001BEFD File Offset: 0x0001A0FD
		public virtual bool IsActive()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001BF05 File Offset: 0x0001A105
		protected virtual void OnRectTransformDimensionsChange()
		{
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001BF07 File Offset: 0x0001A107
		protected virtual void OnBeforeTransformParentChanged()
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001BF09 File Offset: 0x0001A109
		protected virtual void OnTransformParentChanged()
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001BF0B File Offset: 0x0001A10B
		protected virtual void OnDidApplyAnimationProperties()
		{
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001BF0D File Offset: 0x0001A10D
		protected virtual void OnCanvasGroupChanged()
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001BF0F File Offset: 0x0001A10F
		protected virtual void OnCanvasHierarchyChanged()
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001BF11 File Offset: 0x0001A111
		public bool IsDestroyed()
		{
			return this == null;
		}
	}
}
