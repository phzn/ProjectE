using System;
using System.Linq;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B12 RID: 2834
	public static class NpcPortraitRendering
	{
		// Token: 0x0600503D RID: 20541 RVA: 0x001F0EC4 File Offset: 0x001EF0C4
		public static void Render(Camera camera, RenderTexture destination, GameObject preview, bool fullTextureQuality = true)
		{
			if (preview == null || !preview.activeInHierarchy)
			{
				throw new InvalidOperationException("An active NPC preview is required for its photo.");
			}
			int num = LayerMask.NameToLayer("CharacterPreview");
			if (num < 0)
			{
				throw new InvalidOperationException("CharacterPreview layer is missing.");
			}
			GameObject[] array = (from r in preview.GetComponentsInChildren<Renderer>(true)
				select r.gameObject).Distinct<GameObject>().ToArray<GameObject>();
			int[] array2 = array.Select<GameObject, int>((GameObject o) => o.layer).ToArray<int>();
			int cullingMask = camera.cullingMask;
			Vector3 position = camera.transform.position;
			bool orthographic = camera.orthographic;
			float orthographicSize = camera.orthographicSize;
			float aspect = camera.aspect;
			try
			{
				GameObject[] array3 = array;
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i].layer = num;
				}
				camera.cullingMask = 1 << num;
				camera.aspect = (float)destination.width / (float)destination.height;
				NpcPortraitRendering.FrameBust(camera, destination, preview);
				CharacterPreviewRendering.RenderPortrait(camera, destination, fullTextureQuality);
			}
			finally
			{
				camera.cullingMask = cullingMask;
				camera.transform.position = position;
				camera.orthographic = orthographic;
				camera.orthographicSize = orthographicSize;
				camera.aspect = aspect;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] != null)
					{
						array[j].layer = array2[j];
					}
				}
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001F1050 File Offset: 0x001EF250
		private static void FrameBust(Camera camera, RenderTexture destination, GameObject preview)
		{
			Animator componentInChildren = preview.GetComponentInChildren<Animator>();
			if (componentInChildren == null || !componentInChildren.isHuman)
			{
				return;
			}
			Transform boneTransform = componentInChildren.GetBoneTransform(HumanBodyBones.Head);
			if (boneTransform == null)
			{
				return;
			}
			float num = boneTransform.position.y;
			float num2 = 0.3f;
			Mesh mesh = new Mesh();
			try
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in preview.GetComponentsInChildren<SkinnedMeshRenderer>())
				{
					if (skinnedMeshRenderer.enabled && !(skinnedMeshRenderer.sharedMesh == null))
					{
						skinnedMeshRenderer.BakeMesh(mesh);
						foreach (Vector3 vector in mesh.vertices)
						{
							Vector3 vector2 = skinnedMeshRenderer.transform.TransformPoint(vector);
							num = Mathf.Max(num, vector2.y);
							if (vector2.y > boneTransform.position.y - 0.1f)
							{
								num2 = Mathf.Max(num2, Mathf.Abs(Vector3.Dot(vector2 - boneTransform.position, camera.transform.right)));
							}
						}
						mesh.Clear();
					}
				}
			}
			finally
			{
				Object.Destroy(mesh);
			}
			foreach (MeshRenderer meshRenderer in preview.GetComponentsInChildren<MeshRenderer>())
			{
				if (meshRenderer.enabled)
				{
					Bounds bounds = meshRenderer.bounds;
					num = Mathf.Max(num, bounds.max.y);
					if (bounds.max.y > boneTransform.position.y - 0.1f)
					{
						Vector3 right = camera.transform.right;
						float num3 = Mathf.Abs(right.x) * bounds.extents.x + Mathf.Abs(right.z) * bounds.extents.z;
						num2 = Mathf.Max(num2, Mathf.Abs(Vector3.Dot(bounds.center - boneTransform.position, right)) + num3);
					}
				}
			}
			camera.orthographic = true;
			camera.orthographicSize = Mathf.Max(new float[]
			{
				0.4f,
				(num - boneTransform.position.y + 0.48f) * 0.5f,
				(num2 + 0.05f) / ((float)destination.width / (float)destination.height)
			});
			Vector3 position = boneTransform.position;
			position.y = num + 0.06f - camera.orthographicSize;
			camera.transform.position = position - camera.transform.forward * 4f;
		}
	}
}
