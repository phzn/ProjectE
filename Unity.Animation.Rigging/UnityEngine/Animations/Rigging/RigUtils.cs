using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000062 RID: 98
	internal static class RigUtils
	{
		// Token: 0x06000294 RID: 660 RVA: 0x00008634 File Offset: 0x00006834
		public static IRigConstraint[] GetConstraints(Rig rig)
		{
			IRigConstraint[] componentsInChildren = rig.GetComponentsInChildren<IRigConstraint>();
			if (componentsInChildren.Length == 0)
			{
				return null;
			}
			List<IRigConstraint> list = new List<IRigConstraint>(componentsInChildren.Length);
			foreach (IRigConstraint rigConstraint in componentsInChildren)
			{
				if (rigConstraint.IsValid())
				{
					list.Add(rigConstraint);
				}
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			return null;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008690 File Offset: 0x00006890
		private static Transform[] GetSyncableRigTransforms(Animator animator)
		{
			RigTransform[] componentsInChildren = animator.GetComponentsInChildren<RigTransform>();
			if (componentsInChildren.Length == 0)
			{
				return null;
			}
			Transform[] array = new Transform[componentsInChildren.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = componentsInChildren[i].transform;
			}
			return array;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000086D0 File Offset: 0x000068D0
		private static bool ExtractTransformType(Animator animator, FieldInfo field, object data, List<Transform> syncableTransforms)
		{
			bool flag = true;
			Type fieldType = field.FieldType;
			if (fieldType == typeof(Transform))
			{
				Transform transform = (Transform)field.GetValue(data);
				if (transform != null && transform.IsChildOf(animator.avatarRoot))
				{
					syncableTransforms.Add(transform);
				}
			}
			else
			{
				if (fieldType == typeof(Transform[]) || fieldType == typeof(List<Transform>))
				{
					using (IEnumerator<Transform> enumerator = ((IEnumerable<Transform>)field.GetValue(data)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Transform transform2 = enumerator.Current;
							if (transform2 != null && transform2.IsChildOf(animator.avatarRoot))
							{
								syncableTransforms.Add(transform2);
							}
						}
						return flag;
					}
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000087B4 File Offset: 0x000069B4
		private static bool ExtractPropertyType(FieldInfo field, object data, List<Property> syncableProperties, string namePrefix = "")
		{
			PropertyDescriptor propertyDescriptor;
			if (!RigUtils.s_SupportedPropertyTypeToDescriptor.TryGetValue(field.FieldType, out propertyDescriptor))
			{
				return false;
			}
			syncableProperties.Add(new Property
			{
				name = ConstraintsUtils.ConstructConstraintDataPropertyName(namePrefix + field.Name),
				descriptor = propertyDescriptor
			});
			return true;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00008808 File Offset: 0x00006A08
		private static bool ExtractWeightedTransforms(Animator animator, FieldInfo field, object data, List<Transform> syncableTransforms, List<Property> syncableProperties)
		{
			bool flag = true;
			Type fieldType = field.FieldType;
			if (fieldType == typeof(WeightedTransform))
			{
				Transform transform = ((WeightedTransform)field.GetValue(data)).transform;
				if (transform != null && transform.IsChildOf(animator.avatarRoot))
				{
					syncableTransforms.Add(transform);
				}
				syncableProperties.Add(new Property
				{
					name = ConstraintsUtils.ConstructConstraintDataPropertyName(field.Name + ".weight"),
					descriptor = RigUtils.s_SupportedPropertyTypeToDescriptor[typeof(float)]
				});
			}
			else
			{
				if (fieldType == typeof(WeightedTransformArray))
				{
					IEnumerable<WeightedTransform> enumerable = (IEnumerable<WeightedTransform>)field.GetValue(data);
					int num = 0;
					using (IEnumerator<WeightedTransform> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WeightedTransform weightedTransform = enumerator.Current;
							if (weightedTransform.transform != null && weightedTransform.transform.IsChildOf(animator.avatarRoot))
							{
								syncableTransforms.Add(weightedTransform.transform);
							}
							syncableProperties.Add(new Property
							{
								name = ConstraintsUtils.ConstructConstraintDataPropertyName(field.Name + ".m_Item" + num.ToString() + ".weight"),
								descriptor = RigUtils.s_SupportedPropertyTypeToDescriptor[typeof(float)]
							});
							num++;
						}
						return flag;
					}
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000899C File Offset: 0x00006B9C
		private static bool ExtractNestedPropertyType(Animator animator, FieldInfo field, object data, List<Transform> syncableTransforms, List<Property> syncableProperties, string namePrefix = "")
		{
			Type fieldType = field.FieldType;
			object value = field.GetValue(data);
			string text = namePrefix + field.Name + ".";
			if (!fieldType.IsValueType || fieldType.IsPrimitive)
			{
				return false;
			}
			foreach (FieldInfo fieldInfo in from info in fieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where info.GetCustomAttribute<SyncSceneToStreamAttribute>() != null
				select info)
			{
				if (!RigUtils.ExtractTransformType(animator, fieldInfo, value, syncableTransforms) && !RigUtils.ExtractPropertyType(fieldInfo, value, syncableProperties, text) && !RigUtils.ExtractNestedPropertyType(animator, fieldInfo, value, syncableTransforms, syncableProperties, text))
				{
					string text2 = "Field type [";
					Type fieldType2 = field.FieldType;
					throw new NotSupportedException(text2 + ((fieldType2 != null) ? fieldType2.ToString() : null) + "] is not a supported syncable property type.");
				}
			}
			return true;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00008A90 File Offset: 0x00006C90
		private static void ExtractAllSyncableData(Animator animator, IList<IRigLayer> layers, out List<Transform> syncableTransforms, out List<SyncableProperties> syncableProperties)
		{
			syncableTransforms = new List<Transform>();
			syncableProperties = new List<SyncableProperties>(layers.Count);
			Dictionary<Type, FieldInfo[]> dictionary = new Dictionary<Type, FieldInfo[]>();
			foreach (IRigLayer rigLayer in layers)
			{
				if (rigLayer.IsValid())
				{
					IRigConstraint[] constraints = rigLayer.constraints;
					List<ConstraintProperties> list = new List<ConstraintProperties>(constraints.Length);
					foreach (IRigConstraint rigConstraint in constraints)
					{
						IAnimationJobData data = rigConstraint.data;
						Type type = rigConstraint.data.GetType();
						FieldInfo[] array2;
						if (!dictionary.TryGetValue(type, out array2))
						{
							FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							List<FieldInfo> list2 = new List<FieldInfo>(fields.Length);
							foreach (FieldInfo fieldInfo in fields)
							{
								if (fieldInfo.GetCustomAttribute<SyncSceneToStreamAttribute>() != null)
								{
									list2.Add(fieldInfo);
								}
							}
							array2 = list2.ToArray();
							dictionary[type] = array2;
						}
						List<Property> list3 = new List<Property>(array2.Length);
						foreach (FieldInfo fieldInfo2 in array2)
						{
							if (!RigUtils.ExtractWeightedTransforms(animator, fieldInfo2, data, syncableTransforms, list3) && !RigUtils.ExtractTransformType(animator, fieldInfo2, data, syncableTransforms) && !RigUtils.ExtractPropertyType(fieldInfo2, data, list3, "") && !RigUtils.ExtractNestedPropertyType(animator, fieldInfo2, data, syncableTransforms, list3, ""))
							{
								string text = "Field type [";
								Type fieldType = fieldInfo2.FieldType;
								throw new NotSupportedException(text + ((fieldType != null) ? fieldType.ToString() : null) + "] is not a supported syncable property type.");
							}
						}
						list.Add(new ConstraintProperties
						{
							component = rigConstraint.component,
							properties = list3.ToArray()
						});
					}
					syncableProperties.Add(new SyncableProperties
					{
						rig = new RigProperties
						{
							component = rigLayer.rig
						},
						constraints = list.ToArray()
					});
				}
			}
			Transform[] syncableRigTransforms = RigUtils.GetSyncableRigTransforms(animator);
			if (syncableRigTransforms != null)
			{
				syncableTransforms.AddRange(syncableRigTransforms);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public static IAnimationJob[] CreateAnimationJobs(Animator animator, IRigConstraint[] constraints)
		{
			if (constraints == null || constraints.Length == 0)
			{
				return null;
			}
			IAnimationJob[] array = new IAnimationJob[constraints.Length];
			for (int i = 0; i < constraints.Length; i++)
			{
				array[i] = constraints[i].CreateJob(animator);
			}
			return array;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00008D04 File Offset: 0x00006F04
		public static void DestroyAnimationJobs(IRigConstraint[] constraints, IAnimationJob[] jobs)
		{
			if (jobs == null || jobs.Length != constraints.Length)
			{
				return;
			}
			for (int i = 0; i < constraints.Length; i++)
			{
				constraints[i].DestroyJob(jobs[i]);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008D38 File Offset: 0x00006F38
		internal static IAnimationJobData CreateSyncSceneToStreamData(Animator animator, IList<IRigLayer> layers)
		{
			List<Transform> list;
			List<SyncableProperties> list2;
			RigUtils.ExtractAllSyncableData(animator, layers, out list, out list2);
			return new RigUtils.RigSyncSceneToStreamData(list.ToArray(), list2.ToArray(), layers.Count);
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008D6C File Offset: 0x00006F6C
		public static IAnimationJobBinder syncSceneToStreamBinder { get; } = new RigSyncSceneToStreamJobBinder<RigUtils.RigSyncSceneToStreamData>();

		// Token: 0x04000110 RID: 272
		internal static readonly Dictionary<Type, PropertyDescriptor> s_SupportedPropertyTypeToDescriptor = new Dictionary<Type, PropertyDescriptor>
		{
			{
				typeof(float),
				new PropertyDescriptor
				{
					size = 1,
					type = PropertyType.Float
				}
			},
			{
				typeof(int),
				new PropertyDescriptor
				{
					size = 1,
					type = PropertyType.Int
				}
			},
			{
				typeof(bool),
				new PropertyDescriptor
				{
					size = 1,
					type = PropertyType.Bool
				}
			},
			{
				typeof(Vector2),
				new PropertyDescriptor
				{
					size = 2,
					type = PropertyType.Float
				}
			},
			{
				typeof(Vector3),
				new PropertyDescriptor
				{
					size = 3,
					type = PropertyType.Float
				}
			},
			{
				typeof(Vector4),
				new PropertyDescriptor
				{
					size = 4,
					type = PropertyType.Float
				}
			},
			{
				typeof(Quaternion),
				new PropertyDescriptor
				{
					size = 4,
					type = PropertyType.Float
				}
			},
			{
				typeof(Vector3Int),
				new PropertyDescriptor
				{
					size = 3,
					type = PropertyType.Int
				}
			},
			{
				typeof(Vector3Bool),
				new PropertyDescriptor
				{
					size = 3,
					type = PropertyType.Bool
				}
			}
		};

		// Token: 0x02000086 RID: 134
		private struct RigSyncSceneToStreamData : IAnimationJobData, IRigSyncSceneToStreamData
		{
			// Token: 0x0600031D RID: 797 RVA: 0x0000A668 File Offset: 0x00008868
			public RigSyncSceneToStreamData(Transform[] transforms, SyncableProperties[] properties, int rigCount)
			{
				if (transforms != null && transforms.Length != 0)
				{
					int[] array = RigUtils.RigSyncSceneToStreamData.UniqueTransformIndices(transforms);
					if (array.Length != transforms.Length)
					{
						this.syncableTransforms = new Transform[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							this.syncableTransforms[i] = transforms[array[i]];
						}
					}
					else
					{
						this.syncableTransforms = transforms;
					}
				}
				else
				{
					this.syncableTransforms = null;
				}
				this.syncableProperties = properties;
				this.rigStates = ((rigCount > 0) ? new bool[rigCount] : null);
				this.m_IsValid = (this.syncableTransforms != null && this.syncableTransforms.Length != 0) || (this.syncableProperties != null && this.syncableProperties.Length != 0) || this.rigStates != null;
			}

			// Token: 0x0600031E RID: 798 RVA: 0x0000A714 File Offset: 0x00008914
			private static int[] UniqueTransformIndices(Transform[] transforms)
			{
				if (transforms == null || transforms.Length == 0)
				{
					return null;
				}
				HashSet<int> hashSet = new HashSet<int>();
				List<int> list = new List<int>(transforms.Length);
				for (int i = 0; i < transforms.Length; i++)
				{
					if (hashSet.Add(transforms[i].GetInstanceID()))
					{
						list.Add(i);
					}
				}
				return list.ToArray();
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x0600031F RID: 799 RVA: 0x0000A764 File Offset: 0x00008964
			// (set) Token: 0x06000320 RID: 800 RVA: 0x0000A76C File Offset: 0x0000896C
			public Transform[] syncableTransforms { readonly get; private set; }

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x06000321 RID: 801 RVA: 0x0000A775 File Offset: 0x00008975
			// (set) Token: 0x06000322 RID: 802 RVA: 0x0000A77D File Offset: 0x0000897D
			public SyncableProperties[] syncableProperties { readonly get; private set; }

			// Token: 0x1700011F RID: 287
			// (get) Token: 0x06000323 RID: 803 RVA: 0x0000A786 File Offset: 0x00008986
			// (set) Token: 0x06000324 RID: 804 RVA: 0x0000A78E File Offset: 0x0000898E
			public bool[] rigStates { readonly get; set; }

			// Token: 0x06000325 RID: 805 RVA: 0x0000A797 File Offset: 0x00008997
			bool IAnimationJobData.IsValid()
			{
				return this.m_IsValid;
			}

			// Token: 0x06000326 RID: 806 RVA: 0x0000A79F File Offset: 0x0000899F
			void IAnimationJobData.SetDefaultValues()
			{
				this.syncableTransforms = null;
				this.syncableProperties = null;
				this.rigStates = null;
			}

			// Token: 0x04000174 RID: 372
			private readonly bool m_IsValid;
		}
	}
}
