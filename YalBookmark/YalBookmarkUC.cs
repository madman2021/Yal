﻿using System;
using System.Windows.Forms;

namespace YalBookmark
{
    public partial class YalBookmarkUC : UserControl
    {
        private const string backendEnabledString = "True";
        private const string backendDisabledString = "False";

        public YalBookmarkUC()
        {
            InitializeComponent();

            cbUseBrowserIcon.Checked = Properties.Settings.Default.UseBrowserIcon;
            cbOpenWithProvider.Checked = Properties.Settings.Default.OpenWithProvider;


            foreach (var item in YalBookmark.browsers)
            {
                var dbPath = item.Value.GetDbPath();
                var lvi = new ListViewItem(new string[]
                { item.Key,
                  dbPath == null ? "not found" : dbPath,
                  Properties.Settings.Default.EnabledBackends.Contains(item.Key).ToString()
                });
                listViewBrowsers.Items.Add(lvi);
            }
        }

        internal void SaveSettings()
        {
            Properties.Settings.Default.EnabledBackends.Clear();
            foreach (ListViewItem item in listViewBrowsers.Items)
            {
                // the 'Backend enabled' column
                if (item.SubItems[2].Text == "True")
                {
                    Properties.Settings.Default.EnabledBackends.Add(item.SubItems[0].Text);
                }
            }
            Properties.Settings.Default.OpenWithProvider = cbOpenWithProvider.Checked;
            Properties.Settings.Default.UseBrowserIcon = cbUseBrowserIcon.Checked;
            Properties.Settings.Default.Save();
        }

        private void listViewBrowsers_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickedItem = listViewBrowsers.FocusedItem;

                var contextMenu = new ContextMenuStrip();
                var toggleBackendItem = new ToolStripMenuItem(clickedItem.SubItems[2].Text == backendEnabledString ? 
                                                              "Disable backend" : "Enable backend");
                toggleBackendItem.Click += toggleBackendItem_Click;
                contextMenu.Items.Add(toggleBackendItem);
                contextMenu.Show(Cursor.Position);
            }
        }

        private void toggleBackendItem_Click(object sender, EventArgs e)
        {
            ToggleBackend(listViewBrowsers.FocusedItem);
        }

        private void ToggleBackend(ListViewItem backend)
        {
            var name = backend.SubItems[0].Text;
            if (Properties.Settings.Default.EnabledBackends.Contains(name))
            {
                backend.SubItems[2].Text = backendDisabledString;
                Properties.Settings.Default.EnabledBackends.Remove(name);
            }
            else
            {
                backend.SubItems[2].Text = backendEnabledString;
                Properties.Settings.Default.EnabledBackends.Add(name);
            }
        }
    }
}
