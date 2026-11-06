using System;
using System.Collections.Generic;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x020004CD RID: 1229
	internal class QilScopedVisitor : QilVisitor
	{
		// Token: 0x0600320B RID: 12811 RVA: 0x0000B528 File Offset: 0x00009728
		protected virtual void BeginScope(QilNode node)
		{
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0000B528 File Offset: 0x00009728
		protected virtual void EndScope(QilNode node)
		{
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x001228B8 File Offset: 0x00120AB8
		protected virtual void BeforeVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				if (nodeType - QilNodeType.Loop <= 2)
				{
					goto IL_00EF;
				}
				if (nodeType != QilNodeType.Function)
				{
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode qilNode in qilExpression.GlobalParameterList)
				{
					this.BeginScope(qilNode);
				}
				foreach (QilNode qilNode2 in qilExpression.GlobalVariableList)
				{
					this.BeginScope(qilNode2);
				}
				using (IEnumerator<QilNode> enumerator = qilExpression.FunctionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QilNode qilNode3 = enumerator.Current;
						this.BeginScope(qilNode3);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QilNode qilNode4 = enumerator.Current;
					this.BeginScope(qilNode4);
				}
				return;
			}
			IL_00EF:
			this.BeginScope(((QilLoop)node).Variable);
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x001229FC File Offset: 0x00120BFC
		protected virtual void AfterVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				if (nodeType - QilNodeType.Loop <= 2)
				{
					goto IL_00EF;
				}
				if (nodeType != QilNodeType.Function)
				{
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode qilNode in qilExpression.FunctionList)
				{
					this.EndScope(qilNode);
				}
				foreach (QilNode qilNode2 in qilExpression.GlobalVariableList)
				{
					this.EndScope(qilNode2);
				}
				using (IEnumerator<QilNode> enumerator = qilExpression.GlobalParameterList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QilNode qilNode3 = enumerator.Current;
						this.EndScope(qilNode3);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QilNode qilNode4 = enumerator.Current;
					this.EndScope(qilNode4);
				}
				return;
			}
			IL_00EF:
			this.EndScope(((QilLoop)node).Variable);
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x00122B40 File Offset: 0x00120D40
		protected override QilNode Visit(QilNode n)
		{
			this.BeforeVisit(n);
			QilNode qilNode = base.Visit(n);
			this.AfterVisit(n);
			return qilNode;
		}
	}
}
