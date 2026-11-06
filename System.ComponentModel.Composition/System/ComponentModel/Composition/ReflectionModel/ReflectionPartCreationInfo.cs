using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
	// Token: 0x0200007F RID: 127
	internal class ReflectionPartCreationInfo : IReflectionPartCreationInfo, ICompositionElement
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000A40E File Offset: 0x0000860E
		public ReflectionPartCreationInfo(Lazy<Type> partType, bool isDisposalRequired, Lazy<IEnumerable<ImportDefinition>> imports, Lazy<IEnumerable<ExportDefinition>> exports, Lazy<IDictionary<string, object>> metadata, ICompositionElement origin)
		{
			Assumes.NotNull<Lazy<Type>>(partType);
			this._partType = partType;
			this._isDisposalRequired = isDisposalRequired;
			this._imports = imports;
			this._exports = exports;
			this._metadata = metadata;
			this._origin = origin;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000A449 File Offset: 0x00008649
		public Type GetPartType()
		{
			return this._partType.GetNotNullValue<Type>("type");
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000A45B File Offset: 0x0000865B
		public Lazy<Type> GetLazyPartType()
		{
			return this._partType;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000A464 File Offset: 0x00008664
		public ConstructorInfo GetConstructor()
		{
			if (this._constructor == null)
			{
				ConstructorInfo[] array = (from parameterImport in this.GetImports().OfType<ReflectionParameterImportDefinition>()
					select parameterImport.ImportingLazyParameter.Value.Member).OfType<ConstructorInfo>().Distinct<ConstructorInfo>().ToArray<ConstructorInfo>();
				if (array.Length == 1)
				{
					this._constructor = array[0];
				}
				else if (array.Length == 0)
				{
					this._constructor = this.GetPartType().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				}
			}
			return this._constructor;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000A4F5 File Offset: 0x000086F5
		public bool IsDisposalRequired
		{
			get
			{
				return this._isDisposalRequired;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000A4FD File Offset: 0x000086FD
		public IDictionary<string, object> GetMetadata()
		{
			if (this._metadata == null)
			{
				return null;
			}
			return this._metadata.Value;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000A514 File Offset: 0x00008714
		public IEnumerable<ExportDefinition> GetExports()
		{
			if (this._exports == null)
			{
				yield break;
			}
			IEnumerable<ExportDefinition> value = this._exports.Value;
			if (value == null)
			{
				yield break;
			}
			foreach (ExportDefinition exportDefinition in value)
			{
				ReflectionMemberExportDefinition reflectionMemberExportDefinition = exportDefinition as ReflectionMemberExportDefinition;
				if (reflectionMemberExportDefinition == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidExportDefinition, exportDefinition.GetType()));
				}
				yield return reflectionMemberExportDefinition;
			}
			IEnumerator<ExportDefinition> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000A524 File Offset: 0x00008724
		public IEnumerable<ImportDefinition> GetImports()
		{
			if (this._imports == null)
			{
				yield break;
			}
			IEnumerable<ImportDefinition> value = this._imports.Value;
			if (value == null)
			{
				yield break;
			}
			foreach (ImportDefinition importDefinition in value)
			{
				ReflectionImportDefinition reflectionImportDefinition = importDefinition as ReflectionImportDefinition;
				if (reflectionImportDefinition == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidMemberImportDefinition, importDefinition.GetType()));
				}
				yield return reflectionImportDefinition;
			}
			IEnumerator<ImportDefinition> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000A534 File Offset: 0x00008734
		public string DisplayName
		{
			get
			{
				return this.GetPartType().GetDisplayName();
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000A541 File Offset: 0x00008741
		public ICompositionElement Origin
		{
			get
			{
				return this._origin;
			}
		}

		// Token: 0x04000159 RID: 345
		private readonly Lazy<Type> _partType;

		// Token: 0x0400015A RID: 346
		private readonly Lazy<IEnumerable<ImportDefinition>> _imports;

		// Token: 0x0400015B RID: 347
		private readonly Lazy<IEnumerable<ExportDefinition>> _exports;

		// Token: 0x0400015C RID: 348
		private readonly Lazy<IDictionary<string, object>> _metadata;

		// Token: 0x0400015D RID: 349
		private readonly ICompositionElement _origin;

		// Token: 0x0400015E RID: 350
		private ConstructorInfo _constructor;

		// Token: 0x0400015F RID: 351
		private bool _isDisposalRequired;
	}
}
