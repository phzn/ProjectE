using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000042 RID: 66
	public class DC_Occluder : MonoBehaviour
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000077C9 File Offset: 0x000059C9
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000077D1 File Offset: 0x000059D1
		public OccluderType OccluderType { get; set; }

		// Token: 0x06000185 RID: 389 RVA: 0x000077DC File Offset: 0x000059DC
		public bool TryGetBounds(ref Bounds bounds)
		{
			if (this._bounds != null)
			{
				bounds = this._bounds.Value;
				return true;
			}
			LODGroup lodgroup;
			if (this.OccluderType == OccluderType.Collider)
			{
				Collider collider;
				if (base.TryGetComponent<Collider>(out collider))
				{
					this._bounds = new Bounds?(bounds = collider.bounds);
					return true;
				}
			}
			else if (this.OccluderType == OccluderType.Mesh)
			{
				MeshRenderer meshRenderer;
				if (base.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					this._bounds = new Bounds?(bounds = meshRenderer.bounds);
					return true;
				}
			}
			else if (base.TryGetComponent<LODGroup>(out lodgroup))
			{
				LOD[] lods = lodgroup.GetLODs();
				for (int i = 0; i < lods.Length; i++)
				{
					foreach (Renderer renderer in lods[i].renderers)
					{
						if (renderer != null)
						{
							this._bounds = new Bounds?(bounds = renderer.bounds);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000078E4 File Offset: 0x00005AE4
		private void Reset()
		{
			if (base.GetComponent<MeshRenderer>() != null)
			{
				this.OccluderType = OccluderType.Mesh;
				return;
			}
			if (base.GetComponent<LODGroup>() != null)
			{
				this.OccluderType = OccluderType.LODGroup;
				return;
			}
			if (base.GetComponent<Terrain>() != null)
			{
				this.OccluderType = OccluderType.Terrain;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007934 File Offset: 0x00005B34
		private void Start()
		{
			this._layer = DC_Controller.GetCullingLayer();
			if (this.OccluderType == OccluderType.Collider)
			{
				base.gameObject.layer = this._layer;
				return;
			}
			if (this.OccluderType == OccluderType.Mesh)
			{
				this.CreateMeshOccluder();
				return;
			}
			if (this.OccluderType == OccluderType.LODGroup)
			{
				this.CreateLODGroupOccluder();
				return;
			}
			if (this.OccluderType == OccluderType.Terrain)
			{
				this.CreateTerrainOccluder();
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007998 File Offset: 0x00005B98
		private void CreateMeshOccluder()
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null || component.sharedMesh == null)
			{
				Debug.Log(base.gameObject.name + " unable to create occluder, mesh not found");
				return;
			}
			this.CreateCollider(base.gameObject, component.sharedMesh);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000079F0 File Offset: 0x00005BF0
		private void CreateLODGroupOccluder()
		{
			LODGroup component = base.GetComponent<LODGroup>();
			if (component == null)
			{
				Debug.Log(base.gameObject.name + " unable to create occluder, LODGroup not found");
				return;
			}
			foreach (Renderer renderer in component.GetLODs()[0].renderers)
			{
				MeshFilter component2 = renderer.GetComponent<MeshFilter>();
				if (component2 != null && component2.sharedMesh != null)
				{
					this.CreateCollider(renderer.gameObject, component2.sharedMesh);
				}
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007A80 File Offset: 0x00005C80
		private void CreateTerrainOccluder()
		{
			TerrainCollider component = base.GetComponent<TerrainCollider>();
			if (component == null)
			{
				Debug.Log("Unable to create occluder, TerrainCollider not found");
				return;
			}
			new GameObject("DC_Occluder")
			{
				layer = this._layer,
				transform = 
				{
					parent = base.transform,
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity,
					localScale = Vector3.one
				}
			}.AddComponent<TerrainCollider>().terrainData = component.terrainData;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007B10 File Offset: 0x00005D10
		private void CreateCollider(GameObject go, Mesh mesh)
		{
			GameObject gameObject = new GameObject("DC_Collider");
			Transform transform = gameObject.transform;
			gameObject.layer = this._layer;
			transform.parent = go.transform;
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
			transform.localScale = Vector3.one;
			gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
		}

		// Token: 0x04000117 RID: 279
		private Bounds? _bounds;

		// Token: 0x04000118 RID: 280
		private int _layer;
	}
}
