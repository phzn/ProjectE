using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000066 RID: 102
	[AddComponentMenu("Event/Event Trigger")]
	public class EventTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00019112 File Offset: 0x00017312
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x0001911A File Offset: 0x0001731A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
		public List<EventTrigger.Entry> delegates
		{
			get
			{
				return this.triggers;
			}
			set
			{
				this.triggers = value;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00019123 File Offset: 0x00017323
		protected EventTrigger()
		{
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001912B File Offset: 0x0001732B
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00019146 File Offset: 0x00017346
		public List<EventTrigger.Entry> triggers
		{
			get
			{
				if (this.m_Delegates == null)
				{
					this.m_Delegates = new List<EventTrigger.Entry>();
				}
				return this.m_Delegates;
			}
			set
			{
				this.m_Delegates = value;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00019150 File Offset: 0x00017350
		private void Execute(EventTriggerType id, BaseEventData eventData)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				EventTrigger.Entry entry = this.triggers[i];
				if (entry.eventID == id && entry.callback != null)
				{
					entry.callback.Invoke(eventData);
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001919D File Offset: 0x0001739D
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.PointerEnter, eventData);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000191A7 File Offset: 0x000173A7
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.PointerExit, eventData);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000191B1 File Offset: 0x000173B1
		public virtual void OnDrag(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.Drag, eventData);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000191BB File Offset: 0x000173BB
		public virtual void OnDrop(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.Drop, eventData);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000191C5 File Offset: 0x000173C5
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.PointerDown, eventData);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000191CF File Offset: 0x000173CF
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.PointerUp, eventData);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000191D9 File Offset: 0x000173D9
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.PointerClick, eventData);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000191E3 File Offset: 0x000173E3
		public virtual void OnSelect(BaseEventData eventData)
		{
			this.Execute(EventTriggerType.Select, eventData);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000191EE File Offset: 0x000173EE
		public virtual void OnDeselect(BaseEventData eventData)
		{
			this.Execute(EventTriggerType.Deselect, eventData);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000191F9 File Offset: 0x000173F9
		public virtual void OnScroll(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.Scroll, eventData);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00019203 File Offset: 0x00017403
		public virtual void OnMove(AxisEventData eventData)
		{
			this.Execute(EventTriggerType.Move, eventData);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001920E File Offset: 0x0001740E
		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			this.Execute(EventTriggerType.UpdateSelected, eventData);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00019218 File Offset: 0x00017418
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.InitializePotentialDrag, eventData);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00019223 File Offset: 0x00017423
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.BeginDrag, eventData);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001922E File Offset: 0x0001742E
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			this.Execute(EventTriggerType.EndDrag, eventData);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00019239 File Offset: 0x00017439
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Execute(EventTriggerType.Submit, eventData);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00019244 File Offset: 0x00017444
		public virtual void OnCancel(BaseEventData eventData)
		{
			this.Execute(EventTriggerType.Cancel, eventData);
		}

		// Token: 0x040001E3 RID: 483
		[FormerlySerializedAs("delegates")]
		[SerializeField]
		private List<EventTrigger.Entry> m_Delegates;

		// Token: 0x020000C5 RID: 197
		[Serializable]
		public class TriggerEvent : UnityEvent<BaseEventData>
		{
		}

		// Token: 0x020000C6 RID: 198
		[Serializable]
		public class Entry
		{
			// Token: 0x04000353 RID: 851
			public EventTriggerType eventID = EventTriggerType.PointerClick;

			// Token: 0x04000354 RID: 852
			public EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
		}
	}
}
