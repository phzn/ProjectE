using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200029E RID: 670
	internal class PointerDispatchState
	{
		// Token: 0x06001339 RID: 4921 RVA: 0x00043214 File Offset: 0x00041414
		public PointerDispatchState()
		{
			this.Reset();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00043260 File Offset: 0x00041460
		internal void Reset()
		{
			for (int i = 0; i < this.m_PointerCapture.Length; i++)
			{
				this.m_PendingPointerCapture[i] = null;
				this.m_PointerCapture[i] = null;
				this.m_ShouldSendCompatibilityMouseEvents[i] = true;
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000432A4 File Offset: 0x000414A4
		public IEventHandler GetCapturingElement(int pointerId)
		{
			return this.m_PendingPointerCapture[pointerId];
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000432C0 File Offset: 0x000414C0
		public bool HasPointerCapture(IEventHandler handler, int pointerId)
		{
			return this.m_PendingPointerCapture[pointerId] == handler;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000432E0 File Offset: 0x000414E0
		public void CapturePointer(IEventHandler handler, int pointerId)
		{
			bool flag = pointerId == PointerId.mousePointerId && this.m_PendingPointerCapture[pointerId] != handler && GUIUtility.hotControl != 0;
			if (flag)
			{
				GUIUtility.hotControl = 0;
			}
			this.m_PendingPointerCapture[pointerId] = handler;
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00043322 File Offset: 0x00041522
		public void ReleasePointer(int pointerId)
		{
			this.m_PendingPointerCapture[pointerId] = null;
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00043330 File Offset: 0x00041530
		public void ReleasePointer(IEventHandler handler, int pointerId)
		{
			bool flag = handler == this.m_PendingPointerCapture[pointerId];
			if (flag)
			{
				this.m_PendingPointerCapture[pointerId] = null;
			}
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00043358 File Offset: 0x00041558
		public void ProcessPointerCapture(int pointerId)
		{
			bool flag = this.m_PointerCapture[pointerId] == this.m_PendingPointerCapture[pointerId];
			if (!flag)
			{
				bool flag2 = this.m_PointerCapture[pointerId] != null;
				if (flag2)
				{
					using (PointerCaptureOutEvent pooled = PointerCaptureEventBase<PointerCaptureOutEvent>.GetPooled(this.m_PointerCapture[pointerId], this.m_PendingPointerCapture[pointerId], pointerId))
					{
						this.m_PointerCapture[pointerId].SendEvent(pooled);
					}
					bool flag3 = pointerId == PointerId.mousePointerId;
					if (flag3)
					{
						using (MouseCaptureOutEvent pooled2 = PointerCaptureEventBase<MouseCaptureOutEvent>.GetPooled(this.m_PointerCapture[pointerId], this.m_PendingPointerCapture[pointerId], pointerId))
						{
							this.m_PointerCapture[pointerId].SendEvent(pooled2);
						}
					}
				}
				bool flag4 = this.m_PendingPointerCapture[pointerId] != null;
				if (flag4)
				{
					using (PointerCaptureEvent pooled3 = PointerCaptureEventBase<PointerCaptureEvent>.GetPooled(this.m_PendingPointerCapture[pointerId], this.m_PointerCapture[pointerId], pointerId))
					{
						this.m_PendingPointerCapture[pointerId].SendEvent(pooled3);
					}
					bool flag5 = pointerId == PointerId.mousePointerId;
					if (flag5)
					{
						using (MouseCaptureEvent pooled4 = PointerCaptureEventBase<MouseCaptureEvent>.GetPooled(this.m_PendingPointerCapture[pointerId], this.m_PointerCapture[pointerId], pointerId))
						{
							this.m_PendingPointerCapture[pointerId].SendEvent(pooled4);
						}
					}
				}
				this.m_PointerCapture[pointerId] = this.m_PendingPointerCapture[pointerId];
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000434EC File Offset: 0x000416EC
		public void ActivateCompatibilityMouseEvents(int pointerId)
		{
			this.m_ShouldSendCompatibilityMouseEvents[pointerId] = true;
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000434F8 File Offset: 0x000416F8
		public void PreventCompatibilityMouseEvents(int pointerId)
		{
			this.m_ShouldSendCompatibilityMouseEvents[pointerId] = false;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00043504 File Offset: 0x00041704
		public bool ShouldSendCompatibilityMouseEvents(IPointerEvent evt)
		{
			return evt.isPrimary && this.m_ShouldSendCompatibilityMouseEvents[evt.pointerId];
		}

		// Token: 0x040008B9 RID: 2233
		private IEventHandler[] m_PendingPointerCapture = new IEventHandler[PointerId.maxPointers];

		// Token: 0x040008BA RID: 2234
		private IEventHandler[] m_PointerCapture = new IEventHandler[PointerId.maxPointers];

		// Token: 0x040008BB RID: 2235
		private bool[] m_ShouldSendCompatibilityMouseEvents = new bool[PointerId.maxPointers];
	}
}
