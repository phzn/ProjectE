using System;
using UnityEngine.EventSystems;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x02000090 RID: 144
	internal struct PointerModel
	{
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0003E0F9 File Offset: 0x0003C2F9
		public UIPointerType pointerType
		{
			get
			{
				return this.eventData.pointerType;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0003E106 File Offset: 0x0003C306
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x0003E10E File Offset: 0x0003C30E
		public Vector2 screenPosition
		{
			get
			{
				return this.m_ScreenPosition;
			}
			set
			{
				if (this.m_ScreenPosition != value)
				{
					this.m_ScreenPosition = value;
					this.changedThisFrame = true;
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0003E12C File Offset: 0x0003C32C
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x0003E134 File Offset: 0x0003C334
		public Vector3 worldPosition
		{
			get
			{
				return this.m_WorldPosition;
			}
			set
			{
				if (this.m_WorldPosition != value)
				{
					this.m_WorldPosition = value;
					this.changedThisFrame = true;
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003E152 File Offset: 0x0003C352
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x0003E15A File Offset: 0x0003C35A
		public Quaternion worldOrientation
		{
			get
			{
				return this.m_WorldOrientation;
			}
			set
			{
				if (this.m_WorldOrientation != value)
				{
					this.m_WorldOrientation = value;
					this.changedThisFrame = true;
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0003E178 File Offset: 0x0003C378
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0003E180 File Offset: 0x0003C380
		public Vector2 scrollDelta
		{
			get
			{
				return this.m_ScrollDelta;
			}
			set
			{
				if (this.m_ScrollDelta != value)
				{
					this.changedThisFrame = true;
					this.m_ScrollDelta = value;
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0003E19E File Offset: 0x0003C39E
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0003E1A6 File Offset: 0x0003C3A6
		public float pressure
		{
			get
			{
				return this.m_Pressure;
			}
			set
			{
				if (this.m_Pressure != value)
				{
					this.changedThisFrame = true;
					this.m_Pressure = value;
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003E1BF File Offset: 0x0003C3BF
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0003E1C7 File Offset: 0x0003C3C7
		public float azimuthAngle
		{
			get
			{
				return this.m_AzimuthAngle;
			}
			set
			{
				if (this.m_AzimuthAngle != value)
				{
					this.changedThisFrame = true;
					this.m_AzimuthAngle = value;
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0003E1E0 File Offset: 0x0003C3E0
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0003E1E8 File Offset: 0x0003C3E8
		public float altitudeAngle
		{
			get
			{
				return this.m_AltitudeAngle;
			}
			set
			{
				if (this.m_AltitudeAngle != value)
				{
					this.changedThisFrame = true;
					this.m_AltitudeAngle = value;
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0003E201 File Offset: 0x0003C401
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x0003E209 File Offset: 0x0003C409
		public float twist
		{
			get
			{
				return this.m_Twist;
			}
			set
			{
				if (this.m_Twist != value)
				{
					this.changedThisFrame = true;
					this.m_Twist = value;
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0003E222 File Offset: 0x0003C422
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x0003E22A File Offset: 0x0003C42A
		public Vector2 radius
		{
			get
			{
				return this.m_Radius;
			}
			set
			{
				if (this.m_Radius != value)
				{
					this.changedThisFrame = true;
					this.m_Radius = value;
				}
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0003E248 File Offset: 0x0003C448
		public PointerModel(ExtendedPointerEventData eventData)
		{
			this.eventData = eventData;
			this.changedThisFrame = false;
			this.leftButton = default(PointerModel.ButtonState);
			this.leftButton.OnEndFrame();
			this.rightButton = default(PointerModel.ButtonState);
			this.rightButton.OnEndFrame();
			this.middleButton = default(PointerModel.ButtonState);
			this.middleButton.OnEndFrame();
			this.m_ScreenPosition = default(Vector2);
			this.m_ScrollDelta = default(Vector2);
			this.m_WorldOrientation = default(Quaternion);
			this.m_WorldPosition = default(Vector3);
			this.m_Pressure = 0f;
			this.m_AzimuthAngle = 0f;
			this.m_AltitudeAngle = 0f;
			this.m_Twist = 0f;
			this.m_Radius = default(Vector2);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0003E310 File Offset: 0x0003C510
		public void OnFrameFinished()
		{
			this.changedThisFrame = false;
			this.scrollDelta = default(Vector2);
			this.leftButton.OnEndFrame();
			this.rightButton.OnEndFrame();
			this.middleButton.OnEndFrame();
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0003E354 File Offset: 0x0003C554
		public void CopyTouchOrPenStateFrom(PointerEventData eventData)
		{
			this.pressure = eventData.pressure;
			this.azimuthAngle = eventData.azimuthAngle;
			this.altitudeAngle = eventData.altitudeAngle;
			this.twist = eventData.twist;
			this.radius = eventData.radius;
		}

		// Token: 0x04000428 RID: 1064
		public bool changedThisFrame;

		// Token: 0x04000429 RID: 1065
		public PointerModel.ButtonState leftButton;

		// Token: 0x0400042A RID: 1066
		public PointerModel.ButtonState rightButton;

		// Token: 0x0400042B RID: 1067
		public PointerModel.ButtonState middleButton;

		// Token: 0x0400042C RID: 1068
		public ExtendedPointerEventData eventData;

		// Token: 0x0400042D RID: 1069
		private Vector2 m_ScreenPosition;

		// Token: 0x0400042E RID: 1070
		private Vector2 m_ScrollDelta;

		// Token: 0x0400042F RID: 1071
		private Vector3 m_WorldPosition;

		// Token: 0x04000430 RID: 1072
		private Quaternion m_WorldOrientation;

		// Token: 0x04000431 RID: 1073
		private float m_Pressure;

		// Token: 0x04000432 RID: 1074
		private float m_AzimuthAngle;

		// Token: 0x04000433 RID: 1075
		private float m_AltitudeAngle;

		// Token: 0x04000434 RID: 1076
		private float m_Twist;

		// Token: 0x04000435 RID: 1077
		private Vector2 m_Radius;

		// Token: 0x020001D8 RID: 472
		public struct ButtonState
		{
			// Token: 0x17000587 RID: 1415
			// (get) Token: 0x06001495 RID: 5269 RVA: 0x0005F71E File Offset: 0x0005D91E
			// (set) Token: 0x06001496 RID: 5270 RVA: 0x0005F728 File Offset: 0x0005D928
			public bool isPressed
			{
				get
				{
					return this.m_IsPressed;
				}
				set
				{
					if (this.m_IsPressed != value)
					{
						this.m_IsPressed = value;
						if (this.m_FramePressState == PointerEventData.FramePressState.NotChanged && value)
						{
							this.m_FramePressState = PointerEventData.FramePressState.Pressed;
							return;
						}
						if (this.m_FramePressState == PointerEventData.FramePressState.NotChanged && !value)
						{
							this.m_FramePressState = PointerEventData.FramePressState.Released;
							return;
						}
						if (this.m_FramePressState == PointerEventData.FramePressState.Pressed && !value)
						{
							this.m_FramePressState = PointerEventData.FramePressState.PressedAndReleased;
						}
					}
				}
			}

			// Token: 0x17000588 RID: 1416
			// (get) Token: 0x06001497 RID: 5271 RVA: 0x0005F780 File Offset: 0x0005D980
			// (set) Token: 0x06001498 RID: 5272 RVA: 0x0005F788 File Offset: 0x0005D988
			public bool ignoreNextClick
			{
				get
				{
					return this.m_IgnoreNextClick;
				}
				set
				{
					this.m_IgnoreNextClick = value;
				}
			}

			// Token: 0x17000589 RID: 1417
			// (get) Token: 0x06001499 RID: 5273 RVA: 0x0005F791 File Offset: 0x0005D991
			// (set) Token: 0x0600149A RID: 5274 RVA: 0x0005F799 File Offset: 0x0005D999
			public float pressTime
			{
				get
				{
					return this.m_PressTime;
				}
				set
				{
					this.m_PressTime = value;
				}
			}

			// Token: 0x1700058A RID: 1418
			// (get) Token: 0x0600149B RID: 5275 RVA: 0x0005F7A2 File Offset: 0x0005D9A2
			// (set) Token: 0x0600149C RID: 5276 RVA: 0x0005F7AA File Offset: 0x0005D9AA
			public bool clickedOnSameGameObject
			{
				get
				{
					return this.m_ClickedOnSameGameObject;
				}
				set
				{
					this.m_ClickedOnSameGameObject = value;
				}
			}

			// Token: 0x1700058B RID: 1419
			// (get) Token: 0x0600149D RID: 5277 RVA: 0x0005F7B3 File Offset: 0x0005D9B3
			public bool wasPressedThisFrame
			{
				get
				{
					return this.m_FramePressState == PointerEventData.FramePressState.Pressed || this.m_FramePressState == PointerEventData.FramePressState.PressedAndReleased;
				}
			}

			// Token: 0x1700058C RID: 1420
			// (get) Token: 0x0600149E RID: 5278 RVA: 0x0005F7C8 File Offset: 0x0005D9C8
			public bool wasReleasedThisFrame
			{
				get
				{
					return this.m_FramePressState == PointerEventData.FramePressState.Released || this.m_FramePressState == PointerEventData.FramePressState.PressedAndReleased;
				}
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x0005F7E0 File Offset: 0x0005D9E0
			public void CopyPressStateTo(PointerEventData eventData)
			{
				eventData.pointerPressRaycast = this.m_PressRaycast;
				eventData.pressPosition = this.m_PressPosition;
				eventData.clickCount = this.m_ClickCount;
				eventData.clickTime = this.m_ClickTime;
				eventData.pointerPress = this.m_LastPressObject;
				eventData.pointerPress = this.m_PressObject;
				eventData.rawPointerPress = this.m_RawPressObject;
				eventData.pointerDrag = this.m_DragObject;
				eventData.dragging = this.m_Dragging;
				if (this.ignoreNextClick)
				{
					eventData.eligibleForClick = false;
				}
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x0005F868 File Offset: 0x0005DA68
			public void CopyPressStateFrom(PointerEventData eventData)
			{
				this.m_PressRaycast = eventData.pointerPressRaycast;
				this.m_PressObject = eventData.pointerPress;
				this.m_RawPressObject = eventData.rawPointerPress;
				this.m_LastPressObject = eventData.lastPress;
				this.m_PressPosition = eventData.pressPosition;
				this.m_ClickTime = eventData.clickTime;
				this.m_ClickCount = eventData.clickCount;
				this.m_DragObject = eventData.pointerDrag;
				this.m_Dragging = eventData.dragging;
			}

			// Token: 0x060014A1 RID: 5281 RVA: 0x0005F8E1 File Offset: 0x0005DAE1
			public void OnEndFrame()
			{
				this.m_FramePressState = PointerEventData.FramePressState.NotChanged;
			}

			// Token: 0x040009BE RID: 2494
			private bool m_IsPressed;

			// Token: 0x040009BF RID: 2495
			private PointerEventData.FramePressState m_FramePressState;

			// Token: 0x040009C0 RID: 2496
			private float m_PressTime;

			// Token: 0x040009C1 RID: 2497
			private RaycastResult m_PressRaycast;

			// Token: 0x040009C2 RID: 2498
			private GameObject m_PressObject;

			// Token: 0x040009C3 RID: 2499
			private GameObject m_RawPressObject;

			// Token: 0x040009C4 RID: 2500
			private GameObject m_LastPressObject;

			// Token: 0x040009C5 RID: 2501
			private GameObject m_DragObject;

			// Token: 0x040009C6 RID: 2502
			private Vector2 m_PressPosition;

			// Token: 0x040009C7 RID: 2503
			private float m_ClickTime;

			// Token: 0x040009C8 RID: 2504
			private int m_ClickCount;

			// Token: 0x040009C9 RID: 2505
			private bool m_Dragging;

			// Token: 0x040009CA RID: 2506
			private bool m_ClickedOnSameGameObject;

			// Token: 0x040009CB RID: 2507
			private bool m_IgnoreNextClick;
		}
	}
}
