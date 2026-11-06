using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Pool;

namespace UnityEngine.InputSystem.HID
{
	// Token: 0x0200009A RID: 154
	public class HID : InputDevice
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00040B2C File Offset: 0x0003ED2C
		public static FourCC QueryHIDReportDescriptorDeviceCommandType
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'D');
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00040B3B File Offset: 0x0003ED3B
		public static FourCC QueryHIDReportDescriptorSizeDeviceCommandType
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'S');
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00040B4A File Offset: 0x0003ED4A
		public static FourCC QueryHIDParsedReportDescriptorDeviceCommandType
		{
			get
			{
				return new FourCC('H', 'I', 'D', 'P');
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00040B5C File Offset: 0x0003ED5C
		public HID.HIDDeviceDescriptor hidDescriptor
		{
			get
			{
				if (!this.m_HaveParsedHIDDescriptor)
				{
					if (!string.IsNullOrEmpty(base.description.capabilities))
					{
						this.m_HIDDescriptor = JsonUtility.FromJson<HID.HIDDeviceDescriptor>(base.description.capabilities);
					}
					this.m_HaveParsedHIDDescriptor = true;
				}
				return this.m_HIDDescriptor;
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00040BAC File Offset: 0x0003EDAC
		internal static string OnFindLayoutForDevice(ref InputDeviceDescription description, string matchedLayout, InputDeviceExecuteCommandDelegate executeDeviceCommand)
		{
			if (!string.IsNullOrEmpty(matchedLayout))
			{
				return null;
			}
			if (description.interfaceName != "HID")
			{
				return null;
			}
			HID.HIDDeviceDescriptor hiddeviceDescriptor = HID.ReadHIDDeviceDescriptor(ref description, executeDeviceCommand);
			if (!HIDSupport.supportedHIDUsages.Contains(new HIDSupport.HIDPageUsage(hiddeviceDescriptor.usagePage, hiddeviceDescriptor.usage)))
			{
				return null;
			}
			bool flag = false;
			if (hiddeviceDescriptor.elements != null)
			{
				foreach (HID.HIDElementDescriptor hidelementDescriptor in hiddeviceDescriptor.elements)
				{
					if (hidelementDescriptor.IsUsableElement())
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			Type type = typeof(HID);
			string text = "HID";
			if (hiddeviceDescriptor.usagePage == HID.UsagePage.GenericDesktop && (hiddeviceDescriptor.usage == 4 || hiddeviceDescriptor.usage == 5))
			{
				text = "Joystick";
				type = typeof(Joystick);
			}
			string text2 = "";
			if (text != "Joystick")
			{
				text2 = ((hiddeviceDescriptor.usagePage == HID.UsagePage.GenericDesktop) ? string.Format(" {0}", (HID.GenericDesktop)hiddeviceDescriptor.usage) : string.Format(" {0}-{1}", hiddeviceDescriptor.usagePage, hiddeviceDescriptor.usage));
			}
			InputDeviceMatcher inputDeviceMatcher = InputDeviceMatcher.FromDeviceDescription(description);
			string text3;
			if (!string.IsNullOrEmpty(description.product) && !string.IsNullOrEmpty(description.manufacturer))
			{
				text3 = string.Concat(new string[] { "HID::", description.manufacturer, " ", description.product, text2 });
			}
			else if (!string.IsNullOrEmpty(description.product))
			{
				text3 = "HID::" + description.product + text2;
			}
			else
			{
				if (hiddeviceDescriptor.vendorId == 0)
				{
					return null;
				}
				text3 = string.Format("{0}::{1:X}-{2:X}{3}", new object[] { "HID", hiddeviceDescriptor.vendorId, hiddeviceDescriptor.productId, text2 });
				inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("productId", hiddeviceDescriptor.productId).WithCapability<int>("vendorId", hiddeviceDescriptor.vendorId);
			}
			inputDeviceMatcher = inputDeviceMatcher.WithCapability<int>("usage", hiddeviceDescriptor.usage).WithCapability<HID.UsagePage>("usagePage", hiddeviceDescriptor.usagePage);
			HID.HIDLayoutBuilder layout = new HID.HIDLayoutBuilder
			{
				displayName = description.product,
				hidDescriptor = hiddeviceDescriptor,
				parentLayout = text,
				deviceType = (type ?? typeof(HID))
			};
			InputSystem.RegisterLayoutBuilder(() => layout.Build(), text3, text, new InputDeviceMatcher?(inputDeviceMatcher));
			return text3;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00040E54 File Offset: 0x0003F054
		internal unsafe static HID.HIDDeviceDescriptor ReadHIDDeviceDescriptor(ref InputDeviceDescription deviceDescription, InputDeviceExecuteCommandDelegate executeCommandDelegate)
		{
			if (deviceDescription.interfaceName != "HID")
			{
				throw new ArgumentException(string.Format("Device '{0}' is not a HID", deviceDescription));
			}
			bool flag = true;
			HID.HIDDeviceDescriptor hiddeviceDescriptor = default(HID.HIDDeviceDescriptor);
			if (!string.IsNullOrEmpty(deviceDescription.capabilities))
			{
				try
				{
					hiddeviceDescriptor = HID.HIDDeviceDescriptor.FromJson(deviceDescription.capabilities);
					if (hiddeviceDescriptor.elements != null && hiddeviceDescriptor.elements.Length != 0)
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("Could not parse HID descriptor of device '{0}'", deviceDescription));
					Debug.LogException(ex);
				}
			}
			if (flag)
			{
				InputDeviceCommand inputDeviceCommand = new InputDeviceCommand(HID.QueryHIDReportDescriptorSizeDeviceCommandType, 8);
				long num = executeCommandDelegate(ref inputDeviceCommand);
				if (num > 0L)
				{
					using (NativeArray<byte> nativeArray = InputDeviceCommand.AllocateNative(HID.QueryHIDReportDescriptorDeviceCommandType, (int)num))
					{
						InputDeviceCommand* unsafePtr = (InputDeviceCommand*)nativeArray.GetUnsafePtr<byte>();
						if (executeCommandDelegate(ref *unsafePtr) != num)
						{
							HID.HIDDeviceDescriptor hiddeviceDescriptor2 = default(HID.HIDDeviceDescriptor);
							return hiddeviceDescriptor2;
						}
						if (!HIDParser.ParseReportDescriptor((byte*)unsafePtr->payloadPtr, (int)num, ref hiddeviceDescriptor))
						{
							return default(HID.HIDDeviceDescriptor);
						}
					}
					deviceDescription.capabilities = hiddeviceDescriptor.ToJson();
					return hiddeviceDescriptor;
				}
				using (NativeArray<byte> nativeArray2 = InputDeviceCommand.AllocateNative(HID.QueryHIDParsedReportDescriptorDeviceCommandType, 2097152))
				{
					InputDeviceCommand* unsafePtr2 = (InputDeviceCommand*)nativeArray2.GetUnsafePtr<byte>();
					long num2 = executeCommandDelegate(ref *unsafePtr2);
					if (num2 < 0L)
					{
						return default(HID.HIDDeviceDescriptor);
					}
					byte[] array = new byte[num2];
					try
					{
						byte[] array2;
						byte* ptr;
						if ((array2 = array) == null || array2.Length == 0)
						{
							ptr = null;
						}
						else
						{
							ptr = &array2[0];
						}
						UnsafeUtility.MemCpy((void*)ptr, unsafePtr2->payloadPtr, num2);
					}
					finally
					{
						byte[] array2 = null;
					}
					string @string = Encoding.UTF8.GetString(array, 0, (int)num2);
					try
					{
						hiddeviceDescriptor = HID.HIDDeviceDescriptor.FromJson(@string);
					}
					catch (Exception ex2)
					{
						Debug.LogError(string.Format("Could not parse HID descriptor of device '{0}'", deviceDescription));
						Debug.LogException(ex2);
						return default(HID.HIDDeviceDescriptor);
					}
					deviceDescription.capabilities = @string;
				}
				return hiddeviceDescriptor;
			}
			return hiddeviceDescriptor;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000410A0 File Offset: 0x0003F2A0
		public static string UsagePageToString(HID.UsagePage usagePage)
		{
			if (usagePage < HID.UsagePage.VendorDefined)
			{
				return usagePage.ToString();
			}
			return "Vendor-Defined";
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000410C0 File Offset: 0x0003F2C0
		public static string UsageToString(HID.UsagePage usagePage, int usage)
		{
			if (usagePage == HID.UsagePage.GenericDesktop)
			{
				HID.GenericDesktop genericDesktop = (HID.GenericDesktop)usage;
				return genericDesktop.ToString();
			}
			if (usagePage != HID.UsagePage.Simulation)
			{
				return null;
			}
			HID.Simulation simulation = (HID.Simulation)usage;
			return simulation.ToString();
		}

		// Token: 0x04000474 RID: 1140
		internal const string kHIDInterface = "HID";

		// Token: 0x04000475 RID: 1141
		internal const string kHIDNamespace = "HID";

		// Token: 0x04000476 RID: 1142
		private bool m_HaveParsedHIDDescriptor;

		// Token: 0x04000477 RID: 1143
		private HID.HIDDeviceDescriptor m_HIDDescriptor;

		// Token: 0x04000478 RID: 1144
		private static readonly ProfilerMarker k_HIDParseDescriptorFallback = new ProfilerMarker("HIDParseDescriptorFallback");

		// Token: 0x020001E6 RID: 486
		[Serializable]
		private class HIDLayoutBuilder
		{
			// Token: 0x060014B7 RID: 5303 RVA: 0x00060178 File Offset: 0x0005E378
			public InputControlLayout Build()
			{
				InputControlLayout.Builder builder = new InputControlLayout.Builder
				{
					displayName = this.displayName,
					type = this.deviceType,
					extendsLayout = this.parentLayout,
					stateFormat = new FourCC('H', 'I', 'D', ' ')
				};
				HID.HIDElementDescriptor hidelementDescriptor = Array.Find<HID.HIDElementDescriptor>(this.hidDescriptor.elements, (HID.HIDElementDescriptor element) => element.usagePage == HID.UsagePage.GenericDesktop && element.usage == 48);
				HID.HIDElementDescriptor hidelementDescriptor2 = Array.Find<HID.HIDElementDescriptor>(this.hidDescriptor.elements, (HID.HIDElementDescriptor element) => element.usagePage == HID.UsagePage.GenericDesktop && element.usage == 49);
				bool flag = hidelementDescriptor.usage == 48 && hidelementDescriptor2.usage == 49;
				if (flag)
				{
					int num;
					int num2;
					int num3;
					if (hidelementDescriptor.reportOffsetInBits <= hidelementDescriptor2.reportOffsetInBits)
					{
						num = hidelementDescriptor.reportOffsetInBits % 8;
						num2 = hidelementDescriptor.reportOffsetInBits / 8;
						num3 = hidelementDescriptor2.reportOffsetInBits + hidelementDescriptor2.reportSizeInBits - hidelementDescriptor.reportOffsetInBits;
					}
					else
					{
						num = hidelementDescriptor2.reportOffsetInBits % 8;
						num2 = hidelementDescriptor2.reportOffsetInBits / 8;
						num3 = hidelementDescriptor.reportOffsetInBits + hidelementDescriptor.reportSizeInBits - hidelementDescriptor2.reportSizeInBits;
					}
					InputControlLayout.Builder.ControlBuilder controlBuilder = builder.AddControl("stick");
					controlBuilder = controlBuilder.WithDisplayName("Stick");
					controlBuilder = controlBuilder.WithLayout("Stick");
					controlBuilder = controlBuilder.WithBitOffset((uint)num);
					controlBuilder = controlBuilder.WithByteOffset((uint)num2);
					controlBuilder = controlBuilder.WithSizeInBits((uint)num3);
					controlBuilder.WithUsages(new InternedString[] { CommonUsages.Primary2DMotion });
					string text = hidelementDescriptor.DetermineParameters();
					string text2 = hidelementDescriptor2.DetermineParameters();
					controlBuilder = builder.AddControl("stick/x");
					controlBuilder = controlBuilder.WithFormat(hidelementDescriptor.isSigned ? InputStateBlock.FormatSBit : InputStateBlock.FormatBit);
					controlBuilder = controlBuilder.WithByteOffset((uint)(hidelementDescriptor.reportOffsetInBits / 8 - num2));
					controlBuilder = controlBuilder.WithBitOffset((uint)(hidelementDescriptor.reportOffsetInBits % 8));
					controlBuilder = controlBuilder.WithSizeInBits((uint)hidelementDescriptor.reportSizeInBits);
					controlBuilder = controlBuilder.WithParameters(text);
					controlBuilder = controlBuilder.WithDefaultState(hidelementDescriptor.DetermineDefaultState());
					controlBuilder.WithProcessors(hidelementDescriptor.DetermineProcessors());
					controlBuilder = builder.AddControl("stick/y");
					controlBuilder = controlBuilder.WithFormat(hidelementDescriptor2.isSigned ? InputStateBlock.FormatSBit : InputStateBlock.FormatBit);
					controlBuilder = controlBuilder.WithByteOffset((uint)(hidelementDescriptor2.reportOffsetInBits / 8 - num2));
					controlBuilder = controlBuilder.WithBitOffset((uint)(hidelementDescriptor2.reportOffsetInBits % 8));
					controlBuilder = controlBuilder.WithSizeInBits((uint)hidelementDescriptor2.reportSizeInBits);
					controlBuilder = controlBuilder.WithParameters(text2);
					controlBuilder = controlBuilder.WithDefaultState(hidelementDescriptor2.DetermineDefaultState());
					controlBuilder.WithProcessors(hidelementDescriptor2.DetermineProcessors());
					controlBuilder = builder.AddControl("stick/up");
					controlBuilder.WithParameters(StringHelpers.Join<string>(",", new string[] { text2, "clamp=2,clampMin=-1,clampMax=0,invert=true" }));
					controlBuilder = builder.AddControl("stick/down");
					controlBuilder.WithParameters(StringHelpers.Join<string>(",", new string[] { text2, "clamp=2,clampMin=0,clampMax=1,invert=false" }));
					controlBuilder = builder.AddControl("stick/left");
					controlBuilder.WithParameters(StringHelpers.Join<string>(",", new string[] { text, "clamp=2,clampMin=-1,clampMax=0,invert" }));
					controlBuilder = builder.AddControl("stick/right");
					controlBuilder.WithParameters(StringHelpers.Join<string>(",", new string[] { text, "clamp=2,clampMin=0,clampMax=1" }));
				}
				HID.HIDElementDescriptor[] elements = this.hidDescriptor.elements;
				int num4 = elements.Length;
				for (int i = 0; i < num4; i++)
				{
					ref HID.HIDElementDescriptor ptr = ref elements[i];
					if (ptr.reportType == HID.HIDReportType.Input && (!flag || (!ptr.Is(HID.UsagePage.GenericDesktop, 48) && !ptr.Is(HID.UsagePage.GenericDesktop, 49))))
					{
						string text3 = ptr.DetermineLayout();
						if (text3 != null)
						{
							string text4 = ptr.DetermineName();
							text4 = StringHelpers.MakeUniqueName<InputControlLayout.ControlItem>(text4, builder.controls, (InputControlLayout.ControlItem x) => x.name);
							InputControlLayout.Builder.ControlBuilder controlBuilder = builder.AddControl(text4);
							controlBuilder = controlBuilder.WithDisplayName(ptr.DetermineDisplayName());
							controlBuilder = controlBuilder.WithLayout(text3);
							controlBuilder = controlBuilder.WithByteOffset((uint)(ptr.reportOffsetInBits / 8));
							controlBuilder = controlBuilder.WithBitOffset((uint)(ptr.reportOffsetInBits % 8));
							controlBuilder = controlBuilder.WithSizeInBits((uint)ptr.reportSizeInBits);
							controlBuilder = controlBuilder.WithFormat(ptr.DetermineFormat());
							controlBuilder = controlBuilder.WithDefaultState(ptr.DetermineDefaultState());
							InputControlLayout.Builder.ControlBuilder controlBuilder2 = controlBuilder.WithProcessors(ptr.DetermineProcessors());
							string text5 = ptr.DetermineParameters();
							if (!string.IsNullOrEmpty(text5))
							{
								controlBuilder2.WithParameters(text5);
							}
							InternedString[] array = ptr.DetermineUsages();
							if (array != null)
							{
								controlBuilder2.WithUsages(array);
							}
							ptr.AddChildControls(ref ptr, text4, ref builder);
						}
					}
				}
				return builder.Build();
			}

			// Token: 0x04000A1D RID: 2589
			public string displayName;

			// Token: 0x04000A1E RID: 2590
			public HID.HIDDeviceDescriptor hidDescriptor;

			// Token: 0x04000A1F RID: 2591
			public string parentLayout;

			// Token: 0x04000A20 RID: 2592
			public Type deviceType;
		}

		// Token: 0x020001E7 RID: 487
		public enum HIDReportType
		{
			// Token: 0x04000A22 RID: 2594
			Unknown,
			// Token: 0x04000A23 RID: 2595
			Input,
			// Token: 0x04000A24 RID: 2596
			Output,
			// Token: 0x04000A25 RID: 2597
			Feature
		}

		// Token: 0x020001E8 RID: 488
		public enum HIDCollectionType
		{
			// Token: 0x04000A27 RID: 2599
			Physical,
			// Token: 0x04000A28 RID: 2600
			Application,
			// Token: 0x04000A29 RID: 2601
			Logical,
			// Token: 0x04000A2A RID: 2602
			Report,
			// Token: 0x04000A2B RID: 2603
			NamedArray,
			// Token: 0x04000A2C RID: 2604
			UsageSwitch,
			// Token: 0x04000A2D RID: 2605
			UsageModifier
		}

		// Token: 0x020001E9 RID: 489
		[Flags]
		public enum HIDElementFlags
		{
			// Token: 0x04000A2F RID: 2607
			Constant = 1,
			// Token: 0x04000A30 RID: 2608
			Variable = 2,
			// Token: 0x04000A31 RID: 2609
			Relative = 4,
			// Token: 0x04000A32 RID: 2610
			Wrap = 8,
			// Token: 0x04000A33 RID: 2611
			NonLinear = 16,
			// Token: 0x04000A34 RID: 2612
			NoPreferred = 32,
			// Token: 0x04000A35 RID: 2613
			NullState = 64,
			// Token: 0x04000A36 RID: 2614
			Volatile = 128,
			// Token: 0x04000A37 RID: 2615
			BufferedBytes = 256
		}

		// Token: 0x020001EA RID: 490
		[Serializable]
		public struct HIDElementDescriptor
		{
			// Token: 0x17000596 RID: 1430
			// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0006067A File Offset: 0x0005E87A
			public bool hasNullState
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.NullState) == HID.HIDElementFlags.NullState;
				}
			}

			// Token: 0x17000597 RID: 1431
			// (get) Token: 0x060014BA RID: 5306 RVA: 0x00060689 File Offset: 0x0005E889
			public bool hasPreferredState
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.NoPreferred) != HID.HIDElementFlags.NoPreferred;
				}
			}

			// Token: 0x17000598 RID: 1432
			// (get) Token: 0x060014BB RID: 5307 RVA: 0x0006069B File Offset: 0x0005E89B
			public bool isArray
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.Variable) != HID.HIDElementFlags.Variable;
				}
			}

			// Token: 0x17000599 RID: 1433
			// (get) Token: 0x060014BC RID: 5308 RVA: 0x000606AB File Offset: 0x0005E8AB
			public bool isNonLinear
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.NonLinear) == HID.HIDElementFlags.NonLinear;
				}
			}

			// Token: 0x1700059A RID: 1434
			// (get) Token: 0x060014BD RID: 5309 RVA: 0x000606BA File Offset: 0x0005E8BA
			public bool isRelative
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.Relative) == HID.HIDElementFlags.Relative;
				}
			}

			// Token: 0x1700059B RID: 1435
			// (get) Token: 0x060014BE RID: 5310 RVA: 0x000606C7 File Offset: 0x0005E8C7
			public bool isConstant
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.Constant) == HID.HIDElementFlags.Constant;
				}
			}

			// Token: 0x1700059C RID: 1436
			// (get) Token: 0x060014BF RID: 5311 RVA: 0x000606D4 File Offset: 0x0005E8D4
			public bool isWrapping
			{
				get
				{
					return (this.flags & HID.HIDElementFlags.Wrap) == HID.HIDElementFlags.Wrap;
				}
			}

			// Token: 0x1700059D RID: 1437
			// (get) Token: 0x060014C0 RID: 5312 RVA: 0x000606E1 File Offset: 0x0005E8E1
			internal bool isSigned
			{
				get
				{
					return this.logicalMin < 0;
				}
			}

			// Token: 0x1700059E RID: 1438
			// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000606EC File Offset: 0x0005E8EC
			internal float minFloatValue
			{
				get
				{
					if (this.isSigned)
					{
						int num = (int)(-(int)((int)1L << this.reportSizeInBits - 1));
						int num2 = (int)((1L << this.reportSizeInBits - 1) - 1L);
						return NumberHelpers.IntToNormalizedFloat(this.logicalMin, num, num2) * 2f - 1f;
					}
					uint num3 = (uint)((1L << this.reportSizeInBits) - 1L);
					return NumberHelpers.UIntToNormalizedFloat((uint)this.logicalMin, 0U, num3);
				}
			}

			// Token: 0x1700059F RID: 1439
			// (get) Token: 0x060014C2 RID: 5314 RVA: 0x00060760 File Offset: 0x0005E960
			internal float maxFloatValue
			{
				get
				{
					if (this.isSigned)
					{
						int num = (int)(-(int)((int)1L << this.reportSizeInBits - 1));
						int num2 = (int)((1L << this.reportSizeInBits - 1) - 1L);
						return NumberHelpers.IntToNormalizedFloat(this.logicalMax, num, num2) * 2f - 1f;
					}
					uint num3 = (uint)((1L << this.reportSizeInBits) - 1L);
					return NumberHelpers.UIntToNormalizedFloat((uint)this.logicalMax, 0U, num3);
				}
			}

			// Token: 0x060014C3 RID: 5315 RVA: 0x000607D1 File Offset: 0x0005E9D1
			public bool Is(HID.UsagePage usagePage, int usage)
			{
				return usagePage == this.usagePage && usage == this.usage;
			}

			// Token: 0x060014C4 RID: 5316 RVA: 0x000607E8 File Offset: 0x0005E9E8
			internal string DetermineName()
			{
				HID.UsagePage usagePage = this.usagePage;
				if (usagePage != HID.UsagePage.GenericDesktop)
				{
					if (usagePage != HID.UsagePage.Button)
					{
						return string.Format("UsagePage({0:X}) Usage({1:X})", this.usagePage, this.usage);
					}
					if (this.usage == 1)
					{
						return "trigger";
					}
					return string.Format("button{0}", this.usage);
				}
				else
				{
					if (this.usage == 57)
					{
						return "hat";
					}
					HID.GenericDesktop genericDesktop = (HID.GenericDesktop)this.usage;
					string text = genericDesktop.ToString();
					return char.ToLowerInvariant(text[0]).ToString() + text.Substring(1);
				}
			}

			// Token: 0x060014C5 RID: 5317 RVA: 0x00060894 File Offset: 0x0005EA94
			internal string DetermineDisplayName()
			{
				HID.UsagePage usagePage = this.usagePage;
				if (usagePage == HID.UsagePage.GenericDesktop)
				{
					HID.GenericDesktop genericDesktop = (HID.GenericDesktop)this.usage;
					return genericDesktop.ToString();
				}
				if (usagePage != HID.UsagePage.Button)
				{
					return null;
				}
				if (this.usage == 1)
				{
					return "Trigger";
				}
				return string.Format("Button {0}", this.usage);
			}

			// Token: 0x060014C6 RID: 5318 RVA: 0x000608EC File Offset: 0x0005EAEC
			internal bool IsUsableElement()
			{
				int num = this.usage;
				if (num - 48 <= 1)
				{
					return this.usagePage == HID.UsagePage.GenericDesktop;
				}
				return this.DetermineLayout() != null;
			}

			// Token: 0x060014C7 RID: 5319 RVA: 0x0006091C File Offset: 0x0005EB1C
			internal string DetermineLayout()
			{
				if (this.reportType != HID.HIDReportType.Input)
				{
					return null;
				}
				HID.UsagePage usagePage = this.usagePage;
				if (usagePage == HID.UsagePage.GenericDesktop)
				{
					int num = this.usage;
					switch (num)
					{
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 64:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
						return "Axis";
					case 57:
						if (this.logicalMax - this.logicalMin + 1 == 8)
						{
							return "Dpad";
						}
						goto IL_00BC;
					case 58:
					case 59:
					case 60:
					case 63:
						goto IL_00BC;
					case 61:
					case 62:
						break;
					default:
						if (num - 144 > 3)
						{
							goto IL_00BC;
						}
						break;
					}
					return "Button";
				}
				if (usagePage == HID.UsagePage.Button)
				{
					return "Button";
				}
				IL_00BC:
				return null;
			}

			// Token: 0x060014C8 RID: 5320 RVA: 0x000609E8 File Offset: 0x0005EBE8
			internal FourCC DetermineFormat()
			{
				int num = this.reportSizeInBits;
				if (num != 8)
				{
					if (num != 16)
					{
						if (num != 32)
						{
							return InputStateBlock.FormatBit;
						}
						if (!this.isSigned)
						{
							return InputStateBlock.FormatUInt;
						}
						return InputStateBlock.FormatInt;
					}
					else
					{
						if (!this.isSigned)
						{
							return InputStateBlock.FormatUShort;
						}
						return InputStateBlock.FormatShort;
					}
				}
				else
				{
					if (!this.isSigned)
					{
						return InputStateBlock.FormatByte;
					}
					return InputStateBlock.FormatSByte;
				}
			}

			// Token: 0x060014C9 RID: 5321 RVA: 0x00060A50 File Offset: 0x0005EC50
			internal InternedString[] DetermineUsages()
			{
				if (this.usagePage == HID.UsagePage.Button && this.usage == 1)
				{
					return new InternedString[]
					{
						CommonUsages.PrimaryTrigger,
						CommonUsages.PrimaryAction
					};
				}
				if (this.usagePage == HID.UsagePage.Button && this.usage == 2)
				{
					return new InternedString[]
					{
						CommonUsages.SecondaryTrigger,
						CommonUsages.SecondaryAction
					};
				}
				if (this.usagePage == HID.UsagePage.GenericDesktop && this.usage == 53)
				{
					return new InternedString[] { CommonUsages.Twist };
				}
				return null;
			}

			// Token: 0x060014CA RID: 5322 RVA: 0x00060AE8 File Offset: 0x0005ECE8
			internal string DetermineParameters()
			{
				if (this.usagePage == HID.UsagePage.GenericDesktop)
				{
					switch (this.usage)
					{
					case 48:
					case 50:
					case 51:
					case 53:
					case 54:
					case 55:
					case 56:
					case 64:
					case 66:
					case 67:
					case 69:
						return this.DetermineAxisNormalizationParameters();
					case 49:
					case 52:
					case 65:
					case 68:
						return StringHelpers.Join<string>(",", new string[]
						{
							"invert",
							this.DetermineAxisNormalizationParameters()
						});
					}
				}
				return null;
			}

			// Token: 0x060014CB RID: 5323 RVA: 0x00060B98 File Offset: 0x0005ED98
			private string DetermineAxisNormalizationParameters()
			{
				if (this.logicalMin == 0 && this.logicalMax == 0)
				{
					return "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5";
				}
				float minFloatValue = this.minFloatValue;
				float maxFloatValue = this.maxFloatValue;
				if (Mathf.Approximately(0f, minFloatValue) && Mathf.Approximately(0f, maxFloatValue))
				{
					return null;
				}
				float num = minFloatValue + (maxFloatValue - minFloatValue) / 2f;
				return string.Format(CultureInfo.InvariantCulture, "normalize,normalizeMin={0},normalizeMax={1},normalizeZero={2}", minFloatValue, maxFloatValue, num);
			}

			// Token: 0x060014CC RID: 5324 RVA: 0x00060C14 File Offset: 0x0005EE14
			internal string DetermineProcessors()
			{
				if (this.usagePage == HID.UsagePage.GenericDesktop)
				{
					int num = this.usage;
					if (num - 48 <= 8 || num - 64 <= 5)
					{
						return "axisDeadzone";
					}
				}
				return null;
			}

			// Token: 0x060014CD RID: 5325 RVA: 0x00060C48 File Offset: 0x0005EE48
			internal PrimitiveValue DetermineDefaultState()
			{
				if (this.usagePage == HID.UsagePage.GenericDesktop)
				{
					switch (this.usage)
					{
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					case 53:
					case 54:
					case 55:
					case 56:
					case 64:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
						if (!this.isSigned)
						{
							int num = this.logicalMin + (this.logicalMax - this.logicalMin) / 2;
							if (num != 0)
							{
								return new PrimitiveValue(num);
							}
						}
						break;
					case 57:
						if (this.hasNullState)
						{
							if (this.logicalMin >= 1)
							{
								return new PrimitiveValue(this.logicalMin - 1);
							}
							ulong num2 = (1UL << this.reportSizeInBits) - 1UL;
							if ((long)this.logicalMax < (long)num2)
							{
								return new PrimitiveValue(this.logicalMax + 1);
							}
						}
						break;
					}
				}
				return default(PrimitiveValue);
			}

			// Token: 0x060014CE RID: 5326 RVA: 0x00060D44 File Offset: 0x0005EF44
			internal void AddChildControls(ref HID.HIDElementDescriptor element, string controlName, ref InputControlLayout.Builder builder)
			{
				if (this.usagePage == HID.UsagePage.GenericDesktop && this.usage == 57)
				{
					PrimitiveValue primitiveValue = this.DetermineDefaultState();
					if (primitiveValue.isEmpty)
					{
						return;
					}
					builder.AddControl(controlName + "/up").WithFormat(InputStateBlock.FormatBit).WithLayout("DiscreteButton")
						.WithParameters(string.Format(CultureInfo.InvariantCulture, "minValue={0},maxValue={1},nullValue={2},wrapAtValue={3}", new object[]
						{
							this.logicalMax,
							this.logicalMin + 1,
							primitiveValue.ToString(),
							this.logicalMax
						}))
						.WithBitOffset((uint)(element.reportOffsetInBits % 8))
						.WithSizeInBits((uint)this.reportSizeInBits);
					builder.AddControl(controlName + "/right").WithFormat(InputStateBlock.FormatBit).WithLayout("DiscreteButton")
						.WithParameters(string.Format(CultureInfo.InvariantCulture, "minValue={0},maxValue={1}", this.logicalMin + 1, this.logicalMin + 3))
						.WithBitOffset((uint)(element.reportOffsetInBits % 8))
						.WithSizeInBits((uint)this.reportSizeInBits);
					builder.AddControl(controlName + "/down").WithFormat(InputStateBlock.FormatBit).WithLayout("DiscreteButton")
						.WithParameters(string.Format(CultureInfo.InvariantCulture, "minValue={0},maxValue={1}", this.logicalMin + 3, this.logicalMin + 5))
						.WithBitOffset((uint)(element.reportOffsetInBits % 8))
						.WithSizeInBits((uint)this.reportSizeInBits);
					builder.AddControl(controlName + "/left").WithFormat(InputStateBlock.FormatBit).WithLayout("DiscreteButton")
						.WithParameters(string.Format(CultureInfo.InvariantCulture, "minValue={0},maxValue={1}", this.logicalMin + 5, this.logicalMin + 7))
						.WithBitOffset((uint)(element.reportOffsetInBits % 8))
						.WithSizeInBits((uint)this.reportSizeInBits);
				}
			}

			// Token: 0x04000A38 RID: 2616
			public int usage;

			// Token: 0x04000A39 RID: 2617
			public HID.UsagePage usagePage;

			// Token: 0x04000A3A RID: 2618
			public int unit;

			// Token: 0x04000A3B RID: 2619
			public int unitExponent;

			// Token: 0x04000A3C RID: 2620
			public int logicalMin;

			// Token: 0x04000A3D RID: 2621
			public int logicalMax;

			// Token: 0x04000A3E RID: 2622
			public int physicalMin;

			// Token: 0x04000A3F RID: 2623
			public int physicalMax;

			// Token: 0x04000A40 RID: 2624
			public HID.HIDReportType reportType;

			// Token: 0x04000A41 RID: 2625
			public int collectionIndex;

			// Token: 0x04000A42 RID: 2626
			public int reportId;

			// Token: 0x04000A43 RID: 2627
			public int reportSizeInBits;

			// Token: 0x04000A44 RID: 2628
			public int reportOffsetInBits;

			// Token: 0x04000A45 RID: 2629
			public HID.HIDElementFlags flags;

			// Token: 0x04000A46 RID: 2630
			public int? usageMin;

			// Token: 0x04000A47 RID: 2631
			public int? usageMax;
		}

		// Token: 0x020001EB RID: 491
		[Serializable]
		public struct HIDCollectionDescriptor
		{
			// Token: 0x04000A48 RID: 2632
			public HID.HIDCollectionType type;

			// Token: 0x04000A49 RID: 2633
			public int usage;

			// Token: 0x04000A4A RID: 2634
			public HID.UsagePage usagePage;

			// Token: 0x04000A4B RID: 2635
			public int parent;

			// Token: 0x04000A4C RID: 2636
			public int childCount;

			// Token: 0x04000A4D RID: 2637
			public int firstChild;
		}

		// Token: 0x020001EC RID: 492
		[Serializable]
		public struct HIDDeviceDescriptor
		{
			// Token: 0x060014CF RID: 5327 RVA: 0x00060F93 File Offset: 0x0005F193
			public string ToJson()
			{
				return JsonUtility.ToJson(this, true);
			}

			// Token: 0x060014D0 RID: 5328 RVA: 0x00060FA8 File Offset: 0x0005F1A8
			public static HID.HIDDeviceDescriptor FromJson(string json)
			{
				HID.HIDDeviceDescriptor hiddeviceDescriptor2;
				try
				{
					HID.HIDDeviceDescriptor hiddeviceDescriptor = default(HID.HIDDeviceDescriptor);
					ReadOnlySpan<char> readOnlySpan = json.AsSpan();
					PredictiveParser predictiveParser = default(PredictiveParser);
					predictiveParser.ExpectSingleChar(readOnlySpan, '{');
					ReadOnlySpan<char> readOnlySpan2;
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.vendorId = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.productId = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.usage = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.usagePage = (HID.UsagePage)predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.inputReportSize = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.outputReportSize = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan2);
					predictiveParser.ExpectSingleChar(readOnlySpan, ':');
					hiddeviceDescriptor.featureReportSize = predictiveParser.ExpectInt(readOnlySpan);
					predictiveParser.AcceptSingleChar(readOnlySpan, ',');
					ReadOnlySpan<char> readOnlySpan3;
					predictiveParser.AcceptString(readOnlySpan, out readOnlySpan3);
					if (readOnlySpan3.ToString() != "elements")
					{
						hiddeviceDescriptor2 = hiddeviceDescriptor;
					}
					else
					{
						predictiveParser.ExpectSingleChar(readOnlySpan, ':');
						predictiveParser.ExpectSingleChar(readOnlySpan, '[');
						List<HID.HIDElementDescriptor> list;
						using (CollectionPool<List<HID.HIDElementDescriptor>, HID.HIDElementDescriptor>.Get(out list))
						{
							while (!predictiveParser.AcceptSingleChar(readOnlySpan, ']'))
							{
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectSingleChar(readOnlySpan, '{');
								HID.HIDElementDescriptor hidelementDescriptor = default(HID.HIDElementDescriptor);
								predictiveParser.AcceptSingleChar(readOnlySpan, '}');
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.usage = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.usagePage = (HID.UsagePage)predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.unit = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.unitExponent = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.logicalMin = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.logicalMax = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.physicalMin = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.physicalMax = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.collectionIndex = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.reportType = (HID.HIDReportType)predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.reportId = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								predictiveParser.AcceptInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.reportSizeInBits = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.reportOffsetInBits = predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.AcceptSingleChar(readOnlySpan, ',');
								predictiveParser.ExpectString(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, ':');
								hidelementDescriptor.flags = (HID.HIDElementFlags)predictiveParser.ExpectInt(readOnlySpan);
								predictiveParser.ExpectSingleChar(readOnlySpan, '}');
								list.Add(hidelementDescriptor);
							}
							hiddeviceDescriptor.elements = list.ToArray();
							hiddeviceDescriptor2 = hiddeviceDescriptor;
						}
					}
				}
				catch (Exception)
				{
					hiddeviceDescriptor2 = JsonUtility.FromJson<HID.HIDDeviceDescriptor>(json);
				}
				return hiddeviceDescriptor2;
			}

			// Token: 0x04000A4E RID: 2638
			public int vendorId;

			// Token: 0x04000A4F RID: 2639
			public int productId;

			// Token: 0x04000A50 RID: 2640
			public int usage;

			// Token: 0x04000A51 RID: 2641
			public HID.UsagePage usagePage;

			// Token: 0x04000A52 RID: 2642
			public int inputReportSize;

			// Token: 0x04000A53 RID: 2643
			public int outputReportSize;

			// Token: 0x04000A54 RID: 2644
			public int featureReportSize;

			// Token: 0x04000A55 RID: 2645
			public HID.HIDElementDescriptor[] elements;

			// Token: 0x04000A56 RID: 2646
			public HID.HIDCollectionDescriptor[] collections;
		}

		// Token: 0x020001ED RID: 493
		public struct HIDDeviceDescriptorBuilder
		{
			// Token: 0x060014D1 RID: 5329 RVA: 0x000614B8 File Offset: 0x0005F6B8
			public HIDDeviceDescriptorBuilder(HID.UsagePage usagePage, int usage)
			{
				this = default(HID.HIDDeviceDescriptorBuilder);
				this.usagePage = usagePage;
				this.usage = usage;
			}

			// Token: 0x060014D2 RID: 5330 RVA: 0x000614CF File Offset: 0x0005F6CF
			public HIDDeviceDescriptorBuilder(HID.GenericDesktop usage)
			{
				this = new HID.HIDDeviceDescriptorBuilder(HID.UsagePage.GenericDesktop, (int)usage);
			}

			// Token: 0x060014D3 RID: 5331 RVA: 0x000614D9 File Offset: 0x0005F6D9
			public HID.HIDDeviceDescriptorBuilder StartReport(HID.HIDReportType reportType, int reportId = 1)
			{
				this.m_CurrentReportId = reportId;
				this.m_CurrentReportType = reportType;
				this.m_CurrentReportOffsetInBits = 8;
				return this;
			}

			// Token: 0x060014D4 RID: 5332 RVA: 0x000614F8 File Offset: 0x0005F6F8
			public HID.HIDDeviceDescriptorBuilder AddElement(HID.UsagePage usagePage, int usage, int sizeInBits)
			{
				if (this.m_Elements == null)
				{
					this.m_Elements = new List<HID.HIDElementDescriptor>();
				}
				else
				{
					foreach (HID.HIDElementDescriptor hidelementDescriptor in this.m_Elements)
					{
						if (hidelementDescriptor.reportId == this.m_CurrentReportId && hidelementDescriptor.reportType == this.m_CurrentReportType && hidelementDescriptor.usagePage == usagePage && hidelementDescriptor.usage == usage)
						{
							throw new InvalidOperationException(string.Format("Cannot add two elements with the same usage page '{0}' and usage '0x{1:X} the to same device", usagePage, usage));
						}
					}
				}
				this.m_Elements.Add(new HID.HIDElementDescriptor
				{
					usage = usage,
					usagePage = usagePage,
					reportOffsetInBits = this.m_CurrentReportOffsetInBits,
					reportSizeInBits = sizeInBits,
					reportType = this.m_CurrentReportType,
					reportId = this.m_CurrentReportId
				});
				this.m_CurrentReportOffsetInBits += sizeInBits;
				return this;
			}

			// Token: 0x060014D5 RID: 5333 RVA: 0x0006160C File Offset: 0x0005F80C
			public HID.HIDDeviceDescriptorBuilder AddElement(HID.GenericDesktop usage, int sizeInBits)
			{
				return this.AddElement(HID.UsagePage.GenericDesktop, (int)usage, sizeInBits);
			}

			// Token: 0x060014D6 RID: 5334 RVA: 0x00061618 File Offset: 0x0005F818
			public HID.HIDDeviceDescriptorBuilder WithPhysicalMinMax(int min, int max)
			{
				int num = this.m_Elements.Count - 1;
				if (num < 0)
				{
					throw new InvalidOperationException("No element has been added to the descriptor yet");
				}
				HID.HIDElementDescriptor hidelementDescriptor = this.m_Elements[num];
				hidelementDescriptor.physicalMin = min;
				hidelementDescriptor.physicalMax = max;
				this.m_Elements[num] = hidelementDescriptor;
				return this;
			}

			// Token: 0x060014D7 RID: 5335 RVA: 0x00061674 File Offset: 0x0005F874
			public HID.HIDDeviceDescriptorBuilder WithLogicalMinMax(int min, int max)
			{
				int num = this.m_Elements.Count - 1;
				if (num < 0)
				{
					throw new InvalidOperationException("No element has been added to the descriptor yet");
				}
				HID.HIDElementDescriptor hidelementDescriptor = this.m_Elements[num];
				hidelementDescriptor.logicalMin = min;
				hidelementDescriptor.logicalMax = max;
				this.m_Elements[num] = hidelementDescriptor;
				return this;
			}

			// Token: 0x060014D8 RID: 5336 RVA: 0x000616D0 File Offset: 0x0005F8D0
			public HID.HIDDeviceDescriptor Finish()
			{
				HID.HIDDeviceDescriptor hiddeviceDescriptor = default(HID.HIDDeviceDescriptor);
				hiddeviceDescriptor.usage = this.usage;
				hiddeviceDescriptor.usagePage = this.usagePage;
				List<HID.HIDElementDescriptor> elements = this.m_Elements;
				hiddeviceDescriptor.elements = ((elements != null) ? elements.ToArray() : null);
				List<HID.HIDCollectionDescriptor> collections = this.m_Collections;
				hiddeviceDescriptor.collections = ((collections != null) ? collections.ToArray() : null);
				return hiddeviceDescriptor;
			}

			// Token: 0x04000A57 RID: 2647
			public HID.UsagePage usagePage;

			// Token: 0x04000A58 RID: 2648
			public int usage;

			// Token: 0x04000A59 RID: 2649
			private int m_CurrentReportId;

			// Token: 0x04000A5A RID: 2650
			private HID.HIDReportType m_CurrentReportType;

			// Token: 0x04000A5B RID: 2651
			private int m_CurrentReportOffsetInBits;

			// Token: 0x04000A5C RID: 2652
			private List<HID.HIDElementDescriptor> m_Elements;

			// Token: 0x04000A5D RID: 2653
			private List<HID.HIDCollectionDescriptor> m_Collections;

			// Token: 0x04000A5E RID: 2654
			private int m_InputReportSize;

			// Token: 0x04000A5F RID: 2655
			private int m_OutputReportSize;

			// Token: 0x04000A60 RID: 2656
			private int m_FeatureReportSize;
		}

		// Token: 0x020001EE RID: 494
		public enum UsagePage
		{
			// Token: 0x04000A62 RID: 2658
			Undefined,
			// Token: 0x04000A63 RID: 2659
			GenericDesktop,
			// Token: 0x04000A64 RID: 2660
			Simulation,
			// Token: 0x04000A65 RID: 2661
			VRControls,
			// Token: 0x04000A66 RID: 2662
			SportControls,
			// Token: 0x04000A67 RID: 2663
			GameControls,
			// Token: 0x04000A68 RID: 2664
			GenericDeviceControls,
			// Token: 0x04000A69 RID: 2665
			Keyboard,
			// Token: 0x04000A6A RID: 2666
			LEDs,
			// Token: 0x04000A6B RID: 2667
			Button,
			// Token: 0x04000A6C RID: 2668
			Ordinal,
			// Token: 0x04000A6D RID: 2669
			Telephony,
			// Token: 0x04000A6E RID: 2670
			Consumer,
			// Token: 0x04000A6F RID: 2671
			Digitizer,
			// Token: 0x04000A70 RID: 2672
			PID = 15,
			// Token: 0x04000A71 RID: 2673
			Unicode,
			// Token: 0x04000A72 RID: 2674
			AlphanumericDisplay = 20,
			// Token: 0x04000A73 RID: 2675
			MedicalInstruments = 64,
			// Token: 0x04000A74 RID: 2676
			Monitor = 128,
			// Token: 0x04000A75 RID: 2677
			Power = 132,
			// Token: 0x04000A76 RID: 2678
			BarCodeScanner = 140,
			// Token: 0x04000A77 RID: 2679
			MagneticStripeReader = 142,
			// Token: 0x04000A78 RID: 2680
			Camera = 144,
			// Token: 0x04000A79 RID: 2681
			Arcade,
			// Token: 0x04000A7A RID: 2682
			VendorDefined = 65280
		}

		// Token: 0x020001EF RID: 495
		public enum GenericDesktop
		{
			// Token: 0x04000A7C RID: 2684
			Undefined,
			// Token: 0x04000A7D RID: 2685
			Pointer,
			// Token: 0x04000A7E RID: 2686
			Mouse,
			// Token: 0x04000A7F RID: 2687
			Joystick = 4,
			// Token: 0x04000A80 RID: 2688
			Gamepad,
			// Token: 0x04000A81 RID: 2689
			Keyboard,
			// Token: 0x04000A82 RID: 2690
			Keypad,
			// Token: 0x04000A83 RID: 2691
			MultiAxisController,
			// Token: 0x04000A84 RID: 2692
			TabletPCControls,
			// Token: 0x04000A85 RID: 2693
			AssistiveControl,
			// Token: 0x04000A86 RID: 2694
			X = 48,
			// Token: 0x04000A87 RID: 2695
			Y,
			// Token: 0x04000A88 RID: 2696
			Z,
			// Token: 0x04000A89 RID: 2697
			Rx,
			// Token: 0x04000A8A RID: 2698
			Ry,
			// Token: 0x04000A8B RID: 2699
			Rz,
			// Token: 0x04000A8C RID: 2700
			Slider,
			// Token: 0x04000A8D RID: 2701
			Dial,
			// Token: 0x04000A8E RID: 2702
			Wheel,
			// Token: 0x04000A8F RID: 2703
			HatSwitch,
			// Token: 0x04000A90 RID: 2704
			CountedBuffer,
			// Token: 0x04000A91 RID: 2705
			ByteCount,
			// Token: 0x04000A92 RID: 2706
			MotionWakeup,
			// Token: 0x04000A93 RID: 2707
			Start,
			// Token: 0x04000A94 RID: 2708
			Select,
			// Token: 0x04000A95 RID: 2709
			Vx = 64,
			// Token: 0x04000A96 RID: 2710
			Vy,
			// Token: 0x04000A97 RID: 2711
			Vz,
			// Token: 0x04000A98 RID: 2712
			Vbrx,
			// Token: 0x04000A99 RID: 2713
			Vbry,
			// Token: 0x04000A9A RID: 2714
			Vbrz,
			// Token: 0x04000A9B RID: 2715
			Vno,
			// Token: 0x04000A9C RID: 2716
			FeatureNotification,
			// Token: 0x04000A9D RID: 2717
			ResolutionMultiplier,
			// Token: 0x04000A9E RID: 2718
			SystemControl = 128,
			// Token: 0x04000A9F RID: 2719
			SystemPowerDown,
			// Token: 0x04000AA0 RID: 2720
			SystemSleep,
			// Token: 0x04000AA1 RID: 2721
			SystemWakeUp,
			// Token: 0x04000AA2 RID: 2722
			SystemContextMenu,
			// Token: 0x04000AA3 RID: 2723
			SystemMainMenu,
			// Token: 0x04000AA4 RID: 2724
			SystemAppMenu,
			// Token: 0x04000AA5 RID: 2725
			SystemMenuHelp,
			// Token: 0x04000AA6 RID: 2726
			SystemMenuExit,
			// Token: 0x04000AA7 RID: 2727
			SystemMenuSelect,
			// Token: 0x04000AA8 RID: 2728
			SystemMenuRight,
			// Token: 0x04000AA9 RID: 2729
			SystemMenuLeft,
			// Token: 0x04000AAA RID: 2730
			SystemMenuUp,
			// Token: 0x04000AAB RID: 2731
			SystemMenuDown,
			// Token: 0x04000AAC RID: 2732
			SystemColdRestart,
			// Token: 0x04000AAD RID: 2733
			SystemWarmRestart,
			// Token: 0x04000AAE RID: 2734
			DpadUp,
			// Token: 0x04000AAF RID: 2735
			DpadDown,
			// Token: 0x04000AB0 RID: 2736
			DpadRight,
			// Token: 0x04000AB1 RID: 2737
			DpadLeft,
			// Token: 0x04000AB2 RID: 2738
			SystemDock = 160,
			// Token: 0x04000AB3 RID: 2739
			SystemUndock,
			// Token: 0x04000AB4 RID: 2740
			SystemSetup,
			// Token: 0x04000AB5 RID: 2741
			SystemBreak,
			// Token: 0x04000AB6 RID: 2742
			SystemDebuggerBreak,
			// Token: 0x04000AB7 RID: 2743
			ApplicationBreak,
			// Token: 0x04000AB8 RID: 2744
			ApplicationDebuggerBreak,
			// Token: 0x04000AB9 RID: 2745
			SystemSpeakerMute,
			// Token: 0x04000ABA RID: 2746
			SystemHibernate,
			// Token: 0x04000ABB RID: 2747
			SystemDisplayInvert = 176,
			// Token: 0x04000ABC RID: 2748
			SystemDisplayInternal,
			// Token: 0x04000ABD RID: 2749
			SystemDisplayExternal,
			// Token: 0x04000ABE RID: 2750
			SystemDisplayBoth,
			// Token: 0x04000ABF RID: 2751
			SystemDisplayDual,
			// Token: 0x04000AC0 RID: 2752
			SystemDisplayToggleIntExt,
			// Token: 0x04000AC1 RID: 2753
			SystemDisplaySwapPrimarySecondary,
			// Token: 0x04000AC2 RID: 2754
			SystemDisplayLCDAutoScale
		}

		// Token: 0x020001F0 RID: 496
		public enum Simulation
		{
			// Token: 0x04000AC4 RID: 2756
			Undefined,
			// Token: 0x04000AC5 RID: 2757
			FlightSimulationDevice,
			// Token: 0x04000AC6 RID: 2758
			AutomobileSimulationDevice,
			// Token: 0x04000AC7 RID: 2759
			TankSimulationDevice,
			// Token: 0x04000AC8 RID: 2760
			SpaceshipSimulationDevice,
			// Token: 0x04000AC9 RID: 2761
			SubmarineSimulationDevice,
			// Token: 0x04000ACA RID: 2762
			SailingSimulationDevice,
			// Token: 0x04000ACB RID: 2763
			MotorcycleSimulationDevice,
			// Token: 0x04000ACC RID: 2764
			SportsSimulationDevice,
			// Token: 0x04000ACD RID: 2765
			AirplaneSimulationDevice,
			// Token: 0x04000ACE RID: 2766
			HelicopterSimulationDevice,
			// Token: 0x04000ACF RID: 2767
			MagicCarpetSimulationDevice,
			// Token: 0x04000AD0 RID: 2768
			BicylcleSimulationDevice,
			// Token: 0x04000AD1 RID: 2769
			FlightControlStick = 32,
			// Token: 0x04000AD2 RID: 2770
			FlightStick,
			// Token: 0x04000AD3 RID: 2771
			CyclicControl,
			// Token: 0x04000AD4 RID: 2772
			CyclicTrim,
			// Token: 0x04000AD5 RID: 2773
			FlightYoke,
			// Token: 0x04000AD6 RID: 2774
			TrackControl,
			// Token: 0x04000AD7 RID: 2775
			Aileron = 176,
			// Token: 0x04000AD8 RID: 2776
			AileronTrim,
			// Token: 0x04000AD9 RID: 2777
			AntiTorqueControl,
			// Token: 0x04000ADA RID: 2778
			AutopilotEnable,
			// Token: 0x04000ADB RID: 2779
			ChaffRelease,
			// Token: 0x04000ADC RID: 2780
			CollectiveControl,
			// Token: 0x04000ADD RID: 2781
			DiveBreak,
			// Token: 0x04000ADE RID: 2782
			ElectronicCountermeasures,
			// Token: 0x04000ADF RID: 2783
			Elevator,
			// Token: 0x04000AE0 RID: 2784
			ElevatorTrim,
			// Token: 0x04000AE1 RID: 2785
			Rudder,
			// Token: 0x04000AE2 RID: 2786
			Throttle,
			// Token: 0x04000AE3 RID: 2787
			FlightCommunications,
			// Token: 0x04000AE4 RID: 2788
			FlareRelease,
			// Token: 0x04000AE5 RID: 2789
			LandingGear,
			// Token: 0x04000AE6 RID: 2790
			ToeBreak,
			// Token: 0x04000AE7 RID: 2791
			Trigger,
			// Token: 0x04000AE8 RID: 2792
			WeaponsArm,
			// Token: 0x04000AE9 RID: 2793
			WeaponsSelect,
			// Token: 0x04000AEA RID: 2794
			WingFlaps,
			// Token: 0x04000AEB RID: 2795
			Accelerator,
			// Token: 0x04000AEC RID: 2796
			Brake,
			// Token: 0x04000AED RID: 2797
			Clutch,
			// Token: 0x04000AEE RID: 2798
			Shifter,
			// Token: 0x04000AEF RID: 2799
			Steering,
			// Token: 0x04000AF0 RID: 2800
			TurretDirection,
			// Token: 0x04000AF1 RID: 2801
			BarrelElevation,
			// Token: 0x04000AF2 RID: 2802
			DivePlane,
			// Token: 0x04000AF3 RID: 2803
			Ballast,
			// Token: 0x04000AF4 RID: 2804
			BicycleCrank,
			// Token: 0x04000AF5 RID: 2805
			HandleBars,
			// Token: 0x04000AF6 RID: 2806
			FrontBrake,
			// Token: 0x04000AF7 RID: 2807
			RearBrake
		}

		// Token: 0x020001F1 RID: 497
		public enum Button
		{
			// Token: 0x04000AF9 RID: 2809
			Undefined,
			// Token: 0x04000AFA RID: 2810
			Primary,
			// Token: 0x04000AFB RID: 2811
			Secondary,
			// Token: 0x04000AFC RID: 2812
			Tertiary
		}
	}
}
