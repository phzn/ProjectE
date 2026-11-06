using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000012 RID: 18
	public class DAJson
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000315F File Offset: 0x0000135F
		public static string ToJson(object obj)
		{
			return JsonConvert.SerializeObject(obj, DAJson.settings);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000316C File Offset: 0x0000136C
		public static T FromJson<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, DAJson.settings);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000317C File Offset: 0x0000137C
		public static async Task<DAResult<T>> FromJsonAsync<T>(string json)
		{
			DAJson.<>c__DisplayClass3_0<T> CS$<>8__locals1 = new DAJson.<>c__DisplayClass3_0<T>();
			CS$<>8__locals1.json = json;
			CS$<>8__locals1.@return = default(DAResult<T>);
			try
			{
				DAJson.<>c__DisplayClass3_1<T> CS$<>8__locals2 = new DAJson.<>c__DisplayClass3_1<T>();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.jfr = default(JFResult);
				await Task.Run(delegate
				{
					CS$<>8__locals2.jfr = DAFormatter.Format<T>(CS$<>8__locals2.CS$<>8__locals1.json);
				});
				if (!CS$<>8__locals2.jfr.IsValid)
				{
					throw new Exception("Not valid json.");
				}
				if (!CS$<>8__locals2.jfr.MatchTargetType)
				{
					throw new InvalidCastException("The input json does not match the target type.");
				}
				await Task.Run(delegate
				{
					CS$<>8__locals2.CS$<>8__locals1.@return.Object = JsonConvert.DeserializeObject<T>(CS$<>8__locals2.CS$<>8__locals1.json, DAJson.settings);
				});
				CS$<>8__locals2.CS$<>8__locals1.@return.Success = true;
				CS$<>8__locals2 = null;
			}
			catch (InvalidCastException ex)
			{
				CS$<>8__locals1.@return.Success = false;
				CS$<>8__locals1.@return.Error = new WebError(29, ex.Message, ex);
			}
			catch (MissingComponentException ex2)
			{
				CS$<>8__locals1.@return.Success = false;
				CS$<>8__locals1.@return.Error = new WebError(455, ex2.Message, ex2);
			}
			catch (ThreadAbortException ex3)
			{
				CS$<>8__locals1.@return.Success = false;
				CS$<>8__locals1.@return.Error = new WebError(-1, ex3.Message, ex3);
			}
			catch (Exception ex4)
			{
				CS$<>8__locals1.@return.Success = false;
				CS$<>8__locals1.@return.Error = new WebError(422, ex4.Message, ex4);
			}
			return CS$<>8__locals1.@return;
		}

		// Token: 0x0400005C RID: 92
		private static JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Error = delegate(object sender, [Nullable(1)] ErrorEventArgs error)
			{
				error.ErrorContext.Handled = true;
			},
			Formatting = Formatting.Indented
		};
	}
}
