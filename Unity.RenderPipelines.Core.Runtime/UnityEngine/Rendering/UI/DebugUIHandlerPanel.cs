using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000131 RID: 305
	public class DebugUIHandlerPanel : MonoBehaviour
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x0002A0C7 File Offset: 0x000282C7
		private void OnEnable()
		{
			this.m_ScrollTransform = this.scrollRect.GetComponent<RectTransform>();
			this.m_ContentTransform = base.GetComponent<DebugUIHandlerContainer>().contentHolder;
			this.m_MaskTransform = base.GetComponentInChildren<Mask>(true).rectTransform;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002A0FD File Offset: 0x000282FD
		internal void SetPanel(DebugUI.Panel panel)
		{
			this.m_Panel = panel;
			this.nameLabel.text = panel.displayName;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002A117 File Offset: 0x00028317
		internal DebugUI.Panel GetPanel()
		{
			return this.m_Panel;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002A11F File Offset: 0x0002831F
		public void SelectNextItem()
		{
			this.Canvas.SelectNextPanel();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002A12C File Offset: 0x0002832C
		public void SelectPreviousItem()
		{
			this.Canvas.SelectPreviousPanel();
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002A139 File Offset: 0x00028339
		public void OnScrollbarClicked()
		{
			DebugManager.instance.SetScrollTarget(null);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002A146 File Offset: 0x00028346
		internal void SetScrollTarget(DebugUIHandlerWidget target)
		{
			this.m_ScrollTarget = target;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002A150 File Offset: 0x00028350
		internal void UpdateScroll()
		{
			if (this.m_ScrollTarget == null)
			{
				return;
			}
			RectTransform component = this.m_ScrollTarget.GetComponent<RectTransform>();
			float yposInScroll = this.GetYPosInScroll(component);
			float num = (this.GetYPosInScroll(this.m_MaskTransform) - yposInScroll) / (this.m_ContentTransform.rect.size.y - this.m_ScrollTransform.rect.size.y);
			float num2 = this.scrollRect.verticalNormalizedPosition - num;
			num2 = Mathf.Clamp01(num2);
			this.scrollRect.verticalNormalizedPosition = Mathf.Lerp(this.scrollRect.verticalNormalizedPosition, num2, Time.deltaTime * 10f);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002A200 File Offset: 0x00028400
		private float GetYPosInScroll(RectTransform target)
		{
			Vector3 vector = new Vector3((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0f);
			Vector3 vector2 = target.localPosition + vector;
			Vector3 vector3 = target.parent.TransformPoint(vector2);
			return this.m_ScrollTransform.TransformPoint(vector3).y;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002A28E File Offset: 0x0002848E
		internal DebugUIHandlerWidget GetFirstItem()
		{
			return base.GetComponent<DebugUIHandlerContainer>().GetFirstItem();
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002A29B File Offset: 0x0002849B
		public void ResetDebugManager()
		{
			DebugManager.instance.Reset();
		}

		// Token: 0x0400054A RID: 1354
		public Text nameLabel;

		// Token: 0x0400054B RID: 1355
		public ScrollRect scrollRect;

		// Token: 0x0400054C RID: 1356
		public RectTransform viewport;

		// Token: 0x0400054D RID: 1357
		public DebugUIHandlerCanvas Canvas;

		// Token: 0x0400054E RID: 1358
		private RectTransform m_ScrollTransform;

		// Token: 0x0400054F RID: 1359
		private RectTransform m_ContentTransform;

		// Token: 0x04000550 RID: 1360
		private RectTransform m_MaskTransform;

		// Token: 0x04000551 RID: 1361
		private DebugUIHandlerWidget m_ScrollTarget;

		// Token: 0x04000552 RID: 1362
		protected internal DebugUI.Panel m_Panel;
	}
}
