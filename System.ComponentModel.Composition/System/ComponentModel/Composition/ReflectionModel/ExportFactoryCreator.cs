using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
	// Token: 0x02000054 RID: 84
	internal sealed class ExportFactoryCreator
	{
		// Token: 0x0600022E RID: 558 RVA: 0x00006A84 File Offset: 0x00004C84
		public ExportFactoryCreator(Type exportFactoryType)
		{
			Assumes.NotNull<Type>(exportFactoryType);
			this._exportFactoryType = exportFactoryType;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00006A9C File Offset: 0x00004C9C
		public Func<Export, object> CreateStronglyTypedExportFactoryFactory(Type exportType, Type metadataViewType)
		{
			MethodInfo methodInfo;
			if (metadataViewType == null)
			{
				methodInfo = ExportFactoryCreator._createStronglyTypedExportFactoryOfT.MakeGenericMethod(new Type[] { exportType });
			}
			else
			{
				methodInfo = ExportFactoryCreator._createStronglyTypedExportFactoryOfTM.MakeGenericMethod(new Type[] { exportType, metadataViewType });
			}
			Assumes.NotNull<MethodInfo>(methodInfo);
			Func<Export, object> exportFactoryFactory = (Func<Export, object>)Delegate.CreateDelegate(typeof(Func<Export, object>), this, methodInfo);
			return (Export e) => exportFactoryFactory(e);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00006B18 File Offset: 0x00004D18
		private object CreateStronglyTypedExportFactoryOfT<T>(Export export)
		{
			Type[] array = new Type[] { typeof(T) };
			Type type = this._exportFactoryType.MakeGenericType(array);
			ExportFactoryCreator.LifetimeContext lifetimeContext = new ExportFactoryCreator.LifetimeContext();
			Func<Tuple<T, Action>> func = () => lifetimeContext.GetExportLifetimeContextFromExport<T>(export);
			object[] array2 = new object[] { func };
			object obj = Activator.CreateInstance(type, array2);
			lifetimeContext.SetInstance(obj);
			return obj;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00006B90 File Offset: 0x00004D90
		private object CreateStronglyTypedExportFactoryOfTM<T, M>(Export export)
		{
			Type[] array = new Type[]
			{
				typeof(T),
				typeof(M)
			};
			Type type = this._exportFactoryType.MakeGenericType(array);
			ExportFactoryCreator.LifetimeContext lifetimeContext = new ExportFactoryCreator.LifetimeContext();
			Func<Tuple<T, Action>> func = () => lifetimeContext.GetExportLifetimeContextFromExport<T>(export);
			M metadataView = AttributedModelServices.GetMetadataView<M>(export.Metadata);
			object[] array2 = new object[] { func, metadataView };
			object obj = Activator.CreateInstance(type, array2);
			lifetimeContext.SetInstance(obj);
			return obj;
		}

		// Token: 0x040000E9 RID: 233
		private static readonly MethodInfo _createStronglyTypedExportFactoryOfT = typeof(ExportFactoryCreator).GetMethod("CreateStronglyTypedExportFactoryOfT", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		// Token: 0x040000EA RID: 234
		private static readonly MethodInfo _createStronglyTypedExportFactoryOfTM = typeof(ExportFactoryCreator).GetMethod("CreateStronglyTypedExportFactoryOfTM", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		// Token: 0x040000EB RID: 235
		private Type _exportFactoryType;

		// Token: 0x02000055 RID: 85
		private class LifetimeContext
		{
			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x06000233 RID: 563 RVA: 0x00006C69 File Offset: 0x00004E69
			// (set) Token: 0x06000234 RID: 564 RVA: 0x00006C71 File Offset: 0x00004E71
			public Func<ComposablePartDefinition, bool> CatalogFilter { get; private set; }

			// Token: 0x06000235 RID: 565 RVA: 0x00006C7C File Offset: 0x00004E7C
			public void SetInstance(object instance)
			{
				Assumes.NotNull<object>(instance);
				MethodInfo method = instance.GetType().GetMethod("IncludeInScopedCatalog", BindingFlags.Instance | BindingFlags.NonPublic, null, ExportFactoryCreator.LifetimeContext.types, null);
				this.CatalogFilter = (Func<ComposablePartDefinition, bool>)Delegate.CreateDelegate(typeof(Func<ComposablePartDefinition, bool>), instance, method);
			}

			// Token: 0x06000236 RID: 566 RVA: 0x00006CC8 File Offset: 0x00004EC8
			public Tuple<T, Action> GetExportLifetimeContextFromExport<T>(Export export)
			{
				IDisposable disposable = null;
				CatalogExportProvider.ScopeFactoryExport scopeFactoryExport = export as CatalogExportProvider.ScopeFactoryExport;
				T t;
				if (scopeFactoryExport != null)
				{
					Export export2 = scopeFactoryExport.CreateExportProduct(this.CatalogFilter);
					t = ExportServices.GetCastedExportedValue<T>(export2);
					disposable = export2 as IDisposable;
				}
				else
				{
					CatalogExportProvider.FactoryExport factoryExport = export as CatalogExportProvider.FactoryExport;
					if (factoryExport != null)
					{
						Export export3 = factoryExport.CreateExportProduct();
						t = ExportServices.GetCastedExportedValue<T>(export3);
						disposable = export3 as IDisposable;
					}
					else
					{
						ComposablePartDefinition castedExportedValue = ExportServices.GetCastedExportedValue<ComposablePartDefinition>(export);
						ComposablePart composablePart = castedExportedValue.CreatePart();
						ExportDefinition exportDefinition = castedExportedValue.ExportDefinitions.Single<ExportDefinition>();
						t = ExportServices.CastExportedValue<T>(composablePart.ToElement(), composablePart.GetExportedValue(exportDefinition));
						disposable = composablePart as IDisposable;
					}
				}
				Action action;
				if (disposable != null)
				{
					action = delegate
					{
						disposable.Dispose();
					};
				}
				else
				{
					action = delegate
					{
					};
				}
				return new Tuple<T, Action>(t, action);
			}

			// Token: 0x040000EC RID: 236
			private static Type[] types = new Type[] { typeof(ComposablePartDefinition) };
		}
	}
}
