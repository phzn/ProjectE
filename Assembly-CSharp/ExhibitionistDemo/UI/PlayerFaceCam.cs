using System;
using System.Collections.Generic;
using Features.MainMenu;
using Features.WantedPoster;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core.SceneManagement;
using RayznGames.VehicleSystem;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ExhibitionistDemo.UI
{
	// Token: 0x02000795 RID: 1941
	[DefaultExecutionOrder(1100)]
	[RequireComponent(typeof(Camera))]
	public class PlayerFaceCam : MonoBehaviour
	{
		// Token: 0x0600331E RID: 13086 RVA: 0x001221C4 File Offset: 0x001203C4
		private void Awake()
		{
			this.EnsureRenderingContract();
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x001221CC File Offset: 0x001203CC
		private void OnEnable()
		{
			this.EnsureRenderingContract();
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
			GameSettingsService.SettingsChanged += this.ApplyVisibilityPreference;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.ApplyVisibilityPreference();
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0012222C File Offset: 0x0012042C
		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			GameSettingsService.SettingsChanged -= this.ApplyVisibilityPreference;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			this.RestoreVisualLayers();
			this.ReleaseCameraSuspension();
			this._hiddenByPreference = false;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00122290 File Offset: 0x00120490
		private void ApplyVisibilityPreference()
		{
			this.ApplyVisibilityPreferenceForScene(SceneManager.GetActiveScene().name);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x001222B0 File Offset: 0x001204B0
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			this.ApplyVisibilityPreferenceForScene(current.name);
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x001222BF File Offset: 0x001204BF
		private void ApplyVisibilityPreferenceForScene(string sceneName)
		{
			this._hiddenByPreference = sceneName != "MainMenu" && !GameSettingsService.ShowFaceCamera;
			if (this._hiddenByPreference)
			{
				this.RestoreVisualLayers();
				this.SuspendCamera();
				return;
			}
			this.ReleaseCameraSuspension();
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x001222FC File Offset: 0x001204FC
		private void SuspendCamera()
		{
			if (this._faceCamera == null)
			{
				this._faceCamera = base.GetComponent<Camera>();
			}
			if (this._faceCamera == null || !this._faceCamera.enabled)
			{
				return;
			}
			this._ownsCameraSuspension = true;
			this._faceCamera.enabled = false;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x00122352 File Offset: 0x00120552
		private void ReleaseCameraSuspension()
		{
			if (!this._ownsCameraSuspension)
			{
				return;
			}
			this._ownsCameraSuspension = false;
			if (this._faceCamera != null)
			{
				this.UpdateTargetPose(true);
				this._faceCamera.enabled = true;
			}
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00122385 File Offset: 0x00120585
		private void LateUpdate()
		{
			if (this._hiddenByPreference)
			{
				this.SuspendCamera();
				return;
			}
			this.UpdateTargetPose(false);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x001223A0 File Offset: 0x001205A0
		private void UpdateTargetPose(bool snap)
		{
			if (this.target == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.Euler(0f, this.target.eulerAngles.y, 0f);
			Vector3 vector = this.target.position + quaternion * this.offset;
			base.transform.position = (snap ? vector : Vector3.Lerp(base.transform.position, vector, this.smoothSpeed * Time.deltaTime));
			base.transform.LookAt(this.target, Vector3.up);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x0012243D File Offset: 0x0012063D
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
		{
			if (renderingCamera == this._faceCamera)
			{
				this.OverrideActiveVisualLayers();
			}
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00122453 File Offset: 0x00120653
		private void OnEndCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
		{
			if (renderingCamera == this._faceCamera)
			{
				this.RestoreVisualLayers();
			}
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00122469 File Offset: 0x00120669
		private void OnPreCull()
		{
			this.OverrideActiveVisualLayers();
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00122471 File Offset: 0x00120671
		private void OnPostRender()
		{
			this.RestoreVisualLayers();
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0012247C File Offset: 0x0012067C
		private void EnsureRenderingContract()
		{
			if (this._faceCamera == null)
			{
				this._faceCamera = base.GetComponent<Camera>();
			}
			if (this._faceCamera == null)
			{
				return;
			}
			if (!this._layerMaskCached)
			{
				this._requiredLayerMask = 0;
				for (int i = 0; i < PlayerFaceCam.RequiredLayerNames.Length; i++)
				{
					int num = LayerMask.NameToLayer(PlayerFaceCam.RequiredLayerNames[i]);
					if (num >= 0)
					{
						this._requiredLayerMask |= 1 << num;
					}
				}
				this._captureLayer = LayerMask.NameToLayer("Player");
				this._layerMaskCached = true;
			}
			int cullingMask = this._faceCamera.cullingMask;
			int num2 = cullingMask | this._requiredLayerMask;
			if (cullingMask != num2)
			{
				this._faceCamera.cullingMask = num2;
			}
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00122534 File Offset: 0x00120734
		private void OverrideActiveVisualLayers()
		{
			if (this._hiddenByPreference || this._visualLayersOverridden || this.target == null)
			{
				return;
			}
			using (PlayerFaceCam.OverrideVisualLayersMarker.Auto())
			{
				this.EnsureRenderingContract();
				if (this._captureLayer >= 0)
				{
					Transform transform = this.ResolveActiveVisualRoot();
					if (!(transform == null))
					{
						this._interactionRenderers.Clear();
						this._originalVisualLayers.Clear();
						transform.GetComponentsInChildren<Renderer>(false, this._interactionRenderers);
						this._posterVisibility.HideRegisteredPosters(this._faceCamera.cullingMask);
						for (int i = 0; i < this._interactionRenderers.Count; i++)
						{
							Renderer renderer = this._interactionRenderers[i];
							if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !this._posterVisibility.HideIfPoster(renderer))
							{
								GameObject gameObject = renderer.gameObject;
								int layer = gameObject.layer;
								if (layer != this._captureLayer)
								{
									this._originalVisualLayers.Add(gameObject, layer);
									gameObject.layer = this._captureLayer;
								}
							}
						}
						this._visualLayersOverridden = true;
					}
				}
			}
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00122688 File Offset: 0x00120888
		private Transform ResolveActiveVisualRoot()
		{
			if (this.target == null)
			{
				return null;
			}
			if (this._resolvedTarget != this.target || this._actorRoot == null || !this.target.IsChildOf(this._actorRoot))
			{
				this._resolvedTarget = this.target;
				PlayerRegistry componentInParent = this.target.GetComponentInParent<PlayerRegistry>();
				this._actorRoot = ((componentInParent != null) ? componentInParent.transform : null);
				if (this._actorRoot == null)
				{
					vThirdPersonController componentInParent2 = this.target.GetComponentInParent<vThirdPersonController>();
					this._actorRoot = ((componentInParent2 != null) ? componentInParent2.transform : null);
				}
				this._activeVisualRoot = null;
			}
			if (this._actorRoot == null)
			{
				return this.target.root;
			}
			Transform parent = this._actorRoot.parent;
			if (this._activeVisualRoot != null && parent == this._actorParent)
			{
				return this._activeVisualRoot;
			}
			this._actorParent = parent;
			this._activeVisualRoot = this._actorRoot;
			Transform transform = parent;
			while (transform != null)
			{
				if (transform.GetComponent<vVehicleController>() != null || transform.GetComponent<vBikeController>() != null || transform.GetComponent<SkateboardInteractable>() != null || transform.GetComponent<IVehicle>() != null)
				{
					this._activeVisualRoot = transform;
					break;
				}
				transform = transform.parent;
			}
			return this._activeVisualRoot;
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x001227F4 File Offset: 0x001209F4
		private void RestoreVisualLayers()
		{
			this._posterVisibility.Restore();
			if (!this._visualLayersOverridden && this._originalVisualLayers.Count == 0)
			{
				return;
			}
			using (PlayerFaceCam.RestoreVisualLayersMarker.Auto())
			{
				foreach (KeyValuePair<GameObject, int> keyValuePair in this._originalVisualLayers)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.layer = keyValuePair.Value;
					}
				}
				this._originalVisualLayers.Clear();
				this._interactionRenderers.Clear();
				this._visualLayersOverridden = false;
			}
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x001228CC File Offset: 0x00120ACC
		public void SetTarget(Transform newTarget)
		{
			this.RestoreVisualLayers();
			this.target = newTarget;
			this.EnsureRenderingContract();
		}

		// Token: 0x040033C0 RID: 13248
		private const string CaptureLayerName = "Player";

		// Token: 0x040033C1 RID: 13249
		private static readonly ProfilerMarker OverrideVisualLayersMarker = new ProfilerMarker("PlayerFaceCam.OverrideVisualLayers");

		// Token: 0x040033C2 RID: 13250
		private static readonly ProfilerMarker RestoreVisualLayersMarker = new ProfilerMarker("PlayerFaceCam.RestoreVisualLayers");

		// Token: 0x040033C3 RID: 13251
		private static readonly string[] RequiredLayerNames = new string[] { "Player", "PlayerHead", "PlayerClothing", "Interactable" };

		// Token: 0x040033C4 RID: 13252
		[Header("Target")]
		[Tooltip("The target transform to follow (assign your GameObject inside the head)")]
		public Transform target;

		// Token: 0x040033C5 RID: 13253
		[Header("Camera Settings")]
		[Tooltip("Offset position relative to target (X, Y, Z in local space)")]
		public Vector3 offset = new Vector3(0f, 0f, -0.8f);

		// Token: 0x040033C6 RID: 13254
		[Tooltip("Smooth speed for camera movement")]
		public float smoothSpeed = 0.125f;

		// Token: 0x040033C7 RID: 13255
		private readonly List<Renderer> _interactionRenderers = new List<Renderer>(64);

		// Token: 0x040033C8 RID: 13256
		private readonly WantedPosterCaptureVisibility _posterVisibility = new WantedPosterCaptureVisibility();

		// Token: 0x040033C9 RID: 13257
		private readonly Dictionary<GameObject, int> _originalVisualLayers = new Dictionary<GameObject, int>(64);

		// Token: 0x040033CA RID: 13258
		private Camera _faceCamera;

		// Token: 0x040033CB RID: 13259
		private int _captureLayer = -1;

		// Token: 0x040033CC RID: 13260
		private int _requiredLayerMask;

		// Token: 0x040033CD RID: 13261
		private bool _layerMaskCached;

		// Token: 0x040033CE RID: 13262
		private bool _visualLayersOverridden;

		// Token: 0x040033CF RID: 13263
		private Transform _resolvedTarget;

		// Token: 0x040033D0 RID: 13264
		private Transform _actorRoot;

		// Token: 0x040033D1 RID: 13265
		private Transform _actorParent;

		// Token: 0x040033D2 RID: 13266
		private Transform _activeVisualRoot;

		// Token: 0x040033D3 RID: 13267
		private bool _hiddenByPreference;

		// Token: 0x040033D4 RID: 13268
		private bool _ownsCameraSuspension;
	}
}
