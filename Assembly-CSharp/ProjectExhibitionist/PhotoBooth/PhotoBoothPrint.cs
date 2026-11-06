using System;
using System.Collections;
using Features.CensorBars;
using Features.Inventory.Placement;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200098F RID: 2447
	[DisallowMultipleComponent]
	public sealed class PhotoBoothPrint : MonoBehaviour, IInteractable
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x00195D53 File Offset: 0x00193F53
		public InteractionConfigSO Config
		{
			get
			{
				return this._interactionConfig;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x00195D5B File Offset: 0x00193F5B
		public bool IsBeingUsed
		{
			get
			{
				return this._held;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x00195D63 File Offset: 0x00193F63
		public float WorldWidth
		{
			get
			{
				return this.GetWorldSizeAlong(Vector3.right);
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x00195D5B File Offset: 0x00193F5B
		public bool IsHeld
		{
			get
			{
				return this._held;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x00195D70 File Offset: 0x00193F70
		public bool IsCollected
		{
			get
			{
				return this._collected;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x00195D78 File Offset: 0x00193F78
		public bool IsAvailable
		{
			get
			{
				return this._armed && !this._held;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060043CD RID: 17357 RVA: 0x00195D8D File Offset: 0x00193F8D
		public Transform PivotTransform
		{
			get
			{
				if (!(this._centeredPivot != null))
				{
					return base.transform;
				}
				return this._centeredPivot;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060043CE RID: 17358 RVA: 0x00195DAA File Offset: 0x00193FAA
		public Vector2 FrontTextureScale
		{
			get
			{
				return this.GetFrontTextureTransform(true);
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x00195DB3 File Offset: 0x00193FB3
		public Vector2 FrontTextureOffset
		{
			get
			{
				return this.GetFrontTextureTransform(false);
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x00195DBC File Offset: 0x00193FBC
		public Vector2Int PhotoTextureSize
		{
			get
			{
				if (!(this._photoTexture != null))
				{
					return Vector2Int.zero;
				}
				return new Vector2Int(this._photoTexture.width, this._photoTexture.height);
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060043D1 RID: 17361 RVA: 0x00195DED File Offset: 0x00193FED
		public bool UsesUvRotation180
		{
			get
			{
				return this._photoTextureRequires180Rotation;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060043D2 RID: 17362 RVA: 0x00195DF5 File Offset: 0x00193FF5
		public bool IsReservedForNpc
		{
			get
			{
				return this._npcReservationOwner != null;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060043D3 RID: 17363 RVA: 0x00195E03 File Offset: 0x00194003
		public Vector3 VisualCenter
		{
			get
			{
				if (!(this._centeredPivot != null))
				{
					return base.transform.TransformPoint(this._localVisualCenter);
				}
				return this._centeredPivot.position;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060043D4 RID: 17364 RVA: 0x00195E30 File Offset: 0x00194030
		public Vector3 NpcInterestPoint
		{
			get
			{
				return this.VisualCenter;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060043D5 RID: 17365 RVA: 0x00195E38 File Offset: 0x00194038
		public bool IsNpcWallInterestEligible
		{
			get
			{
				if (this._consumedByNpc || this._held || this._collected || !base.gameObject.activeInHierarchy || this._photoTexture == null)
				{
					return false;
				}
				if (this._renderer == null)
				{
					this._renderer = base.GetComponent<Renderer>();
				}
				if (this._renderer == null || !this._renderer.enabled || !this._renderer.gameObject.activeInHierarchy)
				{
					return false;
				}
				PlacedDildoInteractable component = base.GetComponent<PlacedDildoInteractable>();
				return component != null && component.isActiveAndEnabled && !component.IsBeingUsed && component.PlacementType == PlacementSurfaceType.Wall && component.ItemData is PhotoBoothPhotoItemSO;
			}
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x00195EFC File Offset: 0x001940FC
		private void DisableWholePhotoCensor()
		{
			StreamerCensorTarget component = base.GetComponent<StreamerCensorTarget>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x00195F20 File Offset: 0x00194120
		public void Initialize(Texture2D portraitTexture, Texture2D printTexture, InteractionConfigSO interactionConfig, PhotoBoothPrintHolder holder, GameObject visualPrefabSource, int photoSequence, string archiveId = null)
		{
			this._renderer = base.GetComponent<Renderer>();
			this.DisableWholePhotoCensor();
			this.CacheHidePhotoBlendShape();
			this._portraitTexture = portraitTexture;
			this._photoTexture = printTexture;
			this._interactionConfig = interactionConfig;
			this._holder = holder;
			this._visualPrefabSource = visualPrefabSource;
			this._photoSequence = photoSequence;
			this._archiveId = archiveId;
			this._targetWorldScale = base.transform.lossyScale;
			this._worldRestRotation = base.transform.rotation;
			this._ownsTextures = true;
			this.SetHidePhotoWeight(0f);
			this.CacheVisualMetrics();
			this.ApplyPhotoMaterial();
			this.EnsureCollider();
			this.EnsureCenteredPivot();
			this._armed = false;
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x00195FE8 File Offset: 0x001941E8
		public void InitializeInventoryVisual(PhotoBoothPhotoItemSO item, Vector3 targetWorldScale)
		{
			this._renderer = base.GetComponent<Renderer>();
			this.DisableWholePhotoCensor();
			this.CacheHidePhotoBlendShape();
			this._portraitTexture = ((item != null) ? item.PortraitTexture : null);
			this._photoTexture = ((item != null) ? item.PrintTexture : null);
			this._photoTextureRequires180Rotation = item != null && item.PrintTextureRequires180Rotation;
			this._targetWorldScale = targetWorldScale;
			this._ownsTextures = false;
			this._armed = false;
			this._held = false;
			this.SetHidePhotoWeight(0f);
			this.CacheVisualMetrics();
			this.ApplyPhotoMaterial();
			this.EnsureCollider();
			this.EnsureCenteredPivot();
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x001960B0 File Offset: 0x001942B0
		public void InitializeDisplayVisual(Texture2D portraitTexture, Texture2D printTexture, Vector3 targetWorldScale, bool printTextureRequires180Rotation = false)
		{
			this._renderer = base.GetComponent<Renderer>();
			this.DisableWholePhotoCensor();
			this.CacheHidePhotoBlendShape();
			this._portraitTexture = portraitTexture;
			this._photoTexture = printTexture;
			this._photoTextureRequires180Rotation = printTextureRequires180Rotation;
			this._targetWorldScale = targetWorldScale;
			this._ownsTextures = false;
			this._armed = false;
			this._held = false;
			this.SetHidePhotoWeight(0f);
			this.CacheVisualMetrics();
			this.ApplyPhotoMaterial();
			this.EnsureCollider();
			this.EnsureCenteredPivot();
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
			base.enabled = false;
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0019614B File Offset: 0x0019434B
		public void RefreshInventoryVisual(PhotoBoothPhotoItemSO item)
		{
			if (item == null)
			{
				return;
			}
			this._portraitTexture = item.PortraitTexture;
			this._photoTexture = item.PrintTexture;
			this._photoTextureRequires180Rotation = item.PrintTextureRequires180Rotation;
			this.ApplyPhotoMaterial();
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x00196181 File Offset: 0x00194381
		public void ConfigureAsWorldObject()
		{
			this.EnsureCollider();
			this._armed = false;
			this._held = false;
			if (this._collider != null)
			{
				this._collider.enabled = true;
			}
			PhotoBoothPrintNpcDirector.RegisterWorldPrint(this);
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x001961B7 File Offset: 0x001943B7
		public bool TryReserveForNpc(Object owner)
		{
			if (owner == null || !this.IsNpcWallInterestEligible)
			{
				return false;
			}
			if (this._npcReservationOwner != null && this._npcReservationOwner != owner)
			{
				return false;
			}
			this._npcReservationOwner = owner;
			return true;
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x001961F2 File Offset: 0x001943F2
		public void ReleaseNpcReservation(Object owner)
		{
			if (this._npcReservationOwner == owner)
			{
				this._npcReservationOwner = null;
			}
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0019620C File Offset: 0x0019440C
		public bool ConsumeByNpc(Object owner)
		{
			if (this._consumedByNpc || owner == null || this._npcReservationOwner != owner || !this.IsNpcWallInterestEligible)
			{
				return false;
			}
			PlacedDildoInteractable component = base.GetComponent<PlacedDildoInteractable>();
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = ((component != null) ? (component.ItemData as PhotoBoothPhotoItemSO) : null);
			if (photoBoothPhotoItemSO == null || component.DetachItemDataForExternalPickup() != photoBoothPhotoItemSO)
			{
				return false;
			}
			this._npcConsumedItem = photoBoothPhotoItemSO;
			this._consumedByNpc = true;
			this._npcReservationOwner = null;
			this._armed = false;
			this._held = true;
			Collider[] componentsInChildren = this.PivotTransform.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			PhotoBoothPrintNpcDirector.UnregisterWorldPrint(this);
			return true;
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x001962C8 File Offset: 0x001944C8
		public void PrepareForEjection()
		{
			this._armed = false;
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x001962EC File Offset: 0x001944EC
		public void FinishEjection()
		{
			this.SetHidePhotoWeight(0f);
			this.EnsureCenteredPivot();
			this._outputBaseVisualCenter = this.VisualCenter;
			this._hasOutputBasePose = true;
			this._armed = true;
			if (this._collider != null)
			{
				this._collider.enabled = true;
			}
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x00196340 File Offset: 0x00194540
		public void SetOutputStackLevel(int level, Vector3 slideDirection, Vector3 normalReference)
		{
			if (!this._hasOutputBasePose || this._held || this._collected)
			{
				return;
			}
			this.EnsureCenteredPivot();
			Vector3 vector = this.PivotTransform.up;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = normalReference;
			}
			vector.Normalize();
			if (normalReference.sqrMagnitude > 0.0001f && Vector3.Dot(vector, normalReference) < 0f)
			{
				vector = -vector;
			}
			Vector3 vector2 = Vector3.ProjectOnPlane(slideDirection, vector);
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = Vector3.ProjectOnPlane(this.PivotTransform.forward, vector);
			}
			if (vector2.sqrMagnitude > 0.0001f)
			{
				vector2.Normalize();
			}
			int num = Mathf.Max(0, level);
			Vector3 vector3 = this._outputBaseVisualCenter + vector2 * (0.0075f * (float)num) + vector * (0.0022f * (float)num);
			if (this._outputStackRoutine != null)
			{
				base.StopCoroutine(this._outputStackRoutine);
			}
			this._outputStackRoutine = base.StartCoroutine(this.MoveOutputStackTo(vector3));
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0019644B File Offset: 0x0019464B
		private IEnumerator MoveOutputStackTo(Vector3 target)
		{
			Vector3 start = this.VisualCenter;
			Quaternion rotation = this.PivotTransform.rotation;
			float elapsed = 0f;
			while (elapsed < 0.16f)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.16f));
				this.SetVisualWorldPose(Vector3.LerpUnclamped(start, target, num), rotation);
				yield return null;
			}
			this.SetVisualWorldPose(target, rotation);
			this._outputStackRoutine = null;
			yield break;
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060043E3 RID: 17379 RVA: 0x00196461 File Offset: 0x00194661
		public bool HasHidePhotoBlendShape
		{
			get
			{
				return this._hidePhotoBlendShapeIndex >= 0;
			}
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0019646F File Offset: 0x0019466F
		public void SetHidePhotoWeight(float weight)
		{
			if (this._skinnedRenderer == null || this._hidePhotoBlendShapeIndex < 0)
			{
				return;
			}
			this._skinnedRenderer.SetBlendShapeWeight(this._hidePhotoBlendShapeIndex, Mathf.Clamp(weight, 0f, 100f));
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x001964AA File Offset: 0x001946AA
		public void AttachToHand(Transform hand)
		{
			if (hand == null)
			{
				return;
			}
			this.EnsureCenteredPivot();
			this.PivotTransform.SetParent(hand, true);
			this.RecenterPivotPreservingVisual();
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x001964CF File Offset: 0x001946CF
		public void AttachToHand(Transform hand, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
		{
			this.AttachToHand(hand);
			if (hand == null)
			{
				return;
			}
			this.PivotTransform.localPosition = localPosition;
			this.PivotTransform.localEulerAngles = localEulerAngles;
			this.PivotTransform.localScale = localScale;
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x00196507 File Offset: 0x00194707
		public void SetWorldPose(Vector3 position, Quaternion rotation)
		{
			this.EnsureCenteredPivot();
			this.PivotTransform.SetPositionAndRotation(position, rotation);
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x00196507 File Offset: 0x00194707
		public void SetVisualWorldPose(Vector3 visualCenter, Quaternion rotation)
		{
			this.EnsureCenteredPivot();
			this.PivotTransform.SetPositionAndRotation(visualCenter, rotation);
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0019651C File Offset: 0x0019471C
		public void SnapVisibleGeometryToSurface(Vector3 surfacePoint, Vector3 surfaceNormal, float clearance)
		{
			if (this._renderer == null)
			{
				this._renderer = base.GetComponent<Renderer>();
			}
			if (this._renderer == null || surfaceNormal.sqrMagnitude < 0.0001f)
			{
				return;
			}
			this.EnsureCenteredPivot();
			Vector3 normalized = surfaceNormal.normalized;
			float num = float.PositiveInfinity;
			bool flag = false;
			SkinnedMeshRenderer skinnedMeshRenderer = this._renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
			{
				Mesh mesh = new Mesh
				{
					name = "Photo Print Surface Snap"
				};
				skinnedMeshRenderer.BakeMesh(mesh);
				Vector3[] vertices = mesh.vertices;
				Matrix4x4 localToWorldMatrix = skinnedMeshRenderer.transform.localToWorldMatrix;
				for (int i = 0; i < vertices.Length; i++)
				{
					float num2 = Vector3.Dot(localToWorldMatrix.MultiplyPoint3x4(vertices[i]) - surfacePoint, normalized);
					if (num2 < num)
					{
						num = num2;
					}
				}
				flag = vertices.Length != 0;
				Object.Destroy(mesh);
			}
			else
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				Mesh mesh2 = ((component != null) ? component.sharedMesh : null);
				if (mesh2 != null)
				{
					Vector3[] vertices2 = mesh2.vertices;
					Matrix4x4 localToWorldMatrix2 = component.transform.localToWorldMatrix;
					for (int j = 0; j < vertices2.Length; j++)
					{
						float num3 = Vector3.Dot(localToWorldMatrix2.MultiplyPoint3x4(vertices2[j]) - surfacePoint, normalized);
						if (num3 < num)
						{
							num = num3;
						}
					}
					flag = vertices2.Length != 0;
				}
			}
			if (!flag)
			{
				Bounds bounds = this._renderer.bounds;
				Vector3 vector = bounds.center - surfacePoint;
				float num4 = Mathf.Abs(normalized.x) * bounds.extents.x + Mathf.Abs(normalized.y) * bounds.extents.y + Mathf.Abs(normalized.z) * bounds.extents.z;
				num = Vector3.Dot(vector, normalized) - num4;
			}
			float num5 = Mathf.Max(0f, Mathf.Max(0f, clearance) - num);
			if (num5 > 0f)
			{
				this.PivotTransform.position += normalized * num5;
			}
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x00196744 File Offset: 0x00194944
		public bool CanInteract(GameObject player)
		{
			if (this._holder == null && player != null)
			{
				vThirdPersonController vThirdPersonController = player.GetComponentInChildren<vThirdPersonController>(true);
				if (vThirdPersonController == null)
				{
					vThirdPersonController = player.GetComponentInParent<vThirdPersonController>();
				}
				this._holder = PhotoBoothPrintHolder.GetOrCreate(vThirdPersonController);
			}
			return this._armed && !this._held && player != null && this._holder != null;
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x001967B3 File Offset: 0x001949B3
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this._holder.StoreInInventory(this);
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x001967D1 File Offset: 0x001949D1
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x001967DC File Offset: 0x001949DC
		public void SetHeld(bool held)
		{
			this._held = held;
			this._armed = !held;
			if (this._collider != null)
			{
				this._collider.enabled = !held;
			}
			if (held)
			{
				this.EnsureCenteredPivot();
				this.PivotTransform.SetParent(null, true);
			}
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x00196830 File Offset: 0x00194A30
		public PhotoBoothPhotoItemSO CreateInventoryItem()
		{
			if (this._collected || this._portraitTexture == null || this._photoTexture == null || this._visualPrefabSource == null)
			{
				return null;
			}
			return PhotoBoothPhotoItemSO.CreateRuntime(this._portraitTexture, this._photoTexture, this._visualPrefabSource, this._targetWorldScale, this._worldRestRotation, this._photoSequence, this._archiveId);
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x001968A0 File Offset: 0x00194AA0
		public void CommitInventoryCollection()
		{
			if (this._collected)
			{
				return;
			}
			this._collected = true;
			this._armed = false;
			this._held = false;
			this._ownsTextures = false;
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x001968F8 File Offset: 0x00194AF8
		public float GetWorldSizeAlong(Vector3 worldAxis)
		{
			if (worldAxis.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			worldAxis.Normalize();
			Vector3 lossyScale = base.transform.lossyScale;
			return Mathf.Abs(Vector3.Dot(worldAxis, base.transform.right)) * this._localVisualSize.x * Mathf.Abs(lossyScale.x) + Mathf.Abs(Vector3.Dot(worldAxis, base.transform.up)) * this._localVisualSize.y * Mathf.Abs(lossyScale.y) + Mathf.Abs(Vector3.Dot(worldAxis, base.transform.forward)) * this._localVisualSize.z * Mathf.Abs(lossyScale.z);
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x001969B8 File Offset: 0x00194BB8
		public void Drop(Vector3 position, Quaternion rotation)
		{
			this.EnsureCenteredPivot();
			this.PivotTransform.SetParent(null, true);
			this.SetVisualWorldPose(position, rotation);
			this._held = false;
			this._armed = true;
			if (this._collider != null)
			{
				this._collider.enabled = true;
			}
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x00196A08 File Offset: 0x00194C08
		private void ApplyPhotoMaterial()
		{
			if (this._renderer == null || this._photoTexture == null)
			{
				return;
			}
			Material[] sharedMaterials = this._renderer.sharedMaterials;
			if (sharedMaterials == null || sharedMaterials.Length == 0 || sharedMaterials[0] == null)
			{
				return;
			}
			if (this._frontMaterialSource == null)
			{
				this._frontMaterialSource = sharedMaterials[0];
			}
			if (this._frontMaterial == null)
			{
				this._frontMaterial = new Material(this._frontMaterialSource)
				{
					name = "Photo Print Front (Runtime)",
					hideFlags = HideFlags.DontSave
				};
			}
			PhotoBoothPrint.SetTextureIfPresent(this._frontMaterial, "_MainTex", this._photoTexture);
			PhotoBoothPrint.SetTextureIfPresent(this._frontMaterial, "_BaseMap", this._photoTexture);
			PhotoBoothPrint.SetTextureIfPresent(this._frontMaterial, "_BaseColorMap", this._photoTexture);
			this.ApplyAspectPreservingTextureMapping(this._frontMaterial, this._photoTexture);
			PhotoBoothPrint.SetColorIfPresent(this._frontMaterial, "_Color", Color.white);
			PhotoBoothPrint.SetColorIfPresent(this._frontMaterial, "_BaseColor", Color.white);
			sharedMaterials[0] = this._frontMaterial;
			this._renderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x00196B30 File Offset: 0x00194D30
		private void CacheHidePhotoBlendShape()
		{
			this._skinnedRenderer = this._renderer as SkinnedMeshRenderer;
			this._hidePhotoBlendShapeIndex = -1;
			Mesh mesh = ((this._skinnedRenderer != null) ? this._skinnedRenderer.sharedMesh : null);
			if (mesh == null)
			{
				return;
			}
			for (int i = 0; i < mesh.blendShapeCount; i++)
			{
				if (string.Equals(mesh.GetBlendShapeName(i), "Hide_Photo", StringComparison.OrdinalIgnoreCase))
				{
					this._hidePhotoBlendShapeIndex = i;
					return;
				}
			}
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x00196BAC File Offset: 0x00194DAC
		private void CacheVisualMetrics()
		{
			SkinnedMeshRenderer skinnedMeshRenderer = this._renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				this._localVisualCenter = skinnedMeshRenderer.localBounds.center;
				this._localVisualSize = skinnedMeshRenderer.localBounds.size;
				return;
			}
			if (this._renderer == null)
			{
				return;
			}
			this._localVisualCenter = base.transform.InverseTransformPoint(this._renderer.bounds.center);
			Vector3 lossyScale = base.transform.lossyScale;
			this._localVisualSize = new Vector3(this._renderer.bounds.size.x / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x)), this._renderer.bounds.size.y / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y)), this._renderer.bounds.size.z / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z)));
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x00196CC8 File Offset: 0x00194EC8
		private void EnsureCenteredPivot()
		{
			if (this._centeredPivot != null)
			{
				return;
			}
			Transform parent = base.transform.parent;
			int siblingIndex = base.transform.GetSiblingIndex();
			PhotoBoothPrint.SetWorldScale(base.transform, this._targetWorldScale);
			Vector3 vector = base.transform.TransformPoint(this._localVisualCenter);
			Quaternion rotation = base.transform.rotation;
			GameObject gameObject = new GameObject(base.name + "_CenteredPivot");
			this._centeredPivot = gameObject.transform;
			this._ownsCenteredPivot = true;
			this._centeredPivot.SetPositionAndRotation(vector, rotation);
			this._centeredPivot.SetParent(parent, true);
			this._centeredPivot.SetSiblingIndex(siblingIndex);
			base.transform.SetParent(this._centeredPivot, true);
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x00196D90 File Offset: 0x00194F90
		private void RecenterPivotPreservingVisual()
		{
			if (this._centeredPivot == null)
			{
				return;
			}
			Vector3 vector = base.transform.TransformPoint(this._localVisualCenter) - this._centeredPivot.position;
			if (vector.sqrMagnitude <= 1E-10f)
			{
				return;
			}
			this._centeredPivot.position += vector;
			base.transform.position -= vector;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00196E0C File Offset: 0x0019500C
		private void ApplyAspectPreservingTextureMapping(Material material, Texture texture)
		{
			Vector2 vector;
			Vector2 vector2;
			float num;
			float num2;
			if (!this.TryGetFrontUvMetrics(out vector, out vector2, out num, out num2))
			{
				return;
			}
			Vector2 vector3 = vector2 - vector;
			if (vector3.x <= 0.0001f || vector3.y <= 0.0001f || num <= 0.0001f || num2 <= 0.0001f || texture.width <= 0 || texture.height <= 0)
			{
				return;
			}
			float num3 = vector3.x * num / (vector3.y * num2);
			float num4 = (float)texture.width / (float)texture.height;
			Vector2 one = Vector2.one;
			if (num4 < num3)
			{
				one.y = num4 / num3;
			}
			else if (num4 > num3)
			{
				one.x = num3 / num4;
			}
			Vector2 vector4 = (Vector2.one - one) * 0.5f;
			Vector2 vector5 = new Vector2(one.x / vector3.x, one.y / vector3.y);
			Vector2 vector6 = vector4 - Vector2.Scale(vector, vector5);
			if (this._photoTextureRequires180Rotation)
			{
				vector5 = -vector5;
				vector6 = Vector2.one - vector6;
			}
			PhotoBoothPrint.SetTextureTransformIfPresent(material, "_MainTex", vector5, vector6);
			PhotoBoothPrint.SetTextureTransformIfPresent(material, "_BaseMap", vector5, vector6);
			PhotoBoothPrint.SetTextureTransformIfPresent(material, "_BaseColorMap", vector5, vector6);
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x00196F5C File Offset: 0x0019515C
		private bool TryGetFrontUvMetrics(out Vector2 uvMin, out Vector2 uvMax, out float worldUnitsPerU, out float worldUnitsPerV)
		{
			if (this._frontUvMetricsCached)
			{
				uvMin = this._frontUvMin;
				uvMax = this._frontUvMax;
				worldUnitsPerU = this._frontWorldUnitsPerU;
				worldUnitsPerV = this._frontWorldUnitsPerV;
				return true;
			}
			uvMin = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			uvMax = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			worldUnitsPerU = 0f;
			worldUnitsPerV = 0f;
			SkinnedMeshRenderer skinnedMeshRenderer = this._renderer as SkinnedMeshRenderer;
			Mesh mesh;
			if (skinnedMeshRenderer != null)
			{
				mesh = skinnedMeshRenderer.sharedMesh;
			}
			else
			{
				MeshFilter component = base.GetComponent<MeshFilter>();
				mesh = ((component != null) ? component.sharedMesh : null);
			}
			if (mesh == null || mesh.subMeshCount == 0 || mesh.uv == null || mesh.uv.Length != mesh.vertexCount)
			{
				return false;
			}
			Vector2[] uv = mesh.uv;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.GetTriangles(0);
			if (triangles == null || triangles.Length < 3)
			{
				return false;
			}
			for (int i = 0; i < triangles.Length; i++)
			{
				Vector2 vector = uv[triangles[i]];
				uvMin = Vector2.Min(uvMin, vector);
				uvMax = Vector2.Max(uvMax, vector);
			}
			float num = 0f;
			for (int j = 0; j <= triangles.Length - 3; j += 3)
			{
				int num2 = triangles[j];
				int num3 = triangles[j + 1];
				int num4 = triangles[j + 2];
				Vector2 vector2 = uv[num3] - uv[num2];
				Vector2 vector3 = uv[num4] - uv[num2];
				float num5 = vector2.x * vector3.y - vector2.y * vector3.x;
				if (Mathf.Abs(num5) > 1E-06f)
				{
					Vector3 vector4 = vertices[num3] - vertices[num2];
					Vector3 vector5 = vertices[num4] - vertices[num2];
					float sqrMagnitude = Vector3.Cross(vector4, vector5).sqrMagnitude;
					if (sqrMagnitude > num)
					{
						Vector3 vector6 = (vector4 * vector3.y - vector5 * vector2.y) / num5;
						Vector3 vector7 = (vector5 * vector2.x - vector4 * vector3.x) / num5;
						if (vector6.sqrMagnitude > 1E-06f && vector7.sqrMagnitude > 1E-06f)
						{
							num = sqrMagnitude;
							worldUnitsPerU = vector6.magnitude;
							worldUnitsPerV = vector7.magnitude;
						}
					}
				}
			}
			if (worldUnitsPerU <= 0f || worldUnitsPerV <= 0f)
			{
				return false;
			}
			this._frontUvMetricsCached = true;
			this._frontUvMin = uvMin;
			this._frontUvMax = uvMax;
			this._frontWorldUnitsPerU = worldUnitsPerU;
			this._frontWorldUnitsPerV = worldUnitsPerV;
			return true;
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0019724C File Offset: 0x0019544C
		private void EnsureCollider()
		{
			this._collider = base.GetComponent<BoxCollider>();
			if (this._collider == null)
			{
				this._collider = base.gameObject.AddComponent<BoxCollider>();
			}
			SkinnedMeshRenderer skinnedMeshRenderer = this._renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				this._collider.center = skinnedMeshRenderer.localBounds.center;
				this._collider.size = skinnedMeshRenderer.localBounds.size;
				return;
			}
			if (this._renderer != null)
			{
				this._collider.center = base.transform.InverseTransformPoint(this._renderer.bounds.center);
				Vector3 lossyScale = base.transform.lossyScale;
				this._collider.size = new Vector3(this._renderer.bounds.size.x / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x)), this._renderer.bounds.size.y / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y)), this._renderer.bounds.size.z / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z)));
			}
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x001973A6 File Offset: 0x001955A6
		private static void SetTextureIfPresent(Material material, string property, Texture texture)
		{
			if (material.HasProperty(property))
			{
				material.SetTexture(property, texture);
			}
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x001973BC File Offset: 0x001955BC
		private Vector2 GetFrontTextureTransform(bool scale)
		{
			if (this._frontMaterial == null)
			{
				if (!scale)
				{
					return Vector2.zero;
				}
				return Vector2.one;
			}
			else
			{
				string text = (this._frontMaterial.HasProperty("_MainTex") ? "_MainTex" : (this._frontMaterial.HasProperty("_BaseMap") ? "_BaseMap" : null));
				if (string.IsNullOrEmpty(text))
				{
					if (!scale)
					{
						return Vector2.zero;
					}
					return Vector2.one;
				}
				else
				{
					if (!scale)
					{
						return this._frontMaterial.GetTextureOffset(text);
					}
					return this._frontMaterial.GetTextureScale(text);
				}
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0019744D File Offset: 0x0019564D
		private static void SetTextureTransformIfPresent(Material material, string property, Vector2 scale, Vector2 offset)
		{
			if (!material.HasProperty(property))
			{
				return;
			}
			material.SetTextureScale(property, scale);
			material.SetTextureOffset(property, offset);
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x00197469 File Offset: 0x00195669
		private static void SetColorIfPresent(Material material, string property, Color color)
		{
			if (material.HasProperty(property))
			{
				material.SetColor(property, color);
			}
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0019747C File Offset: 0x0019567C
		private void OnDestroy()
		{
			PhotoBoothPrintNpcDirector.UnregisterWorldPrint(this);
			if (this._holder != null)
			{
				this._holder.NotifyPrintDestroyed(this);
			}
			if (this._frontMaterial != null)
			{
				Object.Destroy(this._frontMaterial);
			}
			if (this._ownsTextures)
			{
				if (this._portraitTexture != null)
				{
					Object.Destroy(this._portraitTexture);
				}
				if (this._photoTexture != null && this._photoTexture != this._portraitTexture)
				{
					Object.Destroy(this._photoTexture);
				}
			}
			if (this._npcConsumedItem != null)
			{
				PhotoBoothPhotoItemSO npcConsumedItem = this._npcConsumedItem;
				this._npcConsumedItem = null;
				npcConsumedItem.DisposeRuntimeAssets();
			}
			if (this._ownsCenteredPivot && this._centeredPivot != null)
			{
				GameObject gameObject = this._centeredPivot.gameObject;
				this._centeredPivot = null;
				this._ownsCenteredPivot = false;
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x00197570 File Offset: 0x00195770
		private static void SetWorldScale(Transform target, Vector3 worldScale)
		{
			Transform parent = target.parent;
			if (parent == null)
			{
				target.localScale = worldScale;
				return;
			}
			Vector3 lossyScale = parent.lossyScale;
			target.localScale = new Vector3(worldScale.x / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x)), worldScale.y / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y)), worldScale.z / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z)));
		}

		// Token: 0x0400437D RID: 17277
		private const string HidePhotoBlendShape = "Hide_Photo";

		// Token: 0x0400437E RID: 17278
		private Renderer _renderer;

		// Token: 0x0400437F RID: 17279
		private SkinnedMeshRenderer _skinnedRenderer;

		// Token: 0x04004380 RID: 17280
		private int _hidePhotoBlendShapeIndex = -1;

		// Token: 0x04004381 RID: 17281
		private BoxCollider _collider;

		// Token: 0x04004382 RID: 17282
		private InteractionConfigSO _interactionConfig;

		// Token: 0x04004383 RID: 17283
		private PhotoBoothPrintHolder _holder;

		// Token: 0x04004384 RID: 17284
		private Texture2D _portraitTexture;

		// Token: 0x04004385 RID: 17285
		private Texture2D _photoTexture;

		// Token: 0x04004386 RID: 17286
		private bool _photoTextureRequires180Rotation;

		// Token: 0x04004387 RID: 17287
		private Material _frontMaterial;

		// Token: 0x04004388 RID: 17288
		private Material _frontMaterialSource;

		// Token: 0x04004389 RID: 17289
		private bool _armed;

		// Token: 0x0400438A RID: 17290
		private bool _held;

		// Token: 0x0400438B RID: 17291
		private bool _collected;

		// Token: 0x0400438C RID: 17292
		private bool _ownsTextures;

		// Token: 0x0400438D RID: 17293
		private GameObject _visualPrefabSource;

		// Token: 0x0400438E RID: 17294
		private Vector3 _targetWorldScale = Vector3.one;

		// Token: 0x0400438F RID: 17295
		private Quaternion _worldRestRotation = Quaternion.identity;

		// Token: 0x04004390 RID: 17296
		private int _photoSequence;

		// Token: 0x04004391 RID: 17297
		private string _archiveId;

		// Token: 0x04004392 RID: 17298
		private Vector3 _localVisualCenter;

		// Token: 0x04004393 RID: 17299
		private Vector3 _localVisualSize = new Vector3(0.2f, 0.03f, 0.1f);

		// Token: 0x04004394 RID: 17300
		private bool _frontUvMetricsCached;

		// Token: 0x04004395 RID: 17301
		private Vector2 _frontUvMin;

		// Token: 0x04004396 RID: 17302
		private Vector2 _frontUvMax;

		// Token: 0x04004397 RID: 17303
		private float _frontWorldUnitsPerU;

		// Token: 0x04004398 RID: 17304
		private float _frontWorldUnitsPerV;

		// Token: 0x04004399 RID: 17305
		private Transform _centeredPivot;

		// Token: 0x0400439A RID: 17306
		private bool _ownsCenteredPivot;

		// Token: 0x0400439B RID: 17307
		private Object _npcReservationOwner;

		// Token: 0x0400439C RID: 17308
		private bool _consumedByNpc;

		// Token: 0x0400439D RID: 17309
		private PhotoBoothPhotoItemSO _npcConsumedItem;

		// Token: 0x0400439E RID: 17310
		private bool _hasOutputBasePose;

		// Token: 0x0400439F RID: 17311
		private Vector3 _outputBaseVisualCenter;

		// Token: 0x040043A0 RID: 17312
		private Coroutine _outputStackRoutine;

		// Token: 0x040043A1 RID: 17313
		private const float OutputStackSlideStep = 0.0075f;

		// Token: 0x040043A2 RID: 17314
		private const float OutputStackLiftStep = 0.0022f;

		// Token: 0x040043A3 RID: 17315
		private const float OutputStackMoveDuration = 0.16f;
	}
}
