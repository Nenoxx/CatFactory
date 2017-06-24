using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cat_Factory
{
    public partial class HelpWindow : Form
    {
        public HelpWindow()
        {
            InitializeComponent();
            Background.SendToBack();
            Background.Controls.Add(GotItFam);
            Background.Controls.Add(Instructions);

            GotItFam.BackColor = Color.Transparent;
            Instructions.BackColor = Color.Transparent;
        }

        private void GotItFam_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
