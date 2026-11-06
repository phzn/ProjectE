using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DA_Assets.DAI
{
	// Token: 0x0200000A RID: 10
	public static class Extensions
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000025FC File Offset: 0x000007FC
		public static string[] GetFieldsArray<T>(this Expression<Func<T, object>> pathExpression)
		{
			ExpressionType nodeType = pathExpression.Body.NodeType;
			MemberExpression memberExpression;
			if (nodeType - ExpressionType.Convert <= 1)
			{
				UnaryExpression unaryExpression = pathExpression.Body as UnaryExpression;
				memberExpression = ((unaryExpression != null) ? unaryExpression.Operand : null) as MemberExpression;
			}
			else
			{
				memberExpression = pathExpression.Body as MemberExpression;
			}
			List<string> list = new List<string>();
			while (memberExpression != null)
			{
				SerializePropertyAttribute serializePropertyAttribute = memberExpression.Member.GetCustomAttributes<SerializePropertyAttribute>().ToArray<SerializePropertyAttribute>()[0];
				if (serializePropertyAttribute == null)
				{
					if (!memberExpression.Member.Name.Contains("CS$"))
					{
						list.Add(memberExpression.Member.Name);
					}
				}
				else
				{
					list.Add(serializePropertyAttribute.FieldName);
				}
				memberExpression = memberExpression.Expression as MemberExpression;
			}
			list.Reverse();
			return list.ToArray();
		}
	}
}
