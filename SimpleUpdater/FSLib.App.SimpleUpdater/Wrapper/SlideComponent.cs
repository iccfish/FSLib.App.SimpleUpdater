using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace FSLib.App.SimpleUpdater.Wrapper
{
	/// <summary>
	/// 滑动窗口组件
	/// </summary>
	[ToolboxItem(true), DesignerCategory("鱼的控件库"), System.Drawing.ToolboxBitmap(typeof(SlideComponent))]
	public class SlideComponent : Component
	{
		/// <summary>
		/// 构造器
		/// </summary>
		public SlideComponent()
		{
			_timer = new Timer();
			_timer.Tick += new EventHandler(_timer_Tick);
			_timer.Interval = 20;
		}

		#region 内部类型

		/// <summary>
		/// 横向动画方向
		/// </summary>
		public enum FlyXDirection
		{
			/// <summary>
			/// 从左到右
			/// </summary>
			LeftToRight = 0,
			/// <summary>
			/// 从右向左
			/// </summary>
			RightToLeft = 1,
			/// <summary>
			/// 无动画
			/// </summary>
			None = 2
		}

		/// <summary>
		/// 纵向动画方向
		/// </summary>
		public enum FlyYDirection
		{
			/// <summary>
			/// 从上向下
			/// </summary>
			TopToBottom = 0,
			/// <summary>
			/// 从下向上
			/// </summary>
			BottomToTop = 1,
			/// <summary>
			/// 无动画
			/// </summary>
			None = 2
		}

		#endregion

		#region 成员变量
		Timer _timer;
		Rectangle workRect;
		Timer T;
		int xFrom;
		int yFrom;
		int xTo;
		int yTo;
		//int currentTop;
		int sizeW;
		int sizeH;
		bool currentMode;
		int speedX = 0;
		int speedY = 7;
		bool closeFlag;

		#endregion

		#region 属性

		private FlyXDirection _DirectX = FlyXDirection.RightToLeft;
		[Category("动画"), DefaultValue(FlyXDirection.RightToLeft), Description("水平方向移动方向"), DisplayName("水平方向移动方向")]
		public FlyXDirection DirectX
		{
			get
			{
				return _DirectX;
			}
			set
			{
				_DirectX = value;
			}
		}
		private FlyYDirection _DirectY = FlyYDirection.BottomToTop;
		/// <summary>
		/// Y方向运动方向，True为从上向下
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(FlyYDirection.BottomToTop), Description("垂直方向移动方向，为True时从上向下移动"), DisplayName("垂直方向移动方向")]
		public FlyYDirection DirectY
		{
			get
			{
				return _DirectY;
			}
			set
			{
				_DirectY = value;
			}
		}


		/// <summary>
		/// 运行的速度(X)
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(10), Description("水平方向移动速度，单位为像素。为0时不做移动"), DisplayName("水平方向移动速度")]
		public int MoveSpeedX
		{
			get
			{
				return speedX;
			}
			set
			{
				if (value > -1)
				{
					speedX = value;
				}
				else
				{
					speedX = 10;
				}
			}
		}
		/// <summary>
		/// 运行的速度(Y)
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(0), Description("垂直方向移动速度，单位为像素。为0时不做移动"), DisplayName("垂直方向移动速度")]
		public int MoveSpeedY
		{
			get
			{
				return speedY;
			}
			set
			{
				if (value > -1)
				{
					speedY = value;
				}
				else
				{
					speedY = 7;
				}
			}
		}
		/// <summary>
		/// 定时器间隔
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(20), Description("两次移动之间间隔，单位是毫秒"), DisplayName("周期")]
		public int TimerInterval
		{
			get
			{
				return _timer.Interval;
			}
			set
			{
				if (value > 0)
				{
					_timer.Interval = value;
				}
				else
				{
					_timer.Interval = 20;
				}
			}
		}
		/// <summary>
		/// 窗体动画终点X值
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Browsable(false)]
		public int x_To
		{
			get
			{
				return xTo;
			}
		}
		/// <summary>
		/// 窗体动画终点Y值
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Browsable(false)]
		public int y_To
		{
			get
			{
				return yTo;
			}
		}
		/// <summary>
		/// 窗口运行的起点X值
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(0), Description("窗体起点横坐标值，为0时将会自动设置。"), DisplayName("起点横坐标值")]
		public int x_From
		{
			set
			{
				xFrom = value;
			}
			get
			{
				return xFrom;
			}
		}
		/// <summary>
		/// 窗口运动的起点Y值
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		[Category("动画"), DefaultValue(0), Description("窗体起点纵坐标值，为0时将会自动设置。"), DisplayName("起点纵坐标值")]
		public int y_Form
		{
			set
			{
				yFrom = value;
			}
			get
			{
				return yFrom;
			}
		}


		private Form _attachedForm;

		/// <summary>
		/// 要控制的装体
		/// </summary>
		[DisplayName("动画的窗体"), Category("动画")]
		public Form AttachedForm
		{
			get { return _attachedForm; }
			set
			{
				if (_attachedForm != null)
				{
					_attachedForm.Load -= new EventHandler(SlideForm_Load);
					_attachedForm.FormClosing -= new FormClosingEventHandler(SlideForm_FormClosing);
				}

				_attachedForm = value;

				if (_attachedForm != null)
				{
					_attachedForm.Load += new EventHandler(SlideForm_Load);
					_attachedForm.FormClosing += new FormClosingEventHandler(SlideForm_FormClosing);
				}
			}
		}

		private bool _slideInEnable = true;

		/// <summary>
		/// 是否允许滑入动画
		/// </summary>
		[DisplayName("是否允许滑入动画"), DefaultValue(true), Category("动画")]
		public bool SlideInEnable
		{
			get { return _slideInEnable; }
			set { _slideInEnable = value; }
		}


		private bool _slideOutEnable = true;

		/// <summary>
		/// 是否允许滑出动画
		/// </summary>
		[DisplayName("是否允许滑出动画"), DefaultValue(true), Category("动画")]
		public bool SlideOutEnable
		{
			get { return _slideOutEnable; }
			set { _slideOutEnable = value; }
		}


		private bool _alwaysSetLoc;

		/// <summary>
		/// 总是设置窗体初始位置，哪怕是关闭了滑动动画
		/// </summary>
		[DefaultValue(true), DisplayName("总是设置窗体初始位置"), Description("总是设置窗体初始位置，哪怕是关闭了滑动动画"), Category("动画")]
		public bool AlwaysSetLocation
		{
			get { return _alwaysSetLoc; }
			set { _alwaysSetLoc = value; }
		}

		#endregion


		#region 内部逻辑


		private void _timer_Tick(object sender, EventArgs e)
		{
			bool proc = false;

			if (currentMode)
			{
				//进入
				int x;
				int y;
				x = _attachedForm.Location.X;
				y = _attachedForm.Location.Y;

				if (_DirectX == FlyXDirection.LeftToRight)
				{
					//>
					if (x < xTo)
					{
						x += speedX;
						proc = true;
					}
				}
				else if (_DirectX == FlyXDirection.RightToLeft)
				{
					//<
					if (x > xTo)
					{
						x -= speedX;
						proc = true;
					}
				}
				if (_DirectY == FlyYDirection.TopToBottom)
				{
					//下
					if (y < yTo)
					{
						y += speedY;
						proc = true;
					}
				}
				else if (_DirectY == FlyYDirection.BottomToTop)
				{
					if (y > yTo)
					{
						y -= speedY;
						proc = true;
					}
				}

				if (proc)
				{
					_attachedForm.Location = new Point(x, y);
				}
				else
				{
					_timer.Stop();
					_timer.Enabled = false;
					if (SlideEnd != null) SlideEnd.Invoke(this, new SlideFinishedEventArgs(SlideDirection.SlideIn));
				}
			}
			else
			{
				//退出
				int x;
				int y;
				x = _attachedForm.Location.X;
				y = _attachedForm.Location.Y;

				if (_DirectX == FlyXDirection.LeftToRight)
				{
					//>
					if (x > xFrom)
					{
						x -= speedX;
						proc = true;
					}
				}
				else if (_DirectX == FlyXDirection.RightToLeft)
				{
					//<
					if (x < xFrom)
					{
						x += speedX;
						proc = true;
					}
				}
				if (_DirectY == FlyYDirection.TopToBottom)
				{
					//下
					if (y > yFrom)
					{
						y -= speedY;
						proc = true;
					}
				}
				else if (_DirectY == FlyYDirection.BottomToTop)
				{
					if (y < yFrom)
					{
						y += speedY;
						proc = true;
					}
				}

				if (proc)
				{
					_attachedForm.Location = new Point(x, y);
				}
				else
				{
					_timer.Stop();
					_timer.Enabled = false;
					closeFlag = false;
					if (SlideEnd != null) SlideEnd.Invoke(this, new SlideFinishedEventArgs(SlideDirection.SlideOut));
					_attachedForm.Close();
				}

			}
		}

		private void SlideForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (SlideOutEnable)
			{
				e.Cancel = closeFlag;

				if (closeFlag)
				{
					currentMode = false;
					_timer.Enabled = true;
					_timer.Start();
					if (SlideBegin != null) SlideBegin.Invoke(this, new SlideFinishedEventArgs(SlideDirection.SlideOut));
				}
			}
		}

		private void SlideForm_Load(System.Object sender, System.EventArgs e)
		{
			workRect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
			sizeW = _attachedForm.Width;
			sizeH = _attachedForm.Height;
			//计算起点
			if (xFrom == 0)
			{
				if (_DirectX == FlyXDirection.LeftToRight)
				{
					xFrom = workRect.Left - sizeW;
				}
				else
				{
					if (_DirectX == FlyXDirection.None)
					{
						xFrom = workRect.Right - sizeW;
					}
					else
					{
						xFrom = workRect.Right;
					}
				}
			}
			if (yFrom == 0)
			{
				if (_DirectY == FlyYDirection.TopToBottom)
				{
					yFrom = workRect.Top - sizeH;
				}
				else
				{
					if (_DirectY == FlyYDirection.None)
					{
						yFrom = workRect.Bottom - sizeH;
					}
					else
					{
						yFrom = workRect.Bottom;
					}
				}
			}
			//计算终点
			if (_DirectY != FlyYDirection.None)
			{
				if (_DirectY == FlyYDirection.TopToBottom)
				{
					yTo = yFrom + sizeH;
				}
				else
				{
					yTo = yFrom - sizeH;
				}
			}
			else
			{
				yTo = yFrom;
			}
			if (_DirectX != FlyXDirection.None)
			{
				if (_DirectX == FlyXDirection.LeftToRight)
				{
					xTo = xFrom + sizeW;
				}
				else
				{
					xTo = xFrom - sizeW;
				}
			}
			else
			{
				xTo = xFrom;
			}
			//重置当前窗体位置
			if (SlideInEnable)
			{
				_attachedForm.Location = new Point(xFrom, yFrom);

				closeFlag = true;
				//开始计时器
				currentMode = true;
				_timer.Enabled = true;
				_timer.Start();

				if (SlideBegin != null) SlideBegin.Invoke(this, new SlideFinishedEventArgs(SlideDirection.SlideIn));
			}
			else
			{
				currentMode = false;
				_attachedForm.Location = new Point(xTo, yTo);
				currentMode = false;
				closeFlag = true;
			}
		}

		#endregion


		#region 事件

		/// <summary>
		/// 滑动方向
		/// </summary>
		public enum SlideDirection
		{
			/// <summary>
			/// 滑入
			/// </summary>
			SlideIn,
			/// <summary>
			/// 滑出
			/// </summary>
			SlideOut
		}
		/// <summary>
		/// 参数
		/// </summary>
		public class SlideFinishedEventArgs : EventArgs
		{
			/// <summary>
			/// 滑动方向
			/// </summary>
			public SlideDirection Direction { get; set; }

			public SlideFinishedEventArgs(SlideDirection dir)
			{
				Direction = dir;
			}
		}

		/// <summary>
		/// 开始滑动
		/// </summary>
		/// <param name="?"></param>
		public delegate void SlideBeginEventHandler(object sender, SlideFinishedEventArgs e);

		/// <summary>
		/// 滑动结束
		/// </summary>
		/// <param name="?"></param>
		public delegate void SlideEndEventHandler(object sender, SlideFinishedEventArgs e);

		/// <summary>
		/// 开始滑动事件
		/// </summary>
		public event SlideBeginEventHandler SlideBegin;

		/// <summary>
		/// 滑动结束
		/// </summary>
		public event SlideEndEventHandler SlideEnd;


		#endregion

	}
}
