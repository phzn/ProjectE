using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.Extensions
{
	// Token: 0x02000030 RID: 48
	public static class MonoBehExtensions
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004F3C File Offset: 0x0000313C
		public static void DestroyChilds(this GameObject parent)
		{
			if (parent == null)
			{
				return;
			}
			int childCount = parent.transform.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				parent.transform.GetChild(i).gameObject.Destroy();
			}
			Debug.Log("log_current_canvas_childs_destroy " + childCount.ToString());
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004F9A File Offset: 0x0000319A
		public static bool TryGetComponentSafe<T>(this GameObject gameObject, out T component)
		{
			component = default(T);
			return !(gameObject == null) && gameObject.TryGetComponent<T>(out component);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004FB5 File Offset: 0x000031B5
		public static T[] GetChilds<T>(this GameObject gameObject)
		{
			return gameObject.GetComponentsInChildren<T>(true).Skip<T>(1).ToArray<T>();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004FCC File Offset: 0x000031CC
		public static GameObject RemoveRectTransform(this GameObject gameObject)
		{
			GameObject gameObject2 = MonoBehExtensions.CreateEmptyGameObject(null, null);
			gameObject2.name = gameObject.name;
			int siblingIndex = gameObject.transform.GetSiblingIndex();
			gameObject2.transform.SetParent(gameObject.transform.parent);
			gameObject2.transform.localPosition = gameObject.transform.localPosition;
			gameObject2.transform.localRotation = gameObject.transform.localRotation;
			gameObject2.transform.localScale = gameObject.transform.localScale;
			gameObject2.transform.SetSiblingIndex(siblingIndex);
			for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
			{
				Transform child = gameObject.transform.GetChild(i);
				int siblingIndex2 = child.GetSiblingIndex();
				child.SetParent(gameObject2.transform);
				child.SetSiblingIndex(siblingIndex2);
			}
			foreach (Component component in gameObject.GetComponents<Component>())
			{
				if (!(component is Transform))
				{
					Component component2 = gameObject2.AddComponent(component.GetType());
					component.CopySerializedFields(component2);
				}
			}
			gameObject.Destroy();
			return gameObject2;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000050E8 File Offset: 0x000032E8
		public static List<T> GetComponentsInReverseOrder<T>(this GameObject parent) where T : Component
		{
			MonoBehExtensions.<>c__DisplayClass4_0<T> CS$<>8__locals1;
			CS$<>8__locals1.results = new List<T>();
			MonoBehExtensions.<GetComponentsInReverseOrder>g__AddComponentsInReverseOrder|4_0<T>(parent.transform, ref CS$<>8__locals1);
			return CS$<>8__locals1.results;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005114 File Offset: 0x00003314
		public static bool SaveAsPrefabAsset<T>(this GameObject gameObject, string localPath, out T savedPrefab, out Exception ex) where T : MonoBehaviour
		{
			if (gameObject == null)
			{
				ex = new NullReferenceException("GameObject is null.");
				savedPrefab = default(T);
				return false;
			}
			ex = new Exception("Unsupported in not-Editor mode.");
			savedPrefab = default(T);
			return false;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005148 File Offset: 0x00003348
		public static bool IsPartOfAnyPrefab(this Object gameObject)
		{
			gameObject == null;
			return false;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005153 File Offset: 0x00003353
		public static bool IsExistsOnScene<T>() where T : MonoBehaviour
		{
			return Object.FindObjectsOfType<T>().Length != 0;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005160 File Offset: 0x00003360
		public static bool Destroy(this Object @object)
		{
			if (@object == null)
			{
				return false;
			}
			bool flag;
			try
			{
				@object != null;
				Object.Destroy(@object);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000051A4 File Offset: 0x000033A4
		public static bool TryAddComponent<T>(this GameObject gameObject, out T component, bool supportMultiInstance = false) where T : Component
		{
			component = default(T);
			if (gameObject == null)
			{
				Debug.LogWarning("gameObject is null.");
				return false;
			}
			if (gameObject.TryGetComponent<T>(out component) && !supportMultiInstance)
			{
				return true;
			}
			component = gameObject.AddComponent<T>();
			return false;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000051E0 File Offset: 0x000033E0
		public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component
		{
			bool flag;
			try
			{
				component = gameObject.GetComponent<T>();
				string name = component.name;
				flag = true;
			}
			catch
			{
				component = default(T);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005228 File Offset: 0x00003428
		public static bool TryAddGraphic<T>(this GameObject gameObject, out T graphic) where T : Graphic
		{
			if (gameObject.TryGetComponent<T>(out graphic))
			{
				return false;
			}
			Graphic graphic2;
			if (gameObject.TryGetComponent<Graphic>(out graphic2))
			{
				return false;
			}
			graphic = gameObject.AddComponent<T>();
			return true;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000525C File Offset: 0x0000345C
		public static bool TryDestroyComponent<T>(this GameObject gameObject) where T : Component
		{
			T t;
			if (gameObject.TryGetComponent<T>(out t))
			{
				t.Destroy();
				return true;
			}
			return false;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005282 File Offset: 0x00003482
		public static void SetDirtyExt(this Object @object)
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005284 File Offset: 0x00003484
		public static void MakeGameObjectSelectedInHierarchy(this GameObject activeGameObject)
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005288 File Offset: 0x00003488
		public static GameObject CreateEmptyGameObject(string name = null, Transform parent = null)
		{
			GameObject gameObject = new GameObject();
			GameObject gameObject2;
			if (parent == null)
			{
				gameObject2 = Object.Instantiate<GameObject>(gameObject);
			}
			else
			{
				gameObject2 = Object.Instantiate<GameObject>(gameObject, parent);
			}
			if (name != null)
			{
				gameObject.name = name;
			}
			gameObject.Destroy();
			return gameObject2;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000052C8 File Offset: 0x000034C8
		[CompilerGenerated]
		internal static void <GetComponentsInReverseOrder>g__AddComponentsInReverseOrder|4_0<T>(Transform current, ref MonoBehExtensions.<>c__DisplayClass4_0<T> A_1) where T : Component
		{
			for (int i = current.childCount - 1; i >= 0; i--)
			{
				MonoBehExtensions.<GetComponentsInReverseOrder>g__AddComponentsInReverseOrder|4_0<T>(current.GetChild(i), ref A_1);
			}
			T component = current.GetComponent<T>();
			if (component != null)
			{
				A_1.results.Add(component);
			}
		}
	}
}
