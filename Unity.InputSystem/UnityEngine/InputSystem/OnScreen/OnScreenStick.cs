using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.OnScreen
{
	// Token: 0x02000099 RID: 153
	[AddComponentMenu("Input/On-Screen Stick")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/OnScreen.html#on-screen-sticks")]
	public class OnScreenStick : OnScreenControl, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x00040101 File Offset: 0x0003E301
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.m_UseIsolatedInputActions)
			{
				return;
			}
			if (eventData == null)
			{
				throw new ArgumentNullException("eventData");
			}
			this.BeginInteraction(eventData.position, eventData.pressEventCamera);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004012C File Offset: 0x0003E32C
		public void OnDrag(PointerEventData eventData)
		{
			if (this.m_UseIsolatedInputActions)
			{
				return;
			}
			if (eventData == null)
			{
				throw new ArgumentNullException("eventData");
			}
			this.MoveStick(eventData.position, eventData.pressEventCamera);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00040157 File Offset: 0x0003E357
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.m_UseIsolatedInputActions)
			{
				return;
			}
			this.EndInteraction();
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00040168 File Offset: 0x0003E368
		private void Start()
		{
			if (this.m_UseIsolatedInputActions)
			{
				this.m_RaycastResults = new List<RaycastResult>();
				this.m_PointerEventData = new PointerEventData(EventSystem.current);
				if (this.m_PointerDownAction == null || this.m_PointerDownAction.bindings.Count == 0)
				{
					if (this.m_PointerDownAction == null)
					{
						this.m_PointerDownAction = new InputAction(null, InputActionType.PassThrough, null, null, null, null);
					}
					else if (this.m_PointerDownAction.m_Type != InputActionType.PassThrough)
					{
						this.m_PointerDownAction.m_Type = InputActionType.PassThrough;
					}
					this.m_PointerDownAction.AddBinding("<Mouse>/leftButton", null, null, null);
					this.m_PointerDownAction.AddBinding("<Pen>/tip", null, null, null);
					this.m_PointerDownAction.AddBinding("<Touchscreen>/touch*/press", null, null, null);
					this.m_PointerDownAction.AddBinding("<XRController>/trigger", null, null, null);
				}
				if (this.m_PointerMoveAction == null || this.m_PointerMoveAction.bindings.Count == 0)
				{
					if (this.m_PointerMoveAction == null)
					{
						this.m_PointerMoveAction = new InputAction();
					}
					this.m_PointerMoveAction.AddBinding("<Mouse>/position", null, null, null);
					this.m_PointerMoveAction.AddBinding("<Pen>/position", null, null, null);
					this.m_PointerMoveAction.AddBinding("<Touchscreen>/touch*/position", null, null, null);
				}
				this.m_PointerDownAction.performed += this.OnPointerChanged;
				this.m_PointerDownAction.Enable();
				this.m_PointerMoveAction.Enable();
			}
			if (!(base.transform is RectTransform))
			{
				return;
			}
			this.m_StartPos = ((RectTransform)base.transform).anchoredPosition;
			if (this.m_Behaviour != OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin)
			{
				return;
			}
			this.m_PointerDownPos = this.m_StartPos;
			GameObject gameObject = new GameObject("DynamicOriginClickable", new Type[] { typeof(Image) });
			gameObject.transform.SetParent(base.transform);
			Image component = gameObject.GetComponent<Image>();
			component.color = new Color(1f, 1f, 1f, 0f);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.sizeDelta = new Vector2(this.m_DynamicOriginRange * 2f, this.m_DynamicOriginRange * 2f);
			rectTransform.localScale = new Vector3(1f, 1f, 0f);
			rectTransform.anchoredPosition3D = Vector3.zero;
			component.sprite = SpriteUtilities.CreateCircleSprite(16, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			component.alphaHitTestMinimumThreshold = 0.5f;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000403F3 File Offset: 0x0003E5F3
		private void OnDestroy()
		{
			if (this.m_UseIsolatedInputActions)
			{
				this.m_PointerDownAction.performed -= this.OnPointerChanged;
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00040414 File Offset: 0x0003E614
		private void BeginInteraction(Vector2 pointerPosition, Camera uiCamera)
		{
			RectTransform canvasRectTransform = UGUIOnScreenControlUtils.GetCanvasRectTransform(base.transform);
			if (canvasRectTransform == null)
			{
				Debug.LogError(base.GetWarningMessage());
				return;
			}
			switch (this.m_Behaviour)
			{
			case OnScreenStick.Behaviour.RelativePositionWithStaticOrigin:
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPosition, uiCamera, out this.m_PointerDownPos);
				return;
			case OnScreenStick.Behaviour.ExactPositionWithStaticOrigin:
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPosition, uiCamera, out this.m_PointerDownPos);
				this.MoveStick(pointerPosition, uiCamera);
				return;
			case OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin:
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPosition, uiCamera, out vector);
				this.m_PointerDownPos = (((RectTransform)base.transform).anchoredPosition = vector);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000404AC File Offset: 0x0003E6AC
		private void MoveStick(Vector2 pointerPosition, Camera uiCamera)
		{
			RectTransform canvasRectTransform = UGUIOnScreenControlUtils.GetCanvasRectTransform(base.transform);
			if (canvasRectTransform == null)
			{
				Debug.LogError(base.GetWarningMessage());
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPosition, uiCamera, out vector);
			Vector2 vector2 = vector - this.m_PointerDownPos;
			switch (this.m_Behaviour)
			{
			case OnScreenStick.Behaviour.RelativePositionWithStaticOrigin:
				vector2 = Vector2.ClampMagnitude(vector2, this.movementRange);
				((RectTransform)base.transform).anchoredPosition = this.m_StartPos + vector2;
				break;
			case OnScreenStick.Behaviour.ExactPositionWithStaticOrigin:
				vector2 = vector - this.m_StartPos;
				vector2 = Vector2.ClampMagnitude(vector2, this.movementRange);
				((RectTransform)base.transform).anchoredPosition = this.m_StartPos + vector2;
				break;
			case OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin:
				vector2 = Vector2.ClampMagnitude(vector2, this.movementRange);
				((RectTransform)base.transform).anchoredPosition = this.m_PointerDownPos + vector2;
				break;
			}
			Vector2 vector3 = new Vector2(vector2.x / this.movementRange, vector2.y / this.movementRange);
			base.SendValueToControl<Vector2>(vector3);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000405D8 File Offset: 0x0003E7D8
		private void EndInteraction()
		{
			((RectTransform)base.transform).anchoredPosition = (this.m_PointerDownPos = this.m_StartPos);
			base.SendValueToControl<Vector2>(Vector2.zero);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00040614 File Offset: 0x0003E814
		private void OnPointerDown(InputAction.CallbackContext ctx)
		{
			if (this.m_IsIsolationActive)
			{
				return;
			}
			Vector2 vector = Vector2.zero;
			TouchControl touchControl = null;
			InputControl control = ctx.control;
			TouchControl touchControl2 = ((control != null) ? control.parent : null) as TouchControl;
			if (touchControl2 != null)
			{
				touchControl = touchControl2;
				vector = touchControl2.position.ReadValue();
			}
			else
			{
				InputControl control2 = ctx.control;
				Pointer pointer = ((control2 != null) ? control2.device : null) as Pointer;
				if (pointer != null)
				{
					vector = pointer.position.ReadValue();
				}
			}
			this.m_PointerEventData.position = vector;
			EventSystem.current.RaycastAll(this.m_PointerEventData, this.m_RaycastResults);
			if (this.m_RaycastResults.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (RaycastResult raycastResult in this.m_RaycastResults)
			{
				if (!(raycastResult.gameObject != base.gameObject))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			this.BeginInteraction(vector, this.GetCameraFromCanvas());
			if (touchControl != null)
			{
				this.m_TouchControl = touchControl;
				this.m_PointerMoveAction.ApplyBindingOverride(touchControl.path + "/position", null, "<Touchscreen>/touch*/position");
			}
			this.m_PointerMoveAction.performed += this.OnPointerMove;
			this.m_IsIsolationActive = true;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00040770 File Offset: 0x0003E970
		private void OnPointerChanged(InputAction.CallbackContext ctx)
		{
			if (ctx.control.IsPressed(0f))
			{
				this.OnPointerDown(ctx);
				return;
			}
			this.OnPointerUp(ctx);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00040794 File Offset: 0x0003E994
		private void OnPointerMove(InputAction.CallbackContext ctx)
		{
			Vector2 vector;
			if (this.m_TouchControl != null)
			{
				if (!this.m_TouchControl.isInProgress)
				{
					return;
				}
				vector = this.m_TouchControl.position.ReadValue();
			}
			else
			{
				vector = ((Pointer)ctx.control.device).position.ReadValue();
			}
			this.MoveStick(vector, this.GetCameraFromCanvas());
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000407F4 File Offset: 0x0003E9F4
		private void OnPointerUp(InputAction.CallbackContext ctx)
		{
			if (!this.m_IsIsolationActive)
			{
				return;
			}
			if (this.m_TouchControl != null)
			{
				if (this.m_TouchControl.isInProgress)
				{
					return;
				}
				this.m_PointerMoveAction.ApplyBindingOverride(null, null, "<Touchscreen>/touch*/position");
				this.m_TouchControl = null;
			}
			this.EndInteraction();
			this.m_PointerMoveAction.performed -= this.OnPointerMove;
			this.m_IsIsolationActive = false;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00040860 File Offset: 0x0003EA60
		private Camera GetCameraFromCanvas()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			RenderMode? renderMode = ((componentInParent != null) ? new RenderMode?(componentInParent.renderMode) : null);
			RenderMode? renderMode2 = renderMode;
			RenderMode renderMode3 = RenderMode.ScreenSpaceOverlay;
			if (!((renderMode2.GetValueOrDefault() == renderMode3) & (renderMode2 != null)))
			{
				renderMode2 = renderMode;
				renderMode3 = RenderMode.ScreenSpaceCamera;
				if (!((renderMode2.GetValueOrDefault() == renderMode3) & (renderMode2 != null)) || !(((componentInParent != null) ? componentInParent.worldCamera : null) == null))
				{
					return ((componentInParent != null) ? componentInParent.worldCamera : null) ?? Camera.main;
				}
			}
			return null;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000408EC File Offset: 0x0003EAEC
		private void OnDrawGizmosSelected()
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Gizmos.matrix = rectTransform.localToWorldMatrix;
			Vector2 vector = rectTransform.anchoredPosition;
			if (Application.isPlaying)
			{
				vector = this.m_StartPos;
			}
			Gizmos.color = new Color32(84, 173, 219, byte.MaxValue);
			Vector2 vector2 = vector;
			if (Application.isPlaying && this.m_Behaviour == OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin)
			{
				vector2 = this.m_PointerDownPos;
			}
			this.DrawGizmoCircle(vector2, this.m_MovementRange);
			if (this.m_Behaviour != OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin)
			{
				return;
			}
			Gizmos.color = new Color32(158, 84, 219, byte.MaxValue);
			this.DrawGizmoCircle(vector, this.m_DynamicOriginRange);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000409B8 File Offset: 0x0003EBB8
		private void DrawGizmoCircle(Vector2 center, float radius)
		{
			for (int i = 0; i < 32; i++)
			{
				float num = (float)i / 32f * 3.1415927f * 2f;
				float num2 = (float)(i + 1) / 32f * 3.1415927f * 2f;
				Gizmos.DrawLine(new Vector3(center.x + Mathf.Cos(num) * radius, center.y + Mathf.Sin(num) * radius, 0f), new Vector3(center.x + Mathf.Cos(num2) * radius, center.y + Mathf.Sin(num2) * radius, 0f));
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00040A5C File Offset: 0x0003EC5C
		private void UpdateDynamicOriginClickableArea()
		{
			Transform transform = base.transform.Find("DynamicOriginClickable");
			if (transform)
			{
				((RectTransform)transform).sizeDelta = new Vector2(this.m_DynamicOriginRange * 2f, this.m_DynamicOriginRange * 2f);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00040AAA File Offset: 0x0003ECAA
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00040AB2 File Offset: 0x0003ECB2
		public float movementRange
		{
			get
			{
				return this.m_MovementRange;
			}
			set
			{
				this.m_MovementRange = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00040ABB File Offset: 0x0003ECBB
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00040AC3 File Offset: 0x0003ECC3
		public float dynamicOriginRange
		{
			get
			{
				return this.m_DynamicOriginRange;
			}
			set
			{
				if (this.m_DynamicOriginRange != value)
				{
					this.m_DynamicOriginRange = value;
					this.UpdateDynamicOriginClickableArea();
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00040ADB File Offset: 0x0003ECDB
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00040AE3 File Offset: 0x0003ECE3
		public bool useIsolatedInputActions
		{
			get
			{
				return this.m_UseIsolatedInputActions;
			}
			set
			{
				this.m_UseIsolatedInputActions = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00040AEC File Offset: 0x0003ECEC
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x00040AF4 File Offset: 0x0003ECF4
		protected override string controlPathInternal
		{
			get
			{
				return this.m_ControlPath;
			}
			set
			{
				this.m_ControlPath = value;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00040AFD File Offset: 0x0003ECFD
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x00040B05 File Offset: 0x0003ED05
		public OnScreenStick.Behaviour behaviour
		{
			get
			{
				return this.m_Behaviour;
			}
			set
			{
				this.m_Behaviour = value;
			}
		}

		// Token: 0x04000466 RID: 1126
		private const string kDynamicOriginClickable = "DynamicOriginClickable";

		// Token: 0x04000467 RID: 1127
		[FormerlySerializedAs("movementRange")]
		[SerializeField]
		[Min(0f)]
		private float m_MovementRange = 50f;

		// Token: 0x04000468 RID: 1128
		[SerializeField]
		[Tooltip("Defines the circular region where the onscreen control may have it's origin placed.")]
		[Min(0f)]
		private float m_DynamicOriginRange = 100f;

		// Token: 0x04000469 RID: 1129
		[InputControl(layout = "Vector2")]
		[SerializeField]
		private string m_ControlPath;

		// Token: 0x0400046A RID: 1130
		[SerializeField]
		[Tooltip("Choose how the onscreen stick will move relative to it's origin and the press position.\n\nRelativePositionWithStaticOrigin: The control's center of origin is fixed. The control will begin un-actuated at it's centered position and then move relative to the pointer or finger motion.\n\nExactPositionWithStaticOrigin: The control's center of origin is fixed. The stick will immediately jump to the exact position of the click or touch and begin tracking motion from there.\n\nExactPositionWithDynamicOrigin: The control's center of origin is determined by the initial press position. The stick will begin un-actuated at this center position and then track the current pointer or finger position.")]
		private OnScreenStick.Behaviour m_Behaviour;

		// Token: 0x0400046B RID: 1131
		[SerializeField]
		[Tooltip("Set this to true to prevent cancellation of pointer events due to device switching. Cancellation will appear as the stick jumping back and forth between the pointer position and the stick center.")]
		private bool m_UseIsolatedInputActions;

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		[Tooltip("The action that will be used to detect pointer down events on the stick control. Note that if no bindings are set, default ones will be provided.")]
		private InputAction m_PointerDownAction;

		// Token: 0x0400046D RID: 1133
		[SerializeField]
		[Tooltip("The action that will be used to detect pointer movement on the stick control. Note that if no bindings are set, default ones will be provided.")]
		private InputAction m_PointerMoveAction;

		// Token: 0x0400046E RID: 1134
		private Vector3 m_StartPos;

		// Token: 0x0400046F RID: 1135
		private Vector2 m_PointerDownPos;

		// Token: 0x04000470 RID: 1136
		[NonSerialized]
		private List<RaycastResult> m_RaycastResults;

		// Token: 0x04000471 RID: 1137
		[NonSerialized]
		private PointerEventData m_PointerEventData;

		// Token: 0x04000472 RID: 1138
		[NonSerialized]
		private TouchControl m_TouchControl;

		// Token: 0x04000473 RID: 1139
		[NonSerialized]
		private bool m_IsIsolationActive;

		// Token: 0x020001E5 RID: 485
		public enum Behaviour
		{
			// Token: 0x04000A1A RID: 2586
			RelativePositionWithStaticOrigin,
			// Token: 0x04000A1B RID: 2587
			ExactPositionWithStaticOrigin,
			// Token: 0x04000A1C RID: 2588
			ExactPositionWithDynamicOrigin
		}
	}
}
