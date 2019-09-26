using System;
using System.Collections.Generic;
using System.Text;

namespace FSLib.App.SimpleUpdater.Dialogs
{
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;

	public class DialogStyle
	{
		private static Image _defaultIcon = Properties.Resources.AUTOMATIC_UPDATES_16x16_32;

		/// <summary>
		/// 获得默认的主题
		/// </summary>
		public static DialogStyle Default { get; } = new DialogStyle();

		/// <summary>
		/// 背景色
		/// </summary>
		internal Color BackColor { get; set; } = Color.White;

		/// <summary>
		/// 文本色
		/// </summary>
		internal Color ForeColor { get; set; } = Color.Black;

		/// <summary>
		/// 标题栏背景色
		/// </summary>
		internal Color TitleBackColor { get; set; } = Color.ForestGreen;

		/// <summary>
		/// 标题栏文本色
		/// </summary>
		internal Color TitleForeColor { get; set; } = Color.White;

		/// <summary>
		/// 标题栏图标
		/// </summary>
		internal Image Icon { get; set; } = _defaultIcon;


		public string ThemeBackColor
		{
			get => ColorTranslator.ToHtml(BackColor);
			set => BackColor = ColorTranslator.FromHtml(value);
		}

		public string ThemeForeColor
		{
			get => ColorTranslator.ToHtml(ForeColor);
			set => ForeColor = ColorTranslator.FromHtml(value);
		}


		public string ThemeTitleBackColor
		{
			get => ColorTranslator.ToHtml(TitleBackColor);
			set => TitleBackColor = ColorTranslator.FromHtml(value);
		}


		public string ThemeTitleForeColor
		{
			get => ColorTranslator.ToHtml(TitleForeColor);
			set => TitleForeColor = ColorTranslator.FromHtml(value);
		}


		public string ThemeIcon
		{
			get
			{
				if (Icon == null || _defaultIcon == Icon)
				{
					return null;
				}
				using (var ms = new MemoryStream())
				{
					Icon.Save(ms, ImageFormat.Png);

					return Convert.ToBase64String(ms.ToArray());
				}
			}
			set
			{
				if (string.IsNullOrEmpty(value))
					Icon = Properties.Resources.AUTOMATIC_UPDATES_16x16_32;
				else
				{
					using (var ms = new MemoryStream(Convert.FromBase64String(value)))
					{
						Icon = Image.FromStream(ms);
					}
				}

			}
		}
	}
}
