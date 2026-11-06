using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000018 RID: 24
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023AB File Offset: 0x000005AB
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000023B3 File Offset: 0x000005B3
		public string Id { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023BC File Offset: 0x000005BC
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000023C4 File Offset: 0x000005C4
		public string Title { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023CD File Offset: 0x000005CD
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000023D5 File Offset: 0x000005D5
		public string Description { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023DE File Offset: 0x000005DE
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000023E6 File Offset: 0x000005E6
		public Type ItemConverterType { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000023EF File Offset: 0x000005EF
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000023F7 File Offset: 0x000005F7
		[Nullable(new byte[] { 2, 1 })]
		public object[] ItemConverterParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002400 File Offset: 0x00000600
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002408 File Offset: 0x00000608
		public Type NamingStrategyType
		{
			get
			{
				return this._namingStrategyType;
			}
			set
			{
				this._namingStrategyType = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002418 File Offset: 0x00000618
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002420 File Offset: 0x00000620
		[Nullable(new byte[] { 2, 1 })]
		public object[] NamingStrategyParameters
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._namingStrategyParameters;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._namingStrategyParameters = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002438 File Offset: 0x00000638
		internal NamingStrategy NamingStrategyInstance { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002441 File Offset: 0x00000641
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000244E File Offset: 0x0000064E
		public bool IsReference
		{
			get
			{
				return this._isReference.GetValueOrDefault();
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000245C File Offset: 0x0000065C
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002469 File Offset: 0x00000669
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference.GetValueOrDefault();
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002477 File Offset: 0x00000677
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002484 File Offset: 0x00000684
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return this._itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002492 File Offset: 0x00000692
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000249F File Offset: 0x0000069F
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return this._itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000024AD File Offset: 0x000006AD
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000024B5 File Offset: 0x000006B5
		[NullableContext(1)]
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x0400002E RID: 46
		internal bool? _isReference;

		// Token: 0x0400002F RID: 47
		internal bool? _itemIsReference;

		// Token: 0x04000030 RID: 48
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x04000031 RID: 49
		internal TypeNameHandling? _itemTypeNameHandling;

		// Token: 0x04000032 RID: 50
		private Type _namingStrategyType;

		// Token: 0x04000033 RID: 51
		[Nullable(new byte[] { 2, 1 })]
		private object[] _namingStrategyParameters;
	}
}
