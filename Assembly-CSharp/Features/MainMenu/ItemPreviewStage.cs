using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.CensorBars;
using Features.HideFlash;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200103F RID: 4159
	public sealed class ItemPreviewStage : MonoBehaviour
	{
		// Token: 0x06007E46 RID: 32326 RVA: 0x0033A17C File Offset: 0x0033837C
		public void Show(InventoryItemSO item, int variantIndex)
		{
			this.Hide();
			if (item == null || this.modelAnchor == null || this.previewCamera == null || this.targetImage == null)
			{
				return;
			}
			bool flag;
			GameObject gameObject = ItemPreviewStage.ResolveSource(item, out flag);
			if (gameObject == null)
			{
				return;
			}
			this.EnsureRenderTexture();
			this.modelAnchor.localRotation = Quaternion.identity;
			this.modelAnchor.gameObject.SetActive(false);
			this._instance = Object.Instantiate<GameObject>(gameObject, this.modelAnchor);
			this._instance.name = "Preview - " + item.name;
			this._instance.transform.localPosition = Vector3.zero;
			this._instance.transform.localRotation = gameObject.transform.localRotation;
			ItemPreviewStage.StripRuntimeComponents(this._instance);
			StreamerCensorImage.BindItem(this.targetImage, item);
			ItemPreviewStage.SetLayerRecursively(this._instance.transform, 31);
			MCSurfaceVariantSO mcsurfaceVariantSO = ItemVariantLook.ResolveVariant(item, variantIndex);
			ItemVariantLook.ApplyVariantToRenderers(this._instance, mcsurfaceVariantSO);
			this.MakeMaterialsDoubleSided(mcsurfaceVariantSO != null);
			this._instance.SetActive(true);
			this.modelAnchor.gameObject.SetActive(true);
			Bounds bounds;
			if (!ItemPreviewStage.TryGetVisibleBounds(this._instance, out bounds))
			{
				this.Hide();
				return;
			}
			Vector3 vector = this.modelAnchor.position - bounds.center;
			this._instance.transform.position += vector;
			bounds.center += vector;
			this.Frame(bounds, flag);
			if (this.spinner != null)
			{
				this.spinner.SetTarget(this.modelAnchor, this.previewCamera);
			}
			this.targetImage.texture = this._renderTexture;
			this.targetImage.enabled = true;
			this._rendering = true;
			this.previewCamera.Render();
		}

		// Token: 0x06007E47 RID: 32327 RVA: 0x0033A380 File Offset: 0x00338580
		public void Hide()
		{
			this._rendering = false;
			if (this.spinner != null)
			{
				this.spinner.SetTarget(null, null);
			}
			if (this._instance != null)
			{
				this._instance.SetActive(false);
				ItemPreviewStage.ReleaseObject(this._instance);
			}
			this._instance = null;
			foreach (Material material in this._previewMaterials.Values)
			{
				ItemPreviewStage.ReleaseObject(material);
			}
			this._previewMaterials.Clear();
			if (this.modelAnchor != null)
			{
				this.modelAnchor.localRotation = Quaternion.identity;
				this.modelAnchor.gameObject.SetActive(true);
			}
			this.ReleaseRenderTexture();
		}

		// Token: 0x06007E48 RID: 32328 RVA: 0x0033A464 File Offset: 0x00338664
		private void LateUpdate()
		{
			if (this._rendering && this.previewCamera != null && this._renderTexture != null)
			{
				this.previewCamera.Render();
			}
		}

		// Token: 0x06007E49 RID: 32329 RVA: 0x0033A498 File Offset: 0x00338698
		private void MakeMaterialsDoubleSided(bool variantOwnsMaterials)
		{
			foreach (Renderer renderer in this._instance.GetComponentsInChildren<Renderer>(true))
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					Material material = sharedMaterials[j];
					if (!(material == null))
					{
						Material material2;
						if (!this._previewMaterials.TryGetValue(material, out material2))
						{
							material2 = ((variantOwnsMaterials && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)) ? material : new Material(material));
							material2.name = material.name + " (Collection double sided)";
							material2.hideFlags = HideFlags.DontSave;
							if (material2.HasProperty("_Cull"))
							{
								material2.SetFloat("_Cull", 0f);
							}
							if (material2.HasProperty("_CullMode"))
							{
								material2.SetFloat("_CullMode", 0f);
							}
							if (material2.HasProperty("_CullModeForward"))
							{
								material2.SetFloat("_CullModeForward", 0f);
							}
							if (material2.HasProperty("_TwoSided"))
							{
								material2.SetFloat("_TwoSided", 0f);
							}
							if (material2.HasProperty("_FlipNormal"))
							{
								material2.SetFloat("_FlipNormal", 1f);
							}
							if (material2.HasProperty("_DoubleSidedEnable"))
							{
								material2.SetFloat("_DoubleSidedEnable", 1f);
								material2.EnableKeyword("_DOUBLESIDED_ON");
							}
							this._previewMaterials.Add(material, material2);
						}
						sharedMaterials[j] = material2;
					}
				}
				renderer.sharedMaterials = sharedMaterials;
			}
		}

		// Token: 0x06007E4A RID: 32330 RVA: 0x0033A633 File Offset: 0x00338833
		private void OnDisable()
		{
			this.Hide();
		}

		// Token: 0x06007E4B RID: 32331 RVA: 0x0033A63B File Offset: 0x0033883B
		private void OnDestroy()
		{
			this.ReleaseRenderTexture();
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x0033A644 File Offset: 0x00338844
		private void EnsureRenderTexture()
		{
			if (this._renderTexture != null)
			{
				return;
			}
			this._renderTexture = new RenderTexture(768, 768, 24, RenderTextureFormat.ARGB32)
			{
				name = "Collection Item Preview",
				antiAliasing = 2
			};
			this._renderTexture.Create();
			this.previewCamera.targetTexture = this._renderTexture;
			this.previewCamera.enabled = false;
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x0033A6B4 File Offset: 0x003388B4
		private void ReleaseRenderTexture()
		{
			if (this.targetImage != null)
			{
				this.targetImage.texture = null;
				this.targetImage.enabled = false;
			}
			if (this.previewCamera != null)
			{
				this.previewCamera.targetTexture = null;
			}
			if (this._renderTexture == null)
			{
				return;
			}
			this._renderTexture.Release();
			ItemPreviewStage.ReleaseObject(this._renderTexture);
			this._renderTexture = null;
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x0033A730 File Offset: 0x00338930
		private void Frame(Bounds bounds, bool frontal)
		{
			float num = Mathf.Max(bounds.size.magnitude, 1f) * 2f;
			this.previewCamera.orthographic = true;
			this.previewCamera.aspect = 1f;
			this.previewCamera.nearClipPlane = 0.01f;
			this.previewCamera.farClipPlane = num + bounds.extents.magnitude * 2f + 1f;
			float num2 = (frontal ? Mathf.Max(0.1f, 0.5f * Mathf.Max(bounds.size.x, bounds.size.y) * 1.25f) : Mathf.Max(0.1f, bounds.extents.magnitude * 1.05f));
			this.previewCamera.orthographicSize = Mathf.Max(num2, bounds.extents.magnitude * 1.08f);
			if (frontal)
			{
				this.previewCamera.transform.position = bounds.center + new Vector3(0f, 0f, num);
				this.previewCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			}
			else
			{
				Vector3 normalized = new Vector3(0.5f, 0.25f, 1f).normalized;
				this.previewCamera.transform.position = bounds.center + normalized * num;
				this.previewCamera.transform.LookAt(bounds.center);
			}
			int minValue = int.MinValue;
			this.previewCamera.cullingMask = minValue;
			if (this.keyLight != null)
			{
				this.keyLight.cullingMask = minValue;
			}
			if (this.fillLight != null)
			{
				this.fillLight.cullingMask = minValue;
			}
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x0033A924 File Offset: 0x00338B24
		private static GameObject ResolveSource(InventoryItemSO item, out bool frontal)
		{
			frontal = false;
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.mcStylePreset != null && wearableItemSO.mcStylePreset.physicalPrefab != null)
			{
				frontal = true;
				return wearableItemSO.mcStylePreset.physicalPrefab;
			}
			HideFlashItemSO hideFlashItemSO = item as HideFlashItemSO;
			if (hideFlashItemSO != null && hideFlashItemSO.propPrefab != null)
			{
				return hideFlashItemSO.propPrefab;
			}
			if (item.handPrefab != null)
			{
				return item.handPrefab;
			}
			return item.floorPrefab;
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x0033A9A8 File Offset: 0x00338BA8
		private static void StripRuntimeComponents(GameObject root)
		{
			foreach (Behaviour behaviour in root.GetComponentsInChildren<Behaviour>(true))
			{
				if (behaviour != null && !(behaviour is CollarIdCard))
				{
					behaviour.enabled = false;
				}
			}
			Collider[] componentsInChildren2 = root.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			foreach (Rigidbody rigidbody in root.GetComponentsInChildren<Rigidbody>(true))
			{
				rigidbody.isKinematic = true;
				rigidbody.detectCollisions = false;
			}
			ParticleSystem[] componentsInChildren4 = root.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				renderer.forceRenderingOff = false;
				if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					renderer.shadowCastingMode = ShadowCastingMode.On;
				}
			}
		}

		// Token: 0x06007E51 RID: 32337 RVA: 0x0033AA8C File Offset: 0x00338C8C
		private static bool TryGetVisibleBounds(GameObject root, out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(false))
			{
				if (renderer.enabled && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer))
				{
					Bounds bounds2 = renderer.bounds;
					SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
					if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
					{
						Mesh mesh = new Mesh();
						try
						{
							skinnedMeshRenderer.BakeMesh(mesh, true);
							if (mesh.vertexCount == 0)
							{
								goto IL_01DB;
							}
							Bounds bounds3 = mesh.bounds;
							Matrix4x4 localToWorldMatrix = skinnedMeshRenderer.transform.localToWorldMatrix;
							Vector3 vector = localToWorldMatrix.MultiplyVector(new Vector3(bounds3.extents.x, 0f, 0f));
							Vector3 vector2 = localToWorldMatrix.MultiplyVector(new Vector3(0f, bounds3.extents.y, 0f));
							Vector3 vector3 = localToWorldMatrix.MultiplyVector(new Vector3(0f, 0f, bounds3.extents.z));
							bounds2 = new Bounds(localToWorldMatrix.MultiplyPoint3x4(bounds3.center), 2f * new Vector3(Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x), Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y), Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z)));
							skinnedMeshRenderer.updateWhenOffscreen = true;
						}
						finally
						{
							ItemPreviewStage.ReleaseObject(mesh);
						}
					}
					if (bounds2.size.sqrMagnitude >= 1E-07f)
					{
						if (!flag)
						{
							bounds = bounds2;
						}
						else
						{
							bounds.Encapsulate(bounds2);
						}
						flag = true;
					}
				}
				IL_01DB:;
			}
			return flag;
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void ReleaseObject(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x0033ACA0 File Offset: 0x00338EA0
		private static void SetLayerRecursively(Transform root, int layer)
		{
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				ItemPreviewStage.SetLayerRecursively(root.GetChild(i), layer);
			}
		}

		// Token: 0x040081DF RID: 33247
		private const int PreviewLayer = 31;

		// Token: 0x040081E0 RID: 33248
		private const int TextureSize = 768;

		// Token: 0x040081E1 RID: 33249
		[SerializeField]
		private Transform modelAnchor;

		// Token: 0x040081E2 RID: 33250
		[SerializeField]
		private Camera previewCamera;

		// Token: 0x040081E3 RID: 33251
		[SerializeField]
		private Light keyLight;

		// Token: 0x040081E4 RID: 33252
		[SerializeField]
		private Light fillLight;

		// Token: 0x040081E5 RID: 33253
		[SerializeField]
		private RawImage targetImage;

		// Token: 0x040081E6 RID: 33254
		[SerializeField]
		private ItemPreviewFreeRotator spinner;

		// Token: 0x040081E7 RID: 33255
		private GameObject _instance;

		// Token: 0x040081E8 RID: 33256
		private RenderTexture _renderTexture;

		// Token: 0x040081E9 RID: 33257
		private bool _rendering;

		// Token: 0x040081EA RID: 33258
		private readonly Dictionary<Material, Material> _previewMaterials = new Dictionary<Material, Material>();
	}
}
