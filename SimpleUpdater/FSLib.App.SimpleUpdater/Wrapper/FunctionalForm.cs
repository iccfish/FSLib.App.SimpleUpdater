using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FSLib.App.SimpleUpdater.Wrapper
{
	public class FunctionalForm : System.Windows.Forms.Form
	{
		#region 消息函数重载

		/// <summary>
		/// 显示信息对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Information(string content)
		{
			Infomation("提示", content);
		}

		/// <summary>
		/// 显示信息对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Information(string content, params string[] arguments)
		{
			Infomation("提示", string.Format(content, arguments));
		}

		/// <summary>
		/// 显示信息对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Infomation(string title, string content, params string[] arguments)
		{
			Infomation(title, string.Format(content, arguments));
		}

		/// <summary>
		/// 显示信息对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Infomation(string title, string content)
		{
			MessageBox.Show(content, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// 显示错误对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Error(string content)
		{
			Error("错误", content);
		}

		/// <summary>
		/// 显示错误对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Error(string content, params string[] arguments)
		{
			Error("错误", string.Format(content, arguments));
		}

		/// <summary>
		/// 显示错误对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Error(string title, string content)
		{
			MessageBox.Show(content, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// 显示错误对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Error(string title, string content, params string[] arguments)
		{
			Error(title, string.Format(content, arguments));
		}


		/// <summary>
		/// 显示停止对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Stop(string content)
		{
			Stop("错误", content);
		}

		/// <summary>
		/// 显示停止对话框
		/// </summary>
		/// <param name="content">要显示的内容</param>
		public static void Stop(string content, params string[] arguments)
		{
			Stop("错误", string.Format(content, arguments));
		}

		/// <summary>
		/// 显示停止对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Stop(string title, string content, params string[] arguments)
		{
			Stop(title, content, string.Format(content, arguments));
		}


		/// <summary>
		/// 显示停止对话框
		/// </summary>
		/// <param name="title">要显示的标题</param>
		/// <param name="content">要显示的内容</param>
		public static void Stop(string title, string content)
		{
			MessageBox.Show(title, content, MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		/// <summary>
		/// 提示对话框
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static bool Question(string content)
		{
			return Question(content, "确定", false);
		}

		/// <summary>
		/// 提示对话框
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static bool Question(string content, params string[] arguments)
		{
			return Question(string.Format(content, arguments), "确定", false);
		}

		/// <summary>
		/// 提示对话框
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <param name="isYesNo">提示内容，true是 “是/否”，false为“确定”、“取消”</param>
		/// <returns></returns>
		public static bool Question(string content, bool isYesNo)
		{
			return Question(content, "确定", isYesNo);
		}

		/// <summary>
		/// 提示对话框
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <param name="isYesNo">提示内容，true是 “是/否”，false为“确定”、“取消”</param>
		/// <returns></returns>
		public static bool Question(string content, bool isYesNo, params string[] arguments)
		{
			return Question(string.Format(content, arguments), "确定", isYesNo);
		}

		/// <summary>
		/// 提示对话框
		/// </summary>
		/// <param name="title">标题</param>
		/// <param name="content">提示内容</param>
		/// <param name="isYesNo">提示内容，true是 “是/否”，false为“确定”、“取消”</param>
		/// <returns></returns>
		public static bool Question(string title, string content, bool isYesNo)
		{
			return MessageBox.Show(title, content, isYesNo ? MessageBoxButtons.YesNo : MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == (isYesNo ? DialogResult.Yes : DialogResult.OK);
		}



		/// <summary>
		/// 提示对话框（带有取消）
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static DialogResult QuestionWithCancel(string content)
		{
			return QuestionWithCancel("确定", content);
		}

		/// <summary>
		/// 提示对话框（带有取消）
		/// </summary>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static DialogResult QuestionWithCancel(string content, params object[] param)
		{
			return QuestionWithCancel("确定", string.Format(content, param));
		}

		/// <summary>
		/// 提示对话框（带有取消）
		/// </summary>
		/// <param name="title">标题</param>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static DialogResult QuestionWithCancel(string title, string content)
		{
			return MessageBox.Show(content, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
		}

		/// <summary>
		/// 提示对话框（带有取消）
		/// </summary>
		/// <param name="title">标题</param>
		/// <param name="content">提示内容</param>
		/// <returns></returns>
		public static DialogResult QuestionWithCancel(string title, string content, params object[] param)
		{
			return QuestionWithCancel(title, string.Format(content, param));
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryError(string content)
		{
			return MessageBox.Show("重试", content, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryError(string content, params object[] param)
		{
			return MessageBox.Show("重试", string.Format(content, param), MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryError(string title, string content)
		{
			return MessageBox.Show(title, content, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="title">显示的标题</param>
		/// <param name="content">提示的内容</param>
		/// <param name="param">如果提示的内容中有占位符,则使用此参数进行格式化</param>
		/// <returns></returns>
		public static bool RetryError(string title, string content, params object[] param)
		{
			return MessageBox.Show(title, string.Format(content, param), MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryCommon(string content)
		{
			return MessageBox.Show("重试", content, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry;
		}


		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryCommon(string title, string content, params object[] param)
		{
			return MessageBox.Show(title, string.Format(content, param), MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry;
		}

		/// <summary>
		/// 重试
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool RetryCommon(string title, string content)
		{
			return MessageBox.Show(title, content, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Retry;
		}
		#endregion
	}
}
