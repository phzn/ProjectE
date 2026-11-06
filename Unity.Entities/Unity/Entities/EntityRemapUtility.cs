using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200018C RID: 396
	public static class EntityRemapUtility
	{
		// Token: 0x060009D5 RID: 2517 RVA: 0x00031678 File Offset: 0x0002F878
		public unsafe static void GetTargets(out NativeArray<Entity> output, NativeArray<EntityRemapUtility.EntityRemapInfo> remapping)
		{
			NativeArray<Entity> nativeArray = new NativeArray<Entity>(remapping.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			int num = 0;
			for (int i = 0; i < remapping.Length; i++)
			{
				if (remapping[i].Target != Entity.Null)
				{
					nativeArray[num++] = remapping[i].Target;
				}
			}
			output = new NativeArray<Entity>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			UnsafeUtility.MemCpy(output.GetUnsafePtr<Entity>(), nativeArray.GetUnsafePtr<Entity>(), (long)(sizeof(Entity) * num));
			nativeArray.Dispose();
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00031710 File Offset: 0x0002F910
		public static void AddEntityRemapping(ref NativeArray<EntityRemapUtility.EntityRemapInfo> remapping, Entity source, Entity target)
		{
			remapping[source.Index] = new EntityRemapUtility.EntityRemapInfo
			{
				SourceVersion = source.Version,
				Target = target
			};
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00031747 File Offset: 0x0002F947
		public unsafe static Entity RemapEntity(ref NativeArray<EntityRemapUtility.EntityRemapInfo> remapping, Entity source)
		{
			return EntityRemapUtility.RemapEntity((EntityRemapUtility.EntityRemapInfo*)remapping.GetUnsafeReadOnlyPtr<EntityRemapUtility.EntityRemapInfo>(), source);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0003175A File Offset: 0x0002F95A
		public unsafe static Entity RemapEntity(EntityRemapUtility.EntityRemapInfo* remapping, Entity source)
		{
			if (source.Version == remapping[source.Index].SourceVersion)
			{
				return remapping[source.Index].Target;
			}
			return Entity.Null;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00031794 File Offset: 0x0002F994
		public unsafe static Entity RemapEntityForPrefab(Entity* remapSrc, Entity* remapDst, int remappingCount, Entity source)
		{
			for (int num = 0; num != remappingCount; num++)
			{
				if (source == remapSrc[num])
				{
					return remapDst[num];
				}
			}
			return source;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000317D8 File Offset: 0x0002F9D8
		public static void CalculateFieldOffsetsUnmanaged(Type type, out bool hasEntityRefs, out bool hasBlobRefs, out bool hasWeakAssetRefs, out bool hasUnityObjectRefs, ref NativeList<TypeManager.EntityOffsetInfo> entityOffsets, ref NativeList<TypeManager.EntityOffsetInfo> blobOffsets, ref NativeList<TypeManager.EntityOffsetInfo> weakAssetRefOffsets, ref NativeList<TypeManager.EntityOffsetInfo> unityObjectRefOffsets, HashSet<Type> cache = null)
		{
			if (cache == null)
			{
				cache = new HashSet<Type>();
			}
			int length = entityOffsets.Length;
			int length2 = blobOffsets.Length;
			int length3 = weakAssetRefOffsets.Length;
			int length4 = unityObjectRefOffsets.Length;
			EntityRemapUtility.CalculateOffsetsRecurse(ref entityOffsets, ref blobOffsets, ref weakAssetRefOffsets, ref unityObjectRefOffsets, type, 0, cache);
			hasEntityRefs = entityOffsets.Length != length;
			hasBlobRefs = blobOffsets.Length != length2;
			hasWeakAssetRefs = weakAssetRefOffsets.Length != length3;
			hasUnityObjectRefs = unityObjectRefOffsets.Length != length4;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00031860 File Offset: 0x0002FA60
		private static bool CalculateOffsetsRecurse(ref NativeList<TypeManager.EntityOffsetInfo> entityOffsets, ref NativeList<TypeManager.EntityOffsetInfo> blobOffsets, ref NativeList<TypeManager.EntityOffsetInfo> weakAssetRefOffsets, ref NativeList<TypeManager.EntityOffsetInfo> unityObjectRefOffsets, Type type, int baseOffset, HashSet<Type> noOffsetTypes)
		{
			if (noOffsetTypes.Contains(type))
			{
				return false;
			}
			if (type == typeof(Entity))
			{
				TypeManager.EntityOffsetInfo entityOffsetInfo = default(TypeManager.EntityOffsetInfo);
				entityOffsetInfo.Offset = baseOffset;
				entityOffsets.Add(in entityOffsetInfo);
				return true;
			}
			if (type == typeof(BlobAssetReferenceData))
			{
				TypeManager.EntityOffsetInfo entityOffsetInfo = default(TypeManager.EntityOffsetInfo);
				entityOffsetInfo.Offset = baseOffset;
				blobOffsets.Add(in entityOffsetInfo);
				return true;
			}
			if (type == typeof(UntypedWeakReferenceId))
			{
				TypeManager.EntityOffsetInfo entityOffsetInfo = default(TypeManager.EntityOffsetInfo);
				entityOffsetInfo.Offset = baseOffset;
				weakAssetRefOffsets.Add(in entityOffsetInfo);
				return true;
			}
			if (type == typeof(UntypedUnityObjectRef))
			{
				TypeManager.EntityOffsetInfo entityOffsetInfo = default(TypeManager.EntityOffsetInfo);
				entityOffsetInfo.Offset = baseOffset;
				unityObjectRefOffsets.Add(in entityOffsetInfo);
				return true;
			}
			bool flag = false;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.FieldType.IsValueType && !fieldInfo.FieldType.IsPrimitive)
				{
					flag |= EntityRemapUtility.CalculateOffsetsRecurse(ref entityOffsets, ref blobOffsets, ref weakAssetRefOffsets, ref unityObjectRefOffsets, fieldInfo.FieldType, baseOffset + UnsafeUtility.GetFieldOffset(fieldInfo), noOffsetTypes);
				}
			}
			if (!flag)
			{
				noOffsetTypes.Add(type);
			}
			return flag;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0003199A File Offset: 0x0002FB9A
		public static void HasEntityReferencesManaged(Type type, out EntityRemapUtility.HasRefResult hasEntityReferences, out EntityRemapUtility.HasRefResult hasBlobReferences, out EntityRemapUtility.HasRefResult hasUnityObjectReferences, Dictionary<Type, EntityRemapUtility.EntityBlobRefResult> cache = null, int maxDepth = 128)
		{
			hasEntityReferences = EntityRemapUtility.HasRefResult.NoRef;
			hasBlobReferences = EntityRemapUtility.HasRefResult.NoRef;
			hasUnityObjectReferences = EntityRemapUtility.HasRefResult.NoRef;
			if (cache == null)
			{
				cache = new Dictionary<Type, EntityRemapUtility.EntityBlobRefResult>();
			}
			EntityRemapUtility.ProcessEntityOrBlobReferencesRecursiveManaged(type, ref hasEntityReferences, ref hasBlobReferences, ref hasUnityObjectReferences, 0, ref cache, maxDepth);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x000319C0 File Offset: 0x0002FBC0
		private static void ProcessEntityOrBlobReferencesRecursiveManaged(Type type, ref EntityRemapUtility.HasRefResult hasEntityReferences, ref EntityRemapUtility.HasRefResult hasBlobReferences, ref EntityRemapUtility.HasRefResult hasUnityObjectReferences, int depth, ref Dictionary<Type, EntityRemapUtility.EntityBlobRefResult> cache, int maxDepth = 10)
		{
			if (depth > maxDepth)
			{
				hasEntityReferences = EntityRemapUtility.HasRefResult.MayHaveRef;
				hasBlobReferences = EntityRemapUtility.HasRefResult.MayHaveRef;
				hasUnityObjectReferences = EntityRemapUtility.HasRefResult.MayHaveRef;
				return;
			}
			if (cache.ContainsKey(type))
			{
				EntityRemapUtility.EntityBlobRefResult entityBlobRefResult = cache[type];
				hasEntityReferences = entityBlobRefResult.HasEntityRef;
				hasBlobReferences = entityBlobRefResult.HasBlobRef;
				hasUnityObjectReferences = entityBlobRefResult.HasUnityObjectRef;
				return;
			}
			EntityRemapUtility.HasRefResult hasRefResult = EntityRemapUtility.HasRefResult.NoRef;
			EntityRemapUtility.HasRefResult hasRefResult2 = EntityRemapUtility.HasRefResult.NoRef;
			EntityRemapUtility.HasRefResult hasRefResult3 = EntityRemapUtility.HasRefResult.NoRef;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (hasRefResult > EntityRemapUtility.HasRefResult.NoRef && hasRefResult2 > EntityRemapUtility.HasRefResult.NoRef && hasRefResult3 > EntityRemapUtility.HasRefResult.NoRef)
				{
					break;
				}
				Type type2 = fieldInfo.FieldType;
				if (type2.IsArray)
				{
					type2 = type2.GetElementType();
				}
				else if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(List<>))
				{
					type2 = type2.GetGenericArguments()[0];
				}
				if (!type2.IsPrimitive && !typeof(Object).IsAssignableFrom(type2))
				{
					if (type2 == typeof(Entity))
					{
						hasRefResult = EntityRemapUtility.HasRefResult.HasRef;
					}
					else if (type == typeof(BlobAssetReferenceData))
					{
						hasRefResult2 = EntityRemapUtility.HasRefResult.HasRef;
					}
					else if (type2 == typeof(UntypedUnityObjectRef))
					{
						hasRefResult3 = EntityRemapUtility.HasRefResult.HasRef;
					}
					else if (type2.IsValueType || type2.IsSealed)
					{
						EntityRemapUtility.HasRefResult hasRefResult4 = EntityRemapUtility.HasRefResult.NoRef;
						EntityRemapUtility.HasRefResult hasRefResult5 = EntityRemapUtility.HasRefResult.NoRef;
						EntityRemapUtility.HasRefResult hasRefResult6 = EntityRemapUtility.HasRefResult.NoRef;
						EntityRemapUtility.ProcessEntityOrBlobReferencesRecursiveManaged(type2, ref hasRefResult4, ref hasRefResult5, ref hasRefResult6, depth + 1, ref cache, maxDepth);
						hasRefResult = ((hasRefResult > hasRefResult4) ? hasRefResult : hasRefResult4);
						hasRefResult2 = ((hasRefResult2 > hasRefResult5) ? hasRefResult2 : hasRefResult5);
						hasRefResult3 = ((hasRefResult3 > hasRefResult6) ? hasRefResult3 : hasRefResult6);
					}
					else
					{
						hasRefResult = EntityRemapUtility.HasRefResult.MayHaveRef;
						hasRefResult2 = EntityRemapUtility.HasRefResult.MayHaveRef;
						hasRefResult3 = EntityRemapUtility.HasRefResult.MayHaveRef;
					}
				}
			}
			if (!cache.ContainsKey(type))
			{
				cache[type] = new EntityRemapUtility.EntityBlobRefResult(hasRefResult, hasRefResult2, hasRefResult3);
			}
			hasEntityReferences = ((hasEntityReferences > hasRefResult) ? hasEntityReferences : hasRefResult);
			hasBlobReferences = ((hasBlobReferences > hasRefResult2) ? hasBlobReferences : hasRefResult2);
			hasUnityObjectReferences = ((hasUnityObjectReferences > hasRefResult3) ? hasUnityObjectReferences : hasRefResult3);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00031B98 File Offset: 0x0002FD98
		public unsafe static EntityRemapUtility.EntityPatchInfo* AppendEntityPatches(EntityRemapUtility.EntityPatchInfo* patches, TypeManager.EntityOffsetInfo* offsets, int offsetCount, int baseOffset, int stride)
		{
			if (offsets == null)
			{
				return patches;
			}
			for (int i = 0; i < offsetCount; i++)
			{
				patches[i] = new EntityRemapUtility.EntityPatchInfo
				{
					Offset = baseOffset + offsets[i].Offset,
					Stride = stride
				};
			}
			return patches + offsetCount;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00031C00 File Offset: 0x0002FE00
		public unsafe static EntityRemapUtility.BufferEntityPatchInfo* AppendBufferEntityPatches(EntityRemapUtility.BufferEntityPatchInfo* patches, TypeManager.EntityOffsetInfo* offsets, int offsetCount, int bufferBaseOffset, int bufferStride, int elementStride)
		{
			if (offsets == null)
			{
				return patches;
			}
			for (int i = 0; i < offsetCount; i++)
			{
				patches[i] = new EntityRemapUtility.BufferEntityPatchInfo
				{
					BufferOffset = bufferBaseOffset,
					BufferStride = bufferStride,
					ElementOffset = offsets[i].Offset,
					ElementStride = elementStride
				};
			}
			return patches + offsetCount;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00031C78 File Offset: 0x0002FE78
		public unsafe static void PatchEntities(EntityRemapUtility.EntityPatchInfo* scalarPatches, int scalarPatchCount, EntityRemapUtility.BufferEntityPatchInfo* bufferPatches, int bufferPatchCount, byte* chunkBuffer, int entityCount, ref NativeArray<EntityRemapUtility.EntityRemapInfo> remapping)
		{
			for (int i = 0; i < scalarPatchCount; i++)
			{
				byte* ptr = chunkBuffer + scalarPatches[i].Offset;
				for (int num = 0; num != entityCount; num++)
				{
					Entity* ptr2 = (Entity*)ptr;
					*ptr2 = EntityRemapUtility.RemapEntity(ref remapping, *ptr2);
					ptr += scalarPatches[i].Stride;
				}
			}
			for (int j = 0; j < bufferPatchCount; j++)
			{
				byte* ptr3 = chunkBuffer + bufferPatches[j].BufferOffset;
				for (int num2 = 0; num2 != entityCount; num2++)
				{
					BufferHeader* ptr4 = (BufferHeader*)ptr3;
					byte* ptr5 = BufferHeader.GetElementPointer(ptr4) + bufferPatches[j].ElementOffset;
					int length = ptr4->Length;
					for (int num3 = 0; num3 != length; num3++)
					{
						Entity* ptr6 = (Entity*)ptr5;
						*ptr6 = EntityRemapUtility.RemapEntity(ref remapping, *ptr6);
						ptr5 += bufferPatches[j].ElementStride;
					}
					ptr3 += bufferPatches[j].BufferStride;
				}
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00031D9C File Offset: 0x0002FF9C
		public unsafe static void PatchEntitiesForPrefab(EntityRemapUtility.EntityPatchInfo* scalarPatches, int scalarPatchCount, EntityRemapUtility.BufferEntityPatchInfo* bufferPatches, int bufferPatchCount, byte* chunkBuffer, int indexInChunk, int entityCount, Entity* remapSrc, Entity* remapDst, int remappingCount)
		{
			for (int i = 0; i < scalarPatchCount; i++)
			{
				byte* ptr = chunkBuffer + scalarPatches[i].Offset;
				for (int num = 0; num != entityCount; num++)
				{
					Entity* ptr2 = (Entity*)(ptr + scalarPatches[i].Stride * (num + indexInChunk));
					*ptr2 = EntityRemapUtility.RemapEntityForPrefab(remapSrc, remapDst + num * remappingCount, remappingCount, *ptr2);
				}
			}
			for (int j = 0; j < bufferPatchCount; j++)
			{
				byte* ptr3 = chunkBuffer + bufferPatches[j].BufferOffset;
				for (int num2 = 0; num2 != entityCount; num2++)
				{
					BufferHeader* ptr4 = (BufferHeader*)(ptr3 + bufferPatches[j].BufferStride * (num2 + indexInChunk));
					byte* ptr5 = BufferHeader.GetElementPointer(ptr4) + bufferPatches[j].ElementOffset;
					int length = ptr4->Length;
					for (int num3 = 0; num3 != length; num3++)
					{
						Entity* ptr6 = (Entity*)ptr5;
						*ptr6 = EntityRemapUtility.RemapEntityForPrefab(remapSrc, remapDst + num2 * remappingCount, remappingCount, *ptr6);
						ptr5 += bufferPatches[j].ElementStride;
					}
				}
			}
		}

		// Token: 0x0200018D RID: 397
		public struct EntityRemapInfo
		{
			// Token: 0x040004B0 RID: 1200
			public int SourceVersion;

			// Token: 0x040004B1 RID: 1201
			public Entity Target;
		}

		// Token: 0x0200018E RID: 398
		public struct EntityPatchInfo
		{
			// Token: 0x040004B2 RID: 1202
			public int Offset;

			// Token: 0x040004B3 RID: 1203
			public int Stride;
		}

		// Token: 0x0200018F RID: 399
		public struct BufferEntityPatchInfo
		{
			// Token: 0x040004B4 RID: 1204
			public int BufferOffset;

			// Token: 0x040004B5 RID: 1205
			public int BufferStride;

			// Token: 0x040004B6 RID: 1206
			public int ElementOffset;

			// Token: 0x040004B7 RID: 1207
			public int ElementStride;
		}

		// Token: 0x02000190 RID: 400
		public enum HasRefResult
		{
			// Token: 0x040004B9 RID: 1209
			NoRef,
			// Token: 0x040004BA RID: 1210
			MayHaveRef,
			// Token: 0x040004BB RID: 1211
			HasRef
		}

		// Token: 0x02000191 RID: 401
		public struct EntityBlobRefResult
		{
			// Token: 0x060009E2 RID: 2530 RVA: 0x00031EE7 File Offset: 0x000300E7
			public EntityBlobRefResult(EntityRemapUtility.HasRefResult hasEntityRef, EntityRemapUtility.HasRefResult hasBlobRef, EntityRemapUtility.HasRefResult hasUnityObjectRef)
			{
				this.HasEntityRef = hasEntityRef;
				this.HasBlobRef = hasBlobRef;
				this.HasUnityObjectRef = hasUnityObjectRef;
			}

			// Token: 0x040004BC RID: 1212
			public EntityRemapUtility.HasRefResult HasEntityRef;

			// Token: 0x040004BD RID: 1213
			public EntityRemapUtility.HasRefResult HasBlobRef;

			// Token: 0x040004BE RID: 1214
			public EntityRemapUtility.HasRefResult HasUnityObjectRef;
		}
	}
}
