using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TrayTemplate
{
    class TrayApplicationContext<T> : ApplicationContext where T : Form, new()
    {
        public Container Container { get; set; }
        public NotifyIcon NotifyIcon { get; set; }
        public T Form { get; set; }

        public void ShowForm()
        {
            // Created on first show, and optionally if form has been closed to save resources
            if (Form == null || Form.IsDisposed)
            {
                Form = new T();
            }
            Form.Show();
        }

        public TrayApplicationContext()
        {
            InitializeContext();
        }

        private void InitializeContext()
        {
            Container = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(Container)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = TrayTemplate.Properties.Resources.TrayTemplate,
                Text = "Tray Application Template",
                Visible = true
            };

            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Show Winow", null, ShowWindow_Click));
            NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&About Tray Template", null, HelpAbout_Click));
            NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("E&xit", null, Exit_Click));

            ShowForm();
        }

        private void ShowWindow_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Container != null) 
            { 
                Container.Dispose(); 
            }
        }

        protected override void ExitThreadCore()
        {
            if (MainForm != null && !MainForm.IsDisposed) 
            { 
                MainForm.Close(); 
            }

            // should remove lingering tray icon
            NotifyIcon.Visible = false; 

            base.ExitThreadCore();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void HelpAbout_Click(object sender, EventArgs e)
        {
            DialogAbout dialogAbout = new DialogAbout();
            dialogAbout.ShowDialog();
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            // TODO customize context menu strip here
            e.Cancel = false;
        }
    }
}
