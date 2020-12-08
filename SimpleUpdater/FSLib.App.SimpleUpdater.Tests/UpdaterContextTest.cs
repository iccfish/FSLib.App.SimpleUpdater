using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FSLib.App.SimpleUpdater.Tests
{
	using System.Diagnostics;

	[TestClass]
	public class UpdaterContextTest
	{
		[TestMethod]
		public void UrlInitTest()
		{
			var context = new FSLib.App.SimpleUpdater.Defination.UpdateContext();

			//本地升级，指定模板和文件名
			context.UpdateDownloadUrl = @"c:\{0}";
			context.UpdateInfoFileName = "update.xml";
			context.Init();

			Assert.AreEqual("update.xml", context.UpdateInfoFileName);
			Assert.AreEqual(@"c:\{0}", context.UpdateDownloadUrl);
			
			//本地升级，指定完整路径
			context.UpdateDownloadUrl = @"c:\update.xml";
			context.UpdateInfoFileName = null;
			context.Init();
			Assert.AreEqual("update.xml", context.UpdateInfoFileName);
			Assert.AreEqual(@"c:\{0}", context.UpdateDownloadUrl);

			//本地升级 - 只指定模板，未指定文件
			context.UpdateDownloadUrl = @"c:\{0}";
			context.UpdateInfoFileName = null;
			context.Init();

			Assert.AreEqual("update_c.xml", context.UpdateInfoFileName);
			Assert.AreEqual(@"c:\{0}", context.UpdateDownloadUrl);

			//本地升级 - 只指定目录
			context.UpdateDownloadUrl = @"c:\";
			context.UpdateInfoFileName = null;
			context.Init();
			Assert.AreEqual("update_c.xml", context.UpdateInfoFileName);
			Assert.AreEqual(@"c:\{0}", context.UpdateDownloadUrl);
			
			//网络升级，指定完整模板和文件名
			context.UpdateDownloadUrl = "http://localhost/{0}?query";
			context.UpdateInfoFileName = "update.xml";
			context.Init();
			Assert.AreEqual("update.xml", context.UpdateInfoFileName);
			Assert.AreEqual("http://localhost/{0}?query", context.UpdateDownloadUrl);


			//网络升级，只指定完整xml文件
			context.UpdateDownloadUrl = "http://localhost/update.xml?query";
			context.UpdateInfoFileName = null;
			context.Init();
			Assert.AreEqual("update.xml", context.UpdateInfoFileName);
			Assert.AreEqual("http://localhost/{0}?query", context.UpdateDownloadUrl);


			//网络升级，只指定模板
			context.UpdateDownloadUrl = "http://localhost/{0}?query";
			context.UpdateInfoFileName = null;
			context.Init();
			Assert.AreEqual("update_c.xml", context.UpdateInfoFileName);
			Assert.AreEqual("http://localhost/{0}?query", context.UpdateDownloadUrl);


			//网络升级，只指定目录
			context.UpdateDownloadUrl = "http://localhost/?query";
			context.UpdateInfoFileName = null;
			context.Init();
			Assert.AreEqual("update_c.xml", context.UpdateInfoFileName);
			Assert.AreEqual("http://localhost/{0}?query", context.UpdateDownloadUrl);

		}
	}
}
