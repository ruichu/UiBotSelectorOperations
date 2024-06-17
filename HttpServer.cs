using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Deputy.Robot.Design;
using Deputy.Robot.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace test
{
    class SimpleHttpServer
    {
        private HttpListener _listener;

        public SimpleHttpServer(string prefix)
        {
            // 确保提供的URL前缀以 http:// 作为前缀
            if (!prefix.StartsWith("http://"))
            {
                prefix = "http://" + prefix;
            }

            if(!prefix.EndsWith("/"))
            {
                prefix += "/";
            }

            // 创建一个HttpListener实例
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        public async Task StartServerAsync()
        {
            // 启动监听
            _listener.Start();

            Console.WriteLine("Listening...");

            while (_listener.IsListening)
            {
                // 获取一个传入的请求
                HttpListenerContext context = await _listener.GetContextAsync();

                // 创建响应
                HttpListenerResponse response = context.Response;

                try
                {
                    // 读取请求内容
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string body = reader.ReadToEnd();

                        // 设置响应内容
                        byte[] buffer = Encoding.UTF8.GetBytes(ProcessRequest(body));

                        // 设置响应头
                        response.ContentLength64 = buffer.Length;
                        response.ContentType = "application/json";

                        // 获取响应流
                        using (var outputStream = response.OutputStream)
                        {
                            // 写入响应内容
                            await outputStream.WriteAsync(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 错误处理
                    response.StatusCode = 500;
                    response.StatusDescription = ex.Message;
                }
                finally
                {
                    // 关闭响应
                    response.Close();
                }
            }
        }

        public void StopServer()
        {
            // 停止监听
            _listener.Stop();
            Console.WriteLine("Stopped listening...");
        }

        public string ProcessRequest(string request)
        {
            var requestObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(request);
            object responseObject = null;

            requestObject.TryGetValue("request", out string action);
            if (action == "GetTree")
            {
                var elementTree = Form1.GetChildren(UiNode.Root.Children);
                responseObject = new { ElementTree = elementTree };
            }
            else if(action == "GetSelector")
            {
                requestObject.TryGetValue("X", out string x_str);
                requestObject.TryGetValue("Y", out string y_str);

                int x = Convert.ToInt32(x_str);
                int y = Convert.ToInt32(y_str);

                var UiObject = Object4Record.FromPoint(x, y, x, y, Form1.AutoGetFrameworkName(x, y));
                //获取到的 UiObject 是一个Object4Record对象，可以通过UiObject.Selector获取到selector，其中还有其他属性，可以自己留意一下
                responseObject = new { Selector = UiObject.Selector };
            }
            else if (action == "FindSelector")
            {
                requestObject.TryGetValue("selector", out string selector_str);
                var selector = JObject.Parse(selector_str);

                var UiObject = Object4Record.FromSelector(selector);
                if (UiObject != null)
                {   //同上，获取到的 UiObject 是一个Object4Record对象，可以通过UiObject.ElementRegion获取到元素的Region，其中还有其他属性，可以自己留意一下
                    responseObject = new { Region = UiObject.ElementRegion };
                }
            }
            else
            {
                return "Invalid request";
            }

            if (responseObject == null)
                responseObject = new { Response = "null" };
            return JsonConvert.SerializeObject(responseObject);
        }
    }

}
