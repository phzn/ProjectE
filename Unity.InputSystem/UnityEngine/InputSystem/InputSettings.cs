using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200005B RID: 91
	public class InputSettings : ScriptableObject
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0003391F File Offset: 0x00031B1F
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00033927 File Offset: 0x00031B27
		public InputSettings.UpdateMode updateMode
		{
			get
			{
				return this.m_UpdateMode;
			}
			set
			{
				if (this.m_UpdateMode == value)
				{
					return;
				}
				this.m_UpdateMode = value;
				this.OnChange();
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00033940 File Offset: 0x00031B40
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00033948 File Offset: 0x00031B48
		public InputSettings.ScrollDeltaBehavior scrollDeltaBehavior
		{
			get
			{
				return this.m_ScrollDeltaBehavior;
			}
			set
			{
				if (this.m_ScrollDeltaBehavior == value)
				{
					return;
				}
				this.m_ScrollDeltaBehavior = value;
				this.OnChange();
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00033961 File Offset: 0x00031B61
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00033969 File Offset: 0x00031B69
		public bool compensateForScreenOrientation
		{
			get
			{
				return this.m_CompensateForScreenOrientation;
			}
			set
			{
				if (this.m_CompensateForScreenOrientation == value)
				{
					return;
				}
				this.m_CompensateForScreenOrientation = value;
				this.OnChange();
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00033982 File Offset: 0x00031B82
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00033985 File Offset: 0x00031B85
		[Obsolete("filterNoiseOnCurrent is deprecated, filtering of noise is always enabled now.", false)]
		public bool filterNoiseOnCurrent
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00033987 File Offset: 0x00031B87
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x0003398F File Offset: 0x00031B8F
		public float defaultDeadzoneMin
		{
			get
			{
				return this.m_DefaultDeadzoneMin;
			}
			set
			{
				if (this.m_DefaultDeadzoneMin == value)
				{
					return;
				}
				this.m_DefaultDeadzoneMin = value;
				this.OnChange();
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000339A8 File Offset: 0x00031BA8
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x000339B0 File Offset: 0x00031BB0
		public float defaultDeadzoneMax
		{
			get
			{
				return this.m_DefaultDeadzoneMax;
			}
			set
			{
				if (this.m_DefaultDeadzoneMax == value)
				{
					return;
				}
				this.m_DefaultDeadzoneMax = value;
				this.OnChange();
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x000339C9 File Offset: 0x00031BC9
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x000339D1 File Offset: 0x00031BD1
		public float defaultButtonPressPoint
		{
			get
			{
				return this.m_DefaultButtonPressPoint;
			}
			set
			{
				if (this.m_DefaultButtonPressPoint == value)
				{
					return;
				}
				this.m_DefaultButtonPressPoint = Mathf.Clamp(value, 0.0001f, float.MaxValue);
				this.OnChange();
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x000339F9 File Offset: 0x00031BF9
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00033A01 File Offset: 0x00031C01
		public float buttonReleaseThreshold
		{
			get
			{
				return this.m_ButtonReleaseThreshold;
			}
			set
			{
				if (this.m_ButtonReleaseThreshold == value)
				{
					return;
				}
				this.m_ButtonReleaseThreshold = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00033A1A File Offset: 0x00031C1A
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00033A22 File Offset: 0x00031C22
		public float defaultTapTime
		{
			get
			{
				return this.m_DefaultTapTime;
			}
			set
			{
				if (this.m_DefaultTapTime == value)
				{
					return;
				}
				this.m_DefaultTapTime = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00033A3B File Offset: 0x00031C3B
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00033A43 File Offset: 0x00031C43
		public float defaultSlowTapTime
		{
			get
			{
				return this.m_DefaultSlowTapTime;
			}
			set
			{
				if (this.m_DefaultSlowTapTime == value)
				{
					return;
				}
				this.m_DefaultSlowTapTime = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00033A5C File Offset: 0x00031C5C
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00033A64 File Offset: 0x00031C64
		public float defaultHoldTime
		{
			get
			{
				return this.m_DefaultHoldTime;
			}
			set
			{
				if (this.m_DefaultHoldTime == value)
				{
					return;
				}
				this.m_DefaultHoldTime = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00033A7D File Offset: 0x00031C7D
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00033A85 File Offset: 0x00031C85
		public float tapRadius
		{
			get
			{
				return this.m_TapRadius;
			}
			set
			{
				if (this.m_TapRadius == value)
				{
					return;
				}
				this.m_TapRadius = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00033A9E File Offset: 0x00031C9E
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00033AA6 File Offset: 0x00031CA6
		public float multiTapDelayTime
		{
			get
			{
				return this.m_MultiTapDelayTime;
			}
			set
			{
				if (this.m_MultiTapDelayTime == value)
				{
					return;
				}
				this.m_MultiTapDelayTime = value;
				this.OnChange();
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00033ABF File Offset: 0x00031CBF
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00033AC7 File Offset: 0x00031CC7
		public InputSettings.BackgroundBehavior backgroundBehavior
		{
			get
			{
				return this.m_BackgroundBehavior;
			}
			set
			{
				if (this.m_BackgroundBehavior == value)
				{
					return;
				}
				this.m_BackgroundBehavior = value;
				this.OnChange();
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00033AE0 File Offset: 0x00031CE0
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00033AE8 File Offset: 0x00031CE8
		public InputSettings.EditorInputBehaviorInPlayMode editorInputBehaviorInPlayMode
		{
			get
			{
				return this.m_EditorInputBehaviorInPlayMode;
			}
			set
			{
				if (this.m_EditorInputBehaviorInPlayMode == value)
				{
					return;
				}
				this.m_EditorInputBehaviorInPlayMode = value;
				this.OnChange();
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00033B01 File Offset: 0x00031D01
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00033B09 File Offset: 0x00031D09
		public InputSettings.InputActionPropertyDrawerMode inputActionPropertyDrawerMode
		{
			get
			{
				return this.m_InputActionPropertyDrawerMode;
			}
			set
			{
				if (this.m_InputActionPropertyDrawerMode == value)
				{
					return;
				}
				this.m_InputActionPropertyDrawerMode = value;
				this.OnChange();
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00033B22 File Offset: 0x00031D22
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00033B2A File Offset: 0x00031D2A
		public int maxEventBytesPerUpdate
		{
			get
			{
				return this.m_MaxEventBytesPerUpdate;
			}
			set
			{
				if (this.m_MaxEventBytesPerUpdate == value)
				{
					return;
				}
				this.m_MaxEventBytesPerUpdate = value;
				this.OnChange();
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00033B43 File Offset: 0x00031D43
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00033B4B File Offset: 0x00031D4B
		public int maxQueuedEventsPerUpdate
		{
			get
			{
				return this.m_MaxQueuedEventsPerUpdate;
			}
			set
			{
				if (this.m_MaxQueuedEventsPerUpdate == value)
				{
					return;
				}
				this.m_MaxQueuedEventsPerUpdate = value;
				this.OnChange();
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00033B64 File Offset: 0x00031D64
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00033B74 File Offset: 0x00031D74
		public ReadOnlyArray<string> supportedDevices
		{
			get
			{
				return new ReadOnlyArray<string>(this.m_SupportedDevices);
			}
			set
			{
				if (this.supportedDevices.Count == value.Count)
				{
					bool flag = false;
					for (int i = 0; i < this.supportedDevices.Count; i++)
					{
						if (this.m_SupportedDevices[i] != value[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return;
					}
				}
				this.m_SupportedDevices = value.ToArray();
				this.OnChange();
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00033BE5 File Offset: 0x00031DE5
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x00033BED File Offset: 0x00031DED
		public bool disableRedundantEventsMerging
		{
			get
			{
				return this.m_DisableRedundantEventsMerging;
			}
			set
			{
				if (this.m_DisableRedundantEventsMerging == value)
				{
					return;
				}
				this.m_DisableRedundantEventsMerging = value;
				this.OnChange();
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00033C06 File Offset: 0x00031E06
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x00033C0E File Offset: 0x00031E0E
		public bool shortcutKeysConsumeInput
		{
			get
			{
				return this.m_ShortcutKeysConsumeInputs;
			}
			set
			{
				if (this.m_ShortcutKeysConsumeInputs == value)
				{
					return;
				}
				this.m_ShortcutKeysConsumeInputs = value;
				this.OnChange();
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00033C28 File Offset: 0x00031E28
		public void SetInternalFeatureFlag(string featureName, bool enabled)
		{
			if (string.IsNullOrEmpty(featureName))
			{
				throw new ArgumentNullException("featureName");
			}
			if (this.m_FeatureFlags == null)
			{
				this.m_FeatureFlags = new HashSet<string>();
			}
			if (enabled)
			{
				this.m_FeatureFlags.Add(featureName.ToUpperInvariant());
			}
			else
			{
				this.m_FeatureFlags.Remove(featureName.ToUpperInvariant());
			}
			this.OnChange();
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00033C8A File Offset: 0x00031E8A
		internal bool IsFeatureEnabled(string featureName)
		{
			return this.m_FeatureFlags != null && this.m_FeatureFlags.Contains(featureName.ToUpperInvariant());
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00033CA7 File Offset: 0x00031EA7
		internal void OnChange()
		{
			if (InputSystem.settings == this)
			{
				InputSystem.s_Manager.ApplySettings();
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00033CC0 File Offset: 0x00031EC0
		private static bool CompareFloats(float a, float b)
		{
			return a - b <= float.Epsilon;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00033CD0 File Offset: 0x00031ED0
		private static bool CompareSets<T>(ReadOnlyArray<T> a, ReadOnlyArray<T> b)
		{
			if (a == null)
			{
				return b == null;
			}
			if (b == null)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < b.Count; j++)
				{
					T t = a[i];
					if (t.Equals(b[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00033D4E File Offset: 0x00031F4E
		private static bool CompareFeatureFlag(InputSettings a, InputSettings b, string featureName)
		{
			return a.IsFeatureEnabled(featureName) == b.IsFeatureEnabled(featureName);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00033D60 File Offset: 0x00031F60
		internal static bool AreEqual(InputSettings a, InputSettings b)
		{
			if (a == null)
			{
				return b == null;
			}
			return b != null && (a == b || (a.updateMode == b.updateMode && a.compensateForScreenOrientation == b.compensateForScreenOrientation && InputSettings.CompareFloats(a.defaultDeadzoneMin, b.defaultDeadzoneMin) && InputSettings.CompareFloats(a.defaultDeadzoneMax, b.defaultDeadzoneMax) && InputSettings.CompareFloats(a.defaultButtonPressPoint, b.defaultButtonPressPoint) && InputSettings.CompareFloats(a.buttonReleaseThreshold, b.buttonReleaseThreshold) && InputSettings.CompareFloats(a.defaultTapTime, b.defaultTapTime) && InputSettings.CompareFloats(a.defaultSlowTapTime, b.defaultSlowTapTime) && InputSettings.CompareFloats(a.defaultHoldTime, b.defaultHoldTime) && InputSettings.CompareFloats(a.tapRadius, b.tapRadius) && InputSettings.CompareFloats(a.multiTapDelayTime, b.multiTapDelayTime) && a.backgroundBehavior == b.backgroundBehavior && a.editorInputBehaviorInPlayMode == b.editorInputBehaviorInPlayMode && a.inputActionPropertyDrawerMode == b.inputActionPropertyDrawerMode && a.maxEventBytesPerUpdate == b.maxEventBytesPerUpdate && a.maxQueuedEventsPerUpdate == b.maxQueuedEventsPerUpdate && InputSettings.CompareSets<string>(a.supportedDevices, b.supportedDevices) && a.disableRedundantEventsMerging == b.disableRedundantEventsMerging && a.shortcutKeysConsumeInput == b.shortcutKeysConsumeInput && InputSettings.CompareFeatureFlag(a, b, "USE_OPTIMIZED_CONTROLS") && InputSettings.CompareFeatureFlag(a, b, "USE_READ_VALUE_CACHING") && InputSettings.CompareFeatureFlag(a, b, "PARANOID_READ_VALUE_CACHING_CHECKS") && InputSettings.CompareFeatureFlag(a, b, "DISABLE_UNITY_REMOTE_SUPPORT") && InputSettings.CompareFeatureFlag(a, b, "RUN_PLAYER_UPDATES_IN_EDIT_MODE") && InputSettings.CompareFeatureFlag(a, b, "USE_IMGUI_EDITOR_FOR_ASSETS")));
		}

		// Token: 0x040002C0 RID: 704
		[Tooltip("Determine which type of devices are used by the application. By default, this is empty meaning that all devices recognized by Unity will be used. Restricting the set of supported devices will make only those devices appear in the input system.")]
		[SerializeField]
		private string[] m_SupportedDevices;

		// Token: 0x040002C1 RID: 705
		[Tooltip("Determine when Unity processes events. By default, accumulated input events are flushed out before each fixed update and before each dynamic update. This setting can be used to restrict event processing to only where the application needs it.")]
		[SerializeField]
		private InputSettings.UpdateMode m_UpdateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

		// Token: 0x040002C2 RID: 706
		[SerializeField]
		private InputSettings.ScrollDeltaBehavior m_ScrollDeltaBehavior;

		// Token: 0x040002C3 RID: 707
		[SerializeField]
		private int m_MaxEventBytesPerUpdate = 5242880;

		// Token: 0x040002C4 RID: 708
		[SerializeField]
		private int m_MaxQueuedEventsPerUpdate = 1000;

		// Token: 0x040002C5 RID: 709
		[SerializeField]
		private bool m_CompensateForScreenOrientation = true;

		// Token: 0x040002C6 RID: 710
		[SerializeField]
		private InputSettings.BackgroundBehavior m_BackgroundBehavior;

		// Token: 0x040002C7 RID: 711
		[SerializeField]
		private InputSettings.EditorInputBehaviorInPlayMode m_EditorInputBehaviorInPlayMode;

		// Token: 0x040002C8 RID: 712
		[SerializeField]
		private InputSettings.InputActionPropertyDrawerMode m_InputActionPropertyDrawerMode;

		// Token: 0x040002C9 RID: 713
		[SerializeField]
		private float m_DefaultDeadzoneMin = 0.125f;

		// Token: 0x040002CA RID: 714
		[SerializeField]
		private float m_DefaultDeadzoneMax = 0.925f;

		// Token: 0x040002CB RID: 715
		[Min(0.0001f)]
		[SerializeField]
		private float m_DefaultButtonPressPoint = 0.5f;

		// Token: 0x040002CC RID: 716
		[SerializeField]
		private float m_ButtonReleaseThreshold = 0.75f;

		// Token: 0x040002CD RID: 717
		[SerializeField]
		private float m_DefaultTapTime = 0.2f;

		// Token: 0x040002CE RID: 718
		[SerializeField]
		private float m_DefaultSlowTapTime = 0.5f;

		// Token: 0x040002CF RID: 719
		[SerializeField]
		private float m_DefaultHoldTime = 0.4f;

		// Token: 0x040002D0 RID: 720
		[SerializeField]
		private float m_TapRadius = 5f;

		// Token: 0x040002D1 RID: 721
		[SerializeField]
		private float m_MultiTapDelayTime = 0.75f;

		// Token: 0x040002D2 RID: 722
		[SerializeField]
		private bool m_DisableRedundantEventsMerging;

		// Token: 0x040002D3 RID: 723
		[SerializeField]
		private bool m_ShortcutKeysConsumeInputs;

		// Token: 0x040002D4 RID: 724
		[NonSerialized]
		internal HashSet<string> m_FeatureFlags;

		// Token: 0x040002D5 RID: 725
		internal const int s_OldUnsupportedFixedAndDynamicUpdateSetting = 0;

		// Token: 0x020001B6 RID: 438
		public enum UpdateMode
		{
			// Token: 0x04000946 RID: 2374
			ProcessEventsInDynamicUpdate = 1,
			// Token: 0x04000947 RID: 2375
			ProcessEventsInFixedUpdate,
			// Token: 0x04000948 RID: 2376
			ProcessEventsManually
		}

		// Token: 0x020001B7 RID: 439
		public enum ScrollDeltaBehavior
		{
			// Token: 0x0400094A RID: 2378
			UniformAcrossAllPlatforms,
			// Token: 0x0400094B RID: 2379
			KeepPlatformSpecificInputRange
		}

		// Token: 0x020001B8 RID: 440
		public enum BackgroundBehavior
		{
			// Token: 0x0400094D RID: 2381
			ResetAndDisableNonBackgroundDevices,
			// Token: 0x0400094E RID: 2382
			ResetAndDisableAllDevices,
			// Token: 0x0400094F RID: 2383
			IgnoreFocus
		}

		// Token: 0x020001B9 RID: 441
		public enum EditorInputBehaviorInPlayMode
		{
			// Token: 0x04000951 RID: 2385
			PointersAndKeyboardsRespectGameViewFocus,
			// Token: 0x04000952 RID: 2386
			AllDevicesRespectGameViewFocus,
			// Token: 0x04000953 RID: 2387
			AllDeviceInputAlwaysGoesToGameView
		}

		// Token: 0x020001BA RID: 442
		public enum InputActionPropertyDrawerMode
		{
			// Token: 0x04000955 RID: 2389
			Compact,
			// Token: 0x04000956 RID: 2390
			MultilineEffective,
			// Token: 0x04000957 RID: 2391
			MultilineBoth
		}
	}
}
