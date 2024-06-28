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

namespace test
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

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

        static public object GetChildren(UiNode[] children)
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

                if (child.Children.Length > 0)
                {
                    childData = GetChildren(child.Children);
                }

                if (childData == null)
                    output.Add(new { Name = child.Data.NodeName, Region = child.Data.ElementRegion });
                else
                    output.Add(new { Name = child.Data.NodeName, Region = child.Data.ElementRegion, Process = child.Data.ProcessName, Child = childData });
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
