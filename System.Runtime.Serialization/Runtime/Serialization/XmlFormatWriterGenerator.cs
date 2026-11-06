using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x02000141 RID: 321
	internal sealed class XmlFormatWriterGenerator
	{
		// Token: 0x06000FBD RID: 4029 RVA: 0x0003EB24 File Offset: 0x0003CD24
		[SecurityCritical]
		public XmlFormatWriterGenerator()
		{
			this.helper = new XmlFormatWriterGenerator.CriticalHelper();
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0003EB38 File Offset: 0x0003CD38
		[SecurityCritical]
		internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
		{
			XmlFormatClassWriterDelegate xmlFormatClassWriterDelegate;
			try
			{
				if (TD.DCGenWriterStartIsEnabled())
				{
					TD.DCGenWriterStart("Class", classContract.UnderlyingType.FullName);
				}
				xmlFormatClassWriterDelegate = this.helper.GenerateClassWriter(classContract);
			}
			finally
			{
				if (TD.DCGenWriterStopIsEnabled())
				{
					TD.DCGenWriterStop();
				}
			}
			return xmlFormatClassWriterDelegate;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0003EB90 File Offset: 0x0003CD90
		[SecurityCritical]
		internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
		{
			XmlFormatCollectionWriterDelegate xmlFormatCollectionWriterDelegate;
			try
			{
				if (TD.DCGenWriterStartIsEnabled())
				{
					TD.DCGenWriterStart("Collection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatCollectionWriterDelegate = this.helper.GenerateCollectionWriter(collectionContract);
			}
			finally
			{
				if (TD.DCGenWriterStopIsEnabled())
				{
					TD.DCGenWriterStop();
				}
			}
			return xmlFormatCollectionWriterDelegate;
		}

		// Token: 0x040006EF RID: 1775
		[SecurityCritical]
		private XmlFormatWriterGenerator.CriticalHelper helper;

		// Token: 0x02000142 RID: 322
		private class CriticalHelper
		{
			// Token: 0x06000FC0 RID: 4032 RVA: 0x0003EBE8 File Offset: 0x0003CDE8
			internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
			{
				return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, ClassDataContract ctr)
				{
					new XmlFormatWriterInterpreter(classContract).WriteToXml(xw, obj, ctx, ctr);
				};
			}

			// Token: 0x06000FC1 RID: 4033 RVA: 0x0003EC01 File Offset: 0x0003CE01
			internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
			{
				return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, CollectionDataContract ctr)
				{
					new XmlFormatWriterInterpreter(collectionContract).WriteCollectionToXml(xw, obj, ctx, ctr);
				};
			}
		}
	}
}
