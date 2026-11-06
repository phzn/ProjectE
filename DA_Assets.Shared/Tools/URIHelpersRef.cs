using System;
using System.Reflection;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000014 RID: 20
	public class URIHelpersRef
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002E7C File Offset: 0x0000107C
		public static string MakeAssetUri(Object asset)
		{
			if (asset == null)
			{
				return null;
			}
			if (URIHelpersRef._uriHelpersType == null)
			{
				URIHelpersRef._uriHelpersType = Type.GetType("UnityEditor.UIElements.StyleSheets.URIHelpers, UnityEditor");
			}
			if (URIHelpersRef._uriHelpersType == null)
			{
				Debug.LogError("Failed to find the type URIHelpers. Ensure that you are using this code within the Unity Editor.");
				return null;
			}
			if (URIHelpersRef._makeAssetUriMethod == null)
			{
				URIHelpersRef._makeAssetUriMethod = URIHelpersRef._uriHelpersType.GetMethod("MakeAssetUri", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Object) }, null);
			}
			if (URIHelpersRef._makeAssetUriMethod == null)
			{
				Debug.LogError("Failed to find the method MakeAssetUri. Check if the method is accessible.");
				return null;
			}
			string text = (string)URIHelpersRef._makeAssetUriMethod.Invoke(null, new object[] { asset });
			int num = text.IndexOf("?fileID=");
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			return text;
		}

		// Token: 0x0400002E RID: 46
		private static Type _uriHelpersType;

		// Token: 0x0400002F RID: 47
		private static MethodInfo _makeAssetUriMethod;
	}
}
