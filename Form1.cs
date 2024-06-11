using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Deputy.Robot.Design;
using Deputy.Robot.Support;
using Deputy.Base.Native;
using System.Windows.Automation;
using Newtonsoft.Json.Linq;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string AutoGetFrameworkName(int X, int Y)
        {
            IntPtr hWnd = User32.WindowFromPhysicalPoint(new User32.POINT(X, Y));
            IHoverRegion hoverRegion = null;
            var hoverRegionFactory = new Dictionary<RecordFramework, Func<IHoverRegion>>{
                {RecordFramework.MSAA,() =>new DesktopHoverRegion(hWnd,X,Y)  },
                {RecordFramework.UIA,() =>new DesktopUIAHoverRegion(hWnd,X,Y)  }};
            try
            {
                hoverRegion = SupportFactory._chain.Create(hWnd, (SupportLoader loader) => { return new GeneralHoverRegion(loader, hWnd, X, Y); });
                if (hoverRegion == null)
                {
                    var element = AutomationElement.FromPoint(new System.Windows.Point(X, Y));
                    object iAccessiblePattern = null;//根据LegacyIAccessiblePattern确认走msaa/UIA
                    if (element.TryGetCurrentPattern(LegacyIAccessiblePattern.Pattern, out iAccessiblePattern))
                        hoverRegion = hoverRegionFactory[RecordFramework.MSAA]();
                    else
                        hoverRegion = hoverRegionFactory[RecordFramework.UIA]();
                }
                return hoverRegion.GetFWName();
            }
            catch (Exception ex)
            {
            }
            return RecordFramework.MSAA.ToString();
        }

        private void GetSelector_Click(object sender, EventArgs e)
        {
            //convert selector_x.Text into int
            int x = Convert.ToInt32(selector_x.Text);
            int y = Convert.ToInt32(selector_y.Text);

            var UiObject = Object4Record.FromPoint(x, y, x, y, AutoGetFrameworkName(x, y));
            selector_got.Text = UiObject.Selector.ToString();
        }

        private void FindSelector_Click(object sender, EventArgs e)
        {
            JObject selector = null;
            found_region.Text = "";
            try
            {
                selector = JObject.Parse(selector_to_find.Text);
            }
            catch(Exception ex)
            {
            }
            if (selector == null)
                return;

            var UiObject = Object4Record.FromSelector(selector);
            if(UiObject != null)
            {
                found_region.Text = UiObject.ElementRegion.ToString();
            }
        }
    }
}
