using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace RegExTutorial
{
    public partial class Form1 : Form
    {
        // Note: Use Static Regex methods as much as possible
        // In this tool, Regex instance is instantiated primarily to retrieve group names.
        // See video on group name for details
        Regex _regex = null;
        Match _match = null;
        RegexOptions _regexOptions = RegexOptions.None;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            try
            {
                _regex = null;
                _match = null;
                txtResult.Text = string.Empty;
                /******************
                //
                // Note: Use Static Regex methods as much as possible
                // In this tool, Regex instance is instantiated primarily to retrieve group names.
                // Instantiating Regex instance for every click is a bad idea!  Do not use this approach for production code :-)
                // Since this is an interactive tool with pattern possibly changing between
                // every execution, Regex instance is created for every call.
                *******************/
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                _regex = new Regex(txtPattern.Text, _regexOptions);
                _match = _regex.Match(txtData.Text);

                stopWatch.Stop();
                highLightMatch(stopWatch.Elapsed);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }
        }

        private void btnNextMatch_Click(object sender, EventArgs e)
        {
            if (_regex != null && _match != null)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                _match = _match.NextMatch();

                stopWatch.Stop();
                highLightMatch(stopWatch.Elapsed);
            }
        }        

        private void highLightMatch(TimeSpan elapsed)
        {
            if (_regex != null && _match != null && _match.Success)
            {
                txtData.Focus();
                txtData.Select(_match.Index, _match.Length);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Match Found: {0}, Substring: {1}, Index: {2}, Length: {3}, Elapsed Time (ms): {4}{5}",
                 _match.Success, _match.Value, _match.Index, _match.Length, elapsed.TotalMilliseconds, Environment.NewLine);

                // Group 0 is default group that matches the complete expression
                if (_match.Groups.Count > 1)
                {
                    for (int i = 0; i < _match.Groups.Count; i++)
                    {
                        sb.AppendFormat("  Group Index:{0}, Name:{1}, Value:{2}", i, _regex.GroupNameFromNumber(i), _match.Groups[i].Value);

                        sb.AppendLine();
                    }
                }                  
                  
                txtResult.Text += sb.ToString();
            }      
            else
            {
                txtResult.Text += string.Format("END. Elapsed Time (ms): {0}{1}", elapsed.TotalMilliseconds, Environment.NewLine);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            txtResult.Text = string.Empty;

            try
            {
                txtResult.Text = Regex.Replace(txtData.Text, txtPattern.Text, txtReplacementPattern.Text, _regexOptions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            txtResult.Text = string.Empty;

            try
            {
                string[] split = Regex.Split(txtData.Text, txtPattern.Text, _regexOptions);

                StringBuilder sb = new StringBuilder();
                foreach (string s in split)
                {
                    sb.Append(s);
                    sb.Append(Environment.NewLine);
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }
        }

        private void chkRightToLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRightToLeft.Checked)
                _regexOptions = RegexOptions.RightToLeft;
            else
                _regexOptions = RegexOptions.None;
        }
    }
}
