using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000028 RID: 40
	public static class ResourceManagerConfig
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00005110 File Offset: 0x00003310
		public static bool ExtractKeyAndSubKey(object keyObj, out string mainKey, out string subKey)
		{
			string text = keyObj as string;
			if (text != null)
			{
				int num = text.IndexOf('[');
				if (num > 0)
				{
					int num2 = text.LastIndexOf(']');
					if (num2 > num)
					{
						mainKey = text.Substring(0, num);
						subKey = text.Substring(num + 1, num2 - (num + 1));
						return true;
					}
				}
			}
			mainKey = null;
			subKey = null;
			return false;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005164 File Offset: 0x00003364
		public static bool IsPathRemote(string path)
		{
			return path != null && path.StartsWith("http", StringComparison.Ordinal);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005178 File Offset: 0x00003378
		public static string StripQueryParameters(string path)
		{
			if (path != null)
			{
				int num = path.IndexOf('?');
				if (num >= 0)
				{
					return path.Substring(0, num);
				}
			}
			return path;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000519F File Offset: 0x0000339F
		public static bool ShouldPathUseWebRequest(string path)
		{
			return (!ResourceManagerConfig.PlatformCanLoadLocallyFromUrlPath() || !File.Exists(path)) && path != null && path.Contains("://");
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000051C2 File Offset: 0x000033C2
		private static bool PlatformCanLoadLocallyFromUrlPath()
		{
			return new List<RuntimePlatform> { RuntimePlatform.Android }.Contains(Application.platform);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000051DC File Offset: 0x000033DC
		public static Array CreateArrayResult(Type type, Object[] allAssets)
		{
			Type elementType = type.GetElementType();
			if (elementType == null)
			{
				return null;
			}
			int num = 0;
			foreach (Object @object in allAssets)
			{
				if (elementType.IsAssignableFrom(@object.GetType()))
				{
					num++;
				}
			}
			Array array = Array.CreateInstance(elementType, num);
			int num2 = 0;
			foreach (Object object2 in allAssets)
			{
				if (elementType.IsAssignableFrom(object2.GetType()))
				{
					array.SetValue(object2, num2++);
				}
			}
			return array;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005271 File Offset: 0x00003471
		public static TObject CreateArrayResult<TObject>(Object[] allAssets) where TObject : class
		{
			return ResourceManagerConfig.CreateArrayResult(typeof(TObject), allAssets) as TObject;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005290 File Offset: 0x00003490
		public static IList CreateListResult(Type type, Object[] allAssets)
		{
			Type[] genericArguments = type.GetGenericArguments();
			IList list = Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArguments)) as IList;
			Type type2 = genericArguments[0];
			if (list == null)
			{
				return null;
			}
			foreach (Object @object in allAssets)
			{
				if (type2.IsAssignableFrom(@object.GetType()))
				{
					list.Add(@object);
				}
			}
			return list;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000052FB File Offset: 0x000034FB
		public static TObject CreateListResult<TObject>(Object[] allAssets)
		{
			return (TObject)((object)ResourceManagerConfig.CreateListResult(typeof(TObject), allAssets));
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005314 File Offset: 0x00003514
		public static bool IsInstance<T1, T2>()
		{
			Type typeFromHandle = typeof(T1);
			return typeof(T2).IsAssignableFrom(typeFromHandle);
		}
	}
}
