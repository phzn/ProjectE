using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200003C RID: 60
	[BurstCompile]
	internal class ComponentSystemSorter
	{
		// Token: 0x060001CD RID: 461 RVA: 0x000091B4 File Offset: 0x000073B4
		internal unsafe static int LookupSystemElement(SystemTypeIndex typeIndex, NativeHashMap<SystemTypeIndex, int>* lookupDictionaryPtr)
		{
			ref NativeHashMap<SystemTypeIndex, int> ptr = ref *lookupDictionaryPtr;
			if (ptr.ContainsKey(typeIndex))
			{
				return ptr[typeIndex];
			}
			return -1;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000091D8 File Offset: 0x000073D8
		internal unsafe static void Sort(UnsafeList<ComponentSystemSorter.SystemElement>* elementsptr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary)
		{
			NativeList<SystemTypeIndex> nativeList = new NativeList<SystemTypeIndex>(16, Allocator.Temp);
			NativeList<SystemTypeIndex>* ptr = (NativeList<SystemTypeIndex>*)UnsafeUtility.AddressOf<NativeList<SystemTypeIndex>>(ref nativeList);
			ComponentSystemSorter.SortInternal(elementsptr, lookupDictionary, ptr);
			if (nativeList.Length > 0)
			{
				FixedString4096Bytes fixedString4096Bytes = "The following systems form a circular dependency cycle (check their [*Before]/[*After] attributes):\n";
				FixedString32Bytes fixedString32Bytes = "\n";
				for (int i = 0; i < nativeList.Length; i++)
				{
					FixedString512Bytes fixedString512Bytes = "- ";
					NativeText.ReadOnly systemName = TypeManager.GetSystemName(nativeList[i]);
					(ref fixedString512Bytes).Append<FixedString512Bytes, NativeText.ReadOnly>(in systemName);
					(ref fixedString4096Bytes).Append<FixedString4096Bytes, FixedString512Bytes>(in fixedString512Bytes);
					if (i < nativeList.Length - 1)
					{
						(ref fixedString4096Bytes).Append<FixedString4096Bytes, FixedString32Bytes>(in fixedString32Bytes);
					}
				}
				throw new InvalidOperationException(fixedString4096Bytes.ToString());
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009297 File Offset: 0x00007497
		[BurstCompile]
		internal unsafe static void SortInternal(UnsafeList<ComponentSystemSorter.SystemElement>* elementsptr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, NativeList<SystemTypeIndex>* badSystemTypeIndices)
		{
			ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Invoke(elementsptr, lookupDictionary, badSystemTypeIndices);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000092A4 File Offset: 0x000074A4
		private unsafe static void FindExactCycleInSystemGraph(SystemTypeIndex startingSystemTypeIndex, UnsafeList<ComponentSystemSorter.SystemElement> elements, NativeHashMap<SystemTypeIndex, int>* lookup, UnsafeList<SystemTypeIndex>* finalCycle)
		{
			int num = -1;
			for (int i = 0; i < elements.Length; i++)
			{
				if (elements[i].SystemTypeIndex == startingSystemTypeIndex)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException("Internal error starting type index was bad couldn't find it in list");
			}
			NativeList<int> nativeList = new NativeList<int>(16, Allocator.Temp);
			NativeHashSet<int> nativeHashSet = new NativeHashSet<int>(elements.Length, Allocator.Temp);
			SystemTypeIndex systemTypeIndex = startingSystemTypeIndex;
			int num2 = num;
			while (nativeHashSet.Count < elements.Length)
			{
				bool flag = false;
				int num3 = 0;
				int num5;
				int num6;
				for (;;)
				{
					int num4 = num3;
					ComponentSystemSorter.SystemElement systemElement = elements[num2];
					if (num4 >= systemElement.updateBefore.Length)
					{
						break;
					}
					systemElement = elements[num2];
					num5 = systemElement.updateBefore[num3];
					num6 = nativeList.IndexOf(num5);
					if (num6 != -1)
					{
						goto Block_3;
					}
					if (!nativeHashSet.Contains(num5))
					{
						goto Block_5;
					}
					num3++;
				}
				IL_014E:
				if (!flag)
				{
					nativeList.RemoveAt(nativeList.Length - 1);
					systemTypeIndex = nativeList[nativeList.Length - 1];
					num2 = ComponentSystemSorter.LookupSystemElement(systemTypeIndex, lookup);
					continue;
				}
				continue;
				Block_5:
				int num7 = systemTypeIndex;
				nativeList.Add(in num7);
				nativeHashSet.Add(systemTypeIndex);
				systemTypeIndex = num5;
				num2 = ComponentSystemSorter.LookupSystemElement(systemTypeIndex, lookup);
				flag = true;
				goto IL_014E;
				Block_3:
				num7 = systemTypeIndex;
				nativeList.Add(in num7);
				for (int j = num6; j < nativeList.Length; j++)
				{
					SystemTypeIndex systemTypeIndex2 = nativeList[j];
					finalCycle->Add(in systemTypeIndex2);
				}
				return;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009444 File Offset: 0x00007644
		private unsafe static void PopulateSystemElementLookup(NativeHashMap<SystemTypeIndex, int>* lookupDictionary, UnsafeList<ComponentSystemSorter.SystemElement> elements)
		{
			lookupDictionary->Capacity = elements.Length;
			for (int i = 0; i < elements.Length; i++)
			{
				(*lookupDictionary)[elements[i].SystemTypeIndex] = i;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000948C File Offset: 0x0000768C
		internal unsafe static void WarnAboutAnySystemAttributeBadness(int systemTypeIndex, ComponentSystemGroup group)
		{
			Type systemType = TypeManager.GetSystemType(systemTypeIndex);
			Attribute[] systemAttributes = TypeManager.GetSystemAttributes(systemType, typeof(UpdateInGroupAttribute));
			Type type = null;
			if (group != null)
			{
				type = group.GetType();
				foreach (UpdateInGroupAttribute updateInGroupAttribute in systemAttributes)
				{
					type = updateInGroupAttribute.GroupType;
					if (!TypeManager.IsSystemType(type))
					{
						Debug.LogWarning(string.Format("Ignoring invalid UpdateInGroup attribute on {0} targeting {1}, because {2} is not a system type.", systemType, type, type));
					}
					else if (!TypeManager.IsSystemAGroup(type))
					{
						Debug.LogWarning(string.Format("Ignoring invalid UpdateInGroup attribute on {0} targeting {1}, because {2} is not a ComponentSystemGroup.", systemType, type, type));
					}
					else if (type == systemType)
					{
						Debug.LogWarning(string.Format("Ignoring invalid UpdateInGroup attribute on {0} because a system group cannot be updated inside itself.\n", systemType));
					}
					else if (updateInGroupAttribute.OrderFirst && updateInGroupAttribute.OrderLast)
					{
						Debug.LogWarning(string.Format("Ignoring invalid OrderFirst & OrderLast directives on UpdateInGroup attribute on {0} because a system cannot be ordered both first and last in a group.", systemType));
					}
				}
			}
			foreach (Type type2 in new Type[]
			{
				typeof(UpdateAfterAttribute),
				typeof(UpdateBeforeAttribute),
				typeof(CreateAfterAttribute),
				typeof(CreateBeforeAttribute)
			})
			{
				Attribute[] systemAttributes2 = TypeManager.GetSystemAttributes(systemType, type2);
				PropertyInfo property = type2.GetProperty("SystemType");
				foreach (Attribute attribute in systemAttributes2)
				{
					Type type3 = (Type)property.GetValue(attribute);
					if (!TypeManager.IsSystemType(type3))
					{
						Debug.LogWarning(string.Format("Ignoring invalid [{0}] attribute on {1} targeting {2}, because {3} is not a subclass of ComponentSystemBase and does not implement ISystem", new object[] { type2, systemType, type3, type3 }));
					}
					else if (type3 == systemType)
					{
						Debug.LogWarning(string.Format("Ignoring invalid [{0}] attribute on {1} because a system cannot be updated or created after or before itself.\n", type2, systemType));
					}
					else if (group != null && type2.Name.Contains("Update"))
					{
						if (TypeManager.IsSystemManaged(type3))
						{
							bool flag = false;
							for (int k = 0; k < group.m_managedSystemsToUpdate.Count; k++)
							{
								if (group.m_managedSystemsToUpdate[k].GetType() == type3)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								Debug.LogWarning(string.Format("Ignoring invalid [{0}] attribute on {1} targeting {2}.\n", type2, systemType, type3) + "This attribute can only order systems that are members of the same ComponentSystemGroup instance.\n" + string.Format("Make sure that both systems are in the same system group with [UpdateInGroup(typeof({0}))],\n", type) + "or by manually adding both systems to the same group's update list.");
								goto IL_0309;
							}
						}
						else
						{
							bool flag2 = true;
							for (int l = 0; l < group.m_UnmanagedSystemsToUpdate.Length; l++)
							{
								if (TypeManager.GetSystemType(group.World.Unmanaged.ResolveSystemState(group.m_UnmanagedSystemsToUpdate[l])->m_SystemTypeIndex) == type3)
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								Debug.LogWarning(string.Format("Ignoring invalid [{0}] attribute on {1} targeting {2}.\n", type2, systemType, type3) + "This attribute can only order systems that are members of the same ComponentSystemGroup instance.\n" + string.Format("Make sure that both systems are in the same system group with [UpdateInGroup(typeof({0}))],\n", type) + "or by manually adding both systems to the same group's update list.");
								goto IL_0309;
							}
						}
						SystemTypeIndex systemTypeIndex2 = TypeManager.GetSystemTypeIndex(type);
						int num = ComponentSystemGroup.ComputeSystemOrdering(systemTypeIndex, systemTypeIndex2);
						int num2 = ComponentSystemGroup.ComputeSystemOrdering(TypeManager.GetSystemTypeIndex(type3), systemTypeIndex2);
						if (num != num2)
						{
							Debug.LogWarning(string.Format("Ignoring invalid [{0}({1})] attribute on {2} because OrderFirst/OrderLast has higher precedence.", type2, type3, systemType));
						}
					}
					IL_0309:;
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000097C4 File Offset: 0x000079C4
		[BurstCompile]
		internal unsafe static void FindConstraints(int parentTypeIndex, UnsafeList<ComponentSystemSorter.SystemElement>* sysElemsPtr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, TypeManager.SystemAttributeKind afterkind, TypeManager.SystemAttributeKind beforekind, NativeHashSet<SystemTypeIndex>* badSystemTypeIndices)
		{
			ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Invoke(parentTypeIndex, sysElemsPtr, lookupDictionary, afterkind, beforekind, badSystemTypeIndices);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000097D4 File Offset: 0x000079D4
		private static bool CheckBeforeConstraints(int parentTypeIndex, TypeManager.SystemAttribute dep, int systemTypeIndex, out bool warn)
		{
			warn = false;
			if (dep.TargetSystemTypeIndex == systemTypeIndex)
			{
				warn = true;
				return true;
			}
			int num = ComponentSystemGroup.ComputeSystemOrdering(systemTypeIndex, parentTypeIndex);
			int num2 = ComponentSystemGroup.ComputeSystemOrdering(dep.TargetSystemTypeIndex, parentTypeIndex);
			if (num2 > num)
			{
				return true;
			}
			if (num2 < num)
			{
				warn = true;
				return true;
			}
			return false;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009830 File Offset: 0x00007A30
		private static bool CheckAfterConstraints(int parentTypeIndex, TypeManager.SystemAttribute dep, int systemTypeIndex, out bool warn)
		{
			warn = false;
			if (dep.TargetSystemTypeIndex == systemTypeIndex)
			{
				warn = true;
				return true;
			}
			int num = ComponentSystemGroup.ComputeSystemOrdering(systemTypeIndex, parentTypeIndex);
			int num2 = ComponentSystemGroup.ComputeSystemOrdering(dep.TargetSystemTypeIndex, parentTypeIndex);
			if (num2 < num)
			{
				return true;
			}
			if (num2 > num)
			{
				warn = true;
				return true;
			}
			return false;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000988C File Offset: 0x00007A8C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SortInternal$BurstManaged(UnsafeList<ComponentSystemSorter.SystemElement>* elementsptr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, NativeList<SystemTypeIndex>* badSystemTypeIndices)
		{
			ref UnsafeList<ComponentSystemSorter.SystemElement> ptr = ref *elementsptr;
			lookupDictionary->Clear();
			UnsafeList<ComponentSystemSorter.SystemElement> unsafeList = new UnsafeList<ComponentSystemSorter.SystemElement>(ptr.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			unsafeList.Length = ptr.Length;
			UnsafeList<int> unsafeList2 = new UnsafeList<int>(ptr.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			unsafeList2.Length = ptr.Length;
			int num = 0;
			ComponentSystemSorter.Heap heap = new ComponentSystemSorter.Heap(ptr.Length);
			for (int i = 0; i < ptr.Length; i++)
			{
				if (ptr[i].nAfter == 0)
				{
					heap.Insert(new ComponentSystemSorter.TypeHeapElement(i, ptr[i].SystemTypeIndex));
				}
			}
			ComponentSystemSorter.PopulateSystemElementLookup(lookupDictionary, ptr);
			while (!heap.Empty)
			{
				int unsortedIndex = heap.Extract().unsortedIndex;
				ComponentSystemSorter.SystemElement systemElement = ptr[unsortedIndex];
				unsafeList[num++] = new ComponentSystemSorter.SystemElement
				{
					SystemTypeIndex = systemElement.SystemTypeIndex,
					Index = systemElement.Index,
					nAfter = systemElement.nAfter,
					updateBefore = systemElement.updateBefore,
					OrderingBucket = systemElement.OrderingBucket
				};
				foreach (int num2 in systemElement.updateBefore)
				{
					int num3 = ComponentSystemSorter.LookupSystemElement(num2, lookupDictionary);
					if (num3 < 0)
					{
						throw new Exception("Bug in SortSystemUpdateList(), beforeIndex < 0");
					}
					if (ptr[num3].nAfter <= 0)
					{
						throw new Exception("Bug in SortSystemUpdateList(), nAfter <= 0");
					}
					ptr.ElementAt(num3).nAfter--;
					if (ptr[num3].nAfter == 0)
					{
						heap.Insert(new ComponentSystemSorter.TypeHeapElement(num3, ptr[num3].SystemTypeIndex));
					}
				}
				ptr.ElementAt(unsortedIndex).nAfter = -1;
			}
			if (num < ptr.Length)
			{
				NativeHashSet<int> nativeHashSet = new NativeHashSet<int>(num, Allocator.Temp);
				for (int j = 0; j < num; j++)
				{
					nativeHashSet.Add(unsafeList[j].SystemTypeIndex);
				}
				for (int k = 0; k < ptr.Length; k++)
				{
					if (!nativeHashSet.Contains(ptr[k].SystemTypeIndex))
					{
						badSystemTypeIndices->Clear();
						ComponentSystemSorter.FindExactCycleInSystemGraph(ptr[k].SystemTypeIndex, ptr, lookupDictionary, badSystemTypeIndices->m_ListData);
						if (badSystemTypeIndices->Length > 0)
						{
							return;
						}
					}
				}
				throw new InvalidOperationException("Internal error: failed sorting systems but also couldn't find a cycle in the system graph. Please report this with Help->Report a bug...");
			}
			ptr.CopyFrom(in unsafeList);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009B48 File Offset: 0x00007D48
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void FindConstraints$BurstManaged(int parentTypeIndex, UnsafeList<ComponentSystemSorter.SystemElement>* sysElemsPtr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, TypeManager.SystemAttributeKind afterkind, TypeManager.SystemAttributeKind beforekind, NativeHashSet<SystemTypeIndex>* badSystemTypeIndices)
		{
			ref UnsafeList<ComponentSystemSorter.SystemElement> ptr = ref *sysElemsPtr;
			lookupDictionary->Clear();
			ComponentSystemSorter.PopulateSystemElementLookup(lookupDictionary, ptr);
			for (int i = 0; i < ptr.Length; i++)
			{
				int systemTypeIndex = ptr[i].SystemTypeIndex;
				NativeList<TypeManager.SystemAttribute> systemAttributes = TypeManager.GetSystemAttributes(systemTypeIndex, beforekind, Allocator.Temp);
				NativeList<TypeManager.SystemAttribute> systemAttributes2 = TypeManager.GetSystemAttributes(systemTypeIndex, afterkind, Allocator.Temp);
				for (int j = 0; j < systemAttributes.Length; j++)
				{
					TypeManager.SystemAttribute systemAttribute = systemAttributes[j];
					bool flag = false;
					if (ComponentSystemSorter.CheckBeforeConstraints(parentTypeIndex, systemAttribute, systemTypeIndex, out flag))
					{
						if (flag)
						{
							badSystemTypeIndices->Add(systemTypeIndex);
						}
					}
					else
					{
						int num = ComponentSystemSorter.LookupSystemElement(systemAttribute.TargetSystemTypeIndex, lookupDictionary);
						if (num < 0)
						{
							badSystemTypeIndices->Add(systemTypeIndex);
						}
						else
						{
							ref ComponentSystemSorter.SystemElement ptr2 = ref ptr.ElementAt(i);
							int num2 = systemAttribute.TargetSystemTypeIndex;
							ptr2.updateBefore.Add(in num2);
							ptr.ElementAt(num).nAfter++;
						}
					}
				}
				for (int k = 0; k < systemAttributes2.Length; k++)
				{
					TypeManager.SystemAttribute systemAttribute2 = systemAttributes2[k];
					bool flag2 = false;
					if (ComponentSystemSorter.CheckAfterConstraints(parentTypeIndex, systemAttribute2, systemTypeIndex, out flag2))
					{
						if (flag2)
						{
							badSystemTypeIndices->Add(systemTypeIndex);
						}
					}
					else
					{
						int num3 = ComponentSystemSorter.LookupSystemElement(systemAttribute2.TargetSystemTypeIndex, lookupDictionary);
						if (num3 < 0)
						{
							badSystemTypeIndices->Add(systemTypeIndex);
						}
						else
						{
							ptr.ElementAt(num3).updateBefore.Add(in systemTypeIndex);
							ptr.ElementAt(i).nAfter++;
						}
					}
				}
			}
		}

		// Token: 0x0200003D RID: 61
		private struct Heap
		{
			// Token: 0x060001D9 RID: 473 RVA: 0x00009CE0 File Offset: 0x00007EE0
			public Heap(int capacity)
			{
				this._capacity = capacity;
				this._size = 0;
				int num = capacity + ComponentSystemSorter.Heap.BaseIndex;
				this._elements = new UnsafeList<ComponentSystemSorter.TypeHeapElement>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				this._elements.Length = num;
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060001DA RID: 474 RVA: 0x00009D22 File Offset: 0x00007F22
			public bool Empty
			{
				get
				{
					return this._size <= 0;
				}
			}

			// Token: 0x060001DB RID: 475 RVA: 0x00009D30 File Offset: 0x00007F30
			public unsafe void Insert(ComponentSystemSorter.TypeHeapElement e)
			{
				int baseIndex = ComponentSystemSorter.Heap.BaseIndex;
				int size = this._size;
				this._size = size + 1;
				int i;
				int num;
				for (i = baseIndex + size; i > ComponentSystemSorter.Heap.BaseIndex; i = num)
				{
					num = i / 2;
					if (e.CompareTo(this._elements[num]) > 0)
					{
						break;
					}
					*this._elements.ElementAt(i) = this._elements[num];
				}
				*this._elements.ElementAt(i) = e;
			}

			// Token: 0x060001DC RID: 476 RVA: 0x00009DB8 File Offset: 0x00007FB8
			public ComponentSystemSorter.TypeHeapElement Peek()
			{
				return this._elements[ComponentSystemSorter.Heap.BaseIndex];
			}

			// Token: 0x060001DD RID: 477 RVA: 0x00009DD8 File Offset: 0x00007FD8
			public ComponentSystemSorter.TypeHeapElement Extract()
			{
				ComponentSystemSorter.TypeHeapElement typeHeapElement = this._elements[ComponentSystemSorter.Heap.BaseIndex];
				ref ComponentSystemSorter.TypeHeapElement ptr = ref this._elements.ElementAt(ComponentSystemSorter.Heap.BaseIndex);
				UnsafeList<ComponentSystemSorter.TypeHeapElement> elements = this._elements;
				int size = this._size;
				this._size = size - 1;
				ptr = elements[size];
				if (!this.Empty)
				{
					this.Heapify(ComponentSystemSorter.Heap.BaseIndex);
				}
				return typeHeapElement;
			}

			// Token: 0x060001DE RID: 478 RVA: 0x00009E44 File Offset: 0x00008044
			private unsafe void Heapify(int i)
			{
				ComponentSystemSorter.TypeHeapElement typeHeapElement = this._elements[i];
				while (i <= this._size / 2)
				{
					int num = 2 * i;
					if (num < this._size && this._elements[num + 1].CompareTo(this._elements[num]) < 0)
					{
						num++;
					}
					if (typeHeapElement.CompareTo(this._elements[num]) < 0)
					{
						break;
					}
					*this._elements.ElementAt(i) = this._elements[num];
					i = num;
				}
				*this._elements.ElementAt(i) = typeHeapElement;
			}

			// Token: 0x04000073 RID: 115
			private readonly UnsafeList<ComponentSystemSorter.TypeHeapElement> _elements;

			// Token: 0x04000074 RID: 116
			private int _size;

			// Token: 0x04000075 RID: 117
			private readonly int _capacity;

			// Token: 0x04000076 RID: 118
			private static readonly int BaseIndex = 1;
		}

		// Token: 0x0200003E RID: 62
		public struct TypeHeapElement : IComparable<ComponentSystemSorter.TypeHeapElement>
		{
			// Token: 0x060001E0 RID: 480 RVA: 0x00009F09 File Offset: 0x00008109
			public TypeHeapElement(int index, SystemTypeIndex _systemTypeIndex)
			{
				this.unsortedIndex = index;
				this.systemTypeIndex = _systemTypeIndex;
				this.systemTypeHash = TypeManager.GetSystemTypeHash(this.systemTypeIndex);
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x00009F2C File Offset: 0x0000812C
			public int CompareTo(ComponentSystemSorter.TypeHeapElement other)
			{
				int num = this.systemTypeHash.CompareTo(other.systemTypeHash);
				if (num == 0)
				{
					return this.unsortedIndex.CompareTo(other.unsortedIndex);
				}
				return num;
			}

			// Token: 0x04000077 RID: 119
			private readonly SystemTypeIndex systemTypeIndex;

			// Token: 0x04000078 RID: 120
			private readonly long systemTypeHash;

			// Token: 0x04000079 RID: 121
			public int unsortedIndex;
		}

		// Token: 0x0200003F RID: 63
		internal struct SystemElement
		{
			// Token: 0x0400007A RID: 122
			public int SystemTypeIndex;

			// Token: 0x0400007B RID: 123
			public UpdateIndex Index;

			// Token: 0x0400007C RID: 124
			public int OrderingBucket;

			// Token: 0x0400007D RID: 125
			public NativeList<int> updateBefore;

			// Token: 0x0400007E RID: 126
			public int nAfter;
		}

		// Token: 0x02000040 RID: 64
		// (Invoke) Token: 0x060001E3 RID: 483
		internal unsafe delegate void SortInternal_000001AE$PostfixBurstDelegate(UnsafeList<ComponentSystemSorter.SystemElement>* elementsptr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, NativeList<SystemTypeIndex>* badSystemTypeIndices);

		// Token: 0x02000041 RID: 65
		internal static class SortInternal_000001AE$BurstDirectCall
		{
			// Token: 0x060001E6 RID: 486 RVA: 0x00009F61 File Offset: 0x00008161
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Pointer == 0)
				{
					ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.DeferredCompilation, methodof(ComponentSystemSorter.SortInternal$BurstManaged(UnsafeList<ComponentSystemSorter.SystemElement>*, NativeHashMap<SystemTypeIndex, int>*, NativeList<SystemTypeIndex>*)).MethodHandle, typeof(ComponentSystemSorter.SortInternal_000001AE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Pointer;
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x00009F90 File Offset: 0x00008190
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060001E8 RID: 488 RVA: 0x00009FA8 File Offset: 0x000081A8
			public unsafe static void Constructor()
			{
				ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ComponentSystemSorter.SortInternal(UnsafeList<ComponentSystemSorter.SystemElement>*, NativeHashMap<SystemTypeIndex, int>*, NativeList<SystemTypeIndex>*)).MethodHandle);
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060001EA RID: 490 RVA: 0x00009FB9 File Offset: 0x000081B9
			// Note: this type is marked as 'beforefieldinit'.
			static SortInternal_000001AE$BurstDirectCall()
			{
				ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.Constructor();
			}

			// Token: 0x060001EB RID: 491 RVA: 0x00009FC0 File Offset: 0x000081C0
			public unsafe static void Invoke(UnsafeList<ComponentSystemSorter.SystemElement>* elementsptr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, NativeList<SystemTypeIndex>* badSystemTypeIndices)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ComponentSystemSorter.SortInternal_000001AE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ComponentSystemSorter/SystemElement>*,Unity.Collections.NativeHashMap`2<Unity.Entities.SystemTypeIndex,System.Int32>*,Unity.Collections.NativeList`1<Unity.Entities.SystemTypeIndex>*), elementsptr, lookupDictionary, badSystemTypeIndices, functionPointer);
						return;
					}
				}
				ComponentSystemSorter.SortInternal$BurstManaged(elementsptr, lookupDictionary, badSystemTypeIndices);
			}

			// Token: 0x0400007F RID: 127
			private static IntPtr Pointer;

			// Token: 0x04000080 RID: 128
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x060001ED RID: 493
		internal unsafe delegate void FindConstraints_000001B2$PostfixBurstDelegate(int parentTypeIndex, UnsafeList<ComponentSystemSorter.SystemElement>* sysElemsPtr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, TypeManager.SystemAttributeKind afterkind, TypeManager.SystemAttributeKind beforekind, NativeHashSet<SystemTypeIndex>* badSystemTypeIndices);

		// Token: 0x02000043 RID: 67
		internal static class FindConstraints_000001B2$BurstDirectCall
		{
			// Token: 0x060001F0 RID: 496 RVA: 0x00009FF5 File Offset: 0x000081F5
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Pointer == 0)
				{
					ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.DeferredCompilation, methodof(ComponentSystemSorter.FindConstraints$BurstManaged(int, UnsafeList<ComponentSystemSorter.SystemElement>*, NativeHashMap<SystemTypeIndex, int>*, TypeManager.SystemAttributeKind, TypeManager.SystemAttributeKind, NativeHashSet<SystemTypeIndex>*)).MethodHandle, typeof(ComponentSystemSorter.FindConstraints_000001B2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Pointer;
			}

			// Token: 0x060001F1 RID: 497 RVA: 0x0000A024 File Offset: 0x00008224
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x0000A03C File Offset: 0x0000823C
			public unsafe static void Constructor()
			{
				ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ComponentSystemSorter.FindConstraints(int, UnsafeList<ComponentSystemSorter.SystemElement>*, NativeHashMap<SystemTypeIndex, int>*, TypeManager.SystemAttributeKind, TypeManager.SystemAttributeKind, NativeHashSet<SystemTypeIndex>*)).MethodHandle);
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060001F4 RID: 500 RVA: 0x0000A04D File Offset: 0x0000824D
			// Note: this type is marked as 'beforefieldinit'.
			static FindConstraints_000001B2$BurstDirectCall()
			{
				ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.Constructor();
			}

			// Token: 0x060001F5 RID: 501 RVA: 0x0000A054 File Offset: 0x00008254
			public unsafe static void Invoke(int parentTypeIndex, UnsafeList<ComponentSystemSorter.SystemElement>* sysElemsPtr, NativeHashMap<SystemTypeIndex, int>* lookupDictionary, TypeManager.SystemAttributeKind afterkind, TypeManager.SystemAttributeKind beforekind, NativeHashSet<SystemTypeIndex>* badSystemTypeIndices)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ComponentSystemSorter.FindConstraints_000001B2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Int32,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Entities.ComponentSystemSorter/SystemElement>*,Unity.Collections.NativeHashMap`2<Unity.Entities.SystemTypeIndex,System.Int32>*,Unity.Entities.TypeManager/SystemAttributeKind,Unity.Entities.TypeManager/SystemAttributeKind,Unity.Collections.NativeHashSet`1<Unity.Entities.SystemTypeIndex>*), parentTypeIndex, sysElemsPtr, lookupDictionary, afterkind, beforekind, badSystemTypeIndices, functionPointer);
						return;
					}
				}
				ComponentSystemSorter.FindConstraints$BurstManaged(parentTypeIndex, sysElemsPtr, lookupDictionary, afterkind, beforekind, badSystemTypeIndices);
			}

			// Token: 0x04000081 RID: 129
			private static IntPtr Pointer;

			// Token: 0x04000082 RID: 130
			private static IntPtr DeferredCompilation;
		}
	}
}
