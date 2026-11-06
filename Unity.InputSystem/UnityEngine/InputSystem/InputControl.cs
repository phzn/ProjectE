using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200002F RID: 47
	[DebuggerDisplay("{DebuggerDisplay(),nq}")]
	public abstract class InputControl
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0001018A File Offset: 0x0000E38A
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00010197 File Offset: 0x0000E397
		// (set) Token: 0x0600039D RID: 925 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public string displayName
		{
			get
			{
				this.RefreshConfigurationIfNeeded();
				if (this.m_DisplayName != null)
				{
					return this.m_DisplayName;
				}
				if (this.m_DisplayNameFromLayout != null)
				{
					return this.m_DisplayNameFromLayout;
				}
				return this.m_Name;
			}
			protected set
			{
				this.m_DisplayName = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000101D1 File Offset: 0x0000E3D1
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000101F8 File Offset: 0x0000E3F8
		public string shortDisplayName
		{
			get
			{
				this.RefreshConfigurationIfNeeded();
				if (this.m_ShortDisplayName != null)
				{
					return this.m_ShortDisplayName;
				}
				if (this.m_ShortDisplayNameFromLayout != null)
				{
					return this.m_ShortDisplayNameFromLayout;
				}
				return null;
			}
			protected set
			{
				this.m_ShortDisplayName = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00010201 File Offset: 0x0000E401
		public string path
		{
			get
			{
				if (this.m_Path == null)
				{
					this.m_Path = InputControlPath.Combine(this.m_Parent, this.m_Name);
				}
				return this.m_Path;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0001022D File Offset: 0x0000E42D
		public string layout
		{
			get
			{
				return this.m_Layout;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0001023A File Offset: 0x0000E43A
		public string variants
		{
			get
			{
				return this.m_Variants;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010247 File Offset: 0x0000E447
		public InputDevice device
		{
			get
			{
				return this.m_Device;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001024F File Offset: 0x0000E44F
		public InputControl parent
		{
			get
			{
				return this.m_Parent;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010257 File Offset: 0x0000E457
		public ReadOnlyArray<InputControl> children
		{
			get
			{
				return new ReadOnlyArray<InputControl>(this.m_Device.m_ChildrenForEachControl, this.m_ChildStartIndex, this.m_ChildCount);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00010275 File Offset: 0x0000E475
		public ReadOnlyArray<InternedString> usages
		{
			get
			{
				return new ReadOnlyArray<InternedString>(this.m_Device.m_UsagesForEachControl, this.m_UsageStartIndex, this.m_UsageCount);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00010293 File Offset: 0x0000E493
		public ReadOnlyArray<InternedString> aliases
		{
			get
			{
				return new ReadOnlyArray<InternedString>(this.m_Device.m_AliasesForEachControl, this.m_AliasStartIndex, this.m_AliasCount);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000102B1 File Offset: 0x0000E4B1
		public InputStateBlock stateBlock
		{
			get
			{
				return this.m_StateBlock;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000102B9 File Offset: 0x0000E4B9
		// (set) Token: 0x060003AA RID: 938 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public bool noisy
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.IsNoisy) > (InputControl.ControlFlags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.IsNoisy;
					ReadOnlyArray<InputControl> children = this.children;
					for (int i = 0; i < children.Count; i++)
					{
						if (children[i] != null)
						{
							children[i].noisy = true;
						}
					}
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.IsNoisy;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00010327 File Offset: 0x0000E527
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00010334 File Offset: 0x0000E534
		public bool synthetic
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.IsSynthetic) > (InputControl.ControlFlags)0;
			}
			internal set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.IsSynthetic;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.IsSynthetic;
			}
		}

		// Token: 0x17000106 RID: 262
		public InputControl this[string path]
		{
			get
			{
				InputControl inputControl = InputControlPath.TryFindChild(this, path, 0);
				if (inputControl == null)
				{
					throw new KeyNotFoundException(string.Format("Cannot find control '{0}' as child of '{1}'", path, this));
				}
				return inputControl;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003AE RID: 942
		public abstract Type valueType { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003AF RID: 943
		public abstract int valueSizeInBytes { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00010376 File Offset: 0x0000E576
		public float magnitude
		{
			get
			{
				return this.EvaluateMagnitude();
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001037E File Offset: 0x0000E57E
		public override string ToString()
		{
			return this.layout + ":" + this.path;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00010398 File Offset: 0x0000E598
		private string DebuggerDisplay()
		{
			if (!this.device.added)
			{
				return this.ToString();
			}
			string text;
			try
			{
				text = string.Format("{0}:{1}={2}", this.layout, this.path, this.ReadValueAsObject());
			}
			catch (Exception)
			{
				text = this.ToString();
			}
			return text;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public float EvaluateMagnitude()
		{
			return this.EvaluateMagnitude(this.currentStatePtr);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00010402 File Offset: 0x0000E602
		public unsafe virtual float EvaluateMagnitude(void* statePtr)
		{
			return -1f;
		}

		// Token: 0x060003B5 RID: 949
		public unsafe abstract object ReadValueFromBufferAsObject(void* buffer, int bufferSize);

		// Token: 0x060003B6 RID: 950
		public unsafe abstract object ReadValueFromStateAsObject(void* statePtr);

		// Token: 0x060003B7 RID: 951
		public unsafe abstract void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize);

		// Token: 0x060003B8 RID: 952 RVA: 0x00010409 File Offset: 0x0000E609
		public unsafe virtual void WriteValueFromBufferIntoState(void* bufferPtr, int bufferSize, void* statePtr)
		{
			throw new NotSupportedException(string.Format("Control '{0}' does not support writing", this));
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001041B File Offset: 0x0000E61B
		public unsafe virtual void WriteValueFromObjectIntoState(object value, void* statePtr)
		{
			throw new NotSupportedException(string.Format("Control '{0}' does not support writing", this));
		}

		// Token: 0x060003BA RID: 954
		public unsafe abstract bool CompareValue(void* firstStatePtr, void* secondStatePtr);

		// Token: 0x060003BB RID: 955 RVA: 0x0001042D File Offset: 0x0000E62D
		public InputControl TryGetChildControl(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			return InputControlPath.TryFindChild(this, path, 0);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001044C File Offset: 0x0000E64C
		public TControl TryGetChildControl<TControl>(string path) where TControl : InputControl
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			InputControl inputControl = this.TryGetChildControl(path);
			if (inputControl == null)
			{
				return default(TControl);
			}
			TControl tcontrol = inputControl as TControl;
			if (tcontrol == null)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Expected control '",
					path,
					"' to be of type '",
					typeof(TControl).Name,
					"' but is of type '",
					inputControl.GetType().Name,
					"' instead!"
				}));
			}
			return tcontrol;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000104EB File Offset: 0x0000E6EB
		public InputControl GetChildControl(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			InputControl inputControl = this.TryGetChildControl(path);
			if (inputControl == null)
			{
				throw new ArgumentException("Cannot find input control '" + this.MakeChildPath(path) + "'", "path");
			}
			return inputControl;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001052C File Offset: 0x0000E72C
		public TControl GetChildControl<TControl>(string path) where TControl : InputControl
		{
			InputControl childControl = this.GetChildControl(path);
			TControl tcontrol = childControl as TControl;
			if (tcontrol == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Expected control '",
					path,
					"' to be of type '",
					typeof(TControl).Name,
					"' but is of type '",
					childControl.GetType().Name,
					"' instead!"
				}), "path");
			}
			return tcontrol;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000105B0 File Offset: 0x0000E7B0
		protected InputControl()
		{
			this.m_StateBlock.byteOffset = 4294967294U;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000105D3 File Offset: 0x0000E7D3
		protected virtual void FinishSetup()
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000105D5 File Offset: 0x0000E7D5
		protected void RefreshConfigurationIfNeeded()
		{
			if (!this.isConfigUpToDate)
			{
				this.RefreshConfiguration();
				this.isConfigUpToDate = true;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000105EC File Offset: 0x0000E7EC
		protected virtual void RefreshConfiguration()
		{
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000105EE File Offset: 0x0000E7EE
		protected internal unsafe void* currentStatePtr
		{
			get
			{
				return InputStateBuffers.GetFrontBufferForDevice(this.GetDeviceIndex());
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000105FB File Offset: 0x0000E7FB
		protected internal unsafe void* previousFrameStatePtr
		{
			get
			{
				return InputStateBuffers.GetBackBufferForDevice(this.GetDeviceIndex());
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00010608 File Offset: 0x0000E808
		protected internal unsafe void* defaultStatePtr
		{
			get
			{
				return InputStateBuffers.s_DefaultStateBuffer;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001060F File Offset: 0x0000E80F
		protected internal unsafe void* noiseMaskPtr
		{
			get
			{
				return InputStateBuffers.s_NoiseMaskBuffer;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00010618 File Offset: 0x0000E818
		protected internal uint stateOffsetRelativeToDeviceRoot
		{
			get
			{
				uint byteOffset = this.device.m_StateBlock.byteOffset;
				return this.m_StateBlock.byteOffset - byteOffset;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00010643 File Offset: 0x0000E843
		public FourCC optimizedControlDataType
		{
			get
			{
				return this.m_OptimizedControlDataType;
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001064B File Offset: 0x0000E84B
		protected virtual FourCC CalculateOptimizedControlDataType()
		{
			return 0;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00010654 File Offset: 0x0000E854
		public void ApplyParameterChanges()
		{
			this.SetOptimizedControlDataTypeRecursively();
			for (InputControl inputControl = this.parent; inputControl != null; inputControl = inputControl.parent)
			{
				inputControl.SetOptimizedControlDataType();
			}
			this.MarkAsStaleRecursively();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00010686 File Offset: 0x0000E886
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetOptimizedControlDataType()
		{
			this.m_OptimizedControlDataType = (InputSystem.s_Manager.optimizedControlsFeatureEnabled ? this.CalculateOptimizedControlDataType() : 0);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000106A8 File Offset: 0x0000E8A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void SetOptimizedControlDataTypeRecursively()
		{
			if (this.m_ChildCount > 0)
			{
				foreach (InputControl inputControl in this.children)
				{
					inputControl.SetOptimizedControlDataTypeRecursively();
				}
			}
			this.SetOptimizedControlDataType();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001070C File Offset: 0x0000E90C
		[Conditional("UNITY_EDITOR")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void EnsureOptimizationTypeHasNotChanged()
		{
			if (!InputSystem.s_Manager.optimizedControlsFeatureEnabled)
			{
				return;
			}
			FourCC fourCC = this.CalculateOptimizedControlDataType();
			if (fourCC != this.optimizedControlDataType)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Control '",
					this.name,
					"' / '",
					this.path,
					"' suddenly changed optimization state due to either format ",
					string.Format("change or control parameters change (was '{0}' but became '{1}'), ", this.optimizedControlDataType, fourCC),
					"this hinders control hot path optimization, please call control.ApplyParameterChanges() after the changes to the control to fix this error."
				}));
				this.m_OptimizedControlDataType = fourCC;
			}
			if (this.m_ChildCount > 0)
			{
				foreach (InputControl inputControl in this.children)
				{
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000107F0 File Offset: 0x0000E9F0
		// (set) Token: 0x060003CF RID: 975 RVA: 0x000107FF File Offset: 0x0000E9FF
		internal bool isSetupFinished
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.SetupFinished) == InputControl.ControlFlags.SetupFinished;
			}
			set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.SetupFinished;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.SetupFinished;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00010823 File Offset: 0x0000EA23
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x00010830 File Offset: 0x0000EA30
		internal bool isButton
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.IsButton) == InputControl.ControlFlags.IsButton;
			}
			set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.IsButton;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.IsButton;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00010853 File Offset: 0x0000EA53
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00010860 File Offset: 0x0000EA60
		internal bool isConfigUpToDate
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.ConfigUpToDate) == InputControl.ControlFlags.ConfigUpToDate;
			}
			set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.ConfigUpToDate;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.ConfigUpToDate;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00010883 File Offset: 0x0000EA83
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00010892 File Offset: 0x0000EA92
		internal bool dontReset
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.DontReset) == InputControl.ControlFlags.DontReset;
			}
			set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.DontReset;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.DontReset;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000108B6 File Offset: 0x0000EAB6
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x000108C5 File Offset: 0x0000EAC5
		internal bool usesStateFromOtherControl
		{
			get
			{
				return (this.m_ControlFlags & InputControl.ControlFlags.UsesStateFromOtherControl) == InputControl.ControlFlags.UsesStateFromOtherControl;
			}
			set
			{
				if (value)
				{
					this.m_ControlFlags |= InputControl.ControlFlags.UsesStateFromOtherControl;
					return;
				}
				this.m_ControlFlags &= ~InputControl.ControlFlags.UsesStateFromOtherControl;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x000108E9 File Offset: 0x0000EAE9
		internal bool hasDefaultState
		{
			get
			{
				return !this.m_DefaultState.isEmpty;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000108FC File Offset: 0x0000EAFC
		internal void CallFinishSetupRecursive()
		{
			ReadOnlyArray<InputControl> children = this.children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].CallFinishSetupRecursive();
			}
			this.FinishSetup();
			this.SetOptimizedControlDataTypeRecursively();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001093B File Offset: 0x0000EB3B
		internal string MakeChildPath(string path)
		{
			if (this is InputDevice)
			{
				return path;
			}
			return this.path + "/" + path;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00010958 File Offset: 0x0000EB58
		internal void BakeOffsetIntoStateBlockRecursive(uint offset)
		{
			this.m_StateBlock.byteOffset = this.m_StateBlock.byteOffset + offset;
			ReadOnlyArray<InputControl> children = this.children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].BakeOffsetIntoStateBlockRecursive(offset);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000109A0 File Offset: 0x0000EBA0
		internal int GetDeviceIndex()
		{
			int deviceIndex = this.m_Device.m_DeviceIndex;
			if (deviceIndex == -1)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Cannot query value of control '",
					this.path,
					"' before '",
					this.device.name,
					"' has been added to system!"
				}));
			}
			return deviceIndex;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000109FE File Offset: 0x0000EBFE
		internal bool IsValueConsideredPressed(float value)
		{
			if (this.isButton)
			{
				return ((ButtonControl)this).IsValueConsideredPressed(value);
			}
			return value >= ButtonControl.s_GlobalDefaultButtonPressPoint;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00010A20 File Offset: 0x0000EC20
		internal virtual void AddProcessor(object first)
		{
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010A22 File Offset: 0x0000EC22
		internal void MarkAsStale()
		{
			this.m_CachedValueIsStale = true;
			this.m_UnprocessedCachedValueIsStale = true;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00010A34 File Offset: 0x0000EC34
		internal void MarkAsStaleRecursively()
		{
			this.MarkAsStale();
			foreach (InputControl inputControl in this.children)
			{
				inputControl.MarkAsStale();
				ButtonControl buttonControl = inputControl as ButtonControl;
				if (buttonControl != null)
				{
					buttonControl.UpdateWasPressed();
				}
			}
		}

		// Token: 0x04000125 RID: 293
		protected internal InputStateBlock m_StateBlock;

		// Token: 0x04000126 RID: 294
		internal InternedString m_Name;

		// Token: 0x04000127 RID: 295
		internal string m_Path;

		// Token: 0x04000128 RID: 296
		internal string m_DisplayName;

		// Token: 0x04000129 RID: 297
		internal string m_DisplayNameFromLayout;

		// Token: 0x0400012A RID: 298
		internal string m_ShortDisplayName;

		// Token: 0x0400012B RID: 299
		internal string m_ShortDisplayNameFromLayout;

		// Token: 0x0400012C RID: 300
		internal InternedString m_Layout;

		// Token: 0x0400012D RID: 301
		internal InternedString m_Variants;

		// Token: 0x0400012E RID: 302
		internal InputDevice m_Device;

		// Token: 0x0400012F RID: 303
		internal InputControl m_Parent;

		// Token: 0x04000130 RID: 304
		internal int m_UsageCount;

		// Token: 0x04000131 RID: 305
		internal int m_UsageStartIndex;

		// Token: 0x04000132 RID: 306
		internal int m_AliasCount;

		// Token: 0x04000133 RID: 307
		internal int m_AliasStartIndex;

		// Token: 0x04000134 RID: 308
		internal int m_ChildCount;

		// Token: 0x04000135 RID: 309
		internal int m_ChildStartIndex;

		// Token: 0x04000136 RID: 310
		internal InputControl.ControlFlags m_ControlFlags;

		// Token: 0x04000137 RID: 311
		internal bool m_CachedValueIsStale = true;

		// Token: 0x04000138 RID: 312
		internal bool m_UnprocessedCachedValueIsStale = true;

		// Token: 0x04000139 RID: 313
		internal PrimitiveValue m_DefaultState;

		// Token: 0x0400013A RID: 314
		internal PrimitiveValue m_MinValue;

		// Token: 0x0400013B RID: 315
		internal PrimitiveValue m_MaxValue;

		// Token: 0x0400013C RID: 316
		internal FourCC m_OptimizedControlDataType;

		// Token: 0x0200018D RID: 397
		[Flags]
		internal enum ControlFlags
		{
			// Token: 0x040008A6 RID: 2214
			ConfigUpToDate = 1,
			// Token: 0x040008A7 RID: 2215
			IsNoisy = 2,
			// Token: 0x040008A8 RID: 2216
			IsSynthetic = 4,
			// Token: 0x040008A9 RID: 2217
			IsButton = 8,
			// Token: 0x040008AA RID: 2218
			DontReset = 16,
			// Token: 0x040008AB RID: 2219
			SetupFinished = 32,
			// Token: 0x040008AC RID: 2220
			UsesStateFromOtherControl = 64
		}
	}
}
