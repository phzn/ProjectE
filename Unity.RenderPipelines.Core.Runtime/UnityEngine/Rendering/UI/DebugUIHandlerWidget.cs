using System;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200013E RID: 318
	public class DebugUIHandlerWidget : MonoBehaviour
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002BB34 File Offset: 0x00029D34
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0002BB3C File Offset: 0x00029D3C
		public DebugUIHandlerWidget parentUIHandler { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0002BB45 File Offset: 0x00029D45
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0002BB4D File Offset: 0x00029D4D
		public DebugUIHandlerWidget previousUIHandler { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0002BB56 File Offset: 0x00029D56
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0002BB5E File Offset: 0x00029D5E
		public DebugUIHandlerWidget nextUIHandler { get; set; }

		// Token: 0x06000993 RID: 2451 RVA: 0x0002BB67 File Offset: 0x00029D67
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002BB69 File Offset: 0x00029D69
		internal virtual void SetWidget(DebugUI.Widget widget)
		{
			this.m_Widget = widget;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002BB72 File Offset: 0x00029D72
		internal DebugUI.Widget GetWidget()
		{
			return this.m_Widget;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002BB7C File Offset: 0x00029D7C
		protected T CastWidget<T>() where T : DebugUI.Widget
		{
			T t = this.m_Widget as T;
			string text = ((this.m_Widget == null) ? "null" : this.m_Widget.GetType().ToString());
			if (t == null)
			{
				string text2 = "Can't cast ";
				string text3 = text;
				string text4 = " to ";
				Type typeFromHandle = typeof(T);
				throw new InvalidOperationException(text2 + text3 + text4 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
			}
			return t;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002BBED File Offset: 0x00029DED
		public virtual bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			return true;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002BBF0 File Offset: 0x00029DF0
		public virtual void OnDeselection()
		{
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002BBF2 File Offset: 0x00029DF2
		public virtual void OnAction()
		{
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002BBF4 File Offset: 0x00029DF4
		public virtual void OnIncrement(bool fast)
		{
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002BBF6 File Offset: 0x00029DF6
		public virtual void OnDecrement(bool fast)
		{
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002BBF8 File Offset: 0x00029DF8
		public virtual DebugUIHandlerWidget Previous()
		{
			if (this.previousUIHandler != null)
			{
				return this.previousUIHandler;
			}
			if (this.parentUIHandler != null)
			{
				return this.parentUIHandler;
			}
			return null;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002BC28 File Offset: 0x00029E28
		public virtual DebugUIHandlerWidget Next()
		{
			if (this.nextUIHandler != null)
			{
				return this.nextUIHandler;
			}
			if (this.parentUIHandler != null)
			{
				DebugUIHandlerWidget debugUIHandlerWidget = this.parentUIHandler;
				while (debugUIHandlerWidget != null)
				{
					DebugUIHandlerWidget nextUIHandler = debugUIHandlerWidget.nextUIHandler;
					if (nextUIHandler != null)
					{
						return nextUIHandler;
					}
					debugUIHandlerWidget = debugUIHandlerWidget.parentUIHandler;
				}
			}
			return null;
		}

		// Token: 0x04000588 RID: 1416
		[HideInInspector]
		public Color colorDefault = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x04000589 RID: 1417
		[HideInInspector]
		public Color colorSelected = new Color(0.25f, 0.65f, 0.8f, 1f);

		// Token: 0x0400058D RID: 1421
		protected DebugUI.Widget m_Widget;
	}
}
