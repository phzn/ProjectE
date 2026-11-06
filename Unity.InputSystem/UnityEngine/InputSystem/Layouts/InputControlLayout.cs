using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x0200010F RID: 271
	public class InputControlLayout
	{
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0004A6D4 File Offset: 0x000488D4
		public static InternedString DefaultVariant
		{
			get
			{
				return InputControlLayout.s_DefaultVariant;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0004A6DB File Offset: 0x000488DB
		public InternedString name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0004A6E3 File Offset: 0x000488E3
		public string displayName
		{
			get
			{
				return this.m_DisplayName ?? this.m_Name;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0004A6FA File Offset: 0x000488FA
		public Type type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0004A702 File Offset: 0x00048902
		public InternedString variants
		{
			get
			{
				return this.m_Variants;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0004A70A File Offset: 0x0004890A
		public FourCC stateFormat
		{
			get
			{
				return this.m_StateFormat;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0004A712 File Offset: 0x00048912
		public int stateSizeInBytes
		{
			get
			{
				return this.m_StateSizeInBytes;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0004A71A File Offset: 0x0004891A
		public IEnumerable<InternedString> baseLayouts
		{
			get
			{
				return this.m_BaseLayouts;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0004A727 File Offset: 0x00048927
		public IEnumerable<InternedString> appliedOverrides
		{
			get
			{
				return this.m_AppliedOverrides;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0004A734 File Offset: 0x00048934
		public ReadOnlyArray<InternedString> commonUsages
		{
			get
			{
				return new ReadOnlyArray<InternedString>(this.m_CommonUsages);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0004A741 File Offset: 0x00048941
		public ReadOnlyArray<InputControlLayout.ControlItem> controls
		{
			get
			{
				return new ReadOnlyArray<InputControlLayout.ControlItem>(this.m_Controls);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0004A74E File Offset: 0x0004894E
		public bool updateBeforeRender
		{
			get
			{
				return this.m_UpdateBeforeRender.GetValueOrDefault();
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0004A75B File Offset: 0x0004895B
		public bool isDeviceLayout
		{
			get
			{
				return typeof(InputDevice).IsAssignableFrom(this.m_Type);
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0004A772 File Offset: 0x00048972
		public bool isControlLayout
		{
			get
			{
				return !this.isDeviceLayout;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0004A77D File Offset: 0x0004897D
		// (set) Token: 0x06000F7F RID: 3967 RVA: 0x0004A78A File Offset: 0x0004898A
		public bool isOverride
		{
			get
			{
				return (this.m_Flags & InputControlLayout.Flags.IsOverride) > (InputControlLayout.Flags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_Flags |= InputControlLayout.Flags.IsOverride;
					return;
				}
				this.m_Flags &= ~InputControlLayout.Flags.IsOverride;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0004A7AD File Offset: 0x000489AD
		// (set) Token: 0x06000F81 RID: 3969 RVA: 0x0004A7BA File Offset: 0x000489BA
		public bool isGenericTypeOfDevice
		{
			get
			{
				return (this.m_Flags & InputControlLayout.Flags.IsGenericTypeOfDevice) > (InputControlLayout.Flags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_Flags |= InputControlLayout.Flags.IsGenericTypeOfDevice;
					return;
				}
				this.m_Flags &= ~InputControlLayout.Flags.IsGenericTypeOfDevice;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0004A7DD File Offset: 0x000489DD
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x0004A7EA File Offset: 0x000489EA
		public bool hideInUI
		{
			get
			{
				return (this.m_Flags & InputControlLayout.Flags.HideInUI) > (InputControlLayout.Flags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_Flags |= InputControlLayout.Flags.HideInUI;
					return;
				}
				this.m_Flags &= ~InputControlLayout.Flags.HideInUI;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0004A80D File Offset: 0x00048A0D
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x0004A81B File Offset: 0x00048A1B
		public bool isNoisy
		{
			get
			{
				return (this.m_Flags & InputControlLayout.Flags.IsNoisy) > (InputControlLayout.Flags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_Flags |= InputControlLayout.Flags.IsNoisy;
					return;
				}
				this.m_Flags &= ~InputControlLayout.Flags.IsNoisy;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0004A840 File Offset: 0x00048A40
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0004A874 File Offset: 0x00048A74
		public bool? canRunInBackground
		{
			get
			{
				if ((this.m_Flags & InputControlLayout.Flags.CanRunInBackgroundIsSet) == (InputControlLayout.Flags)0)
				{
					return null;
				}
				return new bool?((this.m_Flags & InputControlLayout.Flags.CanRunInBackground) > (InputControlLayout.Flags)0);
			}
			internal set
			{
				if (value == null)
				{
					this.m_Flags &= ~InputControlLayout.Flags.CanRunInBackgroundIsSet;
					return;
				}
				this.m_Flags |= InputControlLayout.Flags.CanRunInBackgroundIsSet;
				if (value.Value)
				{
					this.m_Flags |= InputControlLayout.Flags.CanRunInBackground;
					return;
				}
				this.m_Flags &= ~InputControlLayout.Flags.CanRunInBackground;
			}
		}

		// Token: 0x17000481 RID: 1153
		public InputControlLayout.ControlItem this[string path]
		{
			get
			{
				if (string.IsNullOrEmpty(path))
				{
					throw new ArgumentNullException("path");
				}
				if (this.m_Controls != null)
				{
					for (int i = 0; i < this.m_Controls.Length; i++)
					{
						if (this.m_Controls[i].name == path)
						{
							return this.m_Controls[i];
						}
					}
				}
				throw new KeyNotFoundException(string.Format("Cannot find control '{0}' in layout '{1}'", path, this.name));
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0004A94C File Offset: 0x00048B4C
		public InputControlLayout.ControlItem? FindControl(InternedString path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			if (this.m_Controls == null)
			{
				return null;
			}
			for (int i = 0; i < this.m_Controls.Length; i++)
			{
				if (this.m_Controls[i].name == path)
				{
					return new InputControlLayout.ControlItem?(this.m_Controls[i]);
				}
			}
			return null;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0004A9CC File Offset: 0x00048BCC
		public InputControlLayout.ControlItem? FindControlIncludingArrayElements(string path, out int arrayIndex)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			arrayIndex = -1;
			if (this.m_Controls == null)
			{
				return null;
			}
			int num = 0;
			int num2 = path.Length;
			while (num2 > 0 && char.IsDigit(path[num2 - 1]))
			{
				num2--;
				num *= 10;
				num += (int)(path[num2] - '0');
			}
			int num3 = 0;
			if (num2 < path.Length && num2 > 0)
			{
				num3 = num2;
			}
			for (int i = 0; i < this.m_Controls.Length; i++)
			{
				ref InputControlLayout.ControlItem ptr = ref this.m_Controls[i];
				if (string.Compare(ptr.name, path, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return new InputControlLayout.ControlItem?(ptr);
				}
				if (ptr.isArray && num3 > 0 && num3 == ptr.name.length && string.Compare(ptr.name.ToString(), 0, path, 0, num3, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					arrayIndex = num;
					return new InputControlLayout.ControlItem?(ptr);
				}
			}
			return null;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0004AAF0 File Offset: 0x00048CF0
		public Type GetValueType()
		{
			return TypeHelpers.GetGenericTypeArgumentFromHierarchy(this.type, typeof(InputControl<>), 0);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0004AB08 File Offset: 0x00048D08
		public static InputControlLayout FromType(string name, Type type)
		{
			List<InputControlLayout.ControlItem> list = new List<InputControlLayout.ControlItem>();
			InputControlLayoutAttribute customAttribute = type.GetCustomAttribute<InputControlLayoutAttribute>(true);
			FourCC fourCC = default(FourCC);
			if (customAttribute != null && customAttribute.stateType != null)
			{
				InputControlLayout.AddControlItems(customAttribute.stateType, list, name);
				if (typeof(IInputStateTypeInfo).IsAssignableFrom(customAttribute.stateType))
				{
					fourCC = ((IInputStateTypeInfo)Activator.CreateInstance(customAttribute.stateType)).format;
				}
			}
			else
			{
				InputControlLayout.AddControlItems(type, list, name);
			}
			if (customAttribute != null && !string.IsNullOrEmpty(customAttribute.stateFormat))
			{
				fourCC = new FourCC(customAttribute.stateFormat);
			}
			InternedString internedString = default(InternedString);
			if (customAttribute != null)
			{
				internedString = new InternedString(customAttribute.variants);
			}
			InputControlLayout inputControlLayout = new InputControlLayout(name, type)
			{
				m_Controls = list.ToArray(),
				m_StateFormat = fourCC,
				m_Variants = internedString,
				m_UpdateBeforeRender = ((customAttribute != null) ? customAttribute.updateBeforeRenderInternal : null),
				isGenericTypeOfDevice = (customAttribute != null && customAttribute.isGenericTypeOfDevice),
				hideInUI = (customAttribute != null && customAttribute.hideInUI),
				m_Description = ((customAttribute != null) ? customAttribute.description : null),
				m_DisplayName = ((customAttribute != null) ? customAttribute.displayName : null),
				canRunInBackground = ((customAttribute != null) ? customAttribute.canRunInBackgroundInternal : null),
				isNoisy = (customAttribute != null && customAttribute.isNoisy)
			};
			if (((customAttribute != null) ? customAttribute.commonUsages : null) != null)
			{
				inputControlLayout.m_CommonUsages = ArrayHelpers.Select<string, InternedString>(customAttribute.commonUsages, (string x) => new InternedString(x));
			}
			return inputControlLayout;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0004ACA9 File Offset: 0x00048EA9
		public string ToJson()
		{
			return JsonUtility.ToJson(InputControlLayout.LayoutJson.FromLayout(this), true);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0004ACBC File Offset: 0x00048EBC
		public static InputControlLayout FromJson(string json)
		{
			return JsonUtility.FromJson<InputControlLayout.LayoutJson>(json).ToLayout();
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0004ACD7 File Offset: 0x00048ED7
		private InputControlLayout(string name, Type type)
		{
			this.m_Name = new InternedString(name);
			this.m_Type = type;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0004ACF2 File Offset: 0x00048EF2
		private static void AddControlItems(Type type, List<InputControlLayout.ControlItem> controlLayouts, string layoutName)
		{
			InputControlLayout.AddControlItemsFromFields(type, controlLayouts, layoutName);
			InputControlLayout.AddControlItemsFromProperties(type, controlLayouts, layoutName);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0004AD04 File Offset: 0x00048F04
		private static void AddControlItemsFromFields(Type type, List<InputControlLayout.ControlItem> controlLayouts, string layoutName)
		{
			MemberInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			InputControlLayout.AddControlItemsFromMembers(fields, controlLayouts, layoutName);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0004AD24 File Offset: 0x00048F24
		private static void AddControlItemsFromProperties(Type type, List<InputControlLayout.ControlItem> controlLayouts, string layoutName)
		{
			MemberInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			InputControlLayout.AddControlItemsFromMembers(properties, controlLayouts, layoutName);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0004AD44 File Offset: 0x00048F44
		private static void AddControlItemsFromMembers(MemberInfo[] members, List<InputControlLayout.ControlItem> controlItems, string layoutName)
		{
			foreach (MemberInfo memberInfo in members)
			{
				if (!(memberInfo.DeclaringType == typeof(InputControl)))
				{
					Type valueType = TypeHelpers.GetValueType(memberInfo);
					if (valueType != null && valueType.IsValueType && typeof(IInputStateTypeInfo).IsAssignableFrom(valueType))
					{
						int count = controlItems.Count;
						InputControlLayout.AddControlItems(valueType, controlItems, layoutName);
						if (memberInfo as FieldInfo != null)
						{
							int num = Marshal.OffsetOf(memberInfo.DeclaringType, memberInfo.Name).ToInt32();
							int count2 = controlItems.Count;
							for (int j = count; j < count2; j++)
							{
								InputControlLayout.ControlItem controlItem = controlItems[j];
								if (controlItems[j].offset != 4294967295U)
								{
									controlItem.offset += (uint)num;
									controlItems[j] = controlItem;
								}
							}
						}
					}
					InputControlAttribute[] array = memberInfo.GetCustomAttributes<InputControlAttribute>(false).ToArray<InputControlAttribute>();
					if (array.Length != 0 || (!(valueType == null) && typeof(InputControl).IsAssignableFrom(valueType) && !(memberInfo is PropertyInfo)))
					{
						InputControlLayout.AddControlItemsFromMember(memberInfo, array, controlItems);
					}
				}
			}
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0004AE84 File Offset: 0x00049084
		private static void AddControlItemsFromMember(MemberInfo member, InputControlAttribute[] attributes, List<InputControlLayout.ControlItem> controlItems)
		{
			if (attributes.Length == 0)
			{
				InputControlLayout.ControlItem controlItem = InputControlLayout.CreateControlItemFromMember(member, null);
				controlItems.Add(controlItem);
				return;
			}
			foreach (InputControlAttribute inputControlAttribute in attributes)
			{
				InputControlLayout.ControlItem controlItem2 = InputControlLayout.CreateControlItemFromMember(member, inputControlAttribute);
				controlItems.Add(controlItem2);
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0004AECC File Offset: 0x000490CC
		private static InputControlLayout.ControlItem CreateControlItemFromMember(MemberInfo member, InputControlAttribute attribute)
		{
			string text = ((attribute != null) ? attribute.name : null);
			if (string.IsNullOrEmpty(text))
			{
				text = member.Name;
			}
			bool flag = text.IndexOf('/') != -1;
			string text2 = ((attribute != null) ? attribute.displayName : null);
			string text3 = ((attribute != null) ? attribute.shortDisplayName : null);
			string text4 = ((attribute != null) ? attribute.layout : null);
			if (string.IsNullOrEmpty(text4) && !flag && (!(member is FieldInfo) || member.GetCustomAttribute<FixedBufferAttribute>(false) == null))
			{
				text4 = InputControlLayout.InferLayoutFromValueType(TypeHelpers.GetValueType(member));
			}
			string text5 = null;
			if (attribute != null && !string.IsNullOrEmpty(attribute.variants))
			{
				text5 = attribute.variants;
			}
			uint num = uint.MaxValue;
			if (attribute != null && attribute.offset != 4294967295U)
			{
				num = attribute.offset;
			}
			else if (member is FieldInfo && !flag)
			{
				num = (uint)Marshal.OffsetOf(member.DeclaringType, member.Name).ToInt32();
			}
			uint num2 = uint.MaxValue;
			if (attribute != null)
			{
				num2 = attribute.bit;
			}
			uint num3 = 0U;
			if (attribute != null)
			{
				num3 = attribute.sizeInBits;
			}
			FourCC fourCC = default(FourCC);
			if (attribute != null && !string.IsNullOrEmpty(attribute.format))
			{
				fourCC = new FourCC(attribute.format);
			}
			else if (!flag && num2 == 4294967295U)
			{
				fourCC = InputStateBlock.GetPrimitiveFormatFromType(TypeHelpers.GetValueType(member));
			}
			InternedString[] array = null;
			if (attribute != null)
			{
				string[] array2 = ArrayHelpers.Join<string>(attribute.alias, attribute.aliases);
				if (array2 != null)
				{
					array = array2.Select<string, InternedString>((string x) => new InternedString(x)).ToArray<InternedString>();
				}
			}
			InternedString[] array3 = null;
			if (attribute != null)
			{
				string[] array4 = ArrayHelpers.Join<string>(attribute.usage, attribute.usages);
				if (array4 != null)
				{
					array3 = array4.Select<string, InternedString>((string x) => new InternedString(x)).ToArray<InternedString>();
				}
			}
			NamedValue[] array5 = null;
			if (attribute != null && !string.IsNullOrEmpty(attribute.parameters))
			{
				array5 = NamedValue.ParseMultiple(attribute.parameters);
			}
			NameAndParameters[] array6 = null;
			if (attribute != null && !string.IsNullOrEmpty(attribute.processors))
			{
				array6 = NameAndParameters.ParseMultiple(attribute.processors).ToArray<NameAndParameters>();
			}
			string text6 = null;
			if (attribute != null && !string.IsNullOrEmpty(attribute.useStateFrom))
			{
				text6 = attribute.useStateFrom;
			}
			bool flag2 = false;
			if (attribute != null)
			{
				flag2 = attribute.noisy;
			}
			bool flag3 = false;
			if (attribute != null)
			{
				flag3 = attribute.dontReset;
			}
			bool flag4 = false;
			if (attribute != null)
			{
				flag4 = attribute.synthetic;
			}
			int num4 = 0;
			if (attribute != null)
			{
				num4 = attribute.arraySize;
			}
			PrimitiveValue primitiveValue = default(PrimitiveValue);
			if (attribute != null)
			{
				primitiveValue = PrimitiveValue.FromObject(attribute.defaultState);
			}
			PrimitiveValue primitiveValue2 = default(PrimitiveValue);
			PrimitiveValue primitiveValue3 = default(PrimitiveValue);
			if (attribute != null)
			{
				primitiveValue2 = PrimitiveValue.FromObject(attribute.minValue);
				primitiveValue3 = PrimitiveValue.FromObject(attribute.maxValue);
			}
			return new InputControlLayout.ControlItem
			{
				name = new InternedString(text),
				displayName = text2,
				shortDisplayName = text3,
				layout = new InternedString(text4),
				variants = new InternedString(text5),
				useStateFrom = text6,
				format = fourCC,
				offset = num,
				bit = num2,
				sizeInBits = num3,
				parameters = new ReadOnlyArray<NamedValue>(array5),
				processors = new ReadOnlyArray<NameAndParameters>(array6),
				usages = new ReadOnlyArray<InternedString>(array3),
				aliases = new ReadOnlyArray<InternedString>(array),
				isModifyingExistingControl = flag,
				isFirstDefinedInThisLayout = true,
				isNoisy = flag2,
				dontReset = flag3,
				isSynthetic = flag4,
				arraySize = num4,
				defaultState = primitiveValue,
				minValue = primitiveValue2,
				maxValue = primitiveValue3
			};
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0004B274 File Offset: 0x00049474
		private static string InferLayoutFromValueType(Type type)
		{
			InternedString internedString = InputControlLayout.s_Layouts.TryFindLayoutForType(type);
			if (internedString.IsEmpty())
			{
				InternedString internedString2 = new InternedString(type.Name);
				if (InputControlLayout.s_Layouts.HasLayout(internedString2))
				{
					internedString = internedString2;
				}
				else if (type.Name.EndsWith("Control"))
				{
					internedString2 = new InternedString(type.Name.Substring(0, type.Name.Length - "Control".Length));
					if (InputControlLayout.s_Layouts.HasLayout(internedString2))
					{
						internedString = internedString2;
					}
				}
			}
			return internedString;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004B304 File Offset: 0x00049504
		public void MergeLayout(InputControlLayout other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			bool? updateBeforeRender = this.m_UpdateBeforeRender;
			this.m_UpdateBeforeRender = ((updateBeforeRender != null) ? updateBeforeRender : other.m_UpdateBeforeRender);
			if (this.m_Variants.IsEmpty())
			{
				this.m_Variants = other.m_Variants;
			}
			if (this.m_Type == null)
			{
				this.m_Type = other.m_Type;
			}
			else if (this.m_Type.IsAssignableFrom(other.m_Type))
			{
				this.m_Type = other.m_Type;
			}
			bool flag = !this.m_Variants.IsEmpty();
			if (this.m_StateFormat == default(FourCC))
			{
				this.m_StateFormat = other.m_StateFormat;
			}
			this.m_CommonUsages = ArrayHelpers.Merge<InternedString>(other.m_CommonUsages, this.m_CommonUsages);
			this.m_AppliedOverrides.Merge(other.m_AppliedOverrides);
			if (string.IsNullOrEmpty(this.m_DisplayName))
			{
				this.m_DisplayName = other.m_DisplayName;
			}
			if (this.m_Controls == null)
			{
				this.m_Controls = other.m_Controls;
				return;
			}
			if (other.m_Controls != null)
			{
				InputControlLayout.ControlItem[] controls = other.m_Controls;
				List<InputControlLayout.ControlItem> list = new List<InputControlLayout.ControlItem>();
				List<string> list2 = new List<string>();
				Dictionary<string, InputControlLayout.ControlItem> dictionary = InputControlLayout.CreateLookupTableForControls(controls, list2);
				foreach (KeyValuePair<string, InputControlLayout.ControlItem> keyValuePair in InputControlLayout.CreateLookupTableForControls(this.m_Controls, null))
				{
					InputControlLayout.ControlItem controlItem;
					if (dictionary.TryGetValue(keyValuePair.Key, out controlItem))
					{
						InputControlLayout.ControlItem controlItem2 = keyValuePair.Value.Merge(controlItem);
						list.Add(controlItem2);
						dictionary.Remove(keyValuePair.Key);
					}
					else if (keyValuePair.Value.variants.IsEmpty() || keyValuePair.Value.variants == InputControlLayout.DefaultVariant)
					{
						bool flag2 = false;
						if (flag)
						{
							for (int i = 0; i < list2.Count; i++)
							{
								if (InputControlLayout.VariantsMatch(this.m_Variants.ToLower(), list2[i]))
								{
									string text = keyValuePair.Key + "@" + list2[i];
									if (dictionary.TryGetValue(text, out controlItem))
									{
										InputControlLayout.ControlItem controlItem3 = keyValuePair.Value.Merge(controlItem);
										list.Add(controlItem3);
										dictionary.Remove(text);
										flag2 = true;
									}
								}
							}
						}
						else
						{
							foreach (string text2 in list2)
							{
								string text3 = keyValuePair.Key + "@" + text2;
								if (dictionary.TryGetValue(text3, out controlItem))
								{
									InputControlLayout.ControlItem controlItem4 = keyValuePair.Value.Merge(controlItem);
									list.Add(controlItem4);
									dictionary.Remove(text3);
									flag2 = true;
								}
							}
						}
						if (!flag2)
						{
							list.Add(keyValuePair.Value);
						}
					}
					else if (dictionary.TryGetValue(keyValuePair.Value.name.ToLower(), out controlItem))
					{
						InputControlLayout.ControlItem controlItem5 = keyValuePair.Value.Merge(controlItem);
						list.Add(controlItem5);
						dictionary.Remove(keyValuePair.Value.name.ToLower());
					}
					else if (InputControlLayout.VariantsMatch(this.m_Variants, keyValuePair.Value.variants))
					{
						list.Add(keyValuePair.Value);
					}
				}
				if (!flag)
				{
					int count = list.Count;
					list.AddRange(dictionary.Values);
					for (int j = count; j < list.Count; j++)
					{
						InputControlLayout.ControlItem controlItem6 = list[j];
						controlItem6.isFirstDefinedInThisLayout = false;
						list[j] = controlItem6;
					}
				}
				else
				{
					int count2 = list.Count;
					list.AddRange(dictionary.Values.Where<InputControlLayout.ControlItem>((InputControlLayout.ControlItem x) => InputControlLayout.VariantsMatch(this.m_Variants, x.variants)));
					for (int k = count2; k < list.Count; k++)
					{
						InputControlLayout.ControlItem controlItem7 = list[k];
						controlItem7.isFirstDefinedInThisLayout = false;
						list[k] = controlItem7;
					}
				}
				this.m_Controls = list.ToArray();
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0004B784 File Offset: 0x00049984
		private static Dictionary<string, InputControlLayout.ControlItem> CreateLookupTableForControls(InputControlLayout.ControlItem[] controlItems, List<string> variants = null)
		{
			Dictionary<string, InputControlLayout.ControlItem> dictionary = new Dictionary<string, InputControlLayout.ControlItem>();
			int i = 0;
			while (i < controlItems.Length)
			{
				string text = controlItems[i].name.ToLower();
				InternedString variants2 = controlItems[i].variants;
				if (variants2.IsEmpty() || !(variants2 != InputControlLayout.DefaultVariant))
				{
					goto IL_00EC;
				}
				if (variants2.ToString().IndexOf(";"[0]) != -1)
				{
					foreach (string text2 in variants2.ToLower().Split(";"[0], StringSplitOptions.None))
					{
						if (variants != null)
						{
							variants.Add(text2);
						}
						text = text + "@" + text2;
						dictionary[text] = controlItems[i];
					}
				}
				else
				{
					text = text + "@" + variants2.ToLower();
					if (variants != null)
					{
						variants.Add(variants2.ToLower());
						goto IL_00EC;
					}
					goto IL_00EC;
				}
				IL_00FA:
				i++;
				continue;
				IL_00EC:
				dictionary[text] = controlItems[i];
				goto IL_00FA;
			}
			return dictionary;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0004B899 File Offset: 0x00049A99
		internal static bool VariantsMatch(InternedString expected, InternedString actual)
		{
			return InputControlLayout.VariantsMatch(expected.ToLower(), actual.ToLower());
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0004B8AE File Offset: 0x00049AAE
		internal static bool VariantsMatch(string expected, string actual)
		{
			return (actual != null && StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(InputControlLayout.DefaultVariant, actual, ";"[0])) || expected == null || actual == null || StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(expected, actual, ";"[0]);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0004B8F0 File Offset: 0x00049AF0
		internal static void ParseHeaderFieldsFromJson(string json, out InternedString name, out InlinedArray<InternedString> baseLayouts, out InputDeviceMatcher deviceMatcher)
		{
			InputControlLayout.LayoutJsonNameAndDescriptorOnly layoutJsonNameAndDescriptorOnly = JsonUtility.FromJson<InputControlLayout.LayoutJsonNameAndDescriptorOnly>(json);
			name = new InternedString(layoutJsonNameAndDescriptorOnly.name);
			baseLayouts = default(InlinedArray<InternedString>);
			if (!string.IsNullOrEmpty(layoutJsonNameAndDescriptorOnly.extend))
			{
				baseLayouts.Append(new InternedString(layoutJsonNameAndDescriptorOnly.extend));
			}
			if (layoutJsonNameAndDescriptorOnly.extendMultiple != null)
			{
				foreach (string text in layoutJsonNameAndDescriptorOnly.extendMultiple)
				{
					baseLayouts.Append(new InternedString(text));
				}
			}
			deviceMatcher = layoutJsonNameAndDescriptorOnly.device.ToMatcher();
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0004B97B File Offset: 0x00049B7B
		internal static ref InputControlLayout.Cache cache
		{
			get
			{
				return ref InputControlLayout.s_CacheInstance;
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0004B984 File Offset: 0x00049B84
		internal static InputControlLayout.CacheRefInstance CacheRef()
		{
			InputControlLayout.s_CacheInstanceRef++;
			return new InputControlLayout.CacheRefInstance
			{
				valid = true
			};
		}

		// Token: 0x04000673 RID: 1651
		private static InternedString s_DefaultVariant = new InternedString("Default");

		// Token: 0x04000674 RID: 1652
		public const string VariantSeparator = ";";

		// Token: 0x04000675 RID: 1653
		private InternedString m_Name;

		// Token: 0x04000676 RID: 1654
		private Type m_Type;

		// Token: 0x04000677 RID: 1655
		private InternedString m_Variants;

		// Token: 0x04000678 RID: 1656
		private FourCC m_StateFormat;

		// Token: 0x04000679 RID: 1657
		internal int m_StateSizeInBytes;

		// Token: 0x0400067A RID: 1658
		internal bool? m_UpdateBeforeRender;

		// Token: 0x0400067B RID: 1659
		internal InlinedArray<InternedString> m_BaseLayouts;

		// Token: 0x0400067C RID: 1660
		private InlinedArray<InternedString> m_AppliedOverrides;

		// Token: 0x0400067D RID: 1661
		private InternedString[] m_CommonUsages;

		// Token: 0x0400067E RID: 1662
		internal InputControlLayout.ControlItem[] m_Controls;

		// Token: 0x0400067F RID: 1663
		internal string m_DisplayName;

		// Token: 0x04000680 RID: 1664
		private string m_Description;

		// Token: 0x04000681 RID: 1665
		private InputControlLayout.Flags m_Flags;

		// Token: 0x04000682 RID: 1666
		internal static InputControlLayout.Collection s_Layouts;

		// Token: 0x04000683 RID: 1667
		internal static InputControlLayout.Cache s_CacheInstance;

		// Token: 0x04000684 RID: 1668
		internal static int s_CacheInstanceRef;

		// Token: 0x0200022B RID: 555
		public struct ControlItem
		{
			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x06001583 RID: 5507 RVA: 0x0006393B File Offset: 0x00061B3B
			// (set) Token: 0x06001584 RID: 5508 RVA: 0x00063943 File Offset: 0x00061B43
			public InternedString name { readonly get; internal set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x06001585 RID: 5509 RVA: 0x0006394C File Offset: 0x00061B4C
			// (set) Token: 0x06001586 RID: 5510 RVA: 0x00063954 File Offset: 0x00061B54
			public InternedString layout { readonly get; internal set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x06001587 RID: 5511 RVA: 0x0006395D File Offset: 0x00061B5D
			// (set) Token: 0x06001588 RID: 5512 RVA: 0x00063965 File Offset: 0x00061B65
			public InternedString variants { readonly get; internal set; }

			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x06001589 RID: 5513 RVA: 0x0006396E File Offset: 0x00061B6E
			// (set) Token: 0x0600158A RID: 5514 RVA: 0x00063976 File Offset: 0x00061B76
			public string useStateFrom { readonly get; internal set; }

			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x0600158B RID: 5515 RVA: 0x0006397F File Offset: 0x00061B7F
			// (set) Token: 0x0600158C RID: 5516 RVA: 0x00063987 File Offset: 0x00061B87
			public string displayName { readonly get; internal set; }

			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x0600158D RID: 5517 RVA: 0x00063990 File Offset: 0x00061B90
			// (set) Token: 0x0600158E RID: 5518 RVA: 0x00063998 File Offset: 0x00061B98
			public string shortDisplayName { readonly get; internal set; }

			// Token: 0x170005D4 RID: 1492
			// (get) Token: 0x0600158F RID: 5519 RVA: 0x000639A1 File Offset: 0x00061BA1
			// (set) Token: 0x06001590 RID: 5520 RVA: 0x000639A9 File Offset: 0x00061BA9
			public ReadOnlyArray<InternedString> usages { readonly get; internal set; }

			// Token: 0x170005D5 RID: 1493
			// (get) Token: 0x06001591 RID: 5521 RVA: 0x000639B2 File Offset: 0x00061BB2
			// (set) Token: 0x06001592 RID: 5522 RVA: 0x000639BA File Offset: 0x00061BBA
			public ReadOnlyArray<InternedString> aliases { readonly get; internal set; }

			// Token: 0x170005D6 RID: 1494
			// (get) Token: 0x06001593 RID: 5523 RVA: 0x000639C3 File Offset: 0x00061BC3
			// (set) Token: 0x06001594 RID: 5524 RVA: 0x000639CB File Offset: 0x00061BCB
			public ReadOnlyArray<NamedValue> parameters { readonly get; internal set; }

			// Token: 0x170005D7 RID: 1495
			// (get) Token: 0x06001595 RID: 5525 RVA: 0x000639D4 File Offset: 0x00061BD4
			// (set) Token: 0x06001596 RID: 5526 RVA: 0x000639DC File Offset: 0x00061BDC
			public ReadOnlyArray<NameAndParameters> processors { readonly get; internal set; }

			// Token: 0x170005D8 RID: 1496
			// (get) Token: 0x06001597 RID: 5527 RVA: 0x000639E5 File Offset: 0x00061BE5
			// (set) Token: 0x06001598 RID: 5528 RVA: 0x000639ED File Offset: 0x00061BED
			public uint offset { readonly get; internal set; }

			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x06001599 RID: 5529 RVA: 0x000639F6 File Offset: 0x00061BF6
			// (set) Token: 0x0600159A RID: 5530 RVA: 0x000639FE File Offset: 0x00061BFE
			public uint bit { readonly get; internal set; }

			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x0600159B RID: 5531 RVA: 0x00063A07 File Offset: 0x00061C07
			// (set) Token: 0x0600159C RID: 5532 RVA: 0x00063A0F File Offset: 0x00061C0F
			public uint sizeInBits { readonly get; internal set; }

			// Token: 0x170005DB RID: 1499
			// (get) Token: 0x0600159D RID: 5533 RVA: 0x00063A18 File Offset: 0x00061C18
			// (set) Token: 0x0600159E RID: 5534 RVA: 0x00063A20 File Offset: 0x00061C20
			public FourCC format { readonly get; internal set; }

			// Token: 0x170005DC RID: 1500
			// (get) Token: 0x0600159F RID: 5535 RVA: 0x00063A29 File Offset: 0x00061C29
			// (set) Token: 0x060015A0 RID: 5536 RVA: 0x00063A31 File Offset: 0x00061C31
			private InputControlLayout.ControlItem.Flags flags { readonly get; set; }

			// Token: 0x170005DD RID: 1501
			// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00063A3A File Offset: 0x00061C3A
			// (set) Token: 0x060015A2 RID: 5538 RVA: 0x00063A42 File Offset: 0x00061C42
			public int arraySize { readonly get; internal set; }

			// Token: 0x170005DE RID: 1502
			// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00063A4B File Offset: 0x00061C4B
			// (set) Token: 0x060015A4 RID: 5540 RVA: 0x00063A53 File Offset: 0x00061C53
			public PrimitiveValue defaultState { readonly get; internal set; }

			// Token: 0x170005DF RID: 1503
			// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00063A5C File Offset: 0x00061C5C
			// (set) Token: 0x060015A6 RID: 5542 RVA: 0x00063A64 File Offset: 0x00061C64
			public PrimitiveValue minValue { readonly get; internal set; }

			// Token: 0x170005E0 RID: 1504
			// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00063A6D File Offset: 0x00061C6D
			// (set) Token: 0x060015A8 RID: 5544 RVA: 0x00063A75 File Offset: 0x00061C75
			public PrimitiveValue maxValue { readonly get; internal set; }

			// Token: 0x170005E1 RID: 1505
			// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00063A7E File Offset: 0x00061C7E
			// (set) Token: 0x060015AA RID: 5546 RVA: 0x00063A8B File Offset: 0x00061C8B
			public bool isModifyingExistingControl
			{
				get
				{
					return (this.flags & InputControlLayout.ControlItem.Flags.isModifyingExistingControl) == InputControlLayout.ControlItem.Flags.isModifyingExistingControl;
				}
				internal set
				{
					if (value)
					{
						this.flags |= InputControlLayout.ControlItem.Flags.isModifyingExistingControl;
						return;
					}
					this.flags &= ~InputControlLayout.ControlItem.Flags.isModifyingExistingControl;
				}
			}

			// Token: 0x170005E2 RID: 1506
			// (get) Token: 0x060015AB RID: 5547 RVA: 0x00063AAE File Offset: 0x00061CAE
			// (set) Token: 0x060015AC RID: 5548 RVA: 0x00063ABB File Offset: 0x00061CBB
			public bool isNoisy
			{
				get
				{
					return (this.flags & InputControlLayout.ControlItem.Flags.IsNoisy) == InputControlLayout.ControlItem.Flags.IsNoisy;
				}
				internal set
				{
					if (value)
					{
						this.flags |= InputControlLayout.ControlItem.Flags.IsNoisy;
						return;
					}
					this.flags &= ~InputControlLayout.ControlItem.Flags.IsNoisy;
				}
			}

			// Token: 0x170005E3 RID: 1507
			// (get) Token: 0x060015AD RID: 5549 RVA: 0x00063ADE File Offset: 0x00061CDE
			// (set) Token: 0x060015AE RID: 5550 RVA: 0x00063AEB File Offset: 0x00061CEB
			public bool isSynthetic
			{
				get
				{
					return (this.flags & InputControlLayout.ControlItem.Flags.IsSynthetic) == InputControlLayout.ControlItem.Flags.IsSynthetic;
				}
				internal set
				{
					if (value)
					{
						this.flags |= InputControlLayout.ControlItem.Flags.IsSynthetic;
						return;
					}
					this.flags &= ~InputControlLayout.ControlItem.Flags.IsSynthetic;
				}
			}

			// Token: 0x170005E4 RID: 1508
			// (get) Token: 0x060015AF RID: 5551 RVA: 0x00063B0E File Offset: 0x00061D0E
			// (set) Token: 0x060015B0 RID: 5552 RVA: 0x00063B1D File Offset: 0x00061D1D
			public bool dontReset
			{
				get
				{
					return (this.flags & InputControlLayout.ControlItem.Flags.DontReset) == InputControlLayout.ControlItem.Flags.DontReset;
				}
				internal set
				{
					if (value)
					{
						this.flags |= InputControlLayout.ControlItem.Flags.DontReset;
						return;
					}
					this.flags &= ~InputControlLayout.ControlItem.Flags.DontReset;
				}
			}

			// Token: 0x170005E5 RID: 1509
			// (get) Token: 0x060015B1 RID: 5553 RVA: 0x00063B41 File Offset: 0x00061D41
			// (set) Token: 0x060015B2 RID: 5554 RVA: 0x00063B4E File Offset: 0x00061D4E
			public bool isFirstDefinedInThisLayout
			{
				get
				{
					return (this.flags & InputControlLayout.ControlItem.Flags.IsFirstDefinedInThisLayout) > (InputControlLayout.ControlItem.Flags)0;
				}
				internal set
				{
					if (value)
					{
						this.flags |= InputControlLayout.ControlItem.Flags.IsFirstDefinedInThisLayout;
						return;
					}
					this.flags &= ~InputControlLayout.ControlItem.Flags.IsFirstDefinedInThisLayout;
				}
			}

			// Token: 0x170005E6 RID: 1510
			// (get) Token: 0x060015B3 RID: 5555 RVA: 0x00063B71 File Offset: 0x00061D71
			public bool isArray
			{
				get
				{
					return this.arraySize != 0;
				}
			}

			// Token: 0x060015B4 RID: 5556 RVA: 0x00063B7C File Offset: 0x00061D7C
			public InputControlLayout.ControlItem Merge(InputControlLayout.ControlItem other)
			{
				InputControlLayout.ControlItem controlItem = default(InputControlLayout.ControlItem);
				controlItem.name = this.name;
				controlItem.isModifyingExistingControl = this.isModifyingExistingControl;
				controlItem.displayName = (string.IsNullOrEmpty(this.displayName) ? other.displayName : this.displayName);
				controlItem.shortDisplayName = (string.IsNullOrEmpty(this.shortDisplayName) ? other.shortDisplayName : this.shortDisplayName);
				controlItem.layout = (this.layout.IsEmpty() ? other.layout : this.layout);
				controlItem.variants = (this.variants.IsEmpty() ? other.variants : this.variants);
				controlItem.useStateFrom = this.useStateFrom ?? other.useStateFrom;
				controlItem.arraySize = ((!this.isArray) ? other.arraySize : this.arraySize);
				controlItem.isNoisy = this.isNoisy || other.isNoisy;
				controlItem.dontReset = this.dontReset || other.dontReset;
				controlItem.isSynthetic = this.isSynthetic || other.isSynthetic;
				controlItem.isFirstDefinedInThisLayout = false;
				if (this.offset != 4294967295U)
				{
					controlItem.offset = this.offset;
				}
				else
				{
					controlItem.offset = other.offset;
				}
				if (this.bit != 4294967295U)
				{
					controlItem.bit = this.bit;
				}
				else
				{
					controlItem.bit = other.bit;
				}
				if (this.format != 0)
				{
					controlItem.format = this.format;
				}
				else
				{
					controlItem.format = other.format;
				}
				if (this.sizeInBits != 0U)
				{
					controlItem.sizeInBits = this.sizeInBits;
				}
				else
				{
					controlItem.sizeInBits = other.sizeInBits;
				}
				if (this.aliases.Count > 0)
				{
					controlItem.aliases = this.aliases;
				}
				else
				{
					controlItem.aliases = other.aliases;
				}
				if (this.usages.Count > 0)
				{
					controlItem.usages = this.usages;
				}
				else
				{
					controlItem.usages = other.usages;
				}
				if (this.parameters.Count == 0)
				{
					controlItem.parameters = other.parameters;
				}
				else
				{
					controlItem.parameters = this.parameters;
				}
				if (this.processors.Count == 0)
				{
					controlItem.processors = other.processors;
				}
				else
				{
					controlItem.processors = this.processors;
				}
				if (!string.IsNullOrEmpty(this.displayName))
				{
					controlItem.displayName = this.displayName;
				}
				else
				{
					controlItem.displayName = other.displayName;
				}
				if (!this.defaultState.isEmpty)
				{
					controlItem.defaultState = this.defaultState;
				}
				else
				{
					controlItem.defaultState = other.defaultState;
				}
				if (!this.minValue.isEmpty)
				{
					controlItem.minValue = this.minValue;
				}
				else
				{
					controlItem.minValue = other.minValue;
				}
				if (!this.maxValue.isEmpty)
				{
					controlItem.maxValue = this.maxValue;
				}
				else
				{
					controlItem.maxValue = other.maxValue;
				}
				return controlItem;
			}

			// Token: 0x02000284 RID: 644
			[Flags]
			private enum Flags
			{
				// Token: 0x04000D20 RID: 3360
				isModifyingExistingControl = 1,
				// Token: 0x04000D21 RID: 3361
				IsNoisy = 2,
				// Token: 0x04000D22 RID: 3362
				IsSynthetic = 4,
				// Token: 0x04000D23 RID: 3363
				IsFirstDefinedInThisLayout = 8,
				// Token: 0x04000D24 RID: 3364
				DontReset = 16
			}
		}

		// Token: 0x0200022C RID: 556
		public class Builder
		{
			// Token: 0x170005E7 RID: 1511
			// (get) Token: 0x060015B5 RID: 5557 RVA: 0x00063ED3 File Offset: 0x000620D3
			// (set) Token: 0x060015B6 RID: 5558 RVA: 0x00063EDB File Offset: 0x000620DB
			public string name { get; set; }

			// Token: 0x170005E8 RID: 1512
			// (get) Token: 0x060015B7 RID: 5559 RVA: 0x00063EE4 File Offset: 0x000620E4
			// (set) Token: 0x060015B8 RID: 5560 RVA: 0x00063EEC File Offset: 0x000620EC
			public string displayName { get; set; }

			// Token: 0x170005E9 RID: 1513
			// (get) Token: 0x060015B9 RID: 5561 RVA: 0x00063EF5 File Offset: 0x000620F5
			// (set) Token: 0x060015BA RID: 5562 RVA: 0x00063EFD File Offset: 0x000620FD
			public Type type { get; set; }

			// Token: 0x170005EA RID: 1514
			// (get) Token: 0x060015BB RID: 5563 RVA: 0x00063F06 File Offset: 0x00062106
			// (set) Token: 0x060015BC RID: 5564 RVA: 0x00063F0E File Offset: 0x0006210E
			public FourCC stateFormat { get; set; }

			// Token: 0x170005EB RID: 1515
			// (get) Token: 0x060015BD RID: 5565 RVA: 0x00063F17 File Offset: 0x00062117
			// (set) Token: 0x060015BE RID: 5566 RVA: 0x00063F1F File Offset: 0x0006211F
			public int stateSizeInBytes { get; set; }

			// Token: 0x170005EC RID: 1516
			// (get) Token: 0x060015BF RID: 5567 RVA: 0x00063F28 File Offset: 0x00062128
			// (set) Token: 0x060015C0 RID: 5568 RVA: 0x00063F30 File Offset: 0x00062130
			public string extendsLayout
			{
				get
				{
					return this.m_ExtendsLayout;
				}
				set
				{
					if (!string.IsNullOrEmpty(value))
					{
						this.m_ExtendsLayout = value;
						return;
					}
					this.m_ExtendsLayout = null;
				}
			}

			// Token: 0x170005ED RID: 1517
			// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00063F49 File Offset: 0x00062149
			// (set) Token: 0x060015C2 RID: 5570 RVA: 0x00063F51 File Offset: 0x00062151
			public bool? updateBeforeRender { get; set; }

			// Token: 0x170005EE RID: 1518
			// (get) Token: 0x060015C3 RID: 5571 RVA: 0x00063F5A File Offset: 0x0006215A
			public ReadOnlyArray<InputControlLayout.ControlItem> controls
			{
				get
				{
					return new ReadOnlyArray<InputControlLayout.ControlItem>(this.m_Controls, 0, this.m_ControlCount);
				}
			}

			// Token: 0x060015C4 RID: 5572 RVA: 0x00063F70 File Offset: 0x00062170
			public InputControlLayout.Builder.ControlBuilder AddControl(string name)
			{
				if (string.IsNullOrEmpty(name))
				{
					throw new ArgumentException(name);
				}
				int num = ArrayHelpers.AppendWithCapacity<InputControlLayout.ControlItem>(ref this.m_Controls, ref this.m_ControlCount, new InputControlLayout.ControlItem
				{
					name = new InternedString(name),
					isModifyingExistingControl = (name.IndexOf('/') != -1),
					offset = uint.MaxValue,
					bit = uint.MaxValue
				}, 10);
				return new InputControlLayout.Builder.ControlBuilder
				{
					builder = this,
					index = num
				};
			}

			// Token: 0x060015C5 RID: 5573 RVA: 0x00063FF4 File Offset: 0x000621F4
			public InputControlLayout.Builder WithName(string name)
			{
				this.name = name;
				return this;
			}

			// Token: 0x060015C6 RID: 5574 RVA: 0x00063FFE File Offset: 0x000621FE
			public InputControlLayout.Builder WithDisplayName(string displayName)
			{
				this.displayName = displayName;
				return this;
			}

			// Token: 0x060015C7 RID: 5575 RVA: 0x00064008 File Offset: 0x00062208
			public InputControlLayout.Builder WithType<T>() where T : InputControl
			{
				this.type = typeof(T);
				return this;
			}

			// Token: 0x060015C8 RID: 5576 RVA: 0x0006401B File Offset: 0x0006221B
			public InputControlLayout.Builder WithFormat(FourCC format)
			{
				this.stateFormat = format;
				return this;
			}

			// Token: 0x060015C9 RID: 5577 RVA: 0x00064025 File Offset: 0x00062225
			public InputControlLayout.Builder WithFormat(string format)
			{
				return this.WithFormat(new FourCC(format));
			}

			// Token: 0x060015CA RID: 5578 RVA: 0x00064033 File Offset: 0x00062233
			public InputControlLayout.Builder WithSizeInBytes(int sizeInBytes)
			{
				this.stateSizeInBytes = sizeInBytes;
				return this;
			}

			// Token: 0x060015CB RID: 5579 RVA: 0x0006403D File Offset: 0x0006223D
			public InputControlLayout.Builder Extend(string baseLayoutName)
			{
				this.extendsLayout = baseLayoutName;
				return this;
			}

			// Token: 0x060015CC RID: 5580 RVA: 0x00064048 File Offset: 0x00062248
			public InputControlLayout Build()
			{
				InputControlLayout.ControlItem[] array = null;
				if (this.m_ControlCount > 0)
				{
					array = new InputControlLayout.ControlItem[this.m_ControlCount];
					Array.Copy(this.m_Controls, array, this.m_ControlCount);
				}
				return new InputControlLayout(new InternedString(this.name), (this.type == null && string.IsNullOrEmpty(this.extendsLayout)) ? typeof(InputDevice) : this.type)
				{
					m_DisplayName = this.displayName,
					m_StateFormat = this.stateFormat,
					m_StateSizeInBytes = this.stateSizeInBytes,
					m_BaseLayouts = ((!string.IsNullOrEmpty(this.extendsLayout)) ? new InlinedArray<InternedString>(new InternedString(this.extendsLayout)) : default(InlinedArray<InternedString>)),
					m_Controls = array,
					m_UpdateBeforeRender = this.updateBeforeRender
				};
			}

			// Token: 0x04000BF1 RID: 3057
			private string m_ExtendsLayout;

			// Token: 0x04000BF3 RID: 3059
			private int m_ControlCount;

			// Token: 0x04000BF4 RID: 3060
			private InputControlLayout.ControlItem[] m_Controls;

			// Token: 0x02000285 RID: 645
			public struct ControlBuilder
			{
				// Token: 0x0600169F RID: 5791 RVA: 0x00067374 File Offset: 0x00065574
				public InputControlLayout.Builder.ControlBuilder WithDisplayName(string displayName)
				{
					this.builder.m_Controls[this.index].displayName = displayName;
					return this;
				}

				// Token: 0x060016A0 RID: 5792 RVA: 0x00067398 File Offset: 0x00065598
				public InputControlLayout.Builder.ControlBuilder WithLayout(string layout)
				{
					if (string.IsNullOrEmpty(layout))
					{
						throw new ArgumentException("Layout name cannot be null or empty", "layout");
					}
					this.builder.m_Controls[this.index].layout = new InternedString(layout);
					return this;
				}

				// Token: 0x060016A1 RID: 5793 RVA: 0x000673E4 File Offset: 0x000655E4
				public InputControlLayout.Builder.ControlBuilder WithFormat(FourCC format)
				{
					this.builder.m_Controls[this.index].format = format;
					return this;
				}

				// Token: 0x060016A2 RID: 5794 RVA: 0x00067408 File Offset: 0x00065608
				public InputControlLayout.Builder.ControlBuilder WithFormat(string format)
				{
					return this.WithFormat(new FourCC(format));
				}

				// Token: 0x060016A3 RID: 5795 RVA: 0x00067416 File Offset: 0x00065616
				public InputControlLayout.Builder.ControlBuilder WithByteOffset(uint offset)
				{
					this.builder.m_Controls[this.index].offset = offset;
					return this;
				}

				// Token: 0x060016A4 RID: 5796 RVA: 0x0006743A File Offset: 0x0006563A
				public InputControlLayout.Builder.ControlBuilder WithBitOffset(uint bit)
				{
					this.builder.m_Controls[this.index].bit = bit;
					return this;
				}

				// Token: 0x060016A5 RID: 5797 RVA: 0x0006745E File Offset: 0x0006565E
				public InputControlLayout.Builder.ControlBuilder IsSynthetic(bool value)
				{
					this.builder.m_Controls[this.index].isSynthetic = value;
					return this;
				}

				// Token: 0x060016A6 RID: 5798 RVA: 0x00067482 File Offset: 0x00065682
				public InputControlLayout.Builder.ControlBuilder IsNoisy(bool value)
				{
					this.builder.m_Controls[this.index].isNoisy = value;
					return this;
				}

				// Token: 0x060016A7 RID: 5799 RVA: 0x000674A6 File Offset: 0x000656A6
				public InputControlLayout.Builder.ControlBuilder DontReset(bool value)
				{
					this.builder.m_Controls[this.index].dontReset = value;
					return this;
				}

				// Token: 0x060016A8 RID: 5800 RVA: 0x000674CA File Offset: 0x000656CA
				public InputControlLayout.Builder.ControlBuilder WithSizeInBits(uint sizeInBits)
				{
					this.builder.m_Controls[this.index].sizeInBits = sizeInBits;
					return this;
				}

				// Token: 0x060016A9 RID: 5801 RVA: 0x000674F0 File Offset: 0x000656F0
				public InputControlLayout.Builder.ControlBuilder WithRange(float minValue, float maxValue)
				{
					this.builder.m_Controls[this.index].minValue = minValue;
					this.builder.m_Controls[this.index].maxValue = maxValue;
					return this;
				}

				// Token: 0x060016AA RID: 5802 RVA: 0x00067548 File Offset: 0x00065748
				public InputControlLayout.Builder.ControlBuilder WithUsages(params InternedString[] usages)
				{
					if (usages == null || usages.Length == 0)
					{
						return this;
					}
					for (int i = 0; i < usages.Length; i++)
					{
						if (usages[i].IsEmpty())
						{
							throw new ArgumentException(string.Format("Empty usage entry at index {0} for control '{1}' in layout '{2}'", i, this.builder.m_Controls[this.index].name, this.builder.name), "usages");
						}
					}
					this.builder.m_Controls[this.index].usages = new ReadOnlyArray<InternedString>(usages);
					return this;
				}

				// Token: 0x060016AB RID: 5803 RVA: 0x000675EC File Offset: 0x000657EC
				public InputControlLayout.Builder.ControlBuilder WithUsages(IEnumerable<string> usages)
				{
					InternedString[] array = usages.Select<string, InternedString>((string x) => new InternedString(x)).ToArray<InternedString>();
					return this.WithUsages(array);
				}

				// Token: 0x060016AC RID: 5804 RVA: 0x0006762B File Offset: 0x0006582B
				public InputControlLayout.Builder.ControlBuilder WithUsages(params string[] usages)
				{
					return this.WithUsages(usages);
				}

				// Token: 0x060016AD RID: 5805 RVA: 0x00067634 File Offset: 0x00065834
				public InputControlLayout.Builder.ControlBuilder WithParameters(string parameters)
				{
					if (string.IsNullOrEmpty(parameters))
					{
						return this;
					}
					NamedValue[] array = NamedValue.ParseMultiple(parameters);
					this.builder.m_Controls[this.index].parameters = new ReadOnlyArray<NamedValue>(array);
					return this;
				}

				// Token: 0x060016AE RID: 5806 RVA: 0x00067680 File Offset: 0x00065880
				public InputControlLayout.Builder.ControlBuilder WithProcessors(string processors)
				{
					if (string.IsNullOrEmpty(processors))
					{
						return this;
					}
					NameAndParameters[] array = NameAndParameters.ParseMultiple(processors).ToArray<NameAndParameters>();
					this.builder.m_Controls[this.index].processors = new ReadOnlyArray<NameAndParameters>(array);
					return this;
				}

				// Token: 0x060016AF RID: 5807 RVA: 0x000676CF File Offset: 0x000658CF
				public InputControlLayout.Builder.ControlBuilder WithDefaultState(PrimitiveValue value)
				{
					this.builder.m_Controls[this.index].defaultState = value;
					return this;
				}

				// Token: 0x060016B0 RID: 5808 RVA: 0x000676F3 File Offset: 0x000658F3
				public InputControlLayout.Builder.ControlBuilder UsingStateFrom(string path)
				{
					if (string.IsNullOrEmpty(path))
					{
						return this;
					}
					this.builder.m_Controls[this.index].useStateFrom = path;
					return this;
				}

				// Token: 0x060016B1 RID: 5809 RVA: 0x00067726 File Offset: 0x00065926
				public InputControlLayout.Builder.ControlBuilder AsArrayOfControlsWithSize(int arraySize)
				{
					this.builder.m_Controls[this.index].arraySize = arraySize;
					return this;
				}

				// Token: 0x04000D25 RID: 3365
				internal InputControlLayout.Builder builder;

				// Token: 0x04000D26 RID: 3366
				internal int index;
			}
		}

		// Token: 0x0200022D RID: 557
		[Flags]
		private enum Flags
		{
			// Token: 0x04000BF6 RID: 3062
			IsGenericTypeOfDevice = 1,
			// Token: 0x04000BF7 RID: 3063
			HideInUI = 2,
			// Token: 0x04000BF8 RID: 3064
			IsOverride = 4,
			// Token: 0x04000BF9 RID: 3065
			CanRunInBackground = 8,
			// Token: 0x04000BFA RID: 3066
			CanRunInBackgroundIsSet = 16,
			// Token: 0x04000BFB RID: 3067
			IsNoisy = 32
		}

		// Token: 0x0200022E RID: 558
		[Serializable]
		internal struct LayoutJsonNameAndDescriptorOnly
		{
			// Token: 0x04000BFC RID: 3068
			public string name;

			// Token: 0x04000BFD RID: 3069
			public string extend;

			// Token: 0x04000BFE RID: 3070
			public string[] extendMultiple;

			// Token: 0x04000BFF RID: 3071
			public InputDeviceMatcher.MatcherJson device;
		}

		// Token: 0x0200022F RID: 559
		[Serializable]
		private struct LayoutJson
		{
			// Token: 0x060015CE RID: 5582 RVA: 0x00064130 File Offset: 0x00062330
			public InputControlLayout ToLayout()
			{
				Type type = null;
				if (!string.IsNullOrEmpty(this.type))
				{
					type = Type.GetType(this.type, false);
					if (type == null)
					{
						Debug.Log(string.Concat(new string[] { "Cannot find type '", this.type, "' used by layout '", this.name, "'; falling back to using InputDevice" }));
						type = typeof(InputDevice);
					}
					else if (!typeof(InputControl).IsAssignableFrom(type))
					{
						throw new InvalidOperationException(string.Concat(new string[] { "'", this.type, "' used by layout '", this.name, "' is not an InputControl" }));
					}
				}
				else if (string.IsNullOrEmpty(this.extend))
				{
					type = typeof(InputDevice);
				}
				InputControlLayout inputControlLayout = new InputControlLayout(this.name, type);
				inputControlLayout.m_DisplayName = this.displayName;
				inputControlLayout.m_Description = this.description;
				inputControlLayout.isGenericTypeOfDevice = this.isGenericTypeOfDevice;
				inputControlLayout.hideInUI = this.hideInUI;
				inputControlLayout.m_Variants = new InternedString(this.variant);
				inputControlLayout.m_CommonUsages = ArrayHelpers.Select<string, InternedString>(this.commonUsages, (string x) => new InternedString(x));
				InputControlLayout inputControlLayout2 = inputControlLayout;
				if (!string.IsNullOrEmpty(this.format))
				{
					inputControlLayout2.m_StateFormat = new FourCC(this.format);
				}
				if (!string.IsNullOrEmpty(this.extend))
				{
					inputControlLayout2.m_BaseLayouts.Append(new InternedString(this.extend));
				}
				if (this.extendMultiple != null)
				{
					foreach (string text in this.extendMultiple)
					{
						inputControlLayout2.m_BaseLayouts.Append(new InternedString(text));
					}
				}
				if (!string.IsNullOrEmpty(this.beforeRender))
				{
					string text2 = this.beforeRender.ToLowerInvariant();
					if (text2 == "ignore")
					{
						inputControlLayout2.m_UpdateBeforeRender = new bool?(false);
					}
					else
					{
						if (!(text2 == "update"))
						{
							throw new InvalidOperationException("Invalid beforeRender setting '" + this.beforeRender + "' (should be 'ignore' or 'update')");
						}
						inputControlLayout2.m_UpdateBeforeRender = new bool?(true);
					}
				}
				if (!string.IsNullOrEmpty(this.runInBackground))
				{
					string text3 = this.runInBackground.ToLowerInvariant();
					if (text3 == "enabled")
					{
						inputControlLayout2.canRunInBackground = new bool?(true);
					}
					else
					{
						if (!(text3 == "disabled"))
						{
							throw new InvalidOperationException("Invalid runInBackground setting '" + this.beforeRender + "' (should be 'enabled' or 'disabled')");
						}
						inputControlLayout2.canRunInBackground = new bool?(false);
					}
				}
				if (this.controls != null)
				{
					List<InputControlLayout.ControlItem> list = new List<InputControlLayout.ControlItem>();
					foreach (InputControlLayout.ControlItemJson controlItemJson in this.controls)
					{
						if (string.IsNullOrEmpty(controlItemJson.name))
						{
							throw new InvalidOperationException("Control with no name in layout '" + this.name);
						}
						InputControlLayout.ControlItem controlItem = controlItemJson.ToLayout();
						list.Add(controlItem);
					}
					inputControlLayout2.m_Controls = list.ToArray();
				}
				return inputControlLayout2;
			}

			// Token: 0x060015CF RID: 5583 RVA: 0x0006444C File Offset: 0x0006264C
			public static InputControlLayout.LayoutJson FromLayout(InputControlLayout layout)
			{
				InputControlLayout.LayoutJson layoutJson = default(InputControlLayout.LayoutJson);
				layoutJson.name = layout.m_Name;
				Type type = layout.type;
				layoutJson.type = ((type != null) ? type.AssemblyQualifiedName : null);
				layoutJson.variant = layout.m_Variants;
				layoutJson.displayName = layout.m_DisplayName;
				layoutJson.description = layout.m_Description;
				layoutJson.isGenericTypeOfDevice = layout.isGenericTypeOfDevice;
				layoutJson.hideInUI = layout.hideInUI;
				layoutJson.extend = ((layout.m_BaseLayouts.length == 1) ? layout.m_BaseLayouts[0].ToString() : null);
				string[] array;
				if (layout.m_BaseLayouts.length <= 1)
				{
					array = null;
				}
				else
				{
					array = layout.m_BaseLayouts.ToArray<string>((InternedString x) => x.ToString());
				}
				layoutJson.extendMultiple = array;
				layoutJson.format = layout.stateFormat.ToString();
				layoutJson.commonUsages = ArrayHelpers.Select<InternedString, string>(layout.m_CommonUsages, (InternedString x) => x.ToString());
				layoutJson.controls = InputControlLayout.ControlItemJson.FromControlItems(layout.m_Controls);
				layoutJson.beforeRender = ((layout.m_UpdateBeforeRender != null) ? (layout.m_UpdateBeforeRender.Value ? "Update" : "Ignore") : null);
				return layoutJson;
			}

			// Token: 0x04000C00 RID: 3072
			public string name;

			// Token: 0x04000C01 RID: 3073
			public string extend;

			// Token: 0x04000C02 RID: 3074
			public string[] extendMultiple;

			// Token: 0x04000C03 RID: 3075
			public string format;

			// Token: 0x04000C04 RID: 3076
			public string beforeRender;

			// Token: 0x04000C05 RID: 3077
			public string runInBackground;

			// Token: 0x04000C06 RID: 3078
			public string[] commonUsages;

			// Token: 0x04000C07 RID: 3079
			public string displayName;

			// Token: 0x04000C08 RID: 3080
			public string description;

			// Token: 0x04000C09 RID: 3081
			public string type;

			// Token: 0x04000C0A RID: 3082
			public string variant;

			// Token: 0x04000C0B RID: 3083
			public bool isGenericTypeOfDevice;

			// Token: 0x04000C0C RID: 3084
			public bool hideInUI;

			// Token: 0x04000C0D RID: 3085
			public InputControlLayout.ControlItemJson[] controls;
		}

		// Token: 0x02000230 RID: 560
		[Serializable]
		private class ControlItemJson
		{
			// Token: 0x060015D0 RID: 5584 RVA: 0x000645D5 File Offset: 0x000627D5
			public ControlItemJson()
			{
				this.offset = uint.MaxValue;
				this.bit = uint.MaxValue;
			}

			// Token: 0x060015D1 RID: 5585 RVA: 0x000645EC File Offset: 0x000627EC
			public InputControlLayout.ControlItem ToLayout()
			{
				InputControlLayout.ControlItem controlItem = new InputControlLayout.ControlItem
				{
					name = new InternedString(this.name),
					layout = new InternedString(this.layout),
					variants = new InternedString(this.variants),
					displayName = this.displayName,
					shortDisplayName = this.shortDisplayName,
					offset = this.offset,
					useStateFrom = this.useStateFrom,
					bit = this.bit,
					sizeInBits = this.sizeInBits,
					isModifyingExistingControl = (this.name.IndexOf('/') != -1),
					isNoisy = this.noisy,
					dontReset = this.dontReset,
					isSynthetic = this.synthetic,
					isFirstDefinedInThisLayout = true,
					arraySize = this.arraySize
				};
				if (!string.IsNullOrEmpty(this.format))
				{
					controlItem.format = new FourCC(this.format);
				}
				if (!string.IsNullOrEmpty(this.usage) || this.usages != null)
				{
					List<string> list = new List<string>();
					if (!string.IsNullOrEmpty(this.usage))
					{
						list.Add(this.usage);
					}
					if (this.usages != null)
					{
						list.AddRange(this.usages);
					}
					controlItem.usages = new ReadOnlyArray<InternedString>(list.Select<string, InternedString>((string x) => new InternedString(x)).ToArray<InternedString>());
				}
				if (!string.IsNullOrEmpty(this.alias) || this.aliases != null)
				{
					List<string> list2 = new List<string>();
					if (!string.IsNullOrEmpty(this.alias))
					{
						list2.Add(this.alias);
					}
					if (this.aliases != null)
					{
						list2.AddRange(this.aliases);
					}
					controlItem.aliases = new ReadOnlyArray<InternedString>(list2.Select<string, InternedString>((string x) => new InternedString(x)).ToArray<InternedString>());
				}
				if (!string.IsNullOrEmpty(this.parameters))
				{
					controlItem.parameters = new ReadOnlyArray<NamedValue>(NamedValue.ParseMultiple(this.parameters));
				}
				if (!string.IsNullOrEmpty(this.processors))
				{
					controlItem.processors = new ReadOnlyArray<NameAndParameters>(NameAndParameters.ParseMultiple(this.processors).ToArray<NameAndParameters>());
				}
				if (this.defaultState != null)
				{
					controlItem.defaultState = PrimitiveValue.FromObject(this.defaultState);
				}
				if (this.minValue != null)
				{
					controlItem.minValue = PrimitiveValue.FromObject(this.minValue);
				}
				if (this.maxValue != null)
				{
					controlItem.maxValue = PrimitiveValue.FromObject(this.maxValue);
				}
				return controlItem;
			}

			// Token: 0x060015D2 RID: 5586 RVA: 0x00064894 File Offset: 0x00062A94
			public static InputControlLayout.ControlItemJson[] FromControlItems(InputControlLayout.ControlItem[] items)
			{
				if (items == null)
				{
					return null;
				}
				int num = items.Length;
				InputControlLayout.ControlItemJson[] array = new InputControlLayout.ControlItemJson[num];
				for (int i = 0; i < num; i++)
				{
					InputControlLayout.ControlItem controlItem = items[i];
					InputControlLayout.ControlItemJson[] array2 = array;
					int num2 = i;
					InputControlLayout.ControlItemJson controlItemJson = new InputControlLayout.ControlItemJson();
					controlItemJson.name = controlItem.name;
					controlItemJson.layout = controlItem.layout;
					controlItemJson.variants = controlItem.variants;
					controlItemJson.displayName = controlItem.displayName;
					controlItemJson.shortDisplayName = controlItem.shortDisplayName;
					controlItemJson.bit = controlItem.bit;
					controlItemJson.offset = controlItem.offset;
					controlItemJson.sizeInBits = controlItem.sizeInBits;
					controlItemJson.format = controlItem.format.ToString();
					controlItemJson.parameters = string.Join(",", controlItem.parameters.Select<NamedValue, string>((NamedValue x) => x.ToString()).ToArray<string>());
					controlItemJson.processors = string.Join(",", controlItem.processors.Select<NameAndParameters, string>((NameAndParameters x) => x.ToString()).ToArray<string>());
					controlItemJson.usages = controlItem.usages.Select<InternedString, string>((InternedString x) => x.ToString()).ToArray<string>();
					controlItemJson.aliases = controlItem.aliases.Select<InternedString, string>((InternedString x) => x.ToString()).ToArray<string>();
					controlItemJson.noisy = controlItem.isNoisy;
					controlItemJson.dontReset = controlItem.dontReset;
					controlItemJson.synthetic = controlItem.isSynthetic;
					controlItemJson.arraySize = controlItem.arraySize;
					controlItemJson.defaultState = controlItem.defaultState.ToString();
					controlItemJson.minValue = controlItem.minValue.ToString();
					controlItemJson.maxValue = controlItem.maxValue.ToString();
					array2[num2] = controlItemJson;
				}
				return array;
			}

			// Token: 0x04000C0E RID: 3086
			public string name;

			// Token: 0x04000C0F RID: 3087
			public string layout;

			// Token: 0x04000C10 RID: 3088
			public string variants;

			// Token: 0x04000C11 RID: 3089
			public string usage;

			// Token: 0x04000C12 RID: 3090
			public string alias;

			// Token: 0x04000C13 RID: 3091
			public string useStateFrom;

			// Token: 0x04000C14 RID: 3092
			public uint offset;

			// Token: 0x04000C15 RID: 3093
			public uint bit;

			// Token: 0x04000C16 RID: 3094
			public uint sizeInBits;

			// Token: 0x04000C17 RID: 3095
			public string format;

			// Token: 0x04000C18 RID: 3096
			public int arraySize;

			// Token: 0x04000C19 RID: 3097
			public string[] usages;

			// Token: 0x04000C1A RID: 3098
			public string[] aliases;

			// Token: 0x04000C1B RID: 3099
			public string parameters;

			// Token: 0x04000C1C RID: 3100
			public string processors;

			// Token: 0x04000C1D RID: 3101
			public string displayName;

			// Token: 0x04000C1E RID: 3102
			public string shortDisplayName;

			// Token: 0x04000C1F RID: 3103
			public bool noisy;

			// Token: 0x04000C20 RID: 3104
			public bool dontReset;

			// Token: 0x04000C21 RID: 3105
			public bool synthetic;

			// Token: 0x04000C22 RID: 3106
			public string defaultState;

			// Token: 0x04000C23 RID: 3107
			public string minValue;

			// Token: 0x04000C24 RID: 3108
			public string maxValue;
		}

		// Token: 0x02000231 RID: 561
		internal struct Collection
		{
			// Token: 0x060015D3 RID: 5587 RVA: 0x00064AF0 File Offset: 0x00062CF0
			public void Allocate()
			{
				this.layoutTypes = new Dictionary<InternedString, Type>();
				this.layoutStrings = new Dictionary<InternedString, string>();
				this.layoutBuilders = new Dictionary<InternedString, Func<InputControlLayout>>();
				this.baseLayoutTable = new Dictionary<InternedString, InternedString>();
				this.layoutOverrides = new Dictionary<InternedString, InternedString[]>();
				this.layoutOverrideNames = new HashSet<InternedString>();
				this.layoutMatchers = new List<InputControlLayout.Collection.LayoutMatcher>();
				this.precompiledLayouts = new Dictionary<InternedString, InputControlLayout.Collection.PrecompiledLayout>();
			}

			// Token: 0x060015D4 RID: 5588 RVA: 0x00064B58 File Offset: 0x00062D58
			public InternedString TryFindLayoutForType(Type layoutType)
			{
				foreach (KeyValuePair<InternedString, Type> keyValuePair in this.layoutTypes)
				{
					if (keyValuePair.Value == layoutType)
					{
						return keyValuePair.Key;
					}
				}
				return default(InternedString);
			}

			// Token: 0x060015D5 RID: 5589 RVA: 0x00064BC8 File Offset: 0x00062DC8
			public InternedString TryFindMatchingLayout(InputDeviceDescription deviceDescription)
			{
				float num = 0f;
				InternedString internedString = default(InternedString);
				int count = this.layoutMatchers.Count;
				for (int i = 0; i < count; i++)
				{
					InputDeviceMatcher deviceMatcher = this.layoutMatchers[i].deviceMatcher;
					float num2 = deviceMatcher.MatchPercentage(deviceDescription);
					if (num2 > 0f && !this.layoutBuilders.ContainsKey(this.layoutMatchers[i].layoutName))
					{
						num2 += 1f;
					}
					if (num2 > num)
					{
						num = num2;
						internedString = this.layoutMatchers[i].layoutName;
					}
				}
				return internedString;
			}

			// Token: 0x060015D6 RID: 5590 RVA: 0x00064C64 File Offset: 0x00062E64
			public bool HasLayout(InternedString name)
			{
				return this.layoutTypes.ContainsKey(name) || this.layoutStrings.ContainsKey(name) || this.layoutBuilders.ContainsKey(name);
			}

			// Token: 0x060015D7 RID: 5591 RVA: 0x00064C90 File Offset: 0x00062E90
			private InputControlLayout TryLoadLayoutInternal(InternedString name)
			{
				string text;
				if (this.layoutStrings.TryGetValue(name, out text))
				{
					return InputControlLayout.FromJson(text);
				}
				Type type;
				if (this.layoutTypes.TryGetValue(name, out type))
				{
					return InputControlLayout.FromType(name, type);
				}
				Func<InputControlLayout> func;
				if (!this.layoutBuilders.TryGetValue(name, out func))
				{
					return null;
				}
				InputControlLayout inputControlLayout = func();
				if (inputControlLayout == null)
				{
					throw new InvalidOperationException(string.Format("Layout builder '{0}' returned null when invoked", name));
				}
				return inputControlLayout;
			}

			// Token: 0x060015D8 RID: 5592 RVA: 0x00064D04 File Offset: 0x00062F04
			public InputControlLayout TryLoadLayout(InternedString name, Dictionary<InternedString, InputControlLayout> table = null)
			{
				InputControlLayout inputControlLayout;
				if (table != null && table.TryGetValue(name, out inputControlLayout))
				{
					return inputControlLayout;
				}
				inputControlLayout = this.TryLoadLayoutInternal(name);
				if (inputControlLayout != null)
				{
					inputControlLayout.m_Name = name;
					if (this.layoutOverrideNames.Contains(name))
					{
						inputControlLayout.isOverride = true;
					}
					InternedString internedString = default(InternedString);
					if (!inputControlLayout.isOverride && this.baseLayoutTable.TryGetValue(name, out internedString))
					{
						InputControlLayout inputControlLayout2 = this.TryLoadLayout(internedString, table);
						if (inputControlLayout2 == null)
						{
							throw new InputControlLayout.LayoutNotFoundException(string.Format("Cannot find base layout '{0}' of layout '{1}'", internedString, name));
						}
						inputControlLayout.MergeLayout(inputControlLayout2);
						if (inputControlLayout.m_BaseLayouts.length == 0)
						{
							inputControlLayout.m_BaseLayouts.Append(internedString);
						}
					}
					InternedString[] array;
					if (this.layoutOverrides.TryGetValue(name, out array))
					{
						foreach (InternedString internedString2 in array)
						{
							InputControlLayout inputControlLayout3 = this.TryLoadLayout(internedString2, null);
							inputControlLayout3.MergeLayout(inputControlLayout);
							inputControlLayout3.m_BaseLayouts.Clear();
							inputControlLayout3.isOverride = false;
							inputControlLayout3.isGenericTypeOfDevice = inputControlLayout.isGenericTypeOfDevice;
							inputControlLayout3.m_Name = inputControlLayout.name;
							inputControlLayout3.m_BaseLayouts = inputControlLayout.m_BaseLayouts;
							inputControlLayout = inputControlLayout3;
							inputControlLayout.m_AppliedOverrides.Append(internedString2);
						}
					}
					if (table != null)
					{
						table[name] = inputControlLayout;
					}
				}
				return inputControlLayout;
			}

			// Token: 0x060015D9 RID: 5593 RVA: 0x00064E44 File Offset: 0x00063044
			public InternedString GetBaseLayoutName(InternedString layoutName)
			{
				InternedString internedString;
				if (this.baseLayoutTable.TryGetValue(layoutName, out internedString))
				{
					return internedString;
				}
				return default(InternedString);
			}

			// Token: 0x060015DA RID: 5594 RVA: 0x00064E6C File Offset: 0x0006306C
			public InternedString GetRootLayoutName(InternedString layoutName)
			{
				InternedString internedString;
				while (this.baseLayoutTable.TryGetValue(layoutName, out internedString))
				{
					layoutName = internedString;
				}
				return layoutName;
			}

			// Token: 0x060015DB RID: 5595 RVA: 0x00064E90 File Offset: 0x00063090
			public bool ComputeDistanceInInheritanceHierarchy(InternedString firstLayout, InternedString secondLayout, out int distance)
			{
				distance = 0;
				int num = 0;
				InternedString internedString = secondLayout;
				while (!internedString.IsEmpty() && internedString != firstLayout)
				{
					internedString = this.GetBaseLayoutName(internedString);
					num++;
				}
				if (internedString == firstLayout)
				{
					distance = num;
					return true;
				}
				int num2 = 0;
				internedString = firstLayout;
				while (!internedString.IsEmpty() && internedString != secondLayout)
				{
					internedString = this.GetBaseLayoutName(internedString);
					num2++;
				}
				if (internedString == secondLayout)
				{
					distance = num2;
					return true;
				}
				return false;
			}

			// Token: 0x060015DC RID: 5596 RVA: 0x00064F08 File Offset: 0x00063108
			public InternedString FindLayoutThatIntroducesControl(InputControl control, InputControlLayout.Cache cache)
			{
				InputControl inputControl = control;
				while (inputControl.parent != control.device)
				{
					inputControl = inputControl.parent;
				}
				InternedString internedString = control.device.m_Layout;
				InternedString internedString2 = internedString;
				while (this.baseLayoutTable.TryGetValue(internedString2, out internedString2))
				{
					if (cache.FindOrLoadLayout(internedString2, true).FindControl(inputControl.m_Name) != null)
					{
						internedString = internedString2;
					}
				}
				return internedString;
			}

			// Token: 0x060015DD RID: 5597 RVA: 0x00064F74 File Offset: 0x00063174
			public Type GetControlTypeForLayout(InternedString layoutName)
			{
				while (this.layoutStrings.ContainsKey(layoutName))
				{
					InternedString internedString;
					if (!this.baseLayoutTable.TryGetValue(layoutName, out internedString))
					{
						return typeof(InputDevice);
					}
					layoutName = internedString;
				}
				Type type;
				this.layoutTypes.TryGetValue(layoutName, out type);
				return type;
			}

			// Token: 0x060015DE RID: 5598 RVA: 0x00064FC4 File Offset: 0x000631C4
			public bool ValueTypeIsAssignableFrom(InternedString layoutName, Type valueType)
			{
				Type controlTypeForLayout = this.GetControlTypeForLayout(layoutName);
				if (controlTypeForLayout == null)
				{
					return false;
				}
				Type genericTypeArgumentFromHierarchy = TypeHelpers.GetGenericTypeArgumentFromHierarchy(controlTypeForLayout, typeof(InputControl<>), 0);
				return !(genericTypeArgumentFromHierarchy == null) && valueType.IsAssignableFrom(genericTypeArgumentFromHierarchy);
			}

			// Token: 0x060015DF RID: 5599 RVA: 0x00065008 File Offset: 0x00063208
			public bool IsGeneratedLayout(InternedString layout)
			{
				return this.layoutBuilders.ContainsKey(layout);
			}

			// Token: 0x060015E0 RID: 5600 RVA: 0x00065016 File Offset: 0x00063216
			public IEnumerable<InternedString> GetBaseLayouts(InternedString layout, bool includeSelf = true)
			{
				if (includeSelf)
				{
					yield return layout;
				}
				while (this.baseLayoutTable.TryGetValue(layout, out layout))
				{
					yield return layout;
				}
				yield break;
			}

			// Token: 0x060015E1 RID: 5601 RVA: 0x0006503C File Offset: 0x0006323C
			public bool IsBasedOn(InternedString parentLayout, InternedString childLayout)
			{
				InternedString internedString = childLayout;
				while (this.baseLayoutTable.TryGetValue(internedString, out internedString))
				{
					if (internedString == parentLayout)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060015E2 RID: 5602 RVA: 0x0006506C File Offset: 0x0006326C
			public void AddMatcher(InternedString layout, InputDeviceMatcher matcher)
			{
				int count = this.layoutMatchers.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.layoutMatchers[i].deviceMatcher == matcher)
					{
						return;
					}
				}
				this.layoutMatchers.Add(new InputControlLayout.Collection.LayoutMatcher
				{
					layoutName = layout,
					deviceMatcher = matcher
				});
			}

			// Token: 0x04000C25 RID: 3109
			public const float kBaseScoreForNonGeneratedLayouts = 1f;

			// Token: 0x04000C26 RID: 3110
			public Dictionary<InternedString, Type> layoutTypes;

			// Token: 0x04000C27 RID: 3111
			public Dictionary<InternedString, string> layoutStrings;

			// Token: 0x04000C28 RID: 3112
			public Dictionary<InternedString, Func<InputControlLayout>> layoutBuilders;

			// Token: 0x04000C29 RID: 3113
			public Dictionary<InternedString, InternedString> baseLayoutTable;

			// Token: 0x04000C2A RID: 3114
			public Dictionary<InternedString, InternedString[]> layoutOverrides;

			// Token: 0x04000C2B RID: 3115
			public HashSet<InternedString> layoutOverrideNames;

			// Token: 0x04000C2C RID: 3116
			public Dictionary<InternedString, InputControlLayout.Collection.PrecompiledLayout> precompiledLayouts;

			// Token: 0x04000C2D RID: 3117
			public List<InputControlLayout.Collection.LayoutMatcher> layoutMatchers;

			// Token: 0x02000288 RID: 648
			public struct LayoutMatcher
			{
				// Token: 0x04000D32 RID: 3378
				public InternedString layoutName;

				// Token: 0x04000D33 RID: 3379
				public InputDeviceMatcher deviceMatcher;
			}

			// Token: 0x02000289 RID: 649
			public struct PrecompiledLayout
			{
				// Token: 0x04000D34 RID: 3380
				public Func<InputDevice> factoryMethod;

				// Token: 0x04000D35 RID: 3381
				public string metadata;
			}
		}

		// Token: 0x02000232 RID: 562
		public class LayoutNotFoundException : Exception
		{
			// Token: 0x170005EF RID: 1519
			// (get) Token: 0x060015E3 RID: 5603 RVA: 0x000650CF File Offset: 0x000632CF
			public string layout { get; }

			// Token: 0x060015E4 RID: 5604 RVA: 0x000650D7 File Offset: 0x000632D7
			public LayoutNotFoundException()
			{
			}

			// Token: 0x060015E5 RID: 5605 RVA: 0x000650DF File Offset: 0x000632DF
			public LayoutNotFoundException(string name, string message)
				: base(message)
			{
				this.layout = name;
			}

			// Token: 0x060015E6 RID: 5606 RVA: 0x000650EF File Offset: 0x000632EF
			public LayoutNotFoundException(string name)
				: base("Cannot find control layout '" + name + "'")
			{
				this.layout = name;
			}

			// Token: 0x060015E7 RID: 5607 RVA: 0x0006510E File Offset: 0x0006330E
			public LayoutNotFoundException(string message, Exception innerException)
				: base(message, innerException)
			{
			}

			// Token: 0x060015E8 RID: 5608 RVA: 0x00065118 File Offset: 0x00063318
			protected LayoutNotFoundException(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}

		// Token: 0x02000233 RID: 563
		internal struct Cache
		{
			// Token: 0x060015E9 RID: 5609 RVA: 0x00065122 File Offset: 0x00063322
			public void Clear()
			{
				this.table = null;
			}

			// Token: 0x060015EA RID: 5610 RVA: 0x0006512C File Offset: 0x0006332C
			public InputControlLayout FindOrLoadLayout(string name, bool throwIfNotFound = true)
			{
				InternedString internedString = new InternedString(name);
				if (this.table == null)
				{
					this.table = new Dictionary<InternedString, InputControlLayout>();
				}
				InputControlLayout inputControlLayout = InputControlLayout.s_Layouts.TryLoadLayout(internedString, this.table);
				if (inputControlLayout != null)
				{
					return inputControlLayout;
				}
				if (throwIfNotFound)
				{
					throw new InputControlLayout.LayoutNotFoundException(name);
				}
				return null;
			}

			// Token: 0x04000C2F RID: 3119
			public Dictionary<InternedString, InputControlLayout> table;
		}

		// Token: 0x02000234 RID: 564
		internal struct CacheRefInstance : IDisposable
		{
			// Token: 0x060015EB RID: 5611 RVA: 0x00065176 File Offset: 0x00063376
			public void Dispose()
			{
				if (!this.valid)
				{
					return;
				}
				InputControlLayout.s_CacheInstanceRef--;
				if (InputControlLayout.s_CacheInstanceRef <= 0)
				{
					InputControlLayout.s_CacheInstance = default(InputControlLayout.Cache);
					InputControlLayout.s_CacheInstanceRef = 0;
				}
				this.valid = false;
			}

			// Token: 0x04000C30 RID: 3120
			public bool valid;
		}
	}
}
