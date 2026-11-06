using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Slip
{
	// Token: 0x02000C5A RID: 3162
	[ExecuteAlways]
	public sealed class DetachedClothVisual : MonoBehaviour
	{
		// Token: 0x06005B7E RID: 23422 RVA: 0x0023DA74 File Offset: 0x0023BC74
		public static DetachedClothVisual Capture(GameObject worn)
		{
			if (worn == null)
			{
				return null;
			}
			Renderer[] componentsInChildren = worn.GetComponentsInChildren<Renderer>();
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in componentsInChildren)
			{
				if (DetachedClothVisual.CanCapture(renderer))
				{
					if (flag)
					{
						bounds.Encapsulate(renderer.bounds);
					}
					else
					{
						bounds = renderer.bounds;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			GameObject gameObject = new GameObject(worn.name + "_DetachedSlip");
			gameObject.transform.position = bounds.center;
			DetachedClothVisual detachedClothVisual = gameObject.AddComponent<DetachedClothVisual>();
			foreach (Renderer renderer2 in componentsInChildren)
			{
				if (DetachedClothVisual.CanCapture(renderer2))
				{
					SkinnedMeshRenderer skinnedMeshRenderer = renderer2 as SkinnedMeshRenderer;
					Mesh mesh;
					if (skinnedMeshRenderer != null)
					{
						mesh = new Mesh
						{
							name = renderer2.name + "_SlipSnapshot"
						};
						skinnedMeshRenderer.BakeMesh(mesh, false);
						detachedClothVisual._ownedMeshes.Add(mesh);
					}
					else
					{
						mesh = renderer2.GetComponent<MeshFilter>().sharedMesh;
					}
					GameObject gameObject2 = new GameObject(renderer2.name);
					gameObject2.layer = renderer2.gameObject.layer;
					gameObject2.transform.SetParent(gameObject.transform, false);
					gameObject2.transform.SetPositionAndRotation(renderer2.transform.position, renderer2.transform.rotation);
					gameObject2.transform.localScale = renderer2.transform.lossyScale;
					gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
					MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
					meshRenderer.sharedMaterials = renderer2.sharedMaterials;
					meshRenderer.shadowCastingMode = renderer2.shadowCastingMode;
					meshRenderer.receiveShadows = renderer2.receiveShadows;
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					renderer2.GetPropertyBlock(materialPropertyBlock);
					meshRenderer.SetPropertyBlock(materialPropertyBlock);
				}
			}
			return detachedClothVisual;
		}

		// Token: 0x06005B7F RID: 23423 RVA: 0x0023DC54 File Offset: 0x0023BE54
		private static bool CanCapture(Renderer renderer)
		{
			if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
			{
				return false;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				return skinnedMeshRenderer.sharedMesh != null;
			}
			if (!(renderer is MeshRenderer))
			{
				return false;
			}
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			return component != null && component.sharedMesh != null;
		}

		// Token: 0x06005B80 RID: 23424 RVA: 0x0023DCC0 File Offset: 0x0023BEC0
		public static void PlaceOnSurface(GameObject environment, Vector3 point)
		{
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (Renderer renderer in environment.GetComponentsInChildren<Renderer>())
			{
				if (renderer.enabled)
				{
					if (flag)
					{
						bounds.Encapsulate(renderer.bounds);
					}
					else
					{
						bounds = renderer.bounds;
						flag = true;
					}
				}
			}
			if (flag)
			{
				environment.transform.position += point - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
				return;
			}
			environment.transform.position = point;
		}

		// Token: 0x06005B81 RID: 23425 RVA: 0x0023DD6C File Offset: 0x0023BF6C
		private void OnDestroy()
		{
			foreach (Mesh mesh in this._ownedMeshes)
			{
				if (!(mesh == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(mesh);
					}
					else
					{
						Object.DestroyImmediate(mesh);
					}
				}
			}
			this._ownedMeshes.Clear();
		}

		// Token: 0x04005CB0 RID: 23728
		private readonly List<Mesh> _ownedMeshes = new List<Mesh>();
	}
}
