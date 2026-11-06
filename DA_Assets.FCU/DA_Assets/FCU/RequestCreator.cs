using System;
using System.Collections.Generic;
using System.Text;
using DA_Assets.Extensions;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000048 RID: 72
	public class RequestCreator
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00004164 File Offset: 0x00002364
		public static DARequest CreateImageLinksRequest(string projectUrl, string format, float scale, IEnumerable<string> chunk, RequestHeader requestHeader)
		{
			string text = RequestCreator.CreateImagesQuery(chunk, projectUrl, format, scale);
			return new DARequest
			{
				Query = text,
				RequestType = RequestType.Get,
				RequestHeader = requestHeader
			};
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000041A0 File Offset: 0x000023A0
		public static string CreateImagesQuery(IEnumerable<string> chunk, string projectId, string extension, float scale)
		{
			string text = string.Join(",", chunk);
			if (text.IsEmpty())
			{
				return null;
			}
			if (text[0] == ',')
			{
				text = text.Remove(0, 1);
			}
			return string.Concat(new string[]
			{
				"https://api.figma.com/v1/images/",
				projectId,
				"?ids=",
				text,
				"&format=",
				extension,
				"&scale=",
				scale.ToDotString()
			});
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00004218 File Offset: 0x00002418
		public static DARequest CreateTokenRequest(string code)
		{
			string text = "https://api.figma.com/v1/oauth/token";
			DARequest darequest = new DARequest
			{
				Query = text,
				RequestType = RequestType.Post,
				WWWForm = new WWWForm()
			};
			darequest.WWWForm.AddField("grant_type", "authorization_code");
			darequest.WWWForm.AddField("code", code);
			darequest.WWWForm.AddField("redirect_uri", FcuConfig.RedirectUri);
			string text2 = FcuConfig.ClientId + ":" + FcuConfig.ClientSecret;
			string text3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text2));
			darequest.RequestHeader = new RequestHeader
			{
				Name = "Authorization",
				Value = "Basic " + text3
			};
			return darequest;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000042E8 File Offset: 0x000024E8
		public static DARequest CreateProjectRequest(RequestHeader requestHeader, string projectId)
		{
			string text = string.Format("https://api.figma.com/v1/files/{0}?depth={1}&plugin_data=shared", projectId, FcuConfig.FrameListDepth);
			return new DARequest
			{
				Name = RequestName.Project,
				Query = text,
				RequestType = RequestType.Get,
				RequestHeader = requestHeader
			};
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00004334 File Offset: 0x00002534
		public static DARequest CreateNodeRequest(RequestHeader requestHeader, string projectId, string nodeIds)
		{
			string text = string.Format("https://api.figma.com/v1/files/{0}/nodes?ids={1}&geometry=paths&plugin_data=shared", projectId, nodeIds);
			return new DARequest
			{
				Query = text,
				RequestType = RequestType.Get,
				RequestHeader = requestHeader
			};
		}
	}
}
