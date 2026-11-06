using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Detection
{
	// Token: 0x02001268 RID: 4712
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Canvas))]
	public sealed class DetectionChoiceCameraAnchor : MonoBehaviour
	{
		// Token: 0x060092A4 RID: 37540 RVA: 0x003C23A0 File Offset: 0x003C05A0
		public void Bind(Camera outputCamera)
		{
			this._canvas = base.GetComponent<Canvas>();
			this._camera = outputCamera;
			if (this._camera != null)
			{
				this._canvas.worldCamera = this._camera;
			}
			this._choiceRects = base.GetComponentsInChildren<RectTransform>(true);
			if (this._overlayScope == null)
			{
				this._overlayScope = new DetectionChoiceOverlayScope(this._canvas);
			}
			this.CaptureAuthoredPose();
			this.AlignNow();
		}

		// Token: 0x060092A5 RID: 37541 RVA: 0x003C2414 File Offset: 0x003C0614
		private void OnEnable()
		{
			this._canvas = base.GetComponent<Canvas>();
			this._choiceRects = base.GetComponentsInChildren<RectTransform>(true);
			if (this._overlayScope == null)
			{
				this._overlayScope = new DetectionChoiceOverlayScope(this._canvas);
			}
			this.CaptureAuthoredPose();
			Canvas.willRenderCanvases += this.AlignNow;
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.OnCameraPreCull));
			this.AlignNow();
		}

		// Token: 0x060092A6 RID: 37542 RVA: 0x003C24A4 File Offset: 0x003C06A4
		private void OnDisable()
		{
			DetectionChoiceOverlayScope overlayScope = this._overlayScope;
			if (overlayScope != null)
			{
				overlayScope.Dispose();
			}
			this._overlayScope = null;
			Canvas.willRenderCanvases -= this.AlignNow;
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.OnCameraPreCull));
			this.RestoreAuthoredPose();
		}

		// Token: 0x060092A7 RID: 37543 RVA: 0x003C2511 File Offset: 0x003C0711
		private void OnDestroy()
		{
			DetectionChoiceOverlayScope overlayScope = this._overlayScope;
			if (overlayScope != null)
			{
				overlayScope.Dispose();
			}
			this._overlayScope = null;
		}

		// Token: 0x060092A8 RID: 37544 RVA: 0x003C252C File Offset: 0x003C072C
		private void CaptureAuthoredPose()
		{
			if (this._hasAuthoredPose)
			{
				return;
			}
			this._authoredLocalPosition = base.transform.localPosition;
			this._authoredLocalRotation = base.transform.localRotation;
			this._authoredLocalScale = base.transform.localScale;
			this._hasAuthoredPose = true;
		}

		// Token: 0x060092A9 RID: 37545 RVA: 0x003C257C File Offset: 0x003C077C
		public void RestoreAuthoredPose()
		{
			if (!this._hasAuthoredPose)
			{
				return;
			}
			base.transform.localPosition = this._authoredLocalPosition;
			base.transform.localRotation = this._authoredLocalRotation;
			base.transform.localScale = this._authoredLocalScale;
			this._hasAuthoredPose = false;
		}

		// Token: 0x060092AA RID: 37546 RVA: 0x003C25CC File Offset: 0x003C07CC
		public void AlignNow()
		{
			if (!base.isActiveAndEnabled || this._canvas == null || this._canvas.renderMode != RenderMode.WorldSpace)
			{
				return;
			}
			if (this._camera == null)
			{
				this._camera = Camera.main;
			}
			if (this._camera == null)
			{
				return;
			}
			this.CaptureAuthoredPose();
			base.transform.localScale = this._authoredLocalScale;
			Quaternion quaternion = this._camera.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				Quaternion quaternion2 = parent.rotation * this._authoredLocalRotation;
				Quaternion quaternion3 = quaternion * Quaternion.Inverse(quaternion2);
				Vector3 vector = parent.TransformVector(this._authoredLocalPosition);
				base.transform.SetPositionAndRotation(parent.position + quaternion3 * vector, quaternion);
			}
			else
			{
				base.transform.SetPositionAndRotation(this._authoredLocalPosition, quaternion);
			}
			this.BringInFrontOfWorldGeometry();
		}

		// Token: 0x060092AB RID: 37547 RVA: 0x003C26E4 File Offset: 0x003C08E4
		private void BringInFrontOfWorldGeometry()
		{
			if (this._choiceRects == null || this._choiceRects.Length == 0)
			{
				return;
			}
			float num = float.PositiveInfinity;
			Transform transform = this._camera.transform;
			for (int i = 0; i < this._choiceRects.Length; i++)
			{
				if (!(this._choiceRects[i] == null) && this._choiceRects[i].gameObject.activeInHierarchy)
				{
					this._choiceRects[i].GetWorldCorners(this._corners);
					for (int j = 0; j < 4; j++)
					{
						num = Mathf.Min(num, transform.InverseTransformPoint(this._corners[j]).z);
					}
				}
			}
			if (float.IsInfinity(num))
			{
				return;
			}
			float num2 = this._camera.nearClipPlane + 0.05f;
			if (num <= num2)
			{
				return;
			}
			if (this._camera.orthographic)
			{
				base.transform.position -= transform.forward * (num - num2);
				return;
			}
			float num3 = num2 / num;
			base.transform.position = transform.position + (base.transform.position - transform.position) * num3;
			base.transform.localScale = this._authoredLocalScale * num3;
		}

		// Token: 0x060092AC RID: 37548 RVA: 0x003C2830 File Offset: 0x003C0A30
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (camera == this._camera)
			{
				this.AlignNow();
			}
		}

		// Token: 0x060092AD RID: 37549 RVA: 0x003C2846 File Offset: 0x003C0A46
		private void OnCameraPreCull(Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline == null && camera == this._camera)
			{
				this.AlignNow();
			}
		}

		// Token: 0x04009724 RID: 38692
		private Canvas _canvas;

		// Token: 0x04009725 RID: 38693
		private Camera _camera;

		// Token: 0x04009726 RID: 38694
		private Vector3 _authoredLocalPosition;

		// Token: 0x04009727 RID: 38695
		private Quaternion _authoredLocalRotation;

		// Token: 0x04009728 RID: 38696
		private Vector3 _authoredLocalScale;

		// Token: 0x04009729 RID: 38697
		private bool _hasAuthoredPose;

		// Token: 0x0400972A RID: 38698
		private RectTransform[] _choiceRects;

		// Token: 0x0400972B RID: 38699
		private readonly Vector3[] _corners = new Vector3[4];

		// Token: 0x0400972C RID: 38700
		private DetectionChoiceOverlayScope _overlayScope;
	}
}
