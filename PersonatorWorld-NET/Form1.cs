using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace PersonatorWorld_NET
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            String RESTRequest = "";
            String Actions = "";
            String Options = "";

            // *************************************************************************************
            // Set the License String in the Request
            // *************************************************************************************
            RESTRequest += @"id=" + Uri.EscapeDataString(txtLicense.Text);

            // *************************************************************************************
            // Set the Actions in the Request
            // *************************************************************************************
            foreach (object itemChecked in actions.CheckedItems)
            {
                Actions += itemChecked.ToString() + ",";
            }

            // Remove last comma in Actions string
            if (Actions.Contains(","))
            {
                Actions = Actions.Substring(0, Actions.Length - 1);
            }

            RESTRequest += @"&actions=" + Actions;

            // *************************************************************************************
            // Set the Address Options in the Request
            // *************************************************************************************

            // Set the line separator option
            if (!string.IsNullOrEmpty(optLineSeparator.Text))
                Options += "LineSeparator:" + optLineSeparator.Text + ",";

            // Set the output script option
            if (!string.IsNullOrEmpty(optOutputScript.Text)) 
                Options += "OutputScript:" + optOutputScript.Text + ",";

            // Set the records per page returned option
            if (!string.IsNullOrEmpty(optCountryOfOrigin.Text))
                Options += "CountryOfOrigin:" + optCountryOfOrigin.Text + ",";

            // Set the delivery lines option
            if (optDeliveryLines.Checked)
            {
                Options += "DeliveryLines:On,";
            }

            // Set Options
            if (!string.IsNullOrEmpty(Options))
            {
                Options = Options.Substring(0, Options.Length - 1);
            }
            RESTRequest += @"&addrOpt=" + Options;


            // *************************************************************************************
            // Set the Input Parameters
            // *************************************************************************************
            RESTRequest += @"&full=" + Uri.EscapeDataString(txtFullIn.Text);

            RESTRequest += @"&a1=" + Uri.EscapeDataString(txtAddress1In.Text);
            RESTRequest += @"&a2=" + Uri.EscapeDataString(txtAddress2In.Text);
            RESTRequest += @"&a3=" + Uri.EscapeDataString(txtAddress3In.Text);
            RESTRequest += @"&a4=" + Uri.EscapeDataString(txtAddress4In.Text);
            RESTRequest += @"&a5=" + Uri.EscapeDataString(txtAddress5In.Text);
            RESTRequest += @"&a6=" + Uri.EscapeDataString(txtAddress6In.Text);
            RESTRequest += @"&a7=" + Uri.EscapeDataString(txtAddress7In.Text);
            RESTRequest += @"&a8=" + Uri.EscapeDataString(txtAddress8In.Text);
            RESTRequest += @"&loc=" + Uri.EscapeDataString(txtLocIn.Text);
            RESTRequest += @"&admarea=" + Uri.EscapeDataString(txtAdmareaIn.Text);
            RESTRequest += @"&postal=" + Uri.EscapeDataString(txtPostalIn.Text);
            RESTRequest += @"&ctry=" + Uri.EscapeDataString(txtCtryIn.Text);

            RESTRequest += @"&comp=" + Uri.EscapeDataString(txtCompanyIn.Text);
            RESTRequest += @"&nat=" + Uri.EscapeDataString(txtNatIn.Text);
            RESTRequest += @"&phone=" + Uri.EscapeDataString(txtPhoneIn.Text);
            RESTRequest += @"&email=" + Uri.EscapeDataString(txtEmailIn.Text);
            RESTRequest += @"&dob=" + Uri.EscapeDataString(txtDobIn.Text);

            // Set JSON Response Protocol
            RESTRequest += @"&format=json";

            // Build the final REST String Query
            RESTRequest = @"https://globalpersonator.melissadata.net/v1" + @"/doContactVerify?" + RESTRequest;

            // Output the REST Query
            txtRESTRequest.Text = RESTRequest;

            // *************************************************************************************
            // Submit to the Web Service. 
            // Make sure to set a retry block in case of any timeouts
            // *************************************************************************************
            Boolean Success = false;
            Int16 RetryCounter = 0;
            Stream ResponseReaderFile = null;
            do
            {
                try
                {
                    HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(RESTRequest);
                    WebResponse Response = WebRequestObject.GetResponse();
                    ResponseReaderFile = Response.GetResponseStream();
                    Success = true;
                }
                catch (Exception ex)
                {
                    RetryCounter++;
                    MessageBox.Show("Exception: " + ex.Message);
                    return;
                }
            } while ((Success != true) && (RetryCounter < 5));

            // *************************************************************************************
            // Output Formatted JSON String
            // *************************************************************************************
            StreamReader Reader = new StreamReader(ResponseReaderFile, Encoding.UTF8);
            String ResponseString = Reader.ReadToEnd();

            txtResponse.Text = JValue.Parse(ResponseString).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        // *************************************************************************************
        // Clear the Input Strings
        // *************************************************************************************
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Name input fields
            txtFullIn.Text = string.Empty;

            // Address input fields
            txtAddress1In.Text = string.Empty;
            txtAddress2In.Text = string.Empty;
            txtAddress3In.Text = string.Empty;
            txtAddress4In.Text = string.Empty;
            txtAddress5In.Text = string.Empty;
            txtAddress6In.Text = string.Empty;
            txtAddress7In.Text = string.Empty;
            txtAddress8In.Text = string.Empty;
            txtLocIn.Text = string.Empty;
            txtAdmareaIn.Text = string.Empty;
            txtPostalIn.Text = string.Empty;
            txtCtryIn.Text = string.Empty;

            // Other input fields
            txtPhoneIn.Text = string.Empty;
            txtCompanyIn.Text = string.Empty;
            txtEmailIn.Text = string.Empty;
            txtNatIn.Text = string.Empty;
            txtDobIn.Text = string.Empty;

            // Reset options to defaults
            optDeliveryLines.Checked = false;
            optCountryOfOrigin.Text = string.Empty;
            optLineSeparator.SelectedIndex = 0;
            optOutputScript.SelectedIndex = 0;

            // Reset actions to defaults
            for (int i = 0; i < actions.Items.Count; i++)
            {
                actions.SetItemChecked(i, false);
            }
            chkAllActions.Checked = false;
        }

        // *************************************************************************************
        // Wiki Link
        // *************************************************************************************
        private void lnkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://wiki.melissadata.com/index.php?title=Personator_World");
        }

        // *************************************************************************************
        // Check all Column Boxes
        // *************************************************************************************
        private void chkAllCols_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < actions.Items.Count; i++)
            {
                actions.SetItemChecked(i, chkAllActions.Checked);
            }
        }
    }
}
