using System;
using System.Collections.Generic;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x02000037 RID: 55
	public static class TransformExtensions
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00005F64 File Offset: 0x00004164
		public static int ClearChilds(this Transform transform)
		{
			int childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				transform.GetChild(i).gameObject.Destroy();
			}
			return childCount;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005F9C File Offset: 0x0000419C
		public static List<Transform> GetTopLevelChilds(this Transform parentObject)
		{
			List<Transform> list = new List<Transform>();
			Transform transform = parentObject;
			while (transform.parent)
			{
				transform = transform.parent;
			}
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				list.Add(transform2);
			}
			return list;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006014 File Offset: 0x00004214
		public static void SetTransformProps(this TransformProps transformProps, Transform source)
		{
			transformProps.position = source.position;
			transformProps.rotation = source.rotation;
			transformProps.parent = source.parent;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000603D File Offset: 0x0000423D
		public static void SetTransform(this Transform target, TransformProps transformProps)
		{
			target.transform.position = transformProps.position;
			target.transform.rotation = transformProps.rotation;
			target.transform.parent = transformProps.parent;
		}
	}
}
