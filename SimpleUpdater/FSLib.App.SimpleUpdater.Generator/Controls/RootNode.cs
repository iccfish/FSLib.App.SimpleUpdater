using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.App.SimpleUpdater.Generator.Controls
{
	class RootNode : FolderNode
	{
		public RootNode(string path)
			: base(path, path)
		{
			Text = "根目录";
			ImageKey = SelectedImageKey = "home";
		}
	}
}
