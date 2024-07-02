using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Deputy.Robot.Design;
using Deputy.Robot.Support;
using Deputy.Base.Native;
using System.Windows.Automation;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Deputy.Robot;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace test
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        public const int GW_HWNDNEXT = 2; // The next window is below the specified window
        public const int GW_HWNDPREV = 3; // The previous window is above

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindow", SetLastError = true)]
        public static extern IntPtr GetNextWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int wFlag);

        [DllImport("user32.dll")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);


        SimpleHttpServer _server;

        public Form1()
        {
            InitializeComponent();
        }

        public static string AutoGetFrameworkName(int X, int Y)
        {   //根据鼠标位置自动获取最合适的框架名
            //这段代码一般不需要动
            IntPtr hWnd = User32.WindowFromPhysicalPoint(new User32.POINT(X, Y));
            IHoverRegion hoverRegion = null;
            var hoverRegionFactory = new Dictionary<RecordFramework, Func<IHoverRegion>>{
                {RecordFramework.MSAA,() => new DesktopHoverRegion(hWnd,X,Y)  },
                {RecordFramework.UIA,() => new DesktopUIAHoverRegion(hWnd,X,Y)  }};
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

            //这个函数的原型是 public static Object4Record FromPoint(int X, int Y, int X0, int Y0, string fwName);
            //由于我们只需要获取一个点上的界面元素，所以X0、Y0分别和X、Y取同样的值即可
            //fwName是框架名，可以通过AutoGetFrameworkName函数自动获取。有的时候，也需要指定框架名
            var UiObject = Object4Record.FromPoint(x, y, x, y, AutoGetFrameworkName(x, y));

            //获取到的 UiObject 是一个Object4Record对象，可以通过UiObject.Selector获取到selector，其中还有其他属性，可以自己留意一下
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

            //通过selector找到对应的界面元素，如果没有找到，返回null
            var UiObject = Object4Record.FromSelector(selector);
            if(UiObject != null)
            {   //同上，获取到的 UiObject 是一个Object4Record对象，可以通过UiObject.ElementRegion获取到元素的Region，其中还有其他属性，可以自己留意一下
                found_region.Text = UiObject.ElementRegion.ToString();
            }
        }

        static int GetZOrder(IntPtr hwndTarget)
        {
            int z = 0;
            IntPtr hwnd = GetTopWindow((IntPtr)null);
            while (hwnd != null)
            {
                if (hwnd == hwndTarget)
                {
                    return z;
                }
                hwnd = GetNextWindow(hwnd, GW_HWNDNEXT);
                z++;
            }
            return -1; // 窗口未找到
        }

        static public object GetChildren(UiNode[] children, int depth = 0)
        {
            var output = new List<object>();
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];
                object childData = null;

                if (child == null || child.Data == null)
                    continue;

                if(child.Data.WindowHandle != IntPtr.Zero && !IsWindowVisible(child.Data.WindowHandle))
                {   //如果窗口不可见，就不列入结果中
                    continue;
                }

                //如果元素的Region不在屏幕上，就不列入结果中
                var elementRegion = child.Data.ElementRegion;
                if (elementRegion == null)
                    continue;
                if(elementRegion.Left + elementRegion.Width <= 0 || elementRegion.Top + elementRegion.Height <= 0)
                    continue;

                if (child.Children.Length > 0)
                {
                    childData = GetChildren(child.Children, depth + 1);
                }

                if (childData == null)
                {   //叶节点
                    output.Add(new { Name = child.Data.NodeName, Region = elementRegion });
                }
                else
                {
                    if(depth == 0)
                        output.Add(new { Name = child.Data.NodeName, Region = elementRegion, ZOrder = GetZOrder(child.Data.WindowHandle), Process = child.Data.ProcessName, Child = childData });
                    else
                        output.Add(new { Name = child.Data.NodeName, Region = elementRegion, Process = child.Data.ProcessName, Child = childData });
                }
            }
            return output;
        }

        private void GetTree_Click(object sender, EventArgs e)
        {
            var root = UiNode.Root;
            var output = GetChildren(root.Children);

            //open an file to write string 
            StreamWriter file = new StreamWriter("tree.json");

            //convert output into readable json string and write it into file
            file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(output, Newtonsoft.Json.Formatting.Indented));

            file.Close();

            MessageBox.Show("Tree has been saved to tree.json");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //start an http server，and response API requests
            string serverPrefix = "localhost:9000"; // 你可以选择任何可用的端口号
            _server = new SimpleHttpServer(serverPrefix);

            try
            {
                await _server.StartServerAsync();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine("HttpListener exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            _server.StopServer();
        }
    }
}
