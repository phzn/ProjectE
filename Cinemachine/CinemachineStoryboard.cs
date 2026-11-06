using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cinemachine
{
	// Token: 0x0200001C RID: 28
	[SaveDuringPlay]
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineStoryboard.html")]
	public class CinemachineStoryboard : CinemachineExtension
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00009ABC File Offset: 0x00007CBC
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (vcam != base.VirtualCamera || stage != CinemachineCore.Stage.Finalize)
			{
				return;
			}
			this.UpdateRenderCanvas();
			if (this.m_ShowImage)
			{
				state.AddCustomBlendable(new CameraState.CustomBlendable(this, 1f));
			}
			if (this.m_MuteCamera)
			{
				state.BlendHint |= (CameraState.BlendHintValue)67;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009B10 File Offset: 0x00007D10
		private void UpdateRenderCanvas()
		{
			for (int i = 0; i < this.mCanvasInfo.Count; i++)
			{
				if (this.mCanvasInfo[i] == null || this.mCanvasInfo[i].mCanvasComponent == null)
				{
					this.mCanvasInfo.RemoveAt(i--);
				}
				else
				{
					this.mCanvasInfo[i].mCanvasComponent.renderMode = (RenderMode)this.m_RenderMode;
					this.mCanvasInfo[i].mCanvasComponent.planeDistance = this.m_PlaneDistance;
					this.mCanvasInfo[i].mCanvasComponent.sortingOrder = this.m_SortingOrder;
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009BC6 File Offset: 0x00007DC6
		protected override void ConnectToVcam(bool connect)
		{
			base.ConnectToVcam(connect);
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.CameraUpdatedCallback));
			if (connect)
			{
				CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.CameraUpdatedCallback));
				return;
			}
			this.DestroyCanvas();
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00009C08 File Offset: 0x00007E08
		private string CanvasName
		{
			get
			{
				return "_CM_canvas" + base.gameObject.GetInstanceID().ToString();
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00009C34 File Offset: 0x00007E34
		private void CameraUpdatedCallback(CinemachineBrain brain)
		{
			bool flag = base.enabled && this.m_ShowImage && CinemachineCore.Instance.IsLive(base.VirtualCamera);
			int num = 1 << base.gameObject.layer;
			if (brain.OutputCamera == null || (brain.OutputCamera.cullingMask & num) == 0)
			{
				flag = false;
			}
			if (CinemachineStoryboard.s_StoryboardGlobalMute)
			{
				flag = false;
			}
			CinemachineStoryboard.CanvasInfo canvasInfo = this.LocateMyCanvas(brain, flag);
			if (canvasInfo != null && canvasInfo.mCanvas != null)
			{
				canvasInfo.mCanvas.SetActive(flag);
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009CC4 File Offset: 0x00007EC4
		private CinemachineStoryboard.CanvasInfo LocateMyCanvas(CinemachineBrain parent, bool createIfNotFound)
		{
			CinemachineStoryboard.CanvasInfo canvasInfo = null;
			int num = 0;
			while (canvasInfo == null && num < this.mCanvasInfo.Count)
			{
				if (this.mCanvasInfo[num] != null && this.mCanvasInfo[num].mCanvasParent == parent)
				{
					canvasInfo = this.mCanvasInfo[num];
				}
				num++;
			}
			if (createIfNotFound)
			{
				if (canvasInfo == null)
				{
					canvasInfo = new CinemachineStoryboard.CanvasInfo
					{
						mCanvasParent = parent
					};
					int childCount = parent.transform.childCount;
					int num2 = 0;
					while (canvasInfo.mCanvas == null && num2 < childCount)
					{
						RectTransform rectTransform = parent.transform.GetChild(num2) as RectTransform;
						if (rectTransform != null && rectTransform.name == this.CanvasName)
						{
							canvasInfo.mCanvas = rectTransform.gameObject;
							RectTransform[] componentsInChildren = canvasInfo.mCanvas.GetComponentsInChildren<RectTransform>();
							canvasInfo.mViewport = ((componentsInChildren.Length > 1) ? componentsInChildren[1] : null);
							canvasInfo.mRawImage = canvasInfo.mCanvas.GetComponentInChildren<RawImage>();
							canvasInfo.mCanvasComponent = canvasInfo.mCanvas.GetComponent<Canvas>();
						}
						num2++;
					}
					this.mCanvasInfo.Add(canvasInfo);
				}
				if (canvasInfo.mCanvas == null || canvasInfo.mViewport == null || canvasInfo.mRawImage == null || canvasInfo.mCanvasComponent == null)
				{
					this.CreateCanvas(canvasInfo);
				}
			}
			return canvasInfo;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009E34 File Offset: 0x00008034
		private void CreateCanvas(CinemachineStoryboard.CanvasInfo ci)
		{
			ci.mCanvas = new GameObject(this.CanvasName, new Type[] { typeof(RectTransform) });
			ci.mCanvas.layer = base.gameObject.layer;
			ci.mCanvas.hideFlags = HideFlags.HideAndDontSave;
			ci.mCanvas.transform.SetParent(ci.mCanvasParent.transform);
			Canvas canvas = (ci.mCanvasComponent = ci.mCanvas.AddComponent<Canvas>());
			canvas.renderMode = (RenderMode)this.m_RenderMode;
			canvas.sortingOrder = this.m_SortingOrder;
			canvas.planeDistance = this.m_PlaneDistance;
			canvas.worldCamera = ci.mCanvasParent.OutputCamera;
			GameObject gameObject = new GameObject("Viewport", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(ci.mCanvas.transform);
			ci.mViewport = (RectTransform)gameObject.transform;
			gameObject.AddComponent<RectMask2D>();
			gameObject = new GameObject("RawImage", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(ci.mViewport.transform);
			ci.mRawImage = gameObject.AddComponent<RawImage>();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009F78 File Offset: 0x00008178
		private void DestroyCanvas()
		{
			int brainCount = CinemachineCore.Instance.BrainCount;
			for (int i = 0; i < brainCount; i++)
			{
				CinemachineBrain activeBrain = CinemachineCore.Instance.GetActiveBrain(i);
				for (int j = activeBrain.transform.childCount - 1; j >= 0; j--)
				{
					RectTransform rectTransform = activeBrain.transform.GetChild(j) as RectTransform;
					if (rectTransform != null && rectTransform.name == this.CanvasName)
					{
						RuntimeUtility.DestroyObject(rectTransform.gameObject);
					}
				}
			}
			this.mCanvasInfo.Clear();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A008 File Offset: 0x00008208
		private void PlaceImage(CinemachineStoryboard.CanvasInfo ci, float alpha)
		{
			if (ci.mRawImage != null && ci.mViewport != null)
			{
				Rect pixelRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
				if (ci.mCanvasParent.OutputCamera != null)
				{
					pixelRect = ci.mCanvasParent.OutputCamera.pixelRect;
				}
				pixelRect.x -= (float)Screen.width / 2f;
				pixelRect.y -= (float)Screen.height / 2f;
				float num = -Mathf.Clamp(this.m_SplitView, -1f, 1f) * pixelRect.width;
				Vector3 vector = pixelRect.center;
				vector.x -= num / 2f;
				ci.mViewport.localPosition = vector;
				ci.mViewport.localRotation = Quaternion.identity;
				ci.mViewport.localScale = Vector3.one;
				ci.mViewport.ForceUpdateRectTransforms();
				ci.mViewport.sizeDelta = new Vector2(pixelRect.width + 1f - Mathf.Abs(num), pixelRect.height + 1f);
				Vector2 one = Vector2.one;
				if (this.m_Image != null && this.m_Image.width > 0 && this.m_Image.width > 0 && pixelRect.width > 0f && pixelRect.height > 0f)
				{
					float num2 = pixelRect.height * (float)this.m_Image.width / (pixelRect.width * (float)this.m_Image.height);
					switch (this.m_Aspect)
					{
					case CinemachineStoryboard.FillStrategy.BestFit:
						if (num2 >= 1f)
						{
							one.y /= num2;
						}
						else
						{
							one.x *= num2;
						}
						break;
					case CinemachineStoryboard.FillStrategy.CropImageToFit:
						if (num2 >= 1f)
						{
							one.x *= num2;
						}
						else
						{
							one.y /= num2;
						}
						break;
					}
				}
				one.x *= this.m_Scale.x;
				one.y *= (this.m_SyncScale ? this.m_Scale.x : this.m_Scale.y);
				ci.mRawImage.texture = this.m_Image;
				Color white = Color.white;
				white.a = this.m_Alpha * alpha;
				ci.mRawImage.color = white;
				vector = new Vector2(pixelRect.width * this.m_Center.x, pixelRect.height * this.m_Center.y);
				vector.x += num / 2f;
				ci.mRawImage.rectTransform.localPosition = vector;
				ci.mRawImage.rectTransform.localRotation = Quaternion.Euler(this.m_Rotation);
				ci.mRawImage.rectTransform.localScale = one;
				ci.mRawImage.rectTransform.ForceUpdateRectTransforms();
				ci.mRawImage.rectTransform.sizeDelta = pixelRect.size;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A364 File Offset: 0x00008564
		private static void StaticBlendingHandler(CinemachineBrain brain)
		{
			CameraState currentCameraState = brain.CurrentCameraState;
			int numCustomBlendables = currentCameraState.NumCustomBlendables;
			for (int i = 0; i < numCustomBlendables; i++)
			{
				CameraState.CustomBlendable customBlendable = currentCameraState.GetCustomBlendable(i);
				CinemachineStoryboard cinemachineStoryboard = customBlendable.m_Custom as CinemachineStoryboard;
				if (!(cinemachineStoryboard == null))
				{
					bool flag = true;
					int num = 1 << cinemachineStoryboard.gameObject.layer;
					if (brain.OutputCamera == null || (brain.OutputCamera.cullingMask & num) == 0)
					{
						flag = false;
					}
					if (CinemachineStoryboard.s_StoryboardGlobalMute)
					{
						flag = false;
					}
					CinemachineStoryboard.CanvasInfo canvasInfo = cinemachineStoryboard.LocateMyCanvas(brain, flag);
					if (canvasInfo != null)
					{
						cinemachineStoryboard.PlaceImage(canvasInfo, customBlendable.m_Weight);
					}
				}
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A412 File Offset: 0x00008612
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeModule()
		{
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(CinemachineStoryboard.StaticBlendingHandler));
			CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(CinemachineStoryboard.StaticBlendingHandler));
		}

		// Token: 0x040000CA RID: 202
		[Tooltip("If checked, all storyboards are globally muted")]
		public static bool s_StoryboardGlobalMute;

		// Token: 0x040000CB RID: 203
		[Tooltip("If checked, the specified image will be displayed as an overlay over the virtual camera's output")]
		public bool m_ShowImage = true;

		// Token: 0x040000CC RID: 204
		[Tooltip("The image to display")]
		public Texture m_Image;

		// Token: 0x040000CD RID: 205
		[Tooltip("How to handle differences between image aspect and screen aspect")]
		public CinemachineStoryboard.FillStrategy m_Aspect;

		// Token: 0x040000CE RID: 206
		[Tooltip("The opacity of the image.  0 is transparent, 1 is opaque")]
		[Range(0f, 1f)]
		public float m_Alpha = 1f;

		// Token: 0x040000CF RID: 207
		[Tooltip("The screen-space position at which to display the image.  Zero is center")]
		public Vector2 m_Center = Vector2.zero;

		// Token: 0x040000D0 RID: 208
		[Tooltip("The screen-space rotation to apply to the image")]
		public Vector3 m_Rotation = Vector3.zero;

		// Token: 0x040000D1 RID: 209
		[Tooltip("The screen-space scaling to apply to the image")]
		public Vector2 m_Scale = Vector3.one;

		// Token: 0x040000D2 RID: 210
		[Tooltip("If checked, X and Y scale are synchronized")]
		public bool m_SyncScale = true;

		// Token: 0x040000D3 RID: 211
		[Tooltip("If checked, Camera transform will not be controlled by this virtual camera")]
		public bool m_MuteCamera;

		// Token: 0x040000D4 RID: 212
		[Range(-1f, 1f)]
		[Tooltip("Wipe the image on and off horizontally")]
		public float m_SplitView;

		// Token: 0x040000D5 RID: 213
		[Tooltip("The render mode of the canvas on which the storyboard is drawn.")]
		public CinemachineStoryboard.StoryboardRenderMode m_RenderMode;

		// Token: 0x040000D6 RID: 214
		[Tooltip("Allows ordering canvases to render on top or below other canvases.")]
		public int m_SortingOrder;

		// Token: 0x040000D7 RID: 215
		[Tooltip("How far away from the camera is the Canvas generated.")]
		public float m_PlaneDistance = 100f;

		// Token: 0x040000D8 RID: 216
		private List<CinemachineStoryboard.CanvasInfo> mCanvasInfo = new List<CinemachineStoryboard.CanvasInfo>();

		// Token: 0x02000089 RID: 137
		public enum FillStrategy
		{
			// Token: 0x040002FD RID: 765
			BestFit,
			// Token: 0x040002FE RID: 766
			CropImageToFit,
			// Token: 0x040002FF RID: 767
			StretchToFit
		}

		// Token: 0x0200008A RID: 138
		private class CanvasInfo
		{
			// Token: 0x04000300 RID: 768
			public GameObject mCanvas;

			// Token: 0x04000301 RID: 769
			public Canvas mCanvasComponent;

			// Token: 0x04000302 RID: 770
			public CinemachineBrain mCanvasParent;

			// Token: 0x04000303 RID: 771
			public RectTransform mViewport;

			// Token: 0x04000304 RID: 772
			public RawImage mRawImage;
		}

		// Token: 0x0200008B RID: 139
		public enum StoryboardRenderMode
		{
			// Token: 0x04000306 RID: 774
			ScreenSpaceOverlay,
			// Token: 0x04000307 RID: 775
			ScreenSpaceCamera
		}
	}
}
