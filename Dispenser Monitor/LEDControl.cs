using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Meteosoft.ControllerClient
{
	/// <summary>
	/// The LEDBulb is a .Net control for Windows Forms that emulates an
	/// LED light with two states On and Off.  The purpose of the control is to 
	/// provide a sleek looking representation of an LED light that is sizable, 
	/// has a transparent background and can be set to different colors.  
	/// </summary>
	public class LedBulb : Control
    {
		#region Public and Private Members

		private Color m_color;
        private bool m_blink = false;
		private bool m_on = true;
		private Color m_reflectionColor = Color.FromArgb(180, 255, 255, 255);
		private Color[] m_surroundColor = { Color.FromArgb(0, 255, 255, 255) };
		private Timer m_timer = new Timer();

		/// <summary>
		/// Gets or Sets the color of the LED light
		/// </summary>
		[DefaultValue(typeof(Color), "153, 255, 54")]
		public Color Color { 
			get { return m_color; } 
			set 
            { 
				m_color = value;
				DarkColor = ControlPaint.Dark(m_color);
				DarkDarkColor = ControlPaint.DarkDark(m_color);
				Invalidate();	// Redraw the control
			} 
		}
		
		/// <summary>
		/// Dark shade of the LED color used for gradient
		/// </summary>
		public Color DarkColor { get; protected set; }
		
		/// <summary>
		/// Very dark shade of the LED color used for gradient
		/// </summary>
		public Color DarkDarkColor { get; protected set; }

		/// <summary>
		/// Gets or Sets whether the light is turned on
		/// </summary>
		public bool On 
        { 
			get { return m_on; } 
			set { m_on = value; Invalidate(); } 
		}

        /// <summary>
        /// Gets or Sets whether the light blinks
        /// </summary>
        public bool Blink
        {
            get { return m_blink; }
            set 
            {
                m_blink = value;
                m_on = true;
                Invalidate(); 
            }
        }
        
        #endregion

		#region Constructor
		
		public LedBulb() 
        {
			SetStyle(ControlStyles.DoubleBuffer
			| ControlStyles.AllPaintingInWmPaint
			| ControlStyles.ResizeRedraw
			| ControlStyles.UserPaint
			| ControlStyles.SupportsTransparentBackColor, true);
			
			Color = Color.FromArgb(255, 153, 255, 54);
            m_timer.Tick += Timer_Tick;
            m_timer.Interval = 500;
            m_timer.Start();
		}

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (m_blink)
                On = !On;
        }
		
		#endregion

		#region Methods

		/// <summary>
		/// Handles the Paint event for this UserControl
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                // Create an offscreen graphics object for double buffering
                Bitmap offScreenBmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
                using (Graphics g = Graphics.FromImage(offScreenBmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    // Draw the control
                    DrawControl(g, On);
                    // Draw the image to the screen
                    e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

		/// <summary>
		/// Renders the control to an image
		/// </summary>
		private void DrawControl(Graphics g, bool on) 
        {
			// Is the bulb on or off
			Color lightColor = on ? Color : Color.FromArgb(150, DarkColor);
			Color darkColor = on ? DarkColor : DarkDarkColor;
			
			// Calculate the dimensions of the bulb
			int width = Width - (Padding.Left + Padding.Right);
			int height = Height - (Padding.Top + Padding.Bottom);
			// Diameter is the lesser of width and height
			int diameter = Math.Min(width, height);
			// Subtract 1 pixel so ellipse doesn't get cut off
			diameter = Math.Max(diameter - 1, 1);

			// Draw the background ellipse
			var rectangle = new Rectangle(Padding.Left, Padding.Top, diameter, diameter);
			g.FillEllipse(new SolidBrush(darkColor), rectangle);

			// Draw the glow gradient
			var path = new GraphicsPath();
			path.AddEllipse(rectangle);
		    var pathBrush = new PathGradientBrush(path)
		    {
		        CenterColor = lightColor,
		        SurroundColors = new[] {Color.FromArgb(0, lightColor)}
		    };
		    g.FillEllipse(pathBrush, rectangle);

			// Draw the white reflection gradient
			var offset = Convert.ToInt32(diameter * .15F);
			var diameter1 = Convert.ToInt32(rectangle.Width * .8F);
			var whiteRect = new Rectangle(rectangle.X - offset, rectangle.Y - offset, diameter1, diameter1);
			var path1 = new GraphicsPath();
			path1.AddEllipse(whiteRect);
		    var pathBrush1 = new PathGradientBrush(path) {CenterColor = m_reflectionColor, SurroundColors = m_surroundColor};
		    g.FillEllipse(pathBrush1, whiteRect);

			// Draw the border
			g.SetClip(ClientRectangle);
			if (On)
                g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black),1F), rectangle);
		}
		#endregion
	}

    [DesignerCategory("code")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripLedBulb : ToolStripControlHost
    {
        /// <summary>The tool strip containing the LED control</summary>
        public ToolStripLedBulb()
            : base(new LedBulb())
        {
        }

        /// <summary>The LED control within the tool strip</summary>
        public LedBulb LedBulbControl => Control as LedBulb;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // not thread safe!
            if (e != null)
            {
                LedBulbControl.Invalidate();
            }
        }
    }
}
