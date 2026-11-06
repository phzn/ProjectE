using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.MainMenu
{
	// Token: 0x0200103E RID: 4158
	[RequireComponent(typeof(RectTransform))]
	public sealed class ItemPreviewFreeRotator : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IInitializePotentialDragHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
	{
		// Token: 0x06007E3A RID: 32314 RVA: 0x00339DB8 File Offset: 0x00337FB8
		private void Awake()
		{
			this._rectTransform = base.transform as RectTransform;
		}

		// Token: 0x06007E3B RID: 32315 RVA: 0x00339DCB File Offset: 0x00337FCB
		public void SetTarget(Transform target, Camera camera = null)
		{
			this._target = target;
			if (camera != null)
			{
				this.viewCamera = camera;
			}
			this._dragging = false;
		}

		// Token: 0x06007E3C RID: 32316 RVA: 0x00339DEB File Offset: 0x00337FEB
		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x06007E3D RID: 32317 RVA: 0x00339DF4 File Offset: 0x00337FF4
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this._target == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this._dragging = this.TryMapToTrackball(eventData, out this._startTrackballPoint);
			if (this._dragging)
			{
				this._pointerId = eventData.pointerId;
				this._startRotation = this._target.rotation;
				this._viewRotation = ((this.viewCamera != null) ? this.viewCamera.transform.rotation : Quaternion.identity);
				eventData.Use();
			}
		}

		// Token: 0x06007E3E RID: 32318 RVA: 0x00339E84 File Offset: 0x00338084
		public void OnDrag(PointerEventData eventData)
		{
			Vector3 vector;
			if (!this._dragging || this._target == null || eventData.pointerId != this._pointerId || eventData.button != PointerEventData.InputButton.Left || !this.TryMapToTrackball(eventData, out vector))
			{
				return;
			}
			Vector3 vector2 = this._viewRotation * new Vector3(this._startTrackballPoint.x, this._startTrackballPoint.y, -this._startTrackballPoint.z);
			Vector3 vector3 = this._viewRotation * new Vector3(vector.x, vector.y, -vector.z);
			Quaternion quaternion = Quaternion.FromToRotation(vector2, vector3);
			this._target.rotation = Quaternion.SlerpUnclamped(Quaternion.identity, quaternion, this.rotationSensitivity) * this._startRotation;
			eventData.Use();
		}

		// Token: 0x06007E3F RID: 32319 RVA: 0x00339F53 File Offset: 0x00338153
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && eventData.pointerId == this._pointerId)
			{
				this._dragging = false;
			}
		}

		// Token: 0x06007E40 RID: 32320 RVA: 0x00339F72 File Offset: 0x00338172
		public void OnEndDrag(PointerEventData eventData)
		{
			this.OnPointerUp(eventData);
		}

		// Token: 0x06007E41 RID: 32321 RVA: 0x00339F7B File Offset: 0x0033817B
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				this._dragging = false;
			}
		}

		// Token: 0x06007E42 RID: 32322 RVA: 0x00339F87 File Offset: 0x00338187
		private void OnDisable()
		{
			this._dragging = false;
		}

		// Token: 0x06007E43 RID: 32323 RVA: 0x00339F90 File Offset: 0x00338190
		private void Update()
		{
			if (this._target == null || !ControllerInput.IsControllerActive || !ControllerInput.Modifier || !ControllerUINavigation.CanControlPreview(this))
			{
				return;
			}
			Vector2 look = ControllerInput.Look;
			Vector3 vector = ((this.viewCamera != null) ? this.viewCamera.transform.rotation : Quaternion.identity) * new Vector3(look.y, -look.x, 0f);
			if (vector.sqrMagnitude > 0.01f)
			{
				this._target.rotation = Quaternion.AngleAxis(vector.magnitude * 100f * Time.unscaledDeltaTime * this.rotationSensitivity, vector.normalized) * this._target.rotation;
			}
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x0033A05C File Offset: 0x0033825C
		private bool TryMapToTrackball(PointerEventData eventData, out Vector3 point)
		{
			point = Vector3.forward;
			if (this._rectTransform == null)
			{
				this._rectTransform = base.transform as RectTransform;
			}
			Vector2 vector;
			if (this._rectTransform == null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rectTransform, eventData.position, eventData.pressEventCamera, out vector))
			{
				return false;
			}
			Rect rect = this._rectTransform.rect;
			float num = Mathf.Max(1f, Mathf.Min(rect.width, rect.height) * 0.5f);
			float num2 = (vector.x - rect.center.x) / num;
			float num3 = (vector.y - rect.center.y) / num;
			float num4 = num2 * num2 + num3 * num3;
			float num5 = ((num4 <= 0.5f) ? Mathf.Sqrt(1f - num4) : (0.5f / Mathf.Sqrt(num4)));
			point = new Vector3(num2, num3, num5).normalized;
			return true;
		}

		// Token: 0x040081D6 RID: 33238
		[SerializeField]
		[Range(0.25f, 3f)]
		private float rotationSensitivity = 1f;

		// Token: 0x040081D7 RID: 33239
		[SerializeField]
		private Camera viewCamera;

		// Token: 0x040081D8 RID: 33240
		private RectTransform _rectTransform;

		// Token: 0x040081D9 RID: 33241
		private Transform _target;

		// Token: 0x040081DA RID: 33242
		private Vector3 _startTrackballPoint;

		// Token: 0x040081DB RID: 33243
		private Quaternion _startRotation;

		// Token: 0x040081DC RID: 33244
		private Quaternion _viewRotation;

		// Token: 0x040081DD RID: 33245
		private int _pointerId;

		// Token: 0x040081DE RID: 33246
		private bool _dragging;
	}
}
