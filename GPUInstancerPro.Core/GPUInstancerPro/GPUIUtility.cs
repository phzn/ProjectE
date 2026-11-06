using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x02000066 RID: 102
	public static class GPUIUtility
	{
		// Token: 0x060003DC RID: 988 RVA: 0x000156B8 File Offset: 0x000138B8
		public static bool HasComponent<T>(this GameObject go) where T : Component
		{
			return go.GetComponent<T>() != null;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000156CB File Offset: 0x000138CB
		public static bool HasComponentInChildren<T>(this GameObject go) where T : Component
		{
			return go.GetComponentInChildren<T>() != null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000156E0 File Offset: 0x000138E0
		public static bool HasComponentInChildrenExceptParent<T>(this GameObject go) where T : Component
		{
			using (IEnumerator enumerator = go.transform.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Transform)enumerator.Current).GetComponentInChildren<T>() != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001574C File Offset: 0x0001394C
		public static bool HasComponent<T>(this Transform transform) where T : Component
		{
			T t;
			return transform.TryGetComponent<T>(out t);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015764 File Offset: 0x00013964
		public static Matrix4x4 GetTransformOffset(this Transform parentTransform, Transform childTransform)
		{
			Matrix4x4 matrix4x = GPUIConstants.IDENTITY_Matrix4x4;
			Transform transform = childTransform;
			while (transform != parentTransform)
			{
				if (transform == null)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find find parent-child relation for renderers on : " + parentTransform.name, parentTransform);
					break;
				}
				matrix4x = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale) * matrix4x;
				transform = transform.parent;
			}
			return matrix4x;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000157D0 File Offset: 0x000139D0
		public static void GetMeshRenderers(this Transform transform, List<Renderer> meshRenderers, bool includeSkinnedMeshRenderers)
		{
			if (meshRenderers == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "A list must be supplied to call GetMeshRenderers method.");
				return;
			}
			MeshRenderer meshRenderer;
			if (transform.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				meshRenderers.Add(meshRenderer);
			}
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (includeSkinnedMeshRenderers && transform.TryGetComponent<SkinnedMeshRenderer>(out skinnedMeshRenderer))
			{
				meshRenderers.Add(skinnedMeshRenderer);
			}
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				GPUIPrefabDefinition gpuiprefabDefinition;
				if (!child.TryGetComponent<GPUIPrefabDefinition>(out gpuiprefabDefinition))
				{
					child.GetMeshRenderers(meshRenderers, includeSkinnedMeshRenderers);
				}
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001584C File Offset: 0x00013A4C
		public static void GetPrefabRenderers(this Transform transform, List<Renderer> renderers)
		{
			Renderer renderer;
			if (transform.TryGetComponent<Renderer>(out renderer) && (renderer is MeshRenderer || renderer is BillboardRenderer || renderer is SkinnedMeshRenderer))
			{
				renderers.Add(renderer);
			}
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				GPUIPrefabDefinition gpuiprefabDefinition;
				if (!child.TryGetComponent<GPUIPrefabDefinition>(out gpuiprefabDefinition))
				{
					child.GetPrefabRenderers(renderers);
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000158AE File Offset: 0x00013AAE
		public static void SetMatrixToTransform(this Transform transform, Matrix4x4 matrix)
		{
			transform.SetPositionAndRotation(matrix.GetPosition(), matrix.rotation);
			transform.localScale = matrix.lossyScale;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000158D1 File Offset: 0x00013AD1
		public static void DestroyGeneric(this Object uObject)
		{
			if (!uObject)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(uObject);
				return;
			}
			Object.DestroyImmediate(uObject);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000158F0 File Offset: 0x00013AF0
		public static T AddOrGetComponent<T>(this GameObject gameObject, bool allowUndo = false) where T : Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001591C File Offset: 0x00013B1C
		public static T AddOrGetComponent<T>(this Component component, bool allowUndo = false) where T : Component
		{
			T t = component.GetComponent<T>();
			if (t == null)
			{
				t = component.gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001594C File Offset: 0x00013B4C
		public static Bounds GetBounds(this GameObject gameObject, bool isVertexBased = false)
		{
			LODGroup lodgroup;
			Renderer[] array;
			if (gameObject.TryGetComponent<LODGroup>(out lodgroup))
			{
				array = lodgroup.GetLODs()[0].renderers;
			}
			else
			{
				array = gameObject.GetComponentsInChildren<Renderer>();
			}
			return array.GetBounds(isVertexBased);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00015988 File Offset: 0x00013B88
		public static Bounds GetBounds(this Renderer[] renderers, bool isVertexBased = false)
		{
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null))
				{
					Mesh mesh = null;
					MeshFilter meshFilter;
					if (renderer.TryGetComponent<MeshFilter>(out meshFilter))
					{
						mesh = meshFilter.sharedMesh;
					}
					else
					{
						SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
						if (skinnedMeshRenderer != null)
						{
							mesh = skinnedMeshRenderer.sharedMesh;
						}
					}
					if (mesh != null)
					{
						if (isVertexBased && mesh.isReadable)
						{
							Matrix4x4 localToWorldMatrix = renderer.transform.localToWorldMatrix;
							Vector3[] vertices = mesh.vertices;
							if (!flag && vertices.Length != 0)
							{
								flag = true;
								bounds = new Bounds(localToWorldMatrix.MultiplyPoint3x4(vertices[0]), Vector3.zero);
							}
							for (int j = 0; j < vertices.Length; j++)
							{
								bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(vertices[j]));
							}
						}
						else
						{
							Bounds bounds2 = renderer.bounds;
							if (!flag)
							{
								flag = true;
								bounds = new Bounds(bounds2.center, bounds2.size);
							}
							else
							{
								bounds.Encapsulate(bounds2);
							}
						}
					}
				}
			}
			return bounds;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015AA8 File Offset: 0x00013CA8
		public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren = true)
		{
			gameObject.layer = layer;
			if (includeChildren)
			{
				Transform[] componentsInChildren = gameObject.transform.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = layer;
				}
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00015AE8 File Offset: 0x00013CE8
		public static bool EqualOrParentOf(this GameObject parent, GameObject child)
		{
			if (parent == child)
			{
				return true;
			}
			if (parent == null || child == null)
			{
				return false;
			}
			Transform transform = parent.transform;
			Transform transform2 = child.transform.parent;
			while (transform2 != null)
			{
				if (transform == transform2)
				{
					return true;
				}
				transform2 = transform2.transform.parent;
			}
			return false;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00015B4A File Offset: 0x00013D4A
		public static GameObject GetPrefabRoot(this GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			return go.transform.GetPrefabRoot().gameObject;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00015B68 File Offset: 0x00013D68
		public static Transform GetPrefabRoot(this Transform transform)
		{
			Transform transform2 = transform;
			while (transform2 != null)
			{
				transform = transform2;
				transform2 = transform.parent;
			}
			return transform;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015B90 File Offset: 0x00013D90
		public static int GetLODCount(this GameObject gameObject)
		{
			LODGroup lodgroup;
			if (gameObject.TryGetComponent<LODGroup>(out lodgroup))
			{
				return lodgroup.lodCount;
			}
			return 1;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00015BB0 File Offset: 0x00013DB0
		public static int GetVertexCount(this Renderer[] renderers)
		{
			int num = 0;
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null))
				{
					Mesh mesh = null;
					MeshFilter meshFilter;
					if (renderer.TryGetComponent<MeshFilter>(out meshFilter))
					{
						mesh = meshFilter.sharedMesh;
					}
					else
					{
						SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
						if (skinnedMeshRenderer != null)
						{
							mesh = skinnedMeshRenderer.sharedMesh;
						}
					}
					if (mesh != null)
					{
						num += mesh.vertexCount;
					}
				}
			}
			return num;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015C20 File Offset: 0x00013E20
		public static bool IsRenderersDisabled(this GameObject gameObject)
		{
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].enabled)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00015C50 File Offset: 0x00013E50
		public static bool HasShader(this GameObject gameObject, string shaderName)
		{
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			string text = GPUIUtility.ConvertToGPUIShaderName(shaderName, null, null);
			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (Material material in array[i].sharedMaterials)
				{
					if (material != null && material.shader != null && (material.shader.name == shaderName || material.shader.name == text))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00015CE0 File Offset: 0x00013EE0
		public static GameObject InstantiateWithStrippedComponents(GameObject prefabGO, Vector3 position, Quaternion rotation, IEnumerable<Type> allowedComponentTypes)
		{
			bool activeSelf = prefabGO.activeSelf;
			if (activeSelf)
			{
				prefabGO.SetActive(false);
			}
			GameObject gameObject = Object.Instantiate<GameObject>(prefabGO, position, rotation);
			if (activeSelf)
			{
				prefabGO.SetActive(true);
			}
			GPUIUtility.StripUnwantedComponents(gameObject, allowedComponentTypes);
			if (activeSelf)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015D24 File Offset: 0x00013F24
		public static void StripUnwantedComponents(GameObject gameObject, IEnumerable<Type> allowedComponentTypes)
		{
			List<Component> list = new List<Component>();
			gameObject.GetComponentsInChildren<Component>(true, list);
			list.RemoveAll((Component c) => c is Transform);
			List<Component> list2 = new List<Component>();
			foreach (Component component in list)
			{
				if (!(component == null))
				{
					Type type = component.GetType();
					bool flag = false;
					foreach (Type type2 in allowedComponentTypes)
					{
						if (type == type2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						component.GetComponents<Component>(list2);
						GPUIUtility.StripUnwantedComponent(component, list2);
					}
				}
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015E14 File Offset: 0x00014014
		private static void StripUnwantedComponent(Component component, List<Component> components)
		{
			foreach (Component component2 in GPUIUtility.GetDependentComponents(component, components))
			{
				GPUIUtility.StripUnwantedComponent(component2, components);
			}
			Object.DestroyImmediate(component);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015E6C File Offset: 0x0001406C
		private static List<Component> GetDependentComponents(Component component, List<Component> components)
		{
			List<Component> list = new List<Component>();
			Type type = component.GetType();
			for (int i = 0; i < components.Count; i++)
			{
				Component component2 = components[i];
				if (!(component2 == null) && !(component2 == component) && !(component2 is Transform))
				{
					foreach (RequireComponent requireComponent in component2.GetType().GetCustomAttributes<RequireComponent>(true))
					{
						if (requireComponent.m_Type0 == type || requireComponent.m_Type1 == type || requireComponent.m_Type2 == type)
						{
							list.Add(component2);
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015F3C File Offset: 0x0001413C
		public static Transform FindDeepChild(this Transform parentTransform, string childName)
		{
			Transform transform = parentTransform.Find(childName);
			if (transform != null)
			{
				return transform;
			}
			int childCount = parentTransform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				transform = parentTransform.GetChild(i).FindDeepChild(childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00015F89 File Offset: 0x00014189
		public static int GetInstanceIDGPUI(this Object obj)
		{
			return obj.GetInstanceID();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00015F91 File Offset: 0x00014191
		public static T[] FindObjectsByType<T>() where T : Object
		{
			return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00015F99 File Offset: 0x00014199
		public static T FindFirstObjectByType<T>() where T : Object
		{
			return Object.FindFirstObjectByType<T>();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00015FA0 File Offset: 0x000141A0
		public static bool IsShadowCasting(this Renderer renderer)
		{
			return renderer.shadowCastingMode > ShadowCastingMode.Off;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00015FAC File Offset: 0x000141AC
		public static void SetValue(this MaterialPropertyBlock mpb, int nameID, object value)
		{
			if (value == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Given value is null! Can not apply override!");
				return;
			}
			if (value is Vector4)
			{
				Vector4 vector = (Vector4)value;
				mpb.SetVector(nameID, vector);
				return;
			}
			if (value is Vector3)
			{
				Vector3 vector2 = (Vector3)value;
				mpb.SetVector(nameID, vector2);
				return;
			}
			if (value is Vector2)
			{
				Vector2 vector3 = (Vector2)value;
				mpb.SetVector(nameID, vector3);
				return;
			}
			if (value is float)
			{
				float num = (float)value;
				mpb.SetFloat(nameID, num);
				return;
			}
			if (value is int)
			{
				int num2 = (int)value;
				mpb.SetInt(nameID, num2);
				return;
			}
			if (value is Color)
			{
				Color color = (Color)value;
				mpb.SetColor(nameID, color);
				return;
			}
			GraphicsBuffer graphicsBuffer = value as GraphicsBuffer;
			if (graphicsBuffer != null)
			{
				mpb.SetBuffer(nameID, graphicsBuffer);
				return;
			}
			ComputeBuffer computeBuffer = value as ComputeBuffer;
			if (computeBuffer != null)
			{
				mpb.SetBuffer(nameID, computeBuffer);
				return;
			}
			Texture texture = value as Texture;
			if (texture != null)
			{
				mpb.SetTexture(nameID, texture);
				return;
			}
			string log_PREFIX = GPUIConstants.LOG_PREFIX;
			string text = "Can not set value to MaterialPropertyBlock! Type undefined: ";
			Type type = value.GetType();
			Debug.LogError(log_PREFIX + text + ((type != null) ? type.ToString() : null));
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000160D7 File Offset: 0x000142D7
		public static string ToDateString(this DateTime dateTime)
		{
			return dateTime.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000160EA File Offset: 0x000142EA
		public static bool TryParseDateTime(this string dateTimeString, out DateTime result)
		{
			return DateTime.TryParseExact(dateTimeString, "MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00016100 File Offset: 0x00014300
		public static Vector3 Round(this Vector3 vector3, int decimals)
		{
			vector3.x = (float)Math.Round((double)vector3.x, decimals);
			vector3.y = (float)Math.Round((double)vector3.y, decimals);
			vector3.z = (float)Math.Round((double)vector3.z, decimals);
			return vector3;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00016150 File Offset: 0x00014350
		public static int GenerateHash(params int[] numbers)
		{
			HashCode hashCode = default(HashCode);
			for (int i = 0; i < numbers.Length; i++)
			{
				hashCode.Add<int>(numbers[i]);
			}
			return hashCode.ToHashCode();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00016184 File Offset: 0x00014384
		public static void SetPosition(this Matrix4x4 matrix, Vector3 position)
		{
			matrix.m03 = position.x;
			matrix.m13 = position.y;
			matrix.m23 = position.z;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000161AC File Offset: 0x000143AC
		public static void SetScale(this Matrix4x4 matrix, Vector3 scale)
		{
			Matrix4x4 matrix4x = Matrix4x4.Scale(Vector3.Scale(scale, matrix.lossyScale.Reciprocal()));
			matrix *= matrix4x;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000161E4 File Offset: 0x000143E4
		public static bool EqualsMatrix4x4(this Matrix4x4 a, Matrix4x4 b)
		{
			return a.m00 == b.m00 && a.m01 == b.m01 && a.m02 == b.m02 && a.m03 == b.m03 && a.m10 == b.m10 && a.m11 == b.m11 && a.m12 == b.m12 && a.m13 == b.m13 && a.m20 == b.m20 && a.m21 == b.m21 && a.m22 == b.m22 && a.m23 == b.m23 && a.m30 == b.m30 && a.m31 == b.m31 && a.m32 == b.m32 && a.m33 == b.m33;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x000162E8 File Offset: 0x000144E8
		public static void MousePointsToPlanes(Camera cam, Vector2 p1, Vector2 p2, float farPlane, Plane[] planes)
		{
			Vector3 position = cam.transform.position;
			Vector2 vector = Vector2.Min(p1, p2);
			Vector2 vector2 = Vector2.Max(p1, p2);
			vector.y = (float)cam.pixelHeight - vector.y;
			vector2.y = (float)cam.pixelHeight - vector2.y;
			Ray ray = cam.ScreenPointToRay(vector);
			Ray ray2 = cam.ScreenPointToRay(new Vector2(vector.x, vector2.y));
			Ray ray3 = cam.ScreenPointToRay(new Vector2(vector2.x, vector.y));
			Ray ray4 = cam.ScreenPointToRay(vector2);
			planes[0].Set3Points(position, ray.origin + ray.direction, ray2.origin + ray2.direction);
			planes[1].Set3Points(position, ray4.origin + ray4.direction, ray3.origin + ray3.direction);
			planes[2].Set3Points(position, ray2.origin + ray2.direction, ray4.origin + ray4.direction);
			planes[3].Set3Points(position, ray3.origin + ray3.direction, ray.origin + ray.direction);
			planes[4].Set3Points(ray4.origin - ray4.direction + position, ray3.origin - ray3.direction + position, ray.origin - ray.direction + position);
			planes[5].Set3Points(ray2.origin + ray2.direction * farPlane, ray.origin + ray.direction * farPlane, ray4.origin + ray4.direction * farPlane);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00016514 File Offset: 0x00014714
		public static bool TestPlanesAABBComplete(Plane[] planes, Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			foreach (Plane plane in planes)
			{
				if (!plane.GetSide(min) || !plane.GetSide(max))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00016564 File Offset: 0x00014764
		public static string FormatNumberWithSuffix(this long num)
		{
			if (num >= 1000000L)
			{
				return ((double)num / 1000000.0).ToString("0.0M");
			}
			if (num >= 10000L)
			{
				return ((double)num / 1000.0).ToString("0.0k");
			}
			return num.ToString("#,0");
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000165C4 File Offset: 0x000147C4
		public static string FormatNumberWithSuffix(this int num)
		{
			if (num >= 1000000)
			{
				return ((double)num / 1000000.0).ToString("0.0M");
			}
			if (num >= 10000)
			{
				return ((double)num / 1000.0).ToString("0.0k");
			}
			return num.ToString("#,0");
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00016624 File Offset: 0x00014824
		public static string FormatNumberWithSuffix(this uint num)
		{
			if (num >= 1000000U)
			{
				return (num / 1000000.0).ToString("0.0M");
			}
			if (num >= 10000U)
			{
				return (num / 1000.0).ToString("0.0k");
			}
			return num.ToString("#,0");
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00016684 File Offset: 0x00014884
		public static bool Approximately(this Color color, Color other, bool includeAlpha = false, float errorMargin = 0.002f)
		{
			return math.abs(color.r - other.r) < errorMargin && math.abs(color.g - other.g) < errorMargin && math.abs(color.b - other.b) < errorMargin && (!includeAlpha || math.abs(color.a - other.a) < errorMargin);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000166EC File Offset: 0x000148EC
		public static bool Approximately(this Quaternion rotation, Quaternion other, float errorMargin = 0.0001f)
		{
			return 1f - Mathf.Abs(Quaternion.Dot(rotation, other)) < errorMargin;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00016703 File Offset: 0x00014903
		public static bool Approximately(this Vector3 position, Vector3 other, float errorMargin = 0.0001f)
		{
			return Vector3.Distance(position, other) < errorMargin;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00016710 File Offset: 0x00014910
		public static bool Approximately(this Matrix4x4 a, Matrix4x4 b, float errorMargin = 0.0001f)
		{
			for (int i = 0; i < 16; i++)
			{
				if (Mathf.Abs(a[i] - b[i]) > errorMargin)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00016746 File Offset: 0x00014946
		public static float BLerp(float c00, float c10, float c01, float c11, float u, float v)
		{
			return math.lerp(math.lerp(c00, c10, u), math.lerp(c01, c11, u), v);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00016761 File Offset: 0x00014961
		public static int4 RoundToInt(float4 input)
		{
			return new int4(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y), Mathf.RoundToInt(input.z), Mathf.RoundToInt(input.w));
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00016794 File Offset: 0x00014994
		public static float4 GetRGBAFromFloat(float v)
		{
			float4 @float = new float4(1f, 255f, 65025f, 16581375f);
			float num = 0.003921569f;
			float4 float2 = @float * v;
			float2 = math.frac(float2);
			return float2 - float2.yzww * num;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000167E4 File Offset: 0x000149E4
		public static Color32 GetColor32FromFloat(float v)
		{
			float4 rgbafromFloat = GPUIUtility.GetRGBAFromFloat(v);
			return new Color32((byte)Mathf.RoundToInt(rgbafromFloat.x * 255f), (byte)Mathf.RoundToInt(rgbafromFloat.y * 255f), (byte)Mathf.RoundToInt(rgbafromFloat.z * 255f), (byte)Mathf.RoundToInt(rgbafromFloat.w * 255f));
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00016848 File Offset: 0x00014A48
		public static float StoreRGBAInFloat(Vector4 rgba)
		{
			Vector4 vector = new Vector4(1f, 0.003921569f, 1.53787E-05f, 6.2273724E-09f);
			return Vector4.Dot(rgba, vector);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00016878 File Offset: 0x00014A78
		public static float StoreColor32InFloat(Color32 color32)
		{
			Vector4 vector = new Vector4(1f, 0.003921569f, 1.53787E-05f, 6.2273724E-09f);
			return Vector4.Dot(color32, vector);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000168B4 File Offset: 0x00014AB4
		public static Vector3 Reciprocal(this Vector3 vector3)
		{
			return new Vector3((vector3.x == 0f) ? 0f : (1f / vector3.x), (vector3.y == 0f) ? 0f : (1f / vector3.y), (vector3.z == 0f) ? 0f : (1f / vector3.z));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00016928 File Offset: 0x00014B28
		public static Bounds GetMatrixAppliedBounds(this Bounds bounds, Matrix4x4 matrix)
		{
			bounds.size = Vector3.Scale(bounds.size, matrix.lossyScale);
			bounds = bounds.GetRotationAppliedBounds(matrix.rotation);
			bounds.center += matrix.GetPosition();
			return bounds;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00016978 File Offset: 0x00014B78
		public static Bounds GetRotationAppliedBounds(this Bounds b, Quaternion q)
		{
			Vector3 extents = b.extents;
			float num = q.x * q.x;
			float num2 = q.y * q.y;
			float num3 = q.z * q.z;
			float num4 = q.x * q.y;
			float num5 = q.x * q.z;
			float num6 = q.y * q.z;
			float num7 = q.w * q.x;
			float num8 = q.w * q.y;
			float num9 = q.w * q.z;
			float num10 = 1f - 2f * (num2 + num3);
			float num11 = 2f * (num4 - num9);
			float num12 = 2f * (num5 + num8);
			float num13 = 2f * (num4 + num9);
			float num14 = 1f - 2f * (num + num3);
			float num15 = 2f * (num6 - num7);
			float num16 = 2f * (num5 - num8);
			float num17 = 2f * (num6 + num7);
			float num18 = 1f - 2f * (num + num2);
			Vector3 vector = new Vector3(((num10 < 0f) ? (-num10) : num10) * extents.x + ((num11 < 0f) ? (-num11) : num11) * extents.y + ((num12 < 0f) ? (-num12) : num12) * extents.z, ((num13 < 0f) ? (-num13) : num13) * extents.x + ((num14 < 0f) ? (-num14) : num14) * extents.y + ((num15 < 0f) ? (-num15) : num15) * extents.z, ((num16 < 0f) ? (-num16) : num16) * extents.x + ((num17 < 0f) ? (-num17) : num17) * extents.y + ((num18 < 0f) ? (-num18) : num18) * extents.z);
			b.center = GPUIUtility.RotatePositionAround(b.center, Vector3.up, q);
			b.extents = vector;
			return b;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00016B94 File Offset: 0x00014D94
		public static Bounds GetMatrixAppliedBoundsWithPivot(this Bounds bounds, Matrix4x4 matrix, Vector3 pivot)
		{
			bounds.size = Vector3.Scale(bounds.size, matrix.lossyScale);
			bounds = GPUIUtility.GetRotationAppliedBoundsWithPivot(bounds, pivot, matrix.rotation);
			bounds.center += matrix.GetPosition();
			return bounds;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public static Bounds GetRotationAppliedBoundsWithPivot(Bounds bounds, Vector3 pivot, Quaternion rotation)
		{
			GPUIUtility.<>c__DisplayClass57_0 CS$<>8__locals1;
			CS$<>8__locals1.rotation = rotation;
			CS$<>8__locals1.pivot = pivot;
			Vector3 center = bounds.center;
			Vector3 vector = bounds.extents;
			CS$<>8__locals1.centerOffset = center - CS$<>8__locals1.pivot;
			CS$<>8__locals1.min = GPUIConstants.Vector3_MAX;
			CS$<>8__locals1.max = GPUIConstants.Vector3_MIN;
			float x = vector.x;
			float y = vector.y;
			float z = vector.z;
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(x, y, z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(x, y, -z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(x, -y, z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(x, -y, -z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(-x, y, z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(-x, y, -z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(-x, -y, z, ref CS$<>8__locals1);
			GPUIUtility.<GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(-x, -y, -z, ref CS$<>8__locals1);
			vector = (CS$<>8__locals1.max - CS$<>8__locals1.min) * 0.5f;
			bounds.center = CS$<>8__locals1.min + vector;
			bounds.extents = vector;
			return bounds;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00016CFC File Offset: 0x00014EFC
		public static Matrix4x4 GetRotationMatrix(Quaternion q)
		{
			float num = q.x * q.x;
			float num2 = q.y * q.y;
			float num3 = q.z * q.z;
			float num4 = q.x * q.y;
			float num5 = q.x * q.z;
			float num6 = q.y * q.z;
			float num7 = q.w * q.x;
			float num8 = q.w * q.y;
			float num9 = q.w * q.z;
			Matrix4x4 matrix4x;
			matrix4x.m00 = 1f - 2f * (num2 + num3);
			matrix4x.m01 = 2f * (num4 - num9);
			matrix4x.m02 = 2f * (num5 + num8);
			matrix4x.m03 = 0f;
			matrix4x.m10 = 2f * (num4 + num9);
			matrix4x.m11 = 1f - 2f * (num + num3);
			matrix4x.m12 = 2f * (num6 - num7);
			matrix4x.m13 = 0f;
			matrix4x.m20 = 2f * (num5 - num8);
			matrix4x.m21 = 2f * (num6 + num7);
			matrix4x.m22 = 1f - 2f * (num + num2);
			matrix4x.m23 = 0f;
			matrix4x.m30 = 0f;
			matrix4x.m31 = 0f;
			matrix4x.m32 = 0f;
			matrix4x.m33 = 1f;
			return matrix4x;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00016E8E File Offset: 0x0001508E
		public static Vector3 RotatePositionAround(Vector3 position, Vector3 center, Quaternion rotation)
		{
			return rotation * (position - center) + center;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00016EA3 File Offset: 0x000150A3
		private static Quaternion FlipQuaternion(Quaternion q)
		{
			q.x = -q.x;
			q.y = -q.y;
			q.z = -q.z;
			q.w = -q.w;
			return q;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00016EE0 File Offset: 0x000150E0
		public static string CamelToTitleCase(string camelCaseText)
		{
			string text = "";
			while (camelCaseText.StartsWith("_"))
			{
				camelCaseText = camelCaseText.Substring(1);
			}
			if (camelCaseText.StartsWith("editor_"))
			{
				camelCaseText = camelCaseText.Substring(7);
			}
			if (camelCaseText.StartsWith("gpui"))
			{
				text += "GPUI ";
				camelCaseText = camelCaseText.Substring(4);
			}
			camelCaseText = camelCaseText.Substring(0, 1).ToUpper() + camelCaseText.Substring(1);
			return text + Regex.Replace(Regex.Replace(camelCaseText, "([A-Z])([a-z])", " $1$2"), "([a-z])([A-Z])", "$1 $2").Trim();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00016F8A File Offset: 0x0001518A
		public static bool CompareExtensionCode(string c1, string c2)
		{
			return (string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2)) || string.Equals(c1, c2);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00016FA8 File Offset: 0x000151A8
		public static string ConvertToGPUIShaderName(string originalShaderName, string extensionCode, string shaderNamePrefix = null)
		{
			if (originalShaderName.Contains("GPUInstancerPro"))
			{
				originalShaderName = originalShaderName.Replace("GPUInstancerPro/", "").Replace("GPUInstancerPro/CrowdAnimations/", "");
			}
			string shaderNamePrefix2 = GPUIConstants.GetShaderNamePrefix(extensionCode);
			if (string.IsNullOrEmpty(shaderNamePrefix))
			{
				shaderNamePrefix = shaderNamePrefix2;
			}
			bool flag = originalShaderName.StartsWith("Hidden/");
			if (flag)
			{
				originalShaderName = originalShaderName.Substring(7);
			}
			if (originalShaderName.StartsWith(shaderNamePrefix2))
			{
				originalShaderName = originalShaderName.Substring(shaderNamePrefix2.Length, originalShaderName.Length - shaderNamePrefix2.Length);
			}
			string text = shaderNamePrefix + originalShaderName;
			if (flag)
			{
				text = "Hidden/" + text;
			}
			return text;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00017048 File Offset: 0x00015248
		public static string RemoveSpacesAndLimitSize(this string input, int maxSize)
		{
			string text = input.Replace(" ", "");
			if (text.Length > maxSize)
			{
				text = text.Substring(0, maxSize);
			}
			return text;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001707C File Offset: 0x0001527C
		public static string ShortenString(this string value, int maxLength, bool addDots = false)
		{
			if (string.IsNullOrEmpty(value) || maxLength < 0)
			{
				return value;
			}
			if (value.Length <= maxLength)
			{
				return value;
			}
			if (!addDots)
			{
				return value.Substring(0, maxLength);
			}
			int num = 3;
			if (maxLength <= num)
			{
				return value.Substring(0, maxLength);
			}
			return value.Substring(0, maxLength - num) + "...";
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000170D1 File Offset: 0x000152D1
		public static string Matrix4x4ToString(Matrix4x4 matrix4x4)
		{
			return Regex.Replace(matrix4x4.ToString(), "[\\r\\n\\t]+", ";");
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000170F0 File Offset: 0x000152F0
		public static bool TryParseMatrix4x4(string matrixStr, out Matrix4x4 matrix4x4)
		{
			matrix4x4 = default(Matrix4x4);
			if (string.IsNullOrEmpty(matrixStr))
			{
				return false;
			}
			string[] array = matrixStr.Split(';', StringSplitOptions.None);
			if (array.Length < 16)
			{
				return false;
			}
			for (int i = 0; i < 16; i++)
			{
				float num;
				if (!float.TryParse(array[i], NumberStyles.Float, CultureInfo.InvariantCulture, out num))
				{
					return false;
				}
				matrix4x4[i / 4, i % 4] = num;
			}
			return true;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00017158 File Offset: 0x00015358
		public static Matrix4x4 Matrix4x4FromString(string matrixStr)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			string[] array = matrixStr.Split(';', StringSplitOptions.None);
			for (int i = 0; i < 16; i++)
			{
				matrix4x[i / 4, i % 4] = float.Parse(array[i], CultureInfo.InvariantCulture);
			}
			return matrix4x;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000171A0 File Offset: 0x000153A0
		public static string ReadTextFileAtPath(string filePath)
		{
			string text = null;
			if (File.Exists(filePath))
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					text = streamReader.ReadToEnd();
				}
			}
			return text;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000171E4 File Offset: 0x000153E4
		public static string GetRelativePathForShader(string shaderPathString, string includeFilePathString)
		{
			if (string.IsNullOrEmpty(shaderPathString) || string.IsNullOrEmpty(includeFilePathString))
			{
				return string.Empty;
			}
			if (shaderPathString.StartsWith("Packages/"))
			{
				shaderPathString = GPUIConstants.GetGeneratedShaderPath();
			}
			string text = Path.GetRelativePath(Path.GetDirectoryName(shaderPathString), includeFilePathString).Replace("\\", "/");
			if (!text.StartsWith("."))
			{
				text = "./" + text;
			}
			return text;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00017251 File Offset: 0x00015451
		public static string UintToBinaryString(uint value, int length = 32)
		{
			if (length < 1 || length > 32)
			{
				length = 32;
			}
			return Convert.ToString((long)((ulong)value), 2).PadLeft(32, '0').Substring(32 - length, length);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001727C File Offset: 0x0001547C
		public static string FormatBytesToString(long size)
		{
			if (size < 0L)
			{
				return "0";
			}
			double num = (double)size;
			string[] array = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
			int num2 = 0;
			while (num >= 100.0 && num2 < array.Length - 1)
			{
				num /= 1024.0;
				num2++;
			}
			return string.Format("{0:0.00} {1}", num, array[num2]);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00017314 File Offset: 0x00015514
		public static void SetData(this GraphicsBuffer targetBuffer, GraphicsBuffer sourceBuffer, int sourceStartIndex, int targetStartIndex, int count)
		{
			if (sourceBuffer == null || targetBuffer == null)
			{
				return;
			}
			if (count <= 0 || targetBuffer.count < targetStartIndex + count || sourceBuffer.count < sourceStartIndex + count)
			{
				return;
			}
			ComputeShader cs_GraphicsBufferUtility = GPUIConstants.CS_GraphicsBufferUtility;
			int num = 0;
			int stride = targetBuffer.stride;
			if (stride <= 16)
			{
				if (stride == 4)
				{
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_TRANSFORM_DATA");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_FLOAT4_BUFFER");
					cs_GraphicsBufferUtility.EnableKeyword("GPUI_UINT_BUFFER");
					goto IL_0109;
				}
				if (stride == 16)
				{
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_TRANSFORM_DATA");
					cs_GraphicsBufferUtility.EnableKeyword("GPUI_FLOAT4_BUFFER");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_UINT_BUFFER");
					goto IL_0109;
				}
			}
			else
			{
				if (stride == 40)
				{
					cs_GraphicsBufferUtility.EnableKeyword("GPUI_TRANSFORM_DATA");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_FLOAT4_BUFFER");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_UINT_BUFFER");
					goto IL_0109;
				}
				if (stride == 64)
				{
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_TRANSFORM_DATA");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_FLOAT4_BUFFER");
					cs_GraphicsBufferUtility.DisableKeyword("GPUI_UINT_BUFFER");
					goto IL_0109;
				}
			}
			Debug.LogError(GPUIConstants.LOG_PREFIX + "Unknown stride size for buffer: " + targetBuffer.stride.ToString());
			return;
			IL_0109:
			cs_GraphicsBufferUtility.SetBuffer(num, GPUIConstants.PROP_sourceBuffer, sourceBuffer);
			cs_GraphicsBufferUtility.SetBuffer(num, GPUIConstants.PROP_targetBuffer, targetBuffer);
			cs_GraphicsBufferUtility.SetInt(GPUIConstants.PROP_sourceStartIndex, sourceStartIndex);
			cs_GraphicsBufferUtility.SetInt(GPUIConstants.PROP_targetStartIndex, targetStartIndex);
			cs_GraphicsBufferUtility.SetInt(GPUIConstants.PROP_count, count);
			cs_GraphicsBufferUtility.DispatchX(num, count);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00017472 File Offset: 0x00015672
		public static void SetAllDataTo(this GraphicsBuffer buffer, Matrix4x4 value)
		{
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			cs_TransformModifications.SetBuffer(8, GPUIConstants.PROP_gpuiTransformBuffer, buffer);
			cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, buffer.count);
			cs_TransformModifications.SetMatrix(GPUIConstants.PROP_matrix44, value);
			cs_TransformModifications.DispatchX(8, buffer.count);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000174B0 File Offset: 0x000156B0
		public static void ClearBufferData(this GraphicsBuffer buffer)
		{
			if (buffer.stride % 4 != 0)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Given buffer stride size is not divisible by 4. Stride: " + buffer.stride.ToString());
				return;
			}
			int num = 0;
			int num2 = buffer.stride * buffer.count / 4;
			ComputeShader cs_GraphicsBufferUtility = GPUIConstants.CS_GraphicsBufferUtility;
			int num3 = 1;
			cs_GraphicsBufferUtility.SetBuffer(num3, GPUIConstants.PROP_destination, buffer);
			cs_GraphicsBufferUtility.SetInt(GPUIConstants.PROP_targetStartIndex, num);
			cs_GraphicsBufferUtility.SetInt(GPUIConstants.PROP_count, num2);
			cs_GraphicsBufferUtility.DispatchX(num3, num2);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017530 File Offset: 0x00015730
		public static T[] RemoveAtAndReturn<T>(this T[] array, int toRemove)
		{
			if (array == null || toRemove >= array.Length)
			{
				return array;
			}
			T[] array2 = new T[array.Length - 1];
			if (toRemove > 0)
			{
				Array.Copy(array, 0, array2, 0, toRemove);
			}
			if (toRemove < array.Length - 1)
			{
				Array.Copy(array, toRemove + 1, array2, toRemove, array.Length - toRemove - 1);
			}
			return array2;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001757C File Offset: 0x0001577C
		public static T[] AddAndReturn<T>(this T[] array, T toAdd)
		{
			T[] array2 = new T[array.Length + 1];
			Array.Copy(array, 0, array2, 0, array.Length);
			array2[array.Length] = toAdd;
			return array2;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000175AC File Offset: 0x000157AC
		public static T[] AddToBeginningAndReturn<T>(this T[] array, T toAdd)
		{
			T[] array2 = new T[array.Length + 1];
			array2[0] = toAdd;
			Array.Copy(array, 0, array2, 1, array.Length);
			return array2;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000175DC File Offset: 0x000157DC
		public static T[] MirrorAndFlatten<T>(this T[,] array2D)
		{
			T[] array = new T[array2D.GetLength(0) * array2D.GetLength(1)];
			for (int i = 0; i < array2D.GetLength(0); i++)
			{
				for (int j = 0; j < array2D.GetLength(1); j++)
				{
					array[j + i * array2D.GetLength(0)] = array2D[i, j];
				}
			}
			return array;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001763C File Offset: 0x0001583C
		public static bool Contains<T>(this T[] array, T element)
		{
			if (array == null)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					return element == null;
				}
				if (array[i].Equals(element))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00017698 File Offset: 0x00015898
		public static void RemoveNullValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TValue : Object
		{
			List<TKey> list = new List<TKey>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				if (keyValuePair.Value == null)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (TKey tkey in list)
			{
				dictionary.Remove(tkey);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00017748 File Offset: 0x00015948
		public static bool HasNullOrDuplicate<T>(List<T> list) where T : Object
		{
			if (list == null || list.Count == 0)
			{
				return false;
			}
			HashSet<T> hashSet = new HashSet<T>();
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				if (t == null || !hashSet.Add(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001779C File Offset: 0x0001599C
		public static void RemoveNullsAndDuplicates<T>(List<T> list) where T : Object
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			HashSet<T> hashSet = new HashSet<T>();
			int i = 0;
			while (i < list.Count)
			{
				T t = list[i];
				if (t == null || !hashSet.Add(t))
				{
					list.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000177F4 File Offset: 0x000159F4
		public static void ResizeNativeArray<T>(this NativeArray<T> array, int newSize, Allocator allocator) where T : struct
		{
			NativeArray<T> nativeArray = array;
			array = new NativeArray<T>(newSize, allocator, NativeArrayOptions.ClearMemory);
			if (nativeArray.IsCreated)
			{
				int num = Math.Min(nativeArray.Length, newSize);
				NativeSlice<T> nativeSlice = new NativeSlice<T>(array, 0, num);
				NativeSlice<T> nativeSlice2 = new NativeSlice<T>(nativeArray, 0, num);
				nativeSlice.CopyFrom(nativeSlice2);
				nativeArray.Dispose();
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00017858 File Offset: 0x00015A58
		public static void GizmoDrawWireMesh(GPUIPrototype prototype, Matrix4x4 matrix, bool drawBounds = true)
		{
			GPUILODGroupData gpuilodgroupData;
			if (GPUIRenderingSystem.IsActive && GPUIRenderingSystem.Instance.LODGroupDataProvider.TryGetData(prototype.GetKey(), out gpuilodgroupData))
			{
				GPUIUtility.GizmoDrawWireMesh(gpuilodgroupData, matrix, drawBounds);
				return;
			}
			if (prototype.prototypeType == GPUIPrototypeType.Prefab)
			{
				GameObject prefabObject = prototype.prefabObject;
				if (drawBounds)
				{
					GPUIUtility.GizmoDrawWireMesh(prefabObject.GetBounds(false), matrix);
					return;
				}
				LODGroup lodgroup;
				if (!prefabObject.TryGetComponent<LODGroup>(out lodgroup))
				{
					foreach (MeshFilter meshFilter in prefabObject.GetComponentsInChildren<MeshFilter>())
					{
						matrix *= meshFilter.transform.localToWorldMatrix * prefabObject.transform.localToWorldMatrix.inverse;
						for (int j = 0; j < meshFilter.sharedMesh.subMeshCount; j++)
						{
							Gizmos.DrawWireMesh(meshFilter.sharedMesh, j, matrix.GetPosition(), matrix.rotation, matrix.lossyScale);
						}
					}
					return;
				}
				LOD[] lods = lodgroup.GetLODs();
				if (lods.Length != 0)
				{
					Renderer[] renderers = lods[0].renderers;
					for (int i = 0; i < renderers.Length; i++)
					{
						MeshFilter meshFilter2;
						if (renderers[i].TryGetComponent<MeshFilter>(out meshFilter2))
						{
							matrix *= meshFilter2.transform.localToWorldMatrix * prefabObject.transform.localToWorldMatrix.inverse;
							for (int k = 0; k < meshFilter2.sharedMesh.subMeshCount; k++)
							{
								Gizmos.DrawWireMesh(meshFilter2.sharedMesh, k, matrix.GetPosition(), matrix.rotation, matrix.lossyScale);
							}
						}
					}
					return;
				}
			}
			else if (prototype.prototypeType == GPUIPrototypeType.LODGroupData)
			{
				GPUIUtility.GizmoDrawWireMesh(prototype.gpuiLODGroupData, matrix, drawBounds);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00017A10 File Offset: 0x00015C10
		public static void GizmoDrawWireMesh(GPUILODGroupData lodGroupData, Matrix4x4 matrix, bool drawBounds = true)
		{
			if (drawBounds)
			{
				GPUIUtility.GizmoDrawWireMesh(lodGroupData.bounds, matrix);
				return;
			}
			GPUILODData gpuiloddata = lodGroupData[0];
			for (int i = 0; i < gpuiloddata.Length; i++)
			{
				GPUIRendererData gpuirendererData = gpuiloddata[i];
				Matrix4x4 matrix4x = matrix * gpuirendererData.transformOffset;
				for (int j = 0; j < gpuirendererData.rendererMesh.subMeshCount; j++)
				{
					Gizmos.DrawWireMesh(gpuirendererData.rendererMesh, j, matrix4x.GetPosition(), matrix4x.rotation, matrix4x.lossyScale);
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00017A97 File Offset: 0x00015C97
		public static void GizmoDrawWireMesh(Bounds bounds, Matrix4x4 matrix)
		{
			Gizmos.matrix = matrix;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00017AB2 File Offset: 0x00015CB2
		public static bool IsInLayer(int layerMask, int layer)
		{
			return layerMask == (layerMask | (1 << layer));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00017ABF File Offset: 0x00015CBF
		public static void SetBuffer<T>(this ComputeShader cs, int kernelIndex, int nameID, GPUIDataBuffer<T> gpuiDataBuffer) where T : struct
		{
			cs.SetBuffer(kernelIndex, nameID, gpuiDataBuffer.Buffer);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00017ACF File Offset: 0x00015CCF
		public static void DispatchX(this ComputeShader cs, int kernelIndex, int size)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cs.Dispatch(kernelIndex, Mathf.CeilToInt((float)size / GPUIConstants.CS_THREAD_COUNT), 1, 1);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00017AFD File Offset: 0x00015CFD
		public static void DispatchXHeavy(this ComputeShader cs, int kernelIndex, int size)
		{
			if (GPUIConstants.CS_THREAD_COUNT_HEAVY == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cs.Dispatch(kernelIndex, Mathf.CeilToInt((float)size / GPUIConstants.CS_THREAD_COUNT_HEAVY), 1, 1);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00017B2B File Offset: 0x00015D2B
		public static void DispatchXY(this ComputeShader cs, int kernelIndex, int sizeX, int sizeY)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cs.Dispatch(kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_2D), Mathf.CeilToInt((float)sizeY / GPUIConstants.CS_THREAD_COUNT_2D), 1);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017B65 File Offset: 0x00015D65
		public static void DispatchXZ(this ComputeShader cs, int kernelIndex, int sizeX, int sizeZ)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cs.Dispatch(kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_2D), 1, Mathf.CeilToInt((float)sizeZ / GPUIConstants.CS_THREAD_COUNT_2D));
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00017BA0 File Offset: 0x00015DA0
		public static void DispatchXYZ(this ComputeShader cs, int kernelIndex, int sizeX, int sizeY, int sizeZ)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cs.Dispatch(kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_3D), Mathf.CeilToInt((float)sizeY / GPUIConstants.CS_THREAD_COUNT_3D), Mathf.CeilToInt((float)sizeZ / GPUIConstants.CS_THREAD_COUNT_3D));
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017BF2 File Offset: 0x00015DF2
		public static void DispatchComputeX(this CommandBuffer cb, ComputeShader cs, int kernelIndex, int size)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cb.DispatchCompute(cs, kernelIndex, Mathf.CeilToInt((float)size / GPUIConstants.CS_THREAD_COUNT), 1, 1);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00017C21 File Offset: 0x00015E21
		public static void DispatchComputeXHeavy(this CommandBuffer cb, ComputeShader cs, int kernelIndex, int size)
		{
			if (GPUIConstants.CS_THREAD_COUNT_HEAVY == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cb.DispatchCompute(cs, kernelIndex, Mathf.CeilToInt((float)size / GPUIConstants.CS_THREAD_COUNT_HEAVY), 1, 1);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00017C50 File Offset: 0x00015E50
		public static void DispatchComputeXY(this CommandBuffer cb, ComputeShader cs, int kernelIndex, int sizeX, int sizeY)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cb.DispatchCompute(cs, kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_2D), Mathf.CeilToInt((float)sizeY / GPUIConstants.CS_THREAD_COUNT_2D), 1);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00017C8C File Offset: 0x00015E8C
		public static void DispatchComputeXZ(this CommandBuffer cb, ComputeShader cs, int kernelIndex, int sizeX, int sizeZ)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cb.DispatchCompute(cs, kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_2D), 1, Mathf.CeilToInt((float)sizeZ / GPUIConstants.CS_THREAD_COUNT_2D));
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00017CC8 File Offset: 0x00015EC8
		public static void DispatchComputeXYZ(this CommandBuffer cb, ComputeShader cs, int kernelIndex, int sizeX, int sizeY, int sizeZ)
		{
			if (GPUIConstants.CS_THREAD_COUNT == 0f)
			{
				GPUIRuntimeSettings.Instance.DetermineOperationMode();
			}
			cb.DispatchCompute(cs, kernelIndex, Mathf.CeilToInt((float)sizeX / GPUIConstants.CS_THREAD_COUNT_3D), Mathf.CeilToInt((float)sizeY / GPUIConstants.CS_THREAD_COUNT_3D), Mathf.CeilToInt((float)sizeZ / GPUIConstants.CS_THREAD_COUNT_3D));
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00017D1C File Offset: 0x00015F1C
		public static string[] GetPropertyNames(this Shader shader, List<ShaderPropertyType> ignoreTypes = null, bool excludeUnityProperties = true)
		{
			int propertyCount = shader.GetPropertyCount();
			List<string> list = new List<string>();
			for (int i = 0; i < propertyCount; i++)
			{
				if (ignoreTypes == null || !ignoreTypes.Contains(shader.GetPropertyType(i)))
				{
					string propertyName = shader.GetPropertyName(i);
					if (!string.IsNullOrEmpty(propertyName) && (!excludeUnityProperties || !propertyName.StartsWith("unity_")))
					{
						list.Add(shader.GetPropertyName(i));
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00017D88 File Offset: 0x00015F88
		public static string[] GetPropertyNamesForType(this Shader shader, ShaderPropertyType propertyType, bool excludeUnityProperties = true)
		{
			int propertyCount = shader.GetPropertyCount();
			List<string> list = new List<string>();
			for (int i = 0; i < propertyCount; i++)
			{
				if (shader.GetPropertyType(i) == propertyType)
				{
					string propertyName = shader.GetPropertyName(i);
					if (!string.IsNullOrEmpty(propertyName) && (!excludeUnityProperties || !propertyName.StartsWith("unity_")))
					{
						list.Add(shader.GetPropertyName(i));
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00017DEC File Offset: 0x00015FEC
		public static Material CopyWithShader(this Material originalMaterial, Shader instancedShader)
		{
			Material material = new Material(instancedShader);
			material.CopyPropertiesFromMaterial(originalMaterial);
			string text = originalMaterial.name;
			if (!text.EndsWith(GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX))
			{
				text += GPUIShaderBindings.GPUI_REPLACEMENT_MATERIAL_NAME_SUFFIX;
			}
			material.name = text;
			material.hideFlags = HideFlags.HideAndDontSave;
			return material;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00017E38 File Offset: 0x00016038
		public static HashSet<Shader> GetUniqueShaders(GameObject root)
		{
			HashSet<Shader> hashSet = new HashSet<Shader>();
			if (root == null)
			{
				return hashSet;
			}
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (Material material in componentsInChildren[i].sharedMaterials)
				{
					if (!(material == null))
					{
						Shader shader = material.shader;
						if (shader != null)
						{
							hashSet.Add(shader);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00017EB4 File Offset: 0x000160B4
		public static void RenderMeshIndirect(in RenderParams rparams, Mesh mesh, GPUIDataBuffer<GraphicsBuffer.IndirectDrawIndexedArgs> commandBuffer, int commandCount = 1, int startCommand = 0)
		{
			Graphics.RenderMeshIndirect(in rparams, mesh, commandBuffer.Buffer, commandCount, startCommand);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00017EC6 File Offset: 0x000160C6
		public static bool IsDepthTextureAvailable(this Camera camera)
		{
			return camera.depthTextureMode.HasFlag(DepthTextureMode.Depth) || camera.actualRenderingPath == RenderingPath.DeferredShading;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00017EEC File Offset: 0x000160EC
		public static Mesh GenerateQuadMesh(float width, float height, Rect? uvRect = null, bool centerPivotAtBottom = false, float pivotOffsetX = 0f, float pivotOffsetY = 0f, bool setVertexColors = false)
		{
			Mesh mesh = new Mesh();
			mesh.name = "QuadMesh_GPUI";
			mesh.vertices = new Vector3[]
			{
				new Vector3(centerPivotAtBottom ? (-width / 2f - pivotOffsetX) : (-pivotOffsetX), -pivotOffsetY, 0f),
				new Vector3(centerPivotAtBottom ? (-width / 2f - pivotOffsetX) : (-pivotOffsetX), height - pivotOffsetY, 0f),
				new Vector3(centerPivotAtBottom ? (width / 2f - pivotOffsetX) : (width - pivotOffsetX), height - pivotOffsetY, 0f),
				new Vector3(centerPivotAtBottom ? (width / 2f - pivotOffsetX) : (width - pivotOffsetX), -pivotOffsetY, 0f)
			};
			if (uvRect != null)
			{
				mesh.uv = new Vector2[]
				{
					new Vector2(uvRect.Value.x, uvRect.Value.y),
					new Vector2(uvRect.Value.x, uvRect.Value.y + uvRect.Value.height),
					new Vector2(uvRect.Value.x + uvRect.Value.width, uvRect.Value.y + uvRect.Value.height),
					new Vector2(uvRect.Value.x + uvRect.Value.width, uvRect.Value.y)
				};
			}
			else
			{
				mesh.uv = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f)
				};
			}
			mesh.triangles = new int[] { 0, 1, 3, 1, 2, 3 };
			Vector3 vector = new Vector3(0f, 0f, -1f);
			Vector4 vector2 = new Vector4(1f, 0f, 0f, -1f);
			mesh.normals = new Vector3[] { vector, vector, vector, vector };
			mesh.tangents = new Vector4[] { vector2, vector2, vector2, vector2 };
			if (setVertexColors)
			{
				Color[] array = new Color[mesh.vertices.Length];
				for (int i = 0; i < mesh.vertices.Length; i++)
				{
					array[i] = Color.Lerp(Color.clear, Color.white, mesh.vertices[i].y);
				}
				mesh.colors = array;
			}
			return mesh;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00018218 File Offset: 0x00016418
		public static Shader FindShader(string shaderName)
		{
			Shader shader = Shader.Find(shaderName);
			if (GPUIRuntimeSettings.Instance.loadShadersFromAddressables && shader == null)
			{
				try
				{
					shader = Addressables.LoadAssetAsync<Shader>(shaderName).WaitForCompletion();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			return shader;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001826C File Offset: 0x0001646C
		public static T LoadResource<T>(string path) where T : Object
		{
			T t = Resources.Load<T>(path);
			if (GPUIRuntimeSettings.Instance.loadResourcesFromAddressables && t == null)
			{
				try
				{
					t = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			return t;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000182C4 File Offset: 0x000164C4
		public static T CreateDelegate<T>(this MethodInfo methodInfo) where T : Delegate
		{
			return (T)((object)methodInfo.CreateDelegate(typeof(T)));
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000182DB File Offset: 0x000164DB
		public static T[] GetListInternalArray<T>(List<T> list)
		{
			return (T[])GPUIUtility._listItemsField.GetValue(list);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000182F0 File Offset: 0x000164F0
		public unsafe static void CalculateInterpolatedLightAndOcclusionProbes(ref GraphicsBuffer perInstanceLightProbesBuffer, void* p_matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int rsgBufferSize, List<Vector3> positions, List<SphericalHarmonicsL2> sphericalHarmonics, List<Vector4> occlusionProbes, GraphicsBuffer sphericalHarmonicsBuffer, GraphicsBuffer occlusionProbesBuffer, Vector3 positionOffset)
		{
			positions.Clear();
			if (positions.Capacity < count)
			{
				positions.Capacity = count;
			}
			sphericalHarmonics.Clear();
			if (sphericalHarmonics.Capacity < count)
			{
				sphericalHarmonics.Capacity = count;
			}
			occlusionProbes.Clear();
			if (occlusionProbes.Capacity < count)
			{
				occlusionProbes.Capacity = count;
			}
			Vector3 zero = Vector3.zero;
			if (positionOffset == Vector3.zero)
			{
				for (int i = 0; i < count; i++)
				{
					Vector4 vector = UnsafeUtility.ReadArrayElementWithStride<Vector4>(p_matrices, (i + managedBufferStartIndex) * 4 + 3, 16);
					zero.x = vector.x;
					zero.y = vector.y;
					zero.z = vector.z;
					positions.Add(zero);
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					Matrix4x4 matrix4x = UnsafeUtility.ReadArrayElementWithStride<Matrix4x4>(p_matrices, j + managedBufferStartIndex, 64);
					zero.x = matrix4x.m03 + positionOffset.x * (float)Math.Sqrt((double)(matrix4x.m00 * matrix4x.m00 + matrix4x.m01 * matrix4x.m01 + matrix4x.m02 * matrix4x.m02));
					zero.y = matrix4x.m13 + positionOffset.y * (float)Math.Sqrt((double)(matrix4x.m10 * matrix4x.m10 + matrix4x.m11 * matrix4x.m11 + matrix4x.m22 * matrix4x.m22));
					zero.z = matrix4x.m23 + positionOffset.z * (float)Math.Sqrt((double)(matrix4x.m20 * matrix4x.m20 + matrix4x.m21 * matrix4x.m21 + matrix4x.m22 * matrix4x.m22));
					positions.Add(zero);
				}
			}
			LightProbes.CalculateInterpolatedLightAndOcclusionProbes(positions, sphericalHarmonics, occlusionProbes);
			int num = rsgBufferSize * 8;
			if (perInstanceLightProbesBuffer != null)
			{
				if (num != perInstanceLightProbesBuffer.count)
				{
					GraphicsBuffer graphicsBuffer = perInstanceLightProbesBuffer;
					perInstanceLightProbesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 16);
					perInstanceLightProbesBuffer.SetData(graphicsBuffer, 0, 0, Mathf.Min(num, graphicsBuffer.count));
					graphicsBuffer.Dispose();
				}
			}
			else
			{
				perInstanceLightProbesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 16);
			}
			sphericalHarmonicsBuffer.SetData<SphericalHarmonicsL2>(sphericalHarmonics, 0, 0, count);
			occlusionProbesBuffer.SetData<Vector4>(occlusionProbes, 0, 0, count);
			ComputeShader cs_LightProbeUtility = GPUIConstants.CS_LightProbeUtility;
			int num2 = 0;
			cs_LightProbeUtility.SetBuffer(num2, GPUIConstants.PROP_gpuiPerInstanceLightProbesBuffer, perInstanceLightProbesBuffer);
			cs_LightProbeUtility.SetBuffer(num2, GPUIConstants.PROP_sphericalHarmonicsBuffer, sphericalHarmonicsBuffer);
			cs_LightProbeUtility.SetBuffer(num2, GPUIConstants.PROP_occlusionProbesBuffer, occlusionProbesBuffer);
			cs_LightProbeUtility.SetInt(GPUIConstants.PROP_count, count);
			cs_LightProbeUtility.SetInt(GPUIConstants.PROP_startIndex, graphicsBufferStartIndex);
			cs_LightProbeUtility.DispatchX(num2, count);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00018594 File Offset: 0x00016794
		public static byte[] CompressGZip(byte[] data)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Fastest, true))
				{
					gzipStream.Write(data, 0, data.Length);
				}
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000185F8 File Offset: 0x000167F8
		public static byte[] DecompressGZip(byte[] compressed)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						gzipStream.CopyTo(memoryStream2);
						array = memoryStream2.ToArray();
					}
				}
			}
			return array;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00018674 File Offset: 0x00016874
		public static void DecompressGZipAsync(byte[] compressedData, Action<byte[]> onComplete)
		{
			if (compressedData != null && compressedData.Length != 0)
			{
				ulong generationAtStart = GPUIRenderingSystem.Threading_GetGeneration();
				Action <>9__2;
				Task.Run(delegate
				{
					try
					{
						byte[] decompressed = GPUIUtility.DecompressGZip(compressedData);
						if (generationAtStart == GPUIRenderingSystem.Threading_GetGeneration())
						{
							if (onComplete != null)
							{
								GPUIRenderingSystem.Threading_RunOnMain(delegate
								{
									onComplete(decompressed);
								});
							}
						}
					}
					catch (Exception ex)
					{
						if (generationAtStart == GPUIRenderingSystem.Threading_GetGeneration())
						{
							Debug.LogException(ex);
							if (onComplete != null)
							{
								Action action;
								if ((action = <>9__2) == null)
								{
									action = (<>9__2 = delegate
									{
										onComplete(null);
									});
								}
								GPUIRenderingSystem.Threading_RunOnMain(action);
							}
						}
					}
				});
				return;
			}
			Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Compressed data is null or empty.");
			Action<byte[]> onComplete2 = onComplete;
			if (onComplete2 == null)
			{
				return;
			}
			onComplete2(Array.Empty<byte>());
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001870C File Offset: 0x0001690C
		[CompilerGenerated]
		internal static void <GetRotationAppliedBoundsWithPivot>g__Accumulate|57_0(float ox, float oy, float oz, ref GPUIUtility.<>c__DisplayClass57_0 A_3)
		{
			Vector3 vector = A_3.rotation * new Vector3(A_3.centerOffset.x + ox, A_3.centerOffset.y + oy, A_3.centerOffset.z + oz) + A_3.pivot;
			if (vector.x < A_3.min.x)
			{
				A_3.min.x = vector.x;
			}
			if (vector.y < A_3.min.y)
			{
				A_3.min.y = vector.y;
			}
			if (vector.z < A_3.min.z)
			{
				A_3.min.z = vector.z;
			}
			if (vector.x > A_3.max.x)
			{
				A_3.max.x = vector.x;
			}
			if (vector.y > A_3.max.y)
			{
				A_3.max.y = vector.y;
			}
			if (vector.z > A_3.max.z)
			{
				A_3.max.z = vector.z;
			}
		}

		// Token: 0x04000334 RID: 820
		private static readonly FieldInfo _listItemsField = typeof(List<>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
