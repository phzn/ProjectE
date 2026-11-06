using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000415 RID: 1045
	internal sealed class XslAstRewriter
	{
		// Token: 0x06002933 RID: 10547 RVA: 0x000F6D80 File Offset: 0x000F4F80
		public void Rewrite(Compiler compiler)
		{
			this.compiler = compiler;
			this.scope = new CompilerScopeManager<VarPar>();
			this.newTemplates = new Stack<Template>();
			using (List<ProtoTemplate>.Enumerator enumerator = compiler.AllTemplates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProtoTemplate protoTemplate = enumerator.Current;
					this.scope.EnterScope();
					this.CheckNodeCost(protoTemplate);
				}
				goto IL_009C;
			}
			IL_005F:
			Template template = this.newTemplates.Pop();
			compiler.AllTemplates.Add(template);
			compiler.NamedTemplates.Add(template.Name, template);
			this.scope.EnterScope();
			this.CheckNodeCost(template);
			IL_009C:
			if (this.newTemplates.Count <= 0)
			{
				return;
			}
			goto IL_005F;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000F6E48 File Offset: 0x000F5048
		private static int NodeCostForXPath(string xpath)
		{
			int num = 0;
			if (xpath != null)
			{
				num = 2;
				for (int i = 2; i < xpath.Length; i += 2)
				{
					if (xpath[i] == '/' || xpath[i - 1] == '/')
					{
						num += 2;
					}
				}
			}
			return num;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000F6E8A File Offset: 0x000F508A
		private static bool NodeTypeTest(XslNodeType nodetype, int flags)
		{
			return ((flags >> (int)nodetype) & 1) != 0;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000F6E98 File Offset: 0x000F5098
		private int CheckNodeCost(XslNode node)
		{
			this.scope.EnterScope(node.Namespaces);
			bool flag = false;
			int num = 1;
			if (XslAstRewriter.NodeTypeTest(node.NodeType, -247451132))
			{
				num += XslAstRewriter.NodeCostForXPath(node.Select);
			}
			IList<XslNode> content = node.Content;
			int num2 = content.Count - 1;
			int i = 0;
			while (i <= num2)
			{
				XslNode xslNode = content[i];
				int num3 = this.CheckNodeCost(xslNode);
				num += num3;
				if (flag && num > 100)
				{
					if (i < num2 || num3 > 1)
					{
						this.Refactor(node, i);
						num -= num3;
						num++;
						break;
					}
					break;
				}
				else
				{
					if (xslNode.NodeType == XslNodeType.Variable || xslNode.NodeType == XslNodeType.Param)
					{
						this.scope.AddVariable(xslNode.Name, (VarPar)xslNode);
						if (xslNode.NodeType == XslNodeType.Param)
						{
							num -= num3;
						}
					}
					else if (!flag)
					{
						flag = XslAstRewriter.NodeTypeTest(node.NodeType, -1025034872);
					}
					i++;
				}
			}
			this.scope.ExitScope();
			return num;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000F6FA4 File Offset: 0x000F51A4
		private void Refactor(XslNode parent, int split)
		{
			List<XslNode> list = (List<XslNode>)parent.Content;
			XslNode xslNode = list[split];
			QilName qilName = AstFactory.QName("generated", this.compiler.CreatePhantomNamespace(), "compiler");
			XsltInput.ContextInfo contextInfo = new XsltInput.ContextInfo(xslNode.SourceLine);
			XslNodeEx xslNodeEx = AstFactory.CallTemplate(qilName, contextInfo);
			XsltLoader.SetInfo(xslNodeEx, null, contextInfo);
			Template template = AstFactory.Template(qilName, null, XsltLoader.nullMode, double.NaN, xslNode.XslVersion);
			XsltLoader.SetInfo(template, null, contextInfo);
			this.newTemplates.Push(template);
			template.SetContent(new List<XslNode>(list.Count - split + 8));
			foreach (CompilerScopeManager<VarPar>.ScopeRecord scopeRecord in this.scope.GetActiveRecords())
			{
				if (!scopeRecord.IsVariable)
				{
					template.Namespaces = new NsDecl(template.Namespaces, scopeRecord.ncName, scopeRecord.nsUri);
				}
				else
				{
					VarPar value = scopeRecord.value;
					if (!this.compiler.IsPhantomNamespace(value.Name.NamespaceUri))
					{
						QilName qilName2 = AstFactory.QName(value.Name.LocalName, value.Name.NamespaceUri, value.Name.Prefix);
						VarPar varPar = AstFactory.VarPar(XslNodeType.WithParam, qilName2, "$" + qilName2.QualifiedName, XslVersion.Version10);
						XsltLoader.SetInfo(varPar, null, contextInfo);
						varPar.Namespaces = value.Namespaces;
						xslNodeEx.AddContent(varPar);
						VarPar varPar2 = AstFactory.VarPar(XslNodeType.Param, qilName2, null, XslVersion.Version10);
						XsltLoader.SetInfo(varPar2, null, contextInfo);
						varPar2.Namespaces = value.Namespaces;
						template.AddContent(varPar2);
					}
				}
			}
			for (int i = split; i < list.Count; i++)
			{
				template.AddContent(list[i]);
			}
			list[split] = xslNodeEx;
			list.RemoveRange(split + 1, list.Count - split - 1);
		}

		// Token: 0x0400208B RID: 8331
		private static readonly QilName nullMode = AstFactory.QName(string.Empty);

		// Token: 0x0400208C RID: 8332
		private CompilerScopeManager<VarPar> scope;

		// Token: 0x0400208D RID: 8333
		private Stack<Template> newTemplates;

		// Token: 0x0400208E RID: 8334
		private Compiler compiler;

		// Token: 0x0400208F RID: 8335
		private const int FixedNodeCost = 1;

		// Token: 0x04002090 RID: 8336
		private const int IteratorNodeCost = 2;

		// Token: 0x04002091 RID: 8337
		private const int CallTemplateCost = 1;

		// Token: 0x04002092 RID: 8338
		private const int RewriteThreshold = 100;

		// Token: 0x04002093 RID: 8339
		private const int NodesWithSelect = -247451132;

		// Token: 0x04002094 RID: 8340
		private const int ParentsOfCallTemplate = -1025034872;
	}
}
