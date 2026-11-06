using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x0200003E RID: 62
	public class GPUITreeProxyProvider : GPUIDataProvider<int, MeshRenderer>
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000C848 File Offset: 0x0000AA48
		public override void Dispose()
		{
			if (this._dataDict != null)
			{
				foreach (MeshRenderer meshRenderer in base.Values)
				{
					if (meshRenderer != null)
					{
						Material sharedMaterial = meshRenderer.sharedMaterial;
						if (sharedMaterial != null)
						{
							sharedMaterial.DestroyGeneric();
						}
						MeshFilter meshFilter;
						if (meshRenderer.gameObject.TryGetComponent<MeshFilter>(out meshFilter))
						{
							Mesh sharedMesh = meshFilter.sharedMesh;
							if (sharedMesh != null)
							{
								sharedMesh.DestroyGeneric();
							}
						}
						meshRenderer.gameObject.DestroyGeneric();
					}
				}
			}
			this._treeProxyParent.DestroyGeneric();
			base.Dispose();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C904 File Offset: 0x0000AB04
		public void SetTreeProxyPosition(Vector3 position)
		{
			if (this._treeProxyParent != null)
			{
				this._treeProxyParent.position = position;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000C920 File Offset: 0x0000AB20
		public void GetMaterialPropertyBlock(GPUILODGroupData lgd, MaterialPropertyBlock mpb)
		{
			MeshRenderer meshRenderer = this.AddOrGetTreeProxy(lgd.prototype.prefabObject);
			if (meshRenderer == null)
			{
				return;
			}
			meshRenderer.GetPropertyBlock(mpb);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000C950 File Offset: 0x0000AB50
		private MeshRenderer AddOrGetTreeProxy(GameObject treePrefab)
		{
			if (!Application.isPlaying)
			{
				return null;
			}
			int num = GPUIUtility.GenerateHash(new int[] { treePrefab.GetInstanceIDGPUI() });
			MeshRenderer meshRenderer;
			if (!this._dataDict.TryGetValue(num, out meshRenderer) || meshRenderer == null)
			{
				if (this._treeProxyParent == null)
				{
					this._treeProxyParent = new GameObject("GPUI Tree Proxy").transform;
				}
				meshRenderer = GPUITreeProxyProvider.AddTreeProxy(treePrefab, this._treeProxyParent);
				if (meshRenderer != null)
				{
					this.AddOrSet(num, meshRenderer);
				}
			}
			return meshRenderer;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		private static MeshRenderer AddTreeProxy(GameObject treePrefab, Transform parentTransform)
		{
			Shader shader = GPUIUtility.FindShader("Hidden/GPUInstancerPro/Nature/TreeProxy");
			if (shader == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find GPUI Pro Tree Proxy shader! Make sure the shader is included in build: Hidden/GPUInstancerPro/Nature/TreeProxy");
				return null;
			}
			Mesh mesh = new Mesh();
			mesh.name = "TreeProxyMesh";
			Material[] array = new Material[]
			{
				new Material(shader)
			};
			LODGroup component = treePrefab.GetComponent<LODGroup>();
			if (component != null)
			{
				LOD[] lods = component.GetLODs();
				for (int i = 0; i < lods.Length; i++)
				{
					for (int j = 0; j < lods[i].renderers.Length; j++)
					{
						GameObject gameObject = lods[i].renderers[j].gameObject;
						if (gameObject.HasComponent<Tree>() && gameObject.HasComponent<MeshRenderer>() && gameObject.HasComponent<MeshFilter>())
						{
							return GPUITreeProxyProvider.InstantiateTreeProxyObject(gameObject, parentTransform, array, mesh);
						}
					}
				}
			}
			Tree componentInChildren = treePrefab.GetComponentInChildren<Tree>();
			if (componentInChildren != null)
			{
				return GPUITreeProxyProvider.InstantiateTreeProxyObject(componentInChildren.gameObject, parentTransform, array, mesh);
			}
			MeshRenderer componentInChildren2 = treePrefab.GetComponentInChildren<MeshRenderer>();
			if (componentInChildren2 != null)
			{
				return GPUITreeProxyProvider.InstantiateTreeProxyObject(componentInChildren2.gameObject, parentTransform, array, mesh);
			}
			return null;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000CB00 File Offset: 0x0000AD00
		private static MeshRenderer InstantiateTreeProxyObject(GameObject treePrefab, Transform parentTransform, Material[] proxyMaterials, Mesh proxyMesh)
		{
			if (!treePrefab.HasComponent<MeshFilter>() || !treePrefab.HasComponent<MeshRenderer>())
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(treePrefab, parentTransform);
			gameObject.hideFlags = HideFlags.DontSave;
			gameObject.name = treePrefab.name + "_GPUITreeProxy";
			gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			gameObject.transform.localScale = Vector3.one;
			proxyMesh.bounds = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			component.lightProbeUsage = LightProbeUsage.Off;
			component.enabled = true;
			for (int i = 0; i < proxyMaterials.Length; i++)
			{
				proxyMaterials[i].CopyPropertiesFromMaterial(component.sharedMaterials[i]);
				proxyMaterials[i].enableInstancing = true;
			}
			component.sharedMaterials = proxyMaterials;
			component.GetComponent<MeshFilter>().sharedMesh = proxyMesh;
			GPUITreeProxyProvider.StripComponents(gameObject);
			return component;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		private static void StripComponents(GameObject go)
		{
			foreach (object obj in go.transform)
			{
				((Transform)obj).gameObject.DestroyGeneric();
			}
			foreach (Component component in go.GetComponents(typeof(Component)))
			{
				if (!(component is Transform) && !(component is MeshFilter) && !(component is MeshRenderer) && !(component is Tree))
				{
					component.DestroyGeneric();
				}
			}
		}

		// Token: 0x0400016F RID: 367
		private Transform _treeProxyParent;
	}
}
