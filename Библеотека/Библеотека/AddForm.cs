using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Библеотека
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            {
                MainForm main = this.Owner as MainForm;
                if (main != null)
                {
                    DataRow nRow = main.базаДанныхDataSet.Tables[0].NewRow();
                    int rc = main.dataGridView1.RowCount + 1;
                    nRow[0] = rc;
                    nRow[1] = textBox1.Text;
                    nRow[2] = textBox2.Text;
                    nRow[3] = textBox3.Text;
                    
                    main.базаДанныхDataSet.Tables[0].Rows.Add(nRow);
                    main.booksTableAdapter.Update(main.базаДанныхDataSet.Books);
                    main.базаДанныхDataSet.Tables[0].AcceptChanges();
                    main.dataGridView1.Refresh();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    
                }
            }
        }
    }
}
