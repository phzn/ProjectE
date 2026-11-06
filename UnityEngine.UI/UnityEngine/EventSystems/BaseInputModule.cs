using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200006A RID: 106
	[RequireComponent(typeof(EventSystem))]
	public abstract class BaseInputModule : UIBehaviour
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001982C File Offset: 0x00017A2C
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00019834 File Offset: 0x00017A34
		internal bool sendPointerHoverToParent
		{
			get
			{
				return this.m_SendPointerHoverToParent;
			}
			set
			{
				this.m_SendPointerHoverToParent = value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00019840 File Offset: 0x00017A40
		public BaseInput input
		{
			get
			{
				if (this.m_InputOverride != null)
				{
					return this.m_InputOverride;
				}
				if (this.m_DefaultInput == null)
				{
					foreach (BaseInput baseInput in base.GetComponents<BaseInput>())
					{
						if (baseInput != null && baseInput.GetType() == typeof(BaseInput))
						{
							this.m_DefaultInput = baseInput;
							break;
						}
					}
					if (this.m_DefaultInput == null)
					{
						this.m_DefaultInput = base.gameObject.AddComponent<BaseInput>();
					}
				}
				return this.m_DefaultInput;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x000198D7 File Offset: 0x00017AD7
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x000198DF File Offset: 0x00017ADF
		public BaseInput inputOverride
		{
			get
			{
				return this.m_InputOverride;
			}
			set
			{
				this.m_InputOverride = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x000198E8 File Offset: 0x00017AE8
		protected EventSystem eventSystem
		{
			get
			{
				return this.m_EventSystem;
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000198F0 File Offset: 0x00017AF0
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_EventSystem = base.GetComponent<EventSystem>();
			this.m_EventSystem.UpdateModules();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001990F File Offset: 0x00017B0F
		protected override void OnDisable()
		{
			this.m_EventSystem.UpdateModules();
			base.OnDisable();
		}

		// Token: 0x06000618 RID: 1560
		public abstract void Process();

		// Token: 0x06000619 RID: 1561 RVA: 0x00019924 File Offset: 0x00017B24
		protected static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			int count = candidates.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001996C File Offset: 0x00017B6C
		protected static MoveDirection DetermineMoveDirection(float x, float y)
		{
			return BaseInputModule.DetermineMoveDirection(x, y, 0.6f);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001997C File Offset: 0x00017B7C
		protected static MoveDirection DetermineMoveDirection(float x, float y, float deadZone)
		{
			if (new Vector2(x, y).sqrMagnitude < deadZone * deadZone)
			{
				return MoveDirection.None;
			}
			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				if (x <= 0f)
				{
					return MoveDirection.Left;
				}
				return MoveDirection.Right;
			}
			else
			{
				if (y <= 0f)
				{
					return MoveDirection.Down;
				}
				return MoveDirection.Up;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000199C4 File Offset: 0x00017BC4
		protected static GameObject FindCommonRoot(GameObject g1, GameObject g2)
		{
			if (g1 == null || g2 == null)
			{
				return null;
			}
			Transform transform = g1.transform;
			while (transform != null)
			{
				Transform transform2 = g2.transform;
				while (transform2 != null)
				{
					if (transform == transform2)
					{
						return transform.gameObject;
					}
					transform2 = transform2.parent;
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00019A28 File Offset: 0x00017C28
		protected void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
		{
			if (newEnterTarget == null || currentPointerData.pointerEnter == null)
			{
				int count = currentPointerData.hovered.Count;
				for (int i = 0; i < count; i++)
				{
					currentPointerData.fullyExited = true;
					ExecuteEvents.Execute<IPointerMoveHandler>(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerMoveHandler);
					ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerExitHandler);
				}
				currentPointerData.hovered.Clear();
				if (newEnterTarget == null)
				{
					currentPointerData.pointerEnter = null;
					return;
				}
			}
			if (currentPointerData.pointerEnter == newEnterTarget && newEnterTarget)
			{
				if (currentPointerData.IsPointerMoving())
				{
					int count2 = currentPointerData.hovered.Count;
					for (int j = 0; j < count2; j++)
					{
						ExecuteEvents.Execute<IPointerMoveHandler>(currentPointerData.hovered[j], currentPointerData, ExecuteEvents.pointerMoveHandler);
					}
				}
				return;
			}
			GameObject gameObject = BaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
			Component component = (Component)newEnterTarget.GetComponentInParent<IPointerExitHandler>();
			GameObject gameObject2 = ((component != null) ? component.gameObject : null);
			if (currentPointerData.pointerEnter != null)
			{
				Transform transform = currentPointerData.pointerEnter.transform;
				while (transform != null && (!this.m_SendPointerHoverToParent || !(gameObject != null) || !(gameObject.transform == transform)) && (this.m_SendPointerHoverToParent || !(gameObject2 == transform.gameObject)))
				{
					currentPointerData.fullyExited = transform.gameObject != gameObject && currentPointerData.pointerEnter != newEnterTarget;
					ExecuteEvents.Execute<IPointerMoveHandler>(transform.gameObject, currentPointerData, ExecuteEvents.pointerMoveHandler);
					ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
					currentPointerData.hovered.Remove(transform.gameObject);
					if (this.m_SendPointerHoverToParent)
					{
						transform = transform.parent;
					}
					if (gameObject != null && gameObject.transform == transform)
					{
						break;
					}
					if (!this.m_SendPointerHoverToParent)
					{
						transform = transform.parent;
					}
				}
			}
			GameObject pointerEnter = currentPointerData.pointerEnter;
			currentPointerData.pointerEnter = newEnterTarget;
			if (newEnterTarget != null)
			{
				Transform transform2 = newEnterTarget.transform;
				while (transform2 != null)
				{
					currentPointerData.reentered = transform2.gameObject == gameObject && transform2.gameObject != pointerEnter;
					if (this.m_SendPointerHoverToParent && currentPointerData.reentered)
					{
						break;
					}
					ExecuteEvents.Execute<IPointerEnterHandler>(transform2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
					ExecuteEvents.Execute<IPointerMoveHandler>(transform2.gameObject, currentPointerData, ExecuteEvents.pointerMoveHandler);
					currentPointerData.hovered.Add(transform2.gameObject);
					if (!this.m_SendPointerHoverToParent && transform2.gameObject.GetComponent<IPointerEnterHandler>() != null)
					{
						break;
					}
					if (this.m_SendPointerHoverToParent)
					{
						transform2 = transform2.parent;
					}
					if (gameObject != null && gameObject.transform == transform2)
					{
						break;
					}
					if (!this.m_SendPointerHoverToParent)
					{
						transform2 = transform2.parent;
					}
				}
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00019D30 File Offset: 0x00017F30
		protected virtual AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
		{
			if (this.m_AxisEventData == null)
			{
				this.m_AxisEventData = new AxisEventData(this.eventSystem);
			}
			this.m_AxisEventData.Reset();
			this.m_AxisEventData.moveVector = new Vector2(x, y);
			this.m_AxisEventData.moveDir = BaseInputModule.DetermineMoveDirection(x, y, moveDeadZone);
			return this.m_AxisEventData;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00019D8C File Offset: 0x00017F8C
		protected virtual BaseEventData GetBaseEventData()
		{
			if (this.m_BaseEventData == null)
			{
				this.m_BaseEventData = new BaseEventData(this.eventSystem);
			}
			this.m_BaseEventData.Reset();
			return this.m_BaseEventData;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00019DB8 File Offset: 0x00017FB8
		public virtual bool IsPointerOverGameObject(int pointerId)
		{
			return false;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00019DBB File Offset: 0x00017FBB
		public virtual bool ShouldActivateModule()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00019DD2 File Offset: 0x00017FD2
		public virtual void DeactivateModule()
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00019DD4 File Offset: 0x00017FD4
		public virtual void ActivateModule()
		{
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00019DD6 File Offset: 0x00017FD6
		public virtual void UpdateModule()
		{
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00019DD8 File Offset: 0x00017FD8
		public virtual bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00019DDB File Offset: 0x00017FDB
		public virtual int ConvertUIToolkitPointerId(PointerEventData sourcePointerData)
		{
			if (sourcePointerData.pointerId >= 0)
			{
				return PointerId.touchPointerIdBase + sourcePointerData.pointerId;
			}
			return PointerId.mousePointerId;
		}

		// Token: 0x04000209 RID: 521
		[NonSerialized]
		protected List<RaycastResult> m_RaycastResultCache = new List<RaycastResult>();

		// Token: 0x0400020A RID: 522
		[SerializeField]
		private bool m_SendPointerHoverToParent = true;

		// Token: 0x0400020B RID: 523
		private AxisEventData m_AxisEventData;

		// Token: 0x0400020C RID: 524
		private EventSystem m_EventSystem;

		// Token: 0x0400020D RID: 525
		private BaseEventData m_BaseEventData;

		// Token: 0x0400020E RID: 526
		protected BaseInput m_InputOverride;

		// Token: 0x0400020F RID: 527
		private BaseInput m_DefaultInput;
	}
}
