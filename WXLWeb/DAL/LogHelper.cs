using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace WXLWeb.DAL
{
    public class LogHelper
    {
        //声明一个静态的队列集合
        private static Queue<string> _queue;

        //静态构造函数
        //静态构造函数只在第一次使用静态类之前执行一次。
        static LogHelper()
        {
            //new 一个队列集合对象
            _queue = new Queue<string>();

            //获取HttpContext上下文对象
            HttpContext context = HttpContext.Current;
            //获取日志文件的路径
            //string path = context.Request.MapPath("~/App_Data/");
            string path = context.Request.MapPath("~/Logtxt/");
            //开启一个新的线程，这个线程的作用就是把_queue中的错误信息不断的写入到日志文件中
            Thread tWriteLog = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    bool isQueueEmpty = false;//标记当前队列中的信息是否为空
                    string exMsg = string.Empty;
                    //1.判断队列中是否有错误信息
                    lock ("webErrorCheck")
                    {
                        //判断队列中是否有错误信息
                        if (_queue.Count > 0)
                        {
                            //则从队列中获取一条错误信息
                            exMsg = _queue.Dequeue();
                        }
                        else
                        {
                            isQueueEmpty = true;//标记队列中为空
                        }
                    }

                    if (isQueueEmpty)
                    {
                        Thread.Sleep(100);
                    }
                    else if (exMsg != null)
                    {

                        //将错误信息写入到日志文件中。
                        string logFilePath = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                        //1.获取日志文件要保存的路径
                        File.AppendAllText(logFilePath, exMsg);
                    }

                }
            }));
            tWriteLog.IsBackground = true;
            tWriteLog.Start();
        }

        // 向队列中写入数据
        public static void LogEnqueue(string logTxt)
        {
            lock ("errorText")
            {
                //把错误信息入队！
                _queue.Enqueue(logTxt);
            }

        }
    }
}