using System;

namespace Unity.Burst
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
	public class BurstCompileAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020C8 File Offset: 0x000002C8
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020D0 File Offset: 0x000002D0
		public FloatMode FloatMode { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020D9 File Offset: 0x000002D9
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020E1 File Offset: 0x000002E1
		public FloatPrecision FloatPrecision { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020EA File Offset: 0x000002EA
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002106 File Offset: 0x00000306
		public bool CompileSynchronously
		{
			get
			{
				return this._compileSynchronously != null && this._compileSynchronously.Value;
			}
			set
			{
				this._compileSynchronously = new bool?(value);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002114 File Offset: 0x00000314
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002130 File Offset: 0x00000330
		public bool Debug
		{
			get
			{
				return this._debug != null && this._debug.Value;
			}
			set
			{
				this._debug = new bool?(value);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000213E File Offset: 0x0000033E
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000215A File Offset: 0x0000035A
		public bool DisableSafetyChecks
		{
			get
			{
				return this._disableSafetyChecks != null && this._disableSafetyChecks.Value;
			}
			set
			{
				this._disableSafetyChecks = new bool?(value);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002168 File Offset: 0x00000368
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002184 File Offset: 0x00000384
		public bool DisableDirectCall
		{
			get
			{
				return this._disableDirectCall != null && this._disableDirectCall.Value;
			}
			set
			{
				this._disableDirectCall = new bool?(value);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002192 File Offset: 0x00000392
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000219A File Offset: 0x0000039A
		public OptimizeFor OptimizeFor { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000021A3 File Offset: 0x000003A3
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000021AB File Offset: 0x000003AB
		internal string[] Options { get; set; }

		// Token: 0x06000015 RID: 21 RVA: 0x00002050 File Offset: 0x00000250
		public BurstCompileAttribute()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021B4 File Offset: 0x000003B4
		public BurstCompileAttribute(FloatPrecision floatPrecision, FloatMode floatMode)
		{
			this.FloatMode = floatMode;
			this.FloatPrecision = floatPrecision;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021CA File Offset: 0x000003CA
		internal BurstCompileAttribute(string[] options)
		{
			this.Options = options;
		}

		// Token: 0x04000018 RID: 24
		internal bool? _compileSynchronously;

		// Token: 0x04000019 RID: 25
		internal bool? _debug;

		// Token: 0x0400001A RID: 26
		internal bool? _disableSafetyChecks;

		// Token: 0x0400001B RID: 27
		internal bool? _disableDirectCall;
	}
}
