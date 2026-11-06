using System;
using System.Collections.Generic;
using ProjectExhibitionist;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001111 RID: 4369
	public static class CollarIdPortraitCapture
	{
		// Token: 0x060084A0 RID: 33952 RVA: 0x00360D8C File Offset: 0x0035EF8C
		public static bool TryCapture(Transform player, RenderTexture target)
		{
			if (player == null || !player.gameObject.activeInHierarchy || target == null || target.width <= 0 || target.height <= 0)
			{
				return false;
			}
			int num = LayerMask.NameToLayer("CharacterPreview");
			if (num < 0)
			{
				return false;
			}
			Animator animator = player.GetComponent<Animator>();
			if (animator == null)
			{
				animator = player.GetComponentInChildren<Animator>();
			}
			if (animator == null || !animator.isHuman || animator.avatar == null || !animator.avatar.isValid)
			{
				return false;
			}
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Head);
			if (boneTransform == null)
			{
				return false;
			}
			SkinDecalController componentInChildren = player.GetComponentInChildren<SkinDecalController>(true);
			if (componentInChildren != null && componentInChildren.headRenderer != null && !CollarIdPortraitCapture.IsVisible(componentInChildren.headRenderer))
			{
				return false;
			}
			HashSet<Renderer> hashSet = new HashSet<Renderer>();
			foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>())
			{
				if (CollarIdPortraitCapture.IsVisible(renderer) && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer) && !CollarIdPortraitCapture.IsExcluded(renderer.transform, player))
				{
					hashSet.Add(renderer);
				}
			}
			if (hashSet.Count == 0)
			{
				return false;
			}
			Dictionary<GameObject, int> dictionary = new Dictionary<GameObject, int>();
			Dictionary<Renderer, bool> dictionary2 = new Dictionary<Renderer, bool>();
			Dictionary<Renderer, Material[]> dictionary3 = new Dictionary<Renderer, Material[]>();
			Dictionary<Material, Material> dictionary4 = new Dictionary<Material, Material>();
			GameObject gameObject = null;
			IDisposable disposable = null;
			IDisposable disposable2 = null;
			IDisposable disposable3 = null;
			Light sun = RenderSettings.sun;
			bool flag;
			try
			{
				using (FirstPersonController.AllowHeadInAuxiliaryRendering())
				{
					foreach (Renderer renderer2 in hashSet)
					{
						GameObject gameObject2 = renderer2.gameObject;
						if (!dictionary.ContainsKey(gameObject2))
						{
							dictionary.Add(gameObject2, gameObject2.layer);
							gameObject2.layer = num;
						}
					}
					HashSet<Renderer> hashSet2 = new HashSet<Renderer>(Object.FindObjectsOfType<Renderer>(true));
					Scene scene = player.gameObject.scene;
					if (scene.IsValid() && scene.isLoaded)
					{
						GameObject[] rootGameObjects = scene.GetRootGameObjects();
						for (int i = 0; i < rootGameObjects.Length; i++)
						{
							foreach (Renderer renderer3 in rootGameObjects[i].GetComponentsInChildren<Renderer>(true))
							{
								hashSet2.Add(renderer3);
							}
						}
					}
					foreach (Renderer renderer4 in hashSet2)
					{
						if (!(renderer4 == null) && renderer4.gameObject.layer == num && !hashSet.Contains(renderer4))
						{
							dictionary2.Add(renderer4, renderer4.forceRenderingOff);
							renderer4.forceRenderingOff = true;
						}
					}
					gameObject = new GameObject("Collar ID portrait capture")
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					gameObject.transform.SetParent(player, false);
					Camera camera = gameObject.AddComponent<Camera>();
					camera.enabled = false;
					camera.scene = player.gameObject.scene;
					camera.cameraType = CameraType.Game;
					camera.cullingMask = 1 << num;
					camera.clearFlags = CameraClearFlags.Color;
					camera.backgroundColor = CollarIdPortraitCapture.Background;
					camera.nearClipPlane = 0.02f;
					camera.farClipPlane = 8f;
					camera.orthographic = true;
					camera.aspect = (float)target.width / (float)target.height;
					camera.allowHDR = false;
					camera.allowMSAA = true;
					camera.allowDynamicResolution = false;
					camera.useOcclusionCulling = false;
					camera.depthTextureMode = DepthTextureMode.None;
					UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
					universalAdditionalCameraData.renderType = CameraRenderType.Base;
					universalAdditionalCameraData.SetRenderer(1);
					universalAdditionalCameraData.renderPostProcessing = false;
					universalAdditionalCameraData.renderShadows = false;
					universalAdditionalCameraData.requiresDepthOption = CameraOverrideOption.Off;
					universalAdditionalCameraData.requiresColorOption = CameraOverrideOption.Off;
					universalAdditionalCameraData.volumeLayerMask = 0;
					universalAdditionalCameraData.antialiasing = AntialiasingMode.None;
					universalAdditionalCameraData.allowXRRendering = false;
					CollarIdPortraitPose collarIdPortraitPose = Resources.Load<CollarIdPortraitPose>("CollarIdPortraitPose");
					if (collarIdPortraitPose == null)
					{
						flag = false;
					}
					else
					{
						disposable2 = CollarIdPortraitWardrobeScope.Apply(player, hashSet);
						disposable = collarIdPortraitPose.Apply(animator);
						if (componentInChildren != null)
						{
							componentInChildren.SynchronizeMeshesForCapture();
						}
						if (!CollarIdPortraitCapture.FrameHeadAndShoulders(camera, animator, boneTransform, hashSet))
						{
							flag = false;
						}
						else
						{
							CollarIdPortraitCapture.PreparePortraitMaterials(hashSet, dictionary3, dictionary4);
							disposable3 = CollarIdPortraitSkinScope.Apply(hashSet);
							Light light = CollarIdPortraitCapture.CreateLight(gameObject.transform, "Portrait key", camera.transform.position + camera.transform.up * 0.7f - camera.transform.right * 0.65f, boneTransform.position, num, 1.1f, LightType.Directional);
							CollarIdPortraitCapture.CreateLight(gameObject.transform, "Portrait fill", camera.transform.position + camera.transform.right * 0.8f, boneTransform.position, num, 1.1f, LightType.Spot);
							RenderSettings.sun = light;
							CharacterPreviewRendering.RenderPortrait(camera, target, false);
							flag = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Collar ID] Portrait capture deferred: " + ex.Message);
				flag = false;
			}
			finally
			{
				if (disposable3 != null)
				{
					disposable3.Dispose();
				}
				if (disposable != null)
				{
					disposable.Dispose();
				}
				if (disposable2 != null)
				{
					disposable2.Dispose();
				}
				RenderSettings.sun = sun;
				foreach (KeyValuePair<Renderer, Material[]> keyValuePair in dictionary3)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.sharedMaterials = keyValuePair.Value;
					}
				}
				foreach (Material material in dictionary4.Values)
				{
					CollarIdPortraitCapture.DestroyOwned(material);
				}
				foreach (KeyValuePair<Renderer, bool> keyValuePair2 in dictionary2)
				{
					if (keyValuePair2.Key != null)
					{
						keyValuePair2.Key.forceRenderingOff = keyValuePair2.Value;
					}
				}
				foreach (KeyValuePair<GameObject, int> keyValuePair3 in dictionary)
				{
					if (keyValuePair3.Key != null)
					{
						keyValuePair3.Key.layer = keyValuePair3.Value;
					}
				}
				if (gameObject != null)
				{
					gameObject.SetActive(false);
					CollarIdPortraitCapture.DestroyOwned(gameObject);
				}
			}
			return flag;
		}

		// Token: 0x060084A1 RID: 33953 RVA: 0x003614E0 File Offset: 0x0035F6E0
		private static bool IsVisible(Renderer renderer)
		{
			return renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy && !renderer.forceRenderingOff;
		}

		// Token: 0x060084A2 RID: 33954 RVA: 0x0036150C File Offset: 0x0035F70C
		private static bool IsExcluded(Transform visual, Transform player)
		{
			if (visual.GetComponentInParent<FirstPersonVisibleProp>(true) != null)
			{
				return true;
			}
			Transform transform = visual;
			while (transform != null && transform != player)
			{
				if (transform.name.Equals("Card_Mesh", StringComparison.OrdinalIgnoreCase) || transform.name.Equals("Collar Visual", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x060084A3 RID: 33955 RVA: 0x00361570 File Offset: 0x0035F770
		private static bool FrameHeadAndShoulders(Camera camera, Animator animator, Transform head, HashSet<Renderer> subjects)
		{
			Vector3 vector = Vector3.ProjectOnPlane(animator.transform.forward, Vector3.up).normalized;
			if (vector.sqrMagnitude < 0.5f)
			{
				return false;
			}
			Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.LeftEye);
			Transform boneTransform2 = animator.GetBoneTransform(HumanBodyBones.RightEye);
			if (boneTransform != null && boneTransform2 != null)
			{
				Vector3 normalized = Vector3.Cross(boneTransform2.position - boneTransform.position, Vector3.up).normalized;
				if (normalized.sqrMagnitude > 0.5f)
				{
					vector = ((Vector3.Dot(normalized, vector) >= 0f) ? normalized : (-normalized));
				}
			}
			camera.transform.rotation = Quaternion.LookRotation(-vector, Vector3.up);
			Transform boneTransform3 = animator.GetBoneTransform(HumanBodyBones.Neck);
			float num = ((boneTransform != null && boneTransform2 != null) ? Vector3.Distance(boneTransform.position, boneTransform2.position) : 0f);
			float num2 = ((num > 0.01f) ? Mathf.Clamp(num / 0.1f, 0.5f, 2f) : ((boneTransform3 != null) ? Mathf.Clamp(Vector3.Distance(boneTransform3.position, head.position) / 0.08f, 0.5f, 2f) : 1f));
			float num3 = head.position.y;
			bool flag = false;
			Mesh mesh = new Mesh
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			List<Vector3> list = new List<Vector3>();
			try
			{
				foreach (Renderer renderer in subjects)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
					if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
					{
						skinnedMeshRenderer.BakeMesh(mesh, true);
						mesh.GetVertices(list);
						foreach (Vector3 vector2 in list)
						{
							CollarIdPortraitCapture.IncludeHeadPoint(skinnedMeshRenderer.transform.TransformPoint(vector2), head.position, camera.transform.right, num2, ref num3, ref flag);
						}
						mesh.Clear();
					}
					else if (renderer is MeshRenderer)
					{
						Bounds bounds = renderer.bounds;
						if (bounds.max.y >= head.position.y && Mathf.Abs(Vector3.Dot(bounds.center - head.position, camera.transform.right)) <= 0.2f * num2)
						{
							num3 = Mathf.Max(num3, Mathf.Min(bounds.max.y, head.position.y + 0.3f * num2));
						}
					}
				}
			}
			finally
			{
				CollarIdPortraitCapture.DestroyOwned(mesh);
			}
			if (!flag)
			{
				return false;
			}
			camera.orthographicSize = 0.27f * num2;
			Vector3 position = head.position;
			float num4 = Mathf.Clamp(num3 + 0.06f * num2, head.position.y + 0.27f * num2, head.position.y + 0.31f * num2);
			position.y = num4 - camera.orthographicSize;
			camera.transform.position = position + vector * 3f;
			return true;
		}

		// Token: 0x060084A4 RID: 33956 RVA: 0x0036191C File Offset: 0x0035FB1C
		private static void IncludeHeadPoint(Vector3 point, Vector3 head, Vector3 right, float scale, ref float top, ref bool found)
		{
			float num = Mathf.Abs(Vector3.Dot(point - head, right));
			if (point.y < head.y - 0.1f * scale || point.y > head.y + 0.32f * scale || num > 0.2f * scale)
			{
				return;
			}
			top = Mathf.Max(top, point.y);
			if (point.y > head.y + 0.03f * scale)
			{
				found = true;
			}
		}

		// Token: 0x060084A5 RID: 33957 RVA: 0x003619A0 File Offset: 0x0035FBA0
		private static void PreparePortraitMaterials(HashSet<Renderer> subjects, Dictionary<Renderer, Material[]> originals, Dictionary<Material, Material> copies)
		{
			foreach (Renderer renderer in subjects)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				Material[] array = null;
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					Material material = sharedMaterials[i];
					if (!(material == null) && material.HasProperty(CollarIdPortraitCapture.AsUnlit))
					{
						Material material2;
						if (!copies.TryGetValue(material, out material2))
						{
							material2 = new Material(material)
							{
								name = material.name + " (ID portrait)",
								hideFlags = HideFlags.HideAndDontSave
							};
							copies.Add(material, material2);
							material2.SetFloat(CollarIdPortraitCapture.AsUnlit, 1f);
						}
						if (array == null)
						{
							array = (Material[])sharedMaterials.Clone();
						}
						array[i] = material2;
					}
				}
				if (array != null)
				{
					originals.Add(renderer, sharedMaterials);
					renderer.sharedMaterials = array;
				}
			}
		}

		// Token: 0x060084A6 RID: 33958 RVA: 0x00361AA4 File Offset: 0x0035FCA4
		private static Light CreateLight(Transform parent, string name, Vector3 position, Vector3 aim, int layer, float intensity, LightType type)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.transform.SetParent(parent, false);
			gameObject.transform.SetPositionAndRotation(position, Quaternion.LookRotation(aim - position));
			Light light = gameObject.AddComponent<Light>();
			light.type = type;
			light.color = Color.white;
			light.intensity = intensity;
			if (type == LightType.Spot)
			{
				light.range = 6f;
				light.spotAngle = 65f;
				light.innerSpotAngle = 45f;
			}
			light.shadows = LightShadows.None;
			light.cullingMask = 1 << layer;
			light.renderMode = LightRenderMode.ForcePixel;
			return light;
		}

		// Token: 0x060084A7 RID: 33959 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyOwned(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x04008764 RID: 34660
		private static readonly Color Background = new Color32(156, 133, 181, 0);

		// Token: 0x04008765 RID: 34661
		private static readonly int AsUnlit = Shader.PropertyToID("_AsUnlit");
	}
}
