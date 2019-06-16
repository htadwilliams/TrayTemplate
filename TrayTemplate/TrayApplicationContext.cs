using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TrayTemplate
{
    class TrayApplicationContext : ApplicationContext
    {
        public Container Container { get => this.container; set => this.container = value; }
        public NotifyIcon NotifyIcon { get => this.notifyIcon; set => this.notifyIcon = value; }
        public Form Form { get => this.form; set => this.form = value; }

        private Container container;
        private NotifyIcon notifyIcon;

        // Can't use ApplicationContext's MainForm property because if it't closed, it ends the application!
        private Form form = new TrayTemplateForm();

        public void ShowForm()
        {
            // Created on first show, and optionally if form has been closed to save resources
            if (Form == null || Form.IsDisposed)
            {
                Form = new TrayTemplateForm();
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
            NotifyIcon = new NotifyIcon(container)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = TrayTemplate.Properties.Resources.TrayTemplate,
                Text = "Tray Application Template",
                Visible = true
            };

            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Show Winow", null, ShowWindow_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&About Tray Template", null, HelpAbout_Click));
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("E&xit", null, Exit_Click));

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
            if (disposing && container != null) 
            { 
                container.Dispose(); 
            }
        }

        protected override void ExitThreadCore()
        {
            if (MainForm != null && !MainForm.IsDisposed) 
            { 
                MainForm.Close(); 
            }

            // should remove lingering tray icon
            notifyIcon.Visible = false; 

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
            // TODO customize context menu strip here if needed
            e.Cancel = false;
        }
    }
}
