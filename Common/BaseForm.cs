using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Hanhua.Common
{
    /// <summary>
    /// 汉化基类
    /// </summary>
    public partial class BaseForm : Form
    {
        /// <summary>
        /// 将要打开的文件
        /// </summary>
        public string baseFile = string.Empty;

        /// <summary>
        /// 当前目录
        /// </summary>
        protected string baseFolder = string.Empty;

        /// <summary>
        /// 关键字
        /// </summary>
        protected string baseKeyWords = string.Empty;

        /// <summary>
        /// 记录所有ShiftJis字符
        /// </summary>
        protected static string shiftJisCharList = string.Empty;

        /// <summary>
        /// 全局Excel对象，为了资源释放
        /// </summary>
        protected Microsoft.Office.Interop.Excel.Application xApp = null;

        /// <summary>
        /// 具体业务操作的方法
        /// </summary>
        protected delegate void DoSomething();

        /// <summary>
        /// 具体业务操作的方法
        /// </summary>
        protected delegate void DoSomethingWithP(params object[] param);

        /// <summary>
        /// 更新进度条的代理
        /// </summary>
        /// <param name="num"></param>
        protected delegate void DelegateResetProcessBar(int num);

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 初始化等待条
        /// </summary>
        /// <param name="maxNum"></param>
        public void ResetProcessBar(int maxNum)
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.BeginInvoke(new DelegateResetProcessBar(this.ResetProcessBar), new object[] { maxNum });
            }
            else
            {
                this.progressBar.Minimum = 1;
                this.progressBar.Maximum = maxNum;
                this.progressBar.Value = 1;
                this.progressBar.Step = 1;

                this.Height += 25;
                this.pnlProgress.Visible = true;
            }
        }

        /// <summary>
        /// 改变进度条
        /// </summary>
        public void ProcessBarStep()
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.BeginInvoke(new MethodInvoker(this.ProcessBarStep));
            }
            else
            {
                this.progressBar.PerformStep();
            }
        }

        /// <summary>
        /// 关闭进度条
        /// </summary>
        public void CloseProcessBar()
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.BeginInvoke(new MethodInvoker(this.CloseProcessBar));
            }
            else
            {
                if (this.pnlProgress.Visible)
                {
                    this.ResetHeight();
                    this.pnlProgress.Visible = false;
                }
            }
        }

        /// <summary>
        /// 默认显示时，去掉隐藏的进度条的高度
        /// </summary>
        protected void ResetHeight()
        {
            this.Height -= 25;
        }

        /// <summary>
        /// 具体业务操作
        /// </summary>
        /// <param name="doMethod"></param>
        protected void Do(DoSomething doMethod)
        {
            // 由于长时间操作，需要启动新线程异步执行，系统直接向下执行
            Thread t = new Thread(new ParameterizedThreadStart(this.BaseDo));
            t.Start(doMethod);
        }

        /// <summary>
        /// 具体业务操作
        /// </summary>
        /// <param name="doMethod"></param>
        protected void Do(DoSomethingWithP doMethod, params object[] param)
        {
            // 由于长时间操作，需要启动新线程异步执行，系统直接向下执行
            Thread t = new Thread(new ParameterizedThreadStart(this.BaseDoWithP));
            t.Start(new object[] {doMethod, param});
        }

        /// <summary>
        /// 开始多线程，统一处理异常
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool NewThread(System.Threading.WaitCallback callback, object state)
        {
            try
            {
                return System.Threading.ThreadPool.QueueUserWorkItem(callback, state);
            }
            catch (Exception exp)
            {
                MessageBox.Show(this.baseFile + "\n" + exp.Message + "\n" + exp.StackTrace);
            }
            finally
            {
                this.CloseProcessBar();
            }

            return false;
        }

        /// <summary>
        /// 开始多线程，统一处理异常
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool NewThread(System.Threading.WaitCallback callback)
        {
            try
            {
                return System.Threading.ThreadPool.QueueUserWorkItem(callback);
            }
            catch (Exception exp)
            {
                MessageBox.Show(this.baseFile + "\n" + exp.Message + "\n" + exp.StackTrace);
            }
            finally
            {
                this.CloseProcessBar();
            }

            return false;
        } 

        /// <summary>
        /// 父类调用子类的业务操作，主要是加Try，Catch
        /// </summary>
        /// <param name="doMethod"></param>
        private void BaseDo(object doMethod)
        {
            try
            {
                ((DoSomething)doMethod)();
            }
            catch (Exception exp)
            {
                MessageBox.Show(this.baseFile + "\n" + exp.Message + "\n" + exp.StackTrace);
            }
            finally
            {
                this.CloseProcessBar();

                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }
            }
        }

        /// <summary>
        /// 父类调用子类的业务操作，主要是加Try，Catch
        /// </summary>
        /// <param name="doMethodAndP"></param>
        private void BaseDoWithP(object doMethodAndP)
        {
            try
            {
                object[] objP = (object[])doMethodAndP;

                ((DoSomethingWithP)objP[0])((object[])objP[1]);
            }
            catch (Exception exp)
            {
                MessageBox.Show(this.baseFile + "\n" + exp.Message + "\n" + exp.StackTrace);
            }
            finally
            {
                this.CloseProcessBar();

                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }
            }
        }
    }
}