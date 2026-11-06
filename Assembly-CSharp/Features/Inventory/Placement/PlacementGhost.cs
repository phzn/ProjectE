using System;
using System.Collections.Generic;
using Features.CensorBars;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Inventory.Placement
{
	// Token: 0x02001149 RID: 4425
	public class PlacementGhost : MonoBehaviour
	{
		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06008673 RID: 34419 RVA: 0x0036E574 File Offset: 0x0036C774
		public bool IsActive
		{
			get
			{
				return this._ghostInstance != null && this._ghostInstance.activeSelf;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06008674 RID: 34420 RVA: 0x0036E591 File Offset: 0x0036C791
		public GameObject PreviewInstance
		{
			get
			{
				return this._ghostInstance;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06008675 RID: 34421 RVA: 0x0036E599 File Offset: 0x0036C799
		public Vector3 PreviewWorldScale
		{
			get
			{
				if (!(this._ghostInstance != null))
				{
					return Vector3.zero;
				}
				return this._ghostInstance.transform.lossyScale;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06008676 RID: 34422 RVA: 0x0036E5C0 File Offset: 0x0036C7C0
		public Bounds PreviewBounds
		{
			get
			{
				if (this._renderers == null || this._renderers.Length == 0)
				{
					return default(Bounds);
				}
				Bounds bounds = this._renderers[0].bounds;
				for (int i = 1; i < this._renderers.Length; i++)
				{
					bounds.Encapsulate(this._renderers[i].bounds);
				}
				return bounds;
			}
		}

		// Token: 0x06008677 RID: 34423 RVA: 0x0036E620 File Offset: 0x0036C820
		public void Spawn(GameObject sourcePrefab, Material ghostMaterial, Vector3? targetWorldScale = null, bool centerOnRenderedVisual = false)
		{
			this.Destroy();
			if (sourcePrefab == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[GhostHolder_Inactive]");
			gameObject.SetActive(false);
			this._ghostInstance = Object.Instantiate<GameObject>(sourcePrefab, gameObject.transform);
			this._ghostInstance.name = "[PlacementGhost]";
			PlacementGhost.RevealPlacementVisual(this._ghostInstance);
			PlacementGhost.StripScriptsImmediate(this._ghostInstance);
			PlacementGhost.StripPhysicsImmediate(this._ghostInstance);
			if (sourcePrefab.GetComponentInChildren<StreamerCensorTarget>(true) != null)
			{
				this._ghostInstance.AddComponent<StreamerCensorTarget>();
			}
			PlacementGhost.SetLayerRecursive(this._ghostInstance, 0);
			this.ApplyGhostMaterial(ghostMaterial);
			this._ghostInstance.SetActive(false);
			this._ghostInstance.transform.SetParent(null, false);
			if (targetWorldScale != null)
			{
				PlacementGhost.SetWorldScale(this._ghostInstance.transform, targetWorldScale.Value);
			}
			if (centerOnRenderedVisual)
			{
				this.CenterGhostOnRenderedVisual();
			}
			Object.Destroy(gameObject);
			this._ghostInstance.SetActive(false);
		}

		// Token: 0x06008678 RID: 34424 RVA: 0x0036E71C File Offset: 0x0036C91C
		public void SpawnSilhouette(Vector2 worldSize)
		{
			this.Destroy();
			if (worldSize.x <= 0.001f || worldSize.y <= 0.001f)
			{
				return;
			}
			this._ghostInstance = new GameObject("[PlacementGhost]");
			this._ghostInstance.layer = 0;
			float num = worldSize.x * 0.5f;
			float num2 = worldSize.y * 0.5f;
			this._runtimeMesh = new Mesh
			{
				name = "Photo Placement Silhouette"
			};
			this._runtimeMesh.vertices = new Vector3[]
			{
				new Vector3(-num2, 0f, -num),
				new Vector3(-num2, 0f, num),
				new Vector3(num2, 0f, num),
				new Vector3(num2, 0f, -num)
			};
			this._runtimeMesh.normals = new Vector3[]
			{
				Vector3.up,
				Vector3.up,
				Vector3.up,
				Vector3.up
			};
			this._runtimeMesh.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			this._runtimeMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
			this._runtimeMesh.RecalculateBounds();
			this._ghostInstance.AddComponent<MeshFilter>().sharedMesh = this._runtimeMesh;
			MeshRenderer meshRenderer = this._ghostInstance.AddComponent<MeshRenderer>();
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
			if (shader == null)
			{
				shader = Shader.Find("Unlit/Color");
			}
			if (shader == null)
			{
				shader = Shader.Find("Sprites/Default");
			}
			if (shader == null)
			{
				Object.Destroy(this._ghostInstance);
				this._ghostInstance = null;
				return;
			}
			Material material = new Material(shader)
			{
				name = "Photo Placement Green (Runtime)"
			};
			PlacementGhost.SetMaterialTransparent(material);
			PlacementGhost.ConfigureGhostMaterial(material);
			meshRenderer.sharedMaterial = material;
			this._renderers = new Renderer[] { meshRenderer };
			this._ghostMaterials = new Material[] { material };
			this._ghostInstance.SetActive(false);
		}

		// Token: 0x06008679 RID: 34425 RVA: 0x0036E9A0 File Offset: 0x0036CBA0
		private void CenterGhostOnRenderedVisual()
		{
			if (this._ghostInstance == null || this._renderers == null || this._renderers.Length == 0)
			{
				return;
			}
			Bounds bounds = this._renderers[0].bounds;
			for (int i = 1; i < this._renderers.Length; i++)
			{
				bounds.Encapsulate(this._renderers[i].bounds);
			}
			GameObject ghostInstance = this._ghostInstance;
			Transform transform = ghostInstance.transform;
			Vector3 lossyScale = transform.lossyScale;
			Quaternion rotation = transform.rotation;
			GameObject gameObject = new GameObject("[PlacementGhost]");
			gameObject.SetActive(false);
			Transform transform2 = gameObject.transform;
			transform2.SetPositionAndRotation(bounds.center, rotation);
			transform2.localScale = lossyScale;
			ghostInstance.name = "[PlacementGhost_Visual]";
			transform.SetParent(transform2, true);
			this._ghostInstance = gameObject;
		}

		// Token: 0x0600867A RID: 34426 RVA: 0x0036EA74 File Offset: 0x0036CC74
		private static void SetWorldScale(Transform target, Vector3 worldScale)
		{
			if (target == null)
			{
				return;
			}
			Transform parent = target.parent;
			if (parent == null)
			{
				target.localScale = worldScale;
				return;
			}
			Vector3 lossyScale = parent.lossyScale;
			target.localScale = new Vector3(worldScale.x / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.x)), worldScale.y / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.y)), worldScale.z / Mathf.Max(0.0001f, Mathf.Abs(lossyScale.z)));
		}

		// Token: 0x0600867B RID: 34427 RVA: 0x0036EB0C File Offset: 0x0036CD0C
		private static void RevealPlacementVisual(GameObject root)
		{
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				if (!(sharedMesh == null))
				{
					for (int j = 0; j < sharedMesh.blendShapeCount; j++)
					{
						if (string.Equals(sharedMesh.GetBlendShapeName(j), "Hide_Photo", StringComparison.OrdinalIgnoreCase))
						{
							skinnedMeshRenderer.SetBlendShapeWeight(j, 0f);
						}
					}
				}
			}
		}

		// Token: 0x0600867C RID: 34428 RVA: 0x0036EBA0 File Offset: 0x0036CDA0
		private static void SetLayerRecursive(GameObject obj, int layer)
		{
			obj.layer = layer;
			foreach (object obj2 in obj.transform)
			{
				PlacementGhost.SetLayerRecursive(((Transform)obj2).gameObject, layer);
			}
		}

		// Token: 0x0600867D RID: 34429 RVA: 0x0036EC04 File Offset: 0x0036CE04
		public void UpdateTarget(Vector3 position, Quaternion rotation, bool isValid, Transform referenceFrame = null)
		{
			if (this._ghostInstance == null)
			{
				return;
			}
			if (referenceFrame != null)
			{
				this._localTargetPosition = referenceFrame.InverseTransformPoint(position);
				this._localTargetRotation = Quaternion.Inverse(referenceFrame.rotation) * rotation;
				if (referenceFrame != this._referenceFrame || !this._hasTarget)
				{
					this._localPreviewPosition = this._localTargetPosition;
					this._localPreviewRotation = this._localTargetRotation;
				}
			}
			this._referenceFrame = referenceFrame;
			this._targetPosition = position;
			this._targetRotation = rotation;
			this._isValid = isValid;
			this._hasTarget = true;
			if (!this._ghostInstance.activeSelf)
			{
				this._ghostInstance.SetActive(true);
			}
			this.UpdateColor();
		}

		// Token: 0x0600867E RID: 34430 RVA: 0x0036ECC2 File Offset: 0x0036CEC2
		public void SetVisible(bool visible)
		{
			if (this._ghostInstance != null)
			{
				this._ghostInstance.SetActive(visible);
			}
		}

		// Token: 0x0600867F RID: 34431 RVA: 0x0036ECE0 File Offset: 0x0036CEE0
		public void Destroy()
		{
			if (this._ghostInstance != null)
			{
				Object.Destroy(this._ghostInstance);
				this._ghostInstance = null;
			}
			if (this._ghostMaterials != null)
			{
				foreach (Material material in this._ghostMaterials)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
				this._ghostMaterials = null;
			}
			this._renderers = null;
			this._hasTarget = false;
			this._referenceFrame = null;
			if (this._runtimeMesh != null)
			{
				Object.Destroy(this._runtimeMesh);
				this._runtimeMesh = null;
			}
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x0036ED7C File Offset: 0x0036CF7C
		private void LateUpdate()
		{
			if (this._ghostInstance == null || !this._hasTarget)
			{
				return;
			}
			float num = this.followSpeed * Time.deltaTime;
			if (this._referenceFrame != null)
			{
				this._localPreviewPosition = Vector3.Lerp(this._localPreviewPosition, this._localTargetPosition, num);
				this._localPreviewRotation = Quaternion.Slerp(this._localPreviewRotation, this._localTargetRotation, num);
				this._ghostInstance.transform.SetPositionAndRotation(this._referenceFrame.TransformPoint(this._localPreviewPosition), this._referenceFrame.rotation * this._localPreviewRotation);
				return;
			}
			this._ghostInstance.transform.position = Vector3.Lerp(this._ghostInstance.transform.position, this._targetPosition, num);
			this._ghostInstance.transform.rotation = Quaternion.Slerp(this._ghostInstance.transform.rotation, this._targetRotation, num);
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x0036EE7C File Offset: 0x0036D07C
		private void ApplyGhostMaterial(Material sourceMaterial)
		{
			if (this._ghostInstance == null)
			{
				return;
			}
			this._renderers = this._ghostInstance.GetComponentsInChildren<Renderer>(true);
			if (this._renderers == null || this._renderers.Length == 0)
			{
				return;
			}
			List<Material> list = new List<Material>();
			for (int i = 0; i < this._renderers.Length; i++)
			{
				Material[] sharedMaterials = this._renderers[i].sharedMaterials;
				int num = Mathf.Max(1, (sharedMaterials != null) ? sharedMaterials.Length : 0);
				Material[] array = new Material[num];
				for (int j = 0; j < num; j++)
				{
					Material material = ((sharedMaterials != null && j < sharedMaterials.Length) ? sharedMaterials[j] : null);
					Material material2 = ((sourceMaterial != null) ? sourceMaterial : material);
					if (!(material2 == null))
					{
						Material material3 = new Material(material2);
						PlacementGhost.SetMaterialTransparent(material3);
						PlacementGhost.ConfigureGhostMaterial(material3);
						array[j] = material3;
						list.Add(material3);
					}
				}
				this._renderers[i].sharedMaterials = array;
				this._renderers[i].enabled = true;
				this._renderers[i].forceRenderingOff = false;
				this._renderers[i].shadowCastingMode = ShadowCastingMode.Off;
				this._renderers[i].receiveShadows = false;
			}
			this._ghostMaterials = list.ToArray();
		}

		// Token: 0x06008682 RID: 34434 RVA: 0x0036EFB8 File Offset: 0x0036D1B8
		private void UpdateColor()
		{
			if (this._ghostMaterials == null)
			{
				return;
			}
			Color color = (this._isValid ? this.validColor : this.invalidColor);
			foreach (Material material in this._ghostMaterials)
			{
				if (!(material == null))
				{
					if (material.HasProperty("_BaseColor"))
					{
						material.SetColor("_BaseColor", color);
					}
					if (material.HasProperty("_Color"))
					{
						material.SetColor("_Color", color);
					}
					if (material.HasProperty("_EmissionColor"))
					{
						material.SetColor("_EmissionColor", new Color(color.r, color.g, color.b, 1f) * 1.4f);
					}
				}
			}
		}

		// Token: 0x06008683 RID: 34435 RVA: 0x0036F080 File Offset: 0x0036D280
		private static void ConfigureGhostMaterial(Material mat)
		{
			if (mat == null)
			{
				return;
			}
			if (mat.HasProperty("_Cull"))
			{
				mat.SetFloat("_Cull", 0f);
			}
			if (mat.HasProperty("_Surface"))
			{
				mat.SetFloat("_Surface", 1f);
			}
			if (mat.HasProperty("_ZWrite"))
			{
				mat.SetFloat("_ZWrite", 0f);
			}
			mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			mat.EnableKeyword("_EMISSION");
			mat.renderQueue = 3100;
		}

		// Token: 0x06008684 RID: 34436 RVA: 0x0036F110 File Offset: 0x0036D310
		private static void StripPhysicsImmediate(GameObject root)
		{
			WheelCollider[] componentsInChildren = root.GetComponentsInChildren<WheelCollider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren[i]);
			}
			Collider[] componentsInChildren2 = root.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren2[i]);
			}
			Rigidbody[] componentsInChildren3 = root.GetComponentsInChildren<Rigidbody>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren3[i]);
			}
		}

		// Token: 0x06008685 RID: 34437 RVA: 0x0036F178 File Offset: 0x0036D378
		private static void StripScriptsImmediate(GameObject root)
		{
			foreach (MonoBehaviour monoBehaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (!(monoBehaviour == null) && !(monoBehaviour is PlacementGhost))
				{
					Object.DestroyImmediate(monoBehaviour);
				}
			}
		}

		// Token: 0x06008686 RID: 34438 RVA: 0x0036F1B8 File Offset: 0x0036D3B8
		private static void SetMaterialTransparent(Material mat)
		{
			mat.SetFloat("_Mode", 3f);
			mat.SetInt("_SrcBlend", 5);
			mat.SetInt("_DstBlend", 10);
			mat.SetInt("_ZWrite", 0);
			mat.DisableKeyword("_ALPHATEST_ON");
			mat.EnableKeyword("_ALPHABLEND_ON");
			mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			mat.renderQueue = 3000;
		}

		// Token: 0x06008687 RID: 34439 RVA: 0x0036F228 File Offset: 0x0036D428
		private void OnValidate()
		{
			this.followSpeed = Mathf.Max(0.1f, this.followSpeed);
			this.validColor.a = Mathf.Clamp(this.validColor.a, 0f, 1f);
			this.invalidColor.a = Mathf.Clamp(this.invalidColor.a, 0f, 1f);
		}

		// Token: 0x06008688 RID: 34440 RVA: 0x0036F295 File Offset: 0x0036D495
		private void OnDestroy()
		{
			this.Destroy();
		}

		// Token: 0x04008915 RID: 35093
		[Header("Colors")]
		[SerializeField]
		private Color validColor = new Color(0f, 1f, 0f, 0.4f);

		// Token: 0x04008916 RID: 35094
		[SerializeField]
		private Color invalidColor = new Color(1f, 0f, 0f, 0.4f);

		// Token: 0x04008917 RID: 35095
		[Header("Smoothing")]
		[Tooltip("Position/rotation lerp speed for ghost movement")]
		[SerializeField]
		private float followSpeed = 20f;

		// Token: 0x04008918 RID: 35096
		private GameObject _ghostInstance;

		// Token: 0x04008919 RID: 35097
		private Renderer[] _renderers;

		// Token: 0x0400891A RID: 35098
		private Material[] _ghostMaterials;

		// Token: 0x0400891B RID: 35099
		private Mesh _runtimeMesh;

		// Token: 0x0400891C RID: 35100
		private bool _isValid;

		// Token: 0x0400891D RID: 35101
		private bool _hasTarget;

		// Token: 0x0400891E RID: 35102
		private Vector3 _targetPosition;

		// Token: 0x0400891F RID: 35103
		private Quaternion _targetRotation;

		// Token: 0x04008920 RID: 35104
		private Transform _referenceFrame;

		// Token: 0x04008921 RID: 35105
		private Vector3 _localTargetPosition;

		// Token: 0x04008922 RID: 35106
		private Vector3 _localPreviewPosition;

		// Token: 0x04008923 RID: 35107
		private Quaternion _localTargetRotation;

		// Token: 0x04008924 RID: 35108
		private Quaternion _localPreviewRotation;
	}
}
