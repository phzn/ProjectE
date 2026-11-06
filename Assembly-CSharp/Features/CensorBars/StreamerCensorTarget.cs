using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014C5 RID: 5317
	[DisallowMultipleComponent]
	public sealed class StreamerCensorTarget : MonoBehaviour
	{
		// Token: 0x0600A36E RID: 41838 RVA: 0x0043E5EC File Offset: 0x0043C7EC
		public static StreamerCensorTarget ForRenderer(Renderer renderer, int materialSlot = -1)
		{
			if (renderer == null)
			{
				return null;
			}
			string text = "StreamerCensorSlot_" + materialSlot.ToString();
			Transform transform = renderer.transform.Find(text);
			StreamerCensorTarget streamerCensorTarget = ((transform != null) ? transform.GetComponent<StreamerCensorTarget>() : null);
			if (streamerCensorTarget != null)
			{
				return streamerCensorTarget;
			}
			GameObject gameObject = new GameObject(text);
			gameObject.transform.SetParent(renderer.transform, false);
			streamerCensorTarget = gameObject.AddComponent<StreamerCensorTarget>();
			streamerCensorTarget.renderers = new Renderer[] { renderer };
			streamerCensorTarget.materialIndex = materialSlot;
			streamerCensorTarget.enabled = false;
			return streamerCensorTarget;
		}

		// Token: 0x0600A36F RID: 41839 RVA: 0x0043E67E File Offset: 0x0043C87E
		private void Awake()
		{
			if (this.renderers == null || this.renderers.Length == 0)
			{
				this.renderers = base.GetComponentsInChildren<Renderer>(true);
			}
		}

		// Token: 0x0600A370 RID: 41840 RVA: 0x0043E69E File Offset: 0x0043C89E
		private void OnEnable()
		{
			StreamerCensorTarget.Active.Add(this);
		}

		// Token: 0x0600A371 RID: 41841 RVA: 0x0043E6AC File Offset: 0x0043C8AC
		private void OnDisable()
		{
			StreamerCensorTarget.Active.Remove(this);
		}

		// Token: 0x0600A372 RID: 41842 RVA: 0x0043E6AC File Offset: 0x0043C8AC
		private void OnDestroy()
		{
			StreamerCensorTarget.Active.Remove(this);
		}

		// Token: 0x0600A373 RID: 41843 RVA: 0x0043E6BC File Offset: 0x0043C8BC
		internal void Collect(Camera camera, List<Rect> regions)
		{
			if (this.renderers == null)
			{
				return;
			}
			foreach (Renderer renderer in this.renderers)
			{
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !renderer.forceRenderingOff && (camera.cullingMask & (1 << renderer.gameObject.layer)) != 0 && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer || renderer is SpriteRenderer))
				{
					Bounds bounds = renderer.bounds;
					MeshFilter meshFilter;
					if (this.materialIndex >= 0 && renderer is MeshRenderer && renderer.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.sharedMesh != null && this.materialIndex < meshFilter.sharedMesh.subMeshCount)
					{
						Bounds bounds2 = meshFilter.sharedMesh.GetSubMesh(this.materialIndex).bounds;
						if (bounds2.size.sqrMagnitude > 1E-06f)
						{
							bounds = StreamerCensorProjection.TransformBounds(renderer.transform, bounds2);
						}
					}
					StreamerCensorProjection.Add(camera, bounds, regions);
				}
			}
		}

		// Token: 0x0400A72B RID: 42795
		internal static readonly HashSet<StreamerCensorTarget> Active = new HashSet<StreamerCensorTarget>();

		// Token: 0x0400A72C RID: 42796
		[Tooltip("Empty uses mesh renderers under this object. Assign explicitly to censor just a screen or picture.")]
		public Renderer[] renderers;

		// Token: 0x0400A72D RID: 42797
		[Tooltip("-1 protects the whole object. A material slot protects only that submesh on static combined meshes.")]
		public int materialIndex = -1;
	}
}
