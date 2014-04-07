using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Network
{
	using Defination;

	class ServerUpdateInfoProvider
	{
		/// <summary>
		/// 获得当前的更新环境
		/// </summary>
		public UpdateContext Context { get; private set; }

		/// <summary>
		/// 创建 <see cref="ServerUpdateInfoProvider" />  的新实例(ServerUpdateInfoProvider)
		/// </summary>
		/// <param name="context"></param>
		public ServerUpdateInfoProvider(UpdateContext context)
		{
			Context = context;
		}

		public bool FetchUpdateInfo()
		{
			return true;
		}
	}
}
