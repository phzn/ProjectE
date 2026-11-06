using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Serialization;
using Unity.Profiling;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x020002FF RID: 767
	[BurstCompile]
	public struct FastEquality
	{
		// Token: 0x060014D8 RID: 5336 RVA: 0x0004F6C7 File Offset: 0x0004D8C7
		private static ushort AddDelegate(Delegate d)
		{
			ushort count = (ushort)FastEquality.s_ComponentDelegates.Count;
			FastEquality.s_ComponentDelegates.Add(d);
			return count;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0004F6DF File Offset: 0x0004D8DF
		private static Delegate GetDelegate(ushort index)
		{
			return FastEquality.s_ComponentDelegates[(int)index];
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0004F6EC File Offset: 0x0004D8EC
		internal static void Initialize()
		{
			FastEquality.s_ComponentDelegates = new List<Delegate>();
			FastEquality.AddDelegate(null);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004F6FF File Offset: 0x0004D8FF
		internal static void Shutdown()
		{
			FastEquality.s_ComponentDelegates.Clear();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004F70B File Offset: 0x0004D90B
		internal static FastEquality.TypeInfo CreateTypeInfo<T>(Dictionary<Type, List<FastEquality.LayoutInfo>> cache = null) where T : struct
		{
			if (FastEquality.TypeUsesDelegates(typeof(T)))
			{
				return FastEquality.CreateManagedTypeInfo(typeof(T));
			}
			return FastEquality.CreateTypeInfoBlittable(typeof(T), cache);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004F73E File Offset: 0x0004D93E
		internal static FastEquality.TypeInfo CreateTypeInfo(Type type, Dictionary<Type, List<FastEquality.LayoutInfo>> cache = null)
		{
			if (FastEquality.TypeUsesDelegates(type))
			{
				return FastEquality.CreateManagedTypeInfo(type);
			}
			return FastEquality.CreateTypeInfoBlittable(type, cache);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004F758 File Offset: 0x0004D958
		private static FastEquality.TypeInfo CreateManagedTypeInfo(Type t)
		{
			if (typeof(ISharedComponentData).IsAssignableFrom(t))
			{
				if (!typeof(IEquatable<>).MakeGenericType(new Type[] { t }).IsAssignableFrom(t))
				{
					throw new ArgumentException(string.Format("type {0} is a ISharedComponentData and has managed references, you must implement IEquatable<T>", t));
				}
				MethodInfo method = t.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
				if (method == null || method.DeclaringType != t)
				{
					throw new ArgumentException(string.Format("type {0} is a/has managed references or implements IEquatable<T>, you must also override GetHashCode()", t));
				}
			}
			ushort num = FastEquality.TypeInfo.Null.EqualsDelegateIndex;
			ushort num2 = FastEquality.TypeInfo.Null.GetHashCodeDelegateIndex;
			if (!UnsafeUtility.IsUnmanaged(t))
			{
				if (typeof(IEquatable<>).MakeGenericType(new Type[] { t }).IsAssignableFrom(t))
				{
					MethodInfo method2 = typeof(FastEquality.ManagedCompareImpl<>).MakeGenericType(new Type[] { t }).GetMethod("CompareFunc");
					num = FastEquality.AddDelegate(Delegate.CreateDelegate(typeof(FastEquality.TypeInfo.ManagedCompareEqualDelegate), method2));
				}
				MethodInfo method3 = t.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
				if (method3 != null && method3.DeclaringType == t)
				{
					MethodInfo method4 = typeof(FastEquality.ManagedGetHashCodeImpl<>).MakeGenericType(new Type[] { t }).GetMethod("GetHashCodeFunc");
					num2 = FastEquality.AddDelegate(Delegate.CreateDelegate(typeof(FastEquality.TypeInfo.ManagedGetHashCodeDelegate), method4));
				}
			}
			else
			{
				MethodInfo method5 = typeof(FastEquality.CompareImpl<>).MakeGenericType(new Type[] { t }).GetMethod("CompareFunc");
				num = FastEquality.AddDelegate(Delegate.CreateDelegate(typeof(FastEquality.TypeInfo.CompareEqualDelegate), method5));
				MethodInfo method6 = typeof(FastEquality.GetHashCodeImpl<>).MakeGenericType(new Type[] { t }).GetMethod("GetHashCodeFunc");
				num2 = FastEquality.AddDelegate(Delegate.CreateDelegate(typeof(FastEquality.TypeInfo.GetHashCodeDelegate), method6));
			}
			return new FastEquality.TypeInfo
			{
				EqualsDelegateIndex = num,
				GetHashCodeDelegateIndex = num2
			};
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004F968 File Offset: 0x0004DB68
		private static FastEquality.TypeInfo CreateTypeInfoBlittable(Type type, Dictionary<Type, List<FastEquality.LayoutInfo>> cache = null)
		{
			return new FastEquality.TypeInfo
			{
				LayoutInfo = FastEquality.CreateDescriptor(type, cache)
			};
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004F98C File Offset: 0x0004DB8C
		internal static NativeArray<FastEquality.LayoutInfo> CreateDescriptor(Type type, Dictionary<Type, List<FastEquality.LayoutInfo>> cache = null)
		{
			if (cache == null)
			{
				cache = new Dictionary<Type, List<FastEquality.LayoutInfo>>();
			}
			return new NativeArray<FastEquality.LayoutInfo>(FastEquality.FindFields(type, cache, 0, 1).ToArray(), Allocator.Persistent);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004F9AC File Offset: 0x0004DBAC
		private static List<FastEquality.LayoutInfo> FindFields(Type type, Dictionary<Type, List<FastEquality.LayoutInfo>> cache, int parentOffset = 0, int fixedSizeArrayLength = 1)
		{
			List<FastEquality.LayoutInfo> list;
			if (cache.TryGetValue(type, out list))
			{
				return list;
			}
			if (!type.IsValueType)
			{
				throw new ArgumentException(string.Format("{0} is not allowed: only value types are supported", type));
			}
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException(string.Format("{0} is not allowed: only concrete, fully-closed types are supported", type));
			}
			List<FastEquality.LayoutInfo> list2 = new List<FastEquality.LayoutInfo>();
			list2.Add(new FastEquality.LayoutInfo
			{
				Size = 0,
				Offset = 0
			});
			int num = parentOffset;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				int num2 = parentOffset + UnsafeUtility.GetFieldOffset(fieldInfo);
				if (num != num2)
				{
					list2.Add(new FastEquality.LayoutInfo
					{
						Size = 0,
						Offset = (ushort)num2
					});
				}
				if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.IsPointer)
				{
					int num3;
					if (fieldInfo.FieldType.IsPointer)
					{
						num3 = UnsafeUtility.SizeOf<IntPtr>();
					}
					else
					{
						num3 = UnsafeUtility.SizeOf(fieldInfo.FieldType) * fixedSizeArrayLength;
					}
					if (num2 + num3 > 65535)
					{
						throw new ArgumentException("Structures larger than 64k are not supported");
					}
					int num4 = list2.Count - 1;
					FastEquality.LayoutInfo layoutInfo = list2[num4];
					layoutInfo.Size += (ushort)num3;
					list2[num4] = layoutInfo;
					num = num2 + num3;
				}
				else
				{
					FixedBufferAttribute customAttribute = fieldInfo.GetCustomAttribute<FixedBufferAttribute>();
					if (customAttribute != null)
					{
						fixedSizeArrayLength = customAttribute.Length;
					}
					List<FastEquality.LayoutInfo> list3 = FastEquality.FindFields(fieldInfo.FieldType, cache, num2, fixedSizeArrayLength);
					cache.TryAdd(fieldInfo.FieldType, list3);
					if (list3.Count == 1)
					{
						FastEquality.LayoutInfo layoutInfo2 = list2[0];
						layoutInfo2.Size += list3[0].Size;
						list2[0] = layoutInfo2;
					}
					else
					{
						list2.AddRange(list3);
					}
				}
			}
			return list2;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0004FB84 File Offset: 0x0004DD84
		public unsafe static int ManagedGetHashCode(object lhs, FastEquality.TypeInfo typeInfo)
		{
			if (typeInfo.GetHashCodeDelegateIndex != FastEquality.TypeInfo.Null.GetHashCodeDelegateIndex)
			{
				return ((FastEquality.TypeInfo.ManagedGetHashCodeDelegate)FastEquality.GetDelegate(typeInfo.GetHashCodeDelegateIndex))(lhs);
			}
			int num = 0;
			int num2;
			using (UnityObjectRefMap unityObjectRefMap = new UnityObjectRefMap(Allocator.Temp))
			{
				using (UnsafeAppendBuffer unsafeAppendBuffer = new UnsafeAppendBuffer(16, 16, Allocator.Temp))
				{
					new ManagedObjectBinaryWriter(&unsafeAppendBuffer, unityObjectRefMap).WriteObject(lhs);
					num = FastEquality.Hash32(unsafeAppendBuffer.Ptr, unsafeAppendBuffer.Length, 0);
					if (unityObjectRefMap.InstanceIDs.Length > 0)
					{
						List<Object> list = new List<Object>(unityObjectRefMap.InstanceIDs.Length);
						NativeList<int> instanceIDs = unityObjectRefMap.InstanceIDs;
						Resources.InstanceIDToObjectList(instanceIDs.AsArray(), list);
						foreach (Object @object in list)
						{
							num *= 16777619;
							num ^= @object.GetHashCode();
						}
					}
					num2 = num;
				}
			}
			return num2;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004FCB8 File Offset: 0x0004DEB8
		[BurstDiscard]
		private unsafe static void GetHashCodeUsingDelegate(void* dataPtr, FastEquality.TypeInfo typeInfo, ref bool didWork, ref int hash)
		{
			if (typeInfo.GetHashCodeDelegateIndex != FastEquality.TypeInfo.Null.GetHashCodeDelegateIndex)
			{
				FastEquality.TypeInfo.GetHashCodeDelegate getHashCodeDelegate = (FastEquality.TypeInfo.GetHashCodeDelegate)FastEquality.GetDelegate(typeInfo.GetHashCodeDelegateIndex);
				hash = getHashCodeDelegate(dataPtr);
				didWork = true;
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
		[GenerateTestsForBurstCompatibility]
		internal unsafe static int GetHashCode(void* dataPtr, in FastEquality.TypeInfo typeInfo)
		{
			int num = 0;
			bool flag = false;
			FastEquality.GetHashCodeUsingDelegate(dataPtr, typeInfo, ref flag, ref num);
			if (flag)
			{
				return num;
			}
			return FastEquality.GetHashCodeBlittable(dataPtr, in typeInfo);
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0004FD21 File Offset: 0x0004DF21
		[BurstCompile]
		private unsafe static int GetHashCodeBlittable(void* dataPtr, in FastEquality.TypeInfo typeInfo)
		{
			return FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Invoke(dataPtr, in typeInfo);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0004FD2A File Offset: 0x0004DF2A
		[BurstCompile]
		internal unsafe static int Hash32(byte* ptr, int length, int seed = 0)
		{
			return FastEquality.Hash32_00001203$BurstDirectCall.Invoke(ptr, length, seed);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004FD34 File Offset: 0x0004DF34
		public static bool ManagedEquals(object lhs, object rhs, FastEquality.TypeInfo typeInfo)
		{
			if (typeInfo.EqualsDelegateIndex != FastEquality.TypeInfo.Null.EqualsDelegateIndex)
			{
				return ((FastEquality.TypeInfo.ManagedCompareEqualDelegate)FastEquality.GetDelegate(typeInfo.EqualsDelegateIndex))(lhs, rhs);
			}
			bool flag;
			using (FastEquality.ManagedEqualsMarker.Auto())
			{
				flag = new ManagedObjectEqual().CompareEqual(lhs, rhs);
			}
			return flag;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004FDA8 File Offset: 0x0004DFA8
		[BurstDiscard]
		private unsafe static void EqualsUsingDelegate(void* lhsPtr, void* rhsPtr, FastEquality.TypeInfo typeInfo, ref bool didWork, ref int result)
		{
			if (typeInfo.EqualsDelegateIndex != FastEquality.TypeInfo.Null.EqualsDelegateIndex)
			{
				FastEquality.TypeInfo.CompareEqualDelegate compareEqualDelegate = (FastEquality.TypeInfo.CompareEqualDelegate)FastEquality.GetDelegate(typeInfo.EqualsDelegateIndex);
				result = (compareEqualDelegate(lhsPtr, rhsPtr) ? 0 : 1);
				didWork = true;
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004FDEC File Offset: 0x0004DFEC
		[GenerateTestsForBurstCompatibility]
		public unsafe static bool Equals(void* lhsPtr, void* rhsPtr, in FastEquality.TypeInfo typeInfo)
		{
			int num = 0;
			bool flag = false;
			FastEquality.EqualsUsingDelegate(lhsPtr, rhsPtr, typeInfo, ref flag, ref num);
			if (flag)
			{
				return num == 0;
			}
			return FastEquality.EqualsBlittable(lhsPtr, rhsPtr, in typeInfo);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004FE1E File Offset: 0x0004E01E
		[BurstCompile]
		private unsafe static bool EqualsBlittable(void* lhsPtr, void* rhsPtr, in FastEquality.TypeInfo typeInfo)
		{
			return FastEquality.EqualsBlittable_00001207$BurstDirectCall.Invoke(lhsPtr, rhsPtr, in typeInfo);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0004FE28 File Offset: 0x0004E028
		private static bool TypeUsesDelegates(Type t)
		{
			return (t.IsClass && typeof(IComponentData).IsAssignableFrom(t)) || !UnsafeUtility.IsUnmanaged(t) || (typeof(ISharedComponentData).IsAssignableFrom(t) && typeof(IEquatable<>).MakeGenericType(new Type[] { t }).IsAssignableFrom(t));
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0004FE90 File Offset: 0x0004E090
		public static void AddExtraAOTTypes(Type type, HashSet<string> output)
		{
			if (!FastEquality.TypeUsesDelegates(type))
			{
				return;
			}
			if (type.IsClass)
			{
				if (typeof(IEquatable<>).MakeGenericType(new Type[] { type }).IsAssignableFrom(type))
				{
					output.Add(typeof(FastEquality.ManagedCompareImpl<>).MakeGenericType(new Type[] { type }).ToString());
				}
				MethodInfo method = type.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
				if (method != null && method.DeclaringType == type)
				{
					output.Add(typeof(FastEquality.ManagedGetHashCodeImpl<>).MakeGenericType(new Type[] { type }).ToString());
					return;
				}
			}
			else
			{
				output.Add(typeof(FastEquality.CompareImpl<>).MakeGenericType(new Type[] { type }).ToString());
				output.Add(typeof(FastEquality.GetHashCodeImpl<>).MakeGenericType(new Type[] { type }).ToString());
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004FFA4 File Offset: 0x0004E1A4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GetHashCodeBlittable$BurstManaged(void* dataPtr, in FastEquality.TypeInfo typeInfo)
		{
			int num = 0;
			NativeArray<FastEquality.LayoutInfo> layoutInfo = typeInfo.LayoutInfo;
			int length = layoutInfo.Length;
			for (int i = 0; i < length; i++)
			{
				FastEquality.LayoutInfo layoutInfo2 = layoutInfo[i];
				num ^= FastEquality.Hash32((byte*)dataPtr + layoutInfo2.Offset, (int)layoutInfo2.Size, num);
			}
			return num;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Hash32$BurstManaged(byte* ptr, int length, int seed = 0)
		{
			int num = seed;
			for (int i = 0; i < length; i++)
			{
				num *= 16777619;
				num ^= (int)ptr[i];
			}
			return num;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00050020 File Offset: 0x0004E220
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool EqualsBlittable$BurstManaged(void* lhsPtr, void* rhsPtr, in FastEquality.TypeInfo typeInfo)
		{
			int num = 0;
			NativeArray<FastEquality.LayoutInfo> layoutInfo = typeInfo.LayoutInfo;
			int length = layoutInfo.Length;
			for (int i = 0; i < length; i++)
			{
				FastEquality.LayoutInfo layoutInfo2 = layoutInfo[i];
				byte* ptr = (byte*)lhsPtr + layoutInfo2.Offset;
				byte* ptr2 = (byte*)rhsPtr + layoutInfo2.Offset;
				num |= UnsafeUtility.MemCmp((void*)ptr, (void*)ptr2, (long)((ulong)layoutInfo2.Size));
			}
			return num == 0;
		}

		// Token: 0x04000798 RID: 1944
		private static List<Delegate> s_ComponentDelegates;

		// Token: 0x04000799 RID: 1945
		private static readonly ProfilerMarker ManagedEqualsMarker = new ProfilerMarker("FastEquality.ManagedEquals with IPropertyVisitor fallback (Missing IEquatable interface)");

		// Token: 0x0400079A RID: 1946
		private const int FNV_32_PRIME = 16777619;

		// Token: 0x02000300 RID: 768
		internal struct LayoutInfo
		{
			// Token: 0x0400079B RID: 1947
			public ushort Size;

			// Token: 0x0400079C RID: 1948
			public ushort Offset;
		}

		// Token: 0x02000301 RID: 769
		[GenerateTestsForBurstCompatibility]
		public struct TypeInfo : IDisposable
		{
			// Token: 0x170001AA RID: 426
			// (get) Token: 0x060014F1 RID: 5361 RVA: 0x00050084 File Offset: 0x0004E284
			public static FastEquality.TypeInfo Null
			{
				get
				{
					return default(FastEquality.TypeInfo);
				}
			}

			// Token: 0x060014F2 RID: 5362 RVA: 0x0005009A File Offset: 0x0004E29A
			public void Dispose()
			{
				if (this.LayoutInfo.IsCreated)
				{
					this.LayoutInfo.Dispose();
				}
			}

			// Token: 0x0400079D RID: 1949
			internal NativeArray<FastEquality.LayoutInfo> LayoutInfo;

			// Token: 0x0400079E RID: 1950
			internal ushort EqualsDelegateIndex;

			// Token: 0x0400079F RID: 1951
			internal ushort GetHashCodeDelegateIndex;

			// Token: 0x02000302 RID: 770
			// (Invoke) Token: 0x060014F4 RID: 5364
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public unsafe delegate bool CompareEqualDelegate(void* lhs, void* rhs);

			// Token: 0x02000303 RID: 771
			// (Invoke) Token: 0x060014F8 RID: 5368
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public unsafe delegate int GetHashCodeDelegate(void* obj);

			// Token: 0x02000304 RID: 772
			// (Invoke) Token: 0x060014FC RID: 5372
			public delegate bool ManagedCompareEqualDelegate(object lhs, object rhs);

			// Token: 0x02000305 RID: 773
			// (Invoke) Token: 0x06001500 RID: 5376
			public delegate int ManagedGetHashCodeDelegate(object obj);
		}

		// Token: 0x02000306 RID: 774
		private struct Dummy : IEquatable<FastEquality.Dummy>
		{
			// Token: 0x06001503 RID: 5379 RVA: 0x000106DF File Offset: 0x0000E8DF
			public bool Equals(FastEquality.Dummy other)
			{
				return true;
			}

			// Token: 0x06001504 RID: 5380 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
			public override int GetHashCode()
			{
				return 0;
			}
		}

		// Token: 0x02000307 RID: 775
		private struct CompareImpl<T> where T : struct, IEquatable<T>
		{
			// Token: 0x06001505 RID: 5381 RVA: 0x000500B4 File Offset: 0x0004E2B4
			public unsafe static bool CompareFunc(void* lhs, void* rhs)
			{
				return UnsafeUtility.AsRef<T>(lhs).Equals(*UnsafeUtility.AsRef<T>(rhs));
			}
		}

		// Token: 0x02000308 RID: 776
		private struct GetHashCodeImpl<T> where T : struct, IEquatable<T>
		{
			// Token: 0x06001506 RID: 5382 RVA: 0x000500D2 File Offset: 0x0004E2D2
			public unsafe static int GetHashCodeFunc(void* lhs)
			{
				return UnsafeUtility.AsRef<T>(lhs).GetHashCode();
			}
		}

		// Token: 0x02000309 RID: 777
		private struct ManagedCompareImpl<T> where T : IEquatable<T>
		{
			// Token: 0x06001507 RID: 5383 RVA: 0x000500E8 File Offset: 0x0004E2E8
			public static bool CompareFunc(object lhs, object rhs)
			{
				T t = (T)((object)lhs);
				return t.Equals((T)((object)rhs));
			}
		}

		// Token: 0x0200030A RID: 778
		private struct ManagedGetHashCodeImpl<T>
		{
			// Token: 0x06001508 RID: 5384 RVA: 0x00050110 File Offset: 0x0004E310
			public static int GetHashCodeFunc(object val)
			{
				T t = (T)((object)val);
				return t.GetHashCode();
			}
		}

		// Token: 0x0200030B RID: 779
		// (Invoke) Token: 0x0600150A RID: 5386
		internal unsafe delegate int GetHashCodeBlittable_00001202$PostfixBurstDelegate(void* dataPtr, in FastEquality.TypeInfo typeInfo);

		// Token: 0x0200030C RID: 780
		internal static class GetHashCodeBlittable_00001202$BurstDirectCall
		{
			// Token: 0x0600150D RID: 5389 RVA: 0x00050131 File Offset: 0x0004E331
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Pointer == 0)
				{
					FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.DeferredCompilation, methodof(FastEquality.GetHashCodeBlittable$BurstManaged(void*, ref FastEquality.TypeInfo)).MethodHandle, typeof(FastEquality.GetHashCodeBlittable_00001202$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Pointer;
			}

			// Token: 0x0600150E RID: 5390 RVA: 0x00050160 File Offset: 0x0004E360
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600150F RID: 5391 RVA: 0x00050178 File Offset: 0x0004E378
			public unsafe static void Constructor()
			{
				FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(FastEquality.GetHashCodeBlittable(void*, ref FastEquality.TypeInfo)).MethodHandle);
			}

			// Token: 0x06001510 RID: 5392 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001511 RID: 5393 RVA: 0x00050189 File Offset: 0x0004E389
			// Note: this type is marked as 'beforefieldinit'.
			static GetHashCodeBlittable_00001202$BurstDirectCall()
			{
				FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.Constructor();
			}

			// Token: 0x06001512 RID: 5394 RVA: 0x00050190 File Offset: 0x0004E390
			public unsafe static int Invoke(void* dataPtr, in FastEquality.TypeInfo typeInfo)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = FastEquality.GetHashCodeBlittable_00001202$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.Void*,Unity.Entities.FastEquality/TypeInfo&), dataPtr, ref typeInfo, functionPointer);
					}
				}
				return FastEquality.GetHashCodeBlittable$BurstManaged(dataPtr, in typeInfo);
			}

			// Token: 0x040007A0 RID: 1952
			private static IntPtr Pointer;

			// Token: 0x040007A1 RID: 1953
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200030D RID: 781
		// (Invoke) Token: 0x06001514 RID: 5396
		internal unsafe delegate int Hash32_00001203$PostfixBurstDelegate(byte* ptr, int length, int seed = 0);

		// Token: 0x0200030E RID: 782
		internal static class Hash32_00001203$BurstDirectCall
		{
			// Token: 0x06001517 RID: 5399 RVA: 0x000501C3 File Offset: 0x0004E3C3
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (FastEquality.Hash32_00001203$BurstDirectCall.Pointer == 0)
				{
					FastEquality.Hash32_00001203$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(FastEquality.Hash32_00001203$BurstDirectCall.DeferredCompilation, methodof(FastEquality.Hash32$BurstManaged(byte*, int, int)).MethodHandle, typeof(FastEquality.Hash32_00001203$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = FastEquality.Hash32_00001203$BurstDirectCall.Pointer;
			}

			// Token: 0x06001518 RID: 5400 RVA: 0x000501F0 File Offset: 0x0004E3F0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				FastEquality.Hash32_00001203$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001519 RID: 5401 RVA: 0x00050208 File Offset: 0x0004E408
			public unsafe static void Constructor()
			{
				FastEquality.Hash32_00001203$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(FastEquality.Hash32(byte*, int, int)).MethodHandle);
			}

			// Token: 0x0600151A RID: 5402 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600151B RID: 5403 RVA: 0x00050219 File Offset: 0x0004E419
			// Note: this type is marked as 'beforefieldinit'.
			static Hash32_00001203$BurstDirectCall()
			{
				FastEquality.Hash32_00001203$BurstDirectCall.Constructor();
			}

			// Token: 0x0600151C RID: 5404 RVA: 0x00050220 File Offset: 0x0004E420
			public unsafe static int Invoke(byte* ptr, int length, int seed = 0)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = FastEquality.Hash32_00001203$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(System.Byte*,System.Int32,System.Int32), ptr, length, seed, functionPointer);
					}
				}
				return FastEquality.Hash32$BurstManaged(ptr, length, seed);
			}

			// Token: 0x040007A2 RID: 1954
			private static IntPtr Pointer;

			// Token: 0x040007A3 RID: 1955
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200030F RID: 783
		// (Invoke) Token: 0x0600151E RID: 5406
		internal unsafe delegate bool EqualsBlittable_00001207$PostfixBurstDelegate(void* lhsPtr, void* rhsPtr, in FastEquality.TypeInfo typeInfo);

		// Token: 0x02000310 RID: 784
		internal static class EqualsBlittable_00001207$BurstDirectCall
		{
			// Token: 0x06001521 RID: 5409 RVA: 0x00050255 File Offset: 0x0004E455
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (FastEquality.EqualsBlittable_00001207$BurstDirectCall.Pointer == 0)
				{
					FastEquality.EqualsBlittable_00001207$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(FastEquality.EqualsBlittable_00001207$BurstDirectCall.DeferredCompilation, methodof(FastEquality.EqualsBlittable$BurstManaged(void*, void*, ref FastEquality.TypeInfo)).MethodHandle, typeof(FastEquality.EqualsBlittable_00001207$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = FastEquality.EqualsBlittable_00001207$BurstDirectCall.Pointer;
			}

			// Token: 0x06001522 RID: 5410 RVA: 0x00050284 File Offset: 0x0004E484
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				FastEquality.EqualsBlittable_00001207$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001523 RID: 5411 RVA: 0x0005029C File Offset: 0x0004E49C
			public unsafe static void Constructor()
			{
				FastEquality.EqualsBlittable_00001207$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(FastEquality.EqualsBlittable(void*, void*, ref FastEquality.TypeInfo)).MethodHandle);
			}

			// Token: 0x06001524 RID: 5412 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001525 RID: 5413 RVA: 0x000502AD File Offset: 0x0004E4AD
			// Note: this type is marked as 'beforefieldinit'.
			static EqualsBlittable_00001207$BurstDirectCall()
			{
				FastEquality.EqualsBlittable_00001207$BurstDirectCall.Constructor();
			}

			// Token: 0x06001526 RID: 5414 RVA: 0x000502B4 File Offset: 0x0004E4B4
			public unsafe static bool Invoke(void* lhsPtr, void* rhsPtr, in FastEquality.TypeInfo typeInfo)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = FastEquality.EqualsBlittable_00001207$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(System.Void*,System.Void*,Unity.Entities.FastEquality/TypeInfo&), lhsPtr, rhsPtr, ref typeInfo, functionPointer);
					}
				}
				return FastEquality.EqualsBlittable$BurstManaged(lhsPtr, rhsPtr, in typeInfo);
			}

			// Token: 0x040007A4 RID: 1956
			private static IntPtr Pointer;

			// Token: 0x040007A5 RID: 1957
			private static IntPtr DeferredCompilation;
		}
	}
}
