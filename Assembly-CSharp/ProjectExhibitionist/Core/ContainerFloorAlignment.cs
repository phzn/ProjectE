using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A33 RID: 2611
	[DefaultExecutionOrder(10000)]
	[DisallowMultipleComponent]
	public sealed class ContainerFloorAlignment : MonoBehaviour
	{
		// Token: 0x06004A30 RID: 18992 RVA: 0x001CBAA4 File Offset: 0x001C9CA4
		public static void Request(GameObject container)
		{
			if (container == null)
			{
				return;
			}
			ContainerFloorAlignment containerFloorAlignment = container.GetComponent<ContainerFloorAlignment>();
			if (containerFloorAlignment == null)
			{
				containerFloorAlignment = container.AddComponent<ContainerFloorAlignment>();
			}
			containerFloorAlignment._requestedFrame = Time.frameCount;
			containerFloorAlignment._attempts = 0;
			containerFloorAlignment.enabled = true;
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x001CBAEC File Offset: 0x001C9CEC
		private void Update()
		{
			if (!GameSystemsBootstrapper.GameplayReady || Time.frameCount <= this._requestedFrame)
			{
				return;
			}
			if (ContainerFloorAlignment._budgetFrame != Time.frameCount)
			{
				ContainerFloorAlignment._budgetFrame = Time.frameCount;
				ContainerFloorAlignment._usedBudget = 0;
			}
			if (ContainerFloorAlignment._usedBudget >= 4)
			{
				return;
			}
			ContainerFloorAlignment._usedBudget++;
			if (ContainerFloorAlignment._syncFrame != Time.frameCount)
			{
				Physics.SyncTransforms();
				ContainerFloorAlignment._syncFrame = Time.frameCount;
			}
			if (this._renderers.Count == 0)
			{
				base.GetComponentsInChildren<Renderer>(true, this._renderers);
			}
			if (!ContainerFloorAlignment.TryAlign(base.transform, this._renderers))
			{
				int num = this._attempts + 1;
				this._attempts = num;
				if (num < 8)
				{
					return;
				}
			}
			base.enabled = false;
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x001CBBA4 File Offset: 0x001C9DA4
		public static bool TryAlign(Transform container, IReadOnlyList<Renderer> renderers)
		{
			if (container == null || renderers == null)
			{
				return false;
			}
			Bounds bounds = default(Bounds);
			bool flag = false;
			Mesh mesh = null;
			try
			{
				for (int i = 0; i < renderers.Count; i++)
				{
					Renderer renderer = renderers[i];
					if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !(renderer.GetComponentInParent<Canvas>() != null))
					{
						SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
						Bounds bounds2;
						if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
						{
							if (mesh == null)
							{
								mesh = new Mesh();
							}
							skinnedMeshRenderer.BakeMesh(mesh);
							mesh.RecalculateBounds();
							bounds2 = mesh.bounds;
						}
						else
						{
							MeshFilter meshFilter;
							if (!(renderer is MeshRenderer) || !renderer.TryGetComponent<MeshFilter>(out meshFilter) || !(meshFilter.sharedMesh != null))
							{
								goto IL_0116;
							}
							bounds2 = meshFilter.sharedMesh.bounds;
						}
						if (bounds2.size.sqrMagnitude >= 1E-06f)
						{
							Bounds bounds3 = ContainerFloorAlignment.TransformBounds(renderer.transform, bounds2);
							if (!flag)
							{
								bounds = bounds3;
								flag = true;
							}
							else
							{
								bounds.Encapsulate(bounds3);
							}
						}
					}
					IL_0116:;
				}
			}
			finally
			{
				if (mesh != null)
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
			if (!flag)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(new Vector3(bounds.center.x, bounds.min.y + 0.75f + 0.02f, bounds.center.z), Vector3.down, ContainerFloorAlignment.Hits, 1.54f, -5, QueryTriggerInteraction.Ignore);
			if (num == ContainerFloorAlignment.Hits.Length)
			{
				return false;
			}
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < num; j++)
			{
				RaycastHit raycastHit = ContainerFloorAlignment.Hits[j];
				if (!(raycastHit.collider == null) && !raycastHit.transform.IsChildOf(container) && raycastHit.normal.y >= 0.9f && (!(raycastHit.rigidbody != null) || raycastHit.rigidbody.isKinematic))
				{
					float num3 = raycastHit.point.y - bounds.min.y;
					if (Mathf.Abs(num3) <= 0.75f && Mathf.Abs(num3) < Mathf.Abs(num2))
					{
						num2 = num3;
					}
				}
			}
			if (float.IsInfinity(num2))
			{
				return false;
			}
			if (Mathf.Abs(num2) > 0.0001f)
			{
				container.position += Vector3.up * num2;
			}
			return true;
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x001CBE64 File Offset: 0x001CA064
		private static Bounds TransformBounds(Transform owner, Bounds local)
		{
			Vector3 vector = owner.TransformPoint(local.center);
			Vector3 vector2 = owner.TransformVector(Vector3.right * local.extents.x);
			Vector3 vector3 = owner.TransformVector(Vector3.up * local.extents.y);
			Vector3 vector4 = owner.TransformVector(Vector3.forward * local.extents.z);
			return new Bounds(vector, new Vector3(Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x) + Mathf.Abs(vector4.x), Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y) + Mathf.Abs(vector4.y), Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z) + Mathf.Abs(vector4.z)) * 2f);
		}

		// Token: 0x04004C68 RID: 19560
		private const float MaxCorrection = 0.75f;

		// Token: 0x04004C69 RID: 19561
		private const int AlignmentsPerFrame = 4;

		// Token: 0x04004C6A RID: 19562
		private static readonly RaycastHit[] Hits = new RaycastHit[64];

		// Token: 0x04004C6B RID: 19563
		private static int _budgetFrame = -1;

		// Token: 0x04004C6C RID: 19564
		private static int _usedBudget;

		// Token: 0x04004C6D RID: 19565
		private static int _syncFrame = -1;

		// Token: 0x04004C6E RID: 19566
		private readonly List<Renderer> _renderers = new List<Renderer>();

		// Token: 0x04004C6F RID: 19567
		private int _requestedFrame;

		// Token: 0x04004C70 RID: 19568
		private int _attempts;
	}
}
