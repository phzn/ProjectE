using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	/// <summary>Specifies the resource exposure for a member of a class. This class cannot be inherited.</summary>
	// Token: 0x02000640 RID: 1600
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class ResourceExposureAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Versioning.ResourceExposureAttribute" /> class with the specified exposure level.</summary>
		/// <param name="exposureLevel">The scope of the resource.</param>
		// Token: 0x06003C2F RID: 15407 RVA: 0x000D11CB File Offset: 0x000CF3CB
		public ResourceExposureAttribute(ResourceScope exposureLevel)
		{
			this._resourceExposureLevel = exposureLevel;
		}

		/// <summary>Gets the resource exposure scope.</summary>
		/// <returns>A <see cref="T:System.Runtime.Versioning.ResourceScope" /> object.</returns>
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x000D11DA File Offset: 0x000CF3DA
		public ResourceScope ResourceExposureLevel
		{
			get
			{
				return this._resourceExposureLevel;
			}
		}

		// Token: 0x040026F8 RID: 9976
		private ResourceScope _resourceExposureLevel;
	}
}
