﻿using System;
using System.Data;
using System.Drawing;
using PluginInterfaces;
using System.Windows.Forms;

using Utilities;

namespace YalCalc
{
    public class YalCalc : IPlugin
    {
        public string Name { get; } = "YalCalc";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Perform simple calculations using Yal";

        public Icon PluginIcon { get; }
        public bool FileLikeOutput { get; } = false;

        private YalCalcUC CalcPluginInstance { get; set; }

        public YalCalc()
        {
            PluginIcon = Utils.GetPluginIcon(Name);
        }

        public void SaveSettings()
        {
            CalcPluginInstance.SaveSettings();
        }

        public UserControl GetUserControl()
        {
            if (CalcPluginInstance == null || CalcPluginInstance.IsDisposed)
            {
                CalcPluginInstance = new YalCalcUC();
            }
            return CalcPluginInstance;
        }

        public string[] GetResults(string input, out string[] itemInfo)
        {
            itemInfo = null;

            var dt = new DataTable();
            try
            {
                double result = Convert.ToDouble(dt.Compute(input.Substring(1), filter: ""));
                return new string[] { Convert.ToString(Math.Round(result, Properties.Settings.Default.DecimalPlaces)) };
            }
            catch
            {
            }
            return new string[0];
        }

        public void HandleExecution(string input)
        {
            if (Properties.Settings.Default.ReplaceClipboard)
            {
                Clipboard.SetText(input);
            }
        }
    }
}
