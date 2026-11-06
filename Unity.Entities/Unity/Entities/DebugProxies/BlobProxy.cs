using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C7 RID: 1223
	internal static class BlobProxy
	{
		// Token: 0x06001D62 RID: 7522 RVA: 0x000666B6 File Offset: 0x000648B6
		private unsafe static object GetPrimitive<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(IntPtr basePtr) where T : struct, ValueType
		{
			return *(T*)basePtr.ToPointer();
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000666C9 File Offset: 0x000648C9
		private unsafe static string UnpackString(void* basePtr)
		{
			return UnsafeUtility.AsRef<BlobString>(basePtr).ToString();
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000666D8 File Offset: 0x000648D8
		internal unsafe static object UnpackValue(void* basePtr, Type type)
		{
			if (type == typeof(BlobString))
			{
				return BlobProxy.UnpackString(basePtr);
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(BlobArray<>))
				{
					return Activator.CreateInstance(typeof(BlobArrayDebug<>).MakeGenericType(type.GetGenericArguments()), new object[]
					{
						new IntPtr(basePtr)
					});
				}
				if (genericTypeDefinition == typeof(BlobPtr<>))
				{
					return Activator.CreateInstance(typeof(BlobPtrDebug<>).MakeGenericType(type.GetGenericArguments()), new object[]
					{
						new IntPtr(basePtr)
					});
				}
			}
			if (type.IsValueType && !type.IsPrimitive)
			{
				return Activator.CreateInstance(typeof(BlobStruct<>).MakeGenericType(new Type[] { type }), new object[]
				{
					new IntPtr(basePtr)
				});
			}
			return typeof(BlobProxy).GetMethod("GetPrimitive", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[] { type }).Invoke(null, new object[]
			{
				new IntPtr(basePtr)
			});
		}
	}
}
