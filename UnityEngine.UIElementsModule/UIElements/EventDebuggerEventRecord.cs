using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020001BD RID: 445
	[Serializable]
	internal class EventDebuggerEventRecord
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0003580D File Offset: 0x00033A0D
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00035815 File Offset: 0x00033A15
		public string eventBaseName { get; private set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0003581E File Offset: 0x00033A1E
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00035826 File Offset: 0x00033A26
		public long eventTypeId { get; private set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0003582F File Offset: 0x00033A2F
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00035837 File Offset: 0x00033A37
		public ulong eventId { get; private set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00035840 File Offset: 0x00033A40
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x00035848 File Offset: 0x00033A48
		private ulong triggerEventId { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00035851 File Offset: 0x00033A51
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x00035859 File Offset: 0x00033A59
		internal long timestamp { get; private set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00035862 File Offset: 0x00033A62
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0003586A File Offset: 0x00033A6A
		public IEventHandler target { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00035873 File Offset: 0x00033A73
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x0003587B File Offset: 0x00033A7B
		private List<IEventHandler> skipElements { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00035884 File Offset: 0x00033A84
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x0003588C File Offset: 0x00033A8C
		public bool hasUnderlyingPhysicalEvent { get; private set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00035895 File Offset: 0x00033A95
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x0003589D File Offset: 0x00033A9D
		private bool isPropagationStopped { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x000358A6 File Offset: 0x00033AA6
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x000358AE File Offset: 0x00033AAE
		private bool isImmediatePropagationStopped { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x000358B7 File Offset: 0x00033AB7
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x000358BF File Offset: 0x00033ABF
		private bool isDefaultPrevented { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000358C8 File Offset: 0x00033AC8
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x000358D0 File Offset: 0x00033AD0
		public PropagationPhase propagationPhase { get; private set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000358D9 File Offset: 0x00033AD9
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x000358E1 File Offset: 0x00033AE1
		private IEventHandler currentTarget { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000358EA File Offset: 0x00033AEA
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000358F2 File Offset: 0x00033AF2
		private bool dispatch { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x000358FB File Offset: 0x00033AFB
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x00035903 File Offset: 0x00033B03
		private Vector2 originalMousePosition { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0003590C File Offset: 0x00033B0C
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00035914 File Offset: 0x00033B14
		public EventModifiers modifiers { get; private set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0003591D File Offset: 0x00033B1D
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00035925 File Offset: 0x00033B25
		public Vector2 mousePosition { get; private set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0003592E File Offset: 0x00033B2E
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00035936 File Offset: 0x00033B36
		public int clickCount { get; private set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0003593F File Offset: 0x00033B3F
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x00035947 File Offset: 0x00033B47
		public int button { get; private set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00035950 File Offset: 0x00033B50
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x00035958 File Offset: 0x00033B58
		public int pressedButtons { get; private set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x00035961 File Offset: 0x00033B61
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00035969 File Offset: 0x00033B69
		public Vector3 delta { get; private set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x00035972 File Offset: 0x00033B72
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x0003597A File Offset: 0x00033B7A
		public char character { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00035983 File Offset: 0x00033B83
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x0003598B File Offset: 0x00033B8B
		public KeyCode keyCode { get; private set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00035994 File Offset: 0x00033B94
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0003599C File Offset: 0x00033B9C
		public string commandName { get; private set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x000359A5 File Offset: 0x00033BA5
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x000359AD File Offset: 0x00033BAD
		public NavigationDeviceType deviceType { get; private set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x000359B6 File Offset: 0x00033BB6
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x000359BE File Offset: 0x00033BBE
		public NavigationMoveEvent.Direction navigationDirection { get; private set; }

		// Token: 0x06000DBE RID: 3518 RVA: 0x000359C8 File Offset: 0x00033BC8
		private void Init(EventBase evt)
		{
			Type type = evt.GetType();
			this.eventBaseName = EventDebugger.GetTypeDisplayName(type);
			this.eventTypeId = evt.eventTypeId;
			this.eventId = evt.eventId;
			this.triggerEventId = evt.triggerEventId;
			this.timestamp = evt.timestamp;
			this.target = evt.target;
			this.skipElements = evt.skipElements;
			this.isPropagationStopped = evt.isPropagationStopped;
			this.isImmediatePropagationStopped = evt.isImmediatePropagationStopped;
			this.isDefaultPrevented = evt.isDefaultPrevented;
			IMouseEvent mouseEvent = evt as IMouseEvent;
			IMouseEventInternal mouseEventInternal = evt as IMouseEventInternal;
			this.hasUnderlyingPhysicalEvent = mouseEvent != null && mouseEventInternal != null && mouseEventInternal.triggeredByOS;
			this.propagationPhase = evt.propagationPhase;
			this.originalMousePosition = evt.originalMousePosition;
			this.currentTarget = evt.currentTarget;
			this.dispatch = evt.dispatch;
			bool flag = mouseEvent != null;
			if (flag)
			{
				this.modifiers = mouseEvent.modifiers;
				this.mousePosition = mouseEvent.mousePosition;
				this.button = mouseEvent.button;
				this.pressedButtons = mouseEvent.pressedButtons;
				this.clickCount = mouseEvent.clickCount;
				WheelEvent wheelEvent = mouseEvent as WheelEvent;
				bool flag2 = wheelEvent != null;
				if (flag2)
				{
					this.delta = wheelEvent.delta;
				}
			}
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag3 = pointerEvent != null;
			if (flag3)
			{
				IPointerEventInternal pointerEventInternal = evt as IPointerEventInternal;
				this.hasUnderlyingPhysicalEvent = pointerEvent != null && pointerEventInternal != null && pointerEventInternal.triggeredByOS;
				this.modifiers = pointerEvent.modifiers;
				this.mousePosition = pointerEvent.position;
				this.button = pointerEvent.button;
				this.pressedButtons = pointerEvent.pressedButtons;
				this.clickCount = pointerEvent.clickCount;
			}
			IKeyboardEvent keyboardEvent = evt as IKeyboardEvent;
			bool flag4 = keyboardEvent != null;
			if (flag4)
			{
				this.modifiers = keyboardEvent.modifiers;
				this.character = keyboardEvent.character;
				this.keyCode = keyboardEvent.keyCode;
			}
			ICommandEvent commandEvent = evt as ICommandEvent;
			bool flag5 = commandEvent != null;
			if (flag5)
			{
				this.commandName = commandEvent.commandName;
			}
			INavigationEvent navigationEvent = evt as INavigationEvent;
			bool flag6 = navigationEvent != null;
			if (flag6)
			{
				this.deviceType = navigationEvent.deviceType;
				NavigationMoveEvent navigationMoveEvent = evt as NavigationMoveEvent;
				bool flag7 = navigationMoveEvent != null;
				if (flag7)
				{
					this.navigationDirection = navigationMoveEvent.direction;
				}
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00035C48 File Offset: 0x00033E48
		public EventDebuggerEventRecord(EventBase evt)
		{
			this.Init(evt);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00035C5C File Offset: 0x00033E5C
		public string TimestampString()
		{
			long num = (long)((float)this.timestamp / 1000f * 10000000f);
			return new DateTime(num).ToString("HH:mm:ss.ffffff");
		}
	}
}
