using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace System.Xml.XPath
{
	// Token: 0x02000006 RID: 6
	internal readonly struct XPathEvaluator
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003058 File Offset: 0x00001258
		public object Evaluate<T>(XNode node, string expression, IXmlNamespaceResolver resolver) where T : class
		{
			object obj = node.CreateNavigator().Evaluate(expression, resolver);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				return this.EvaluateIterator<T>(xpathNodeIterator);
			}
			if (!(obj is T))
			{
				throw new InvalidOperationException(global::SR.Format("The XPath expression evaluated to unexpected type {0}.", obj.GetType()));
			}
			return (T)((object)obj);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000030AE File Offset: 0x000012AE
		private IEnumerable<T> EvaluateIterator<T>(XPathNodeIterator result)
		{
			foreach (object obj in result)
			{
				XPathNavigator xpathNavigator = (XPathNavigator)obj;
				object r = xpathNavigator.UnderlyingObject;
				if (!(r is T))
				{
					throw new InvalidOperationException(global::SR.Format("The XPath expression evaluated to unexpected type {0}.", r.GetType()));
				}
				yield return (T)((object)r);
				XText t = r as XText;
				if (t != null && t.GetParent() != null)
				{
					do
					{
						t = t.NextNode as XText;
						if (t == null)
						{
							break;
						}
						yield return (T)((object)t);
					}
					while (t != t.GetParent().LastNode);
				}
				r = null;
				t = null;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}
	}
}
