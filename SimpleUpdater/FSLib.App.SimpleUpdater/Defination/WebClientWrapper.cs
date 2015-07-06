using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Defination
{
	using System.Net;

	class WebClientWrapper : WebClient
	{
		/// <summary>
		/// 为指定资源返回一个 <see cref="T:System.Net.WebRequest"/> 对象。
		/// </summary>
		/// <returns>
		/// 一个新的 <see cref="T:System.Net.WebRequest"/> 对象，用于指定的资源。
		/// </returns>
		/// <param name="address">一个 <see cref="T:System.Uri"/>，用于标识要请求的资源。</param>
		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address);

			if (request is FtpWebRequest && request.Credentials == null)
			{
				request.Credentials = new NetworkCredential("anonymous", "anonymouse@fishlee.net");
			}

			return request;
		}
	}
}
