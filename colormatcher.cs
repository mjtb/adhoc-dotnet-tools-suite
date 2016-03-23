using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("WinForms tool to find the nearest standard system colour.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace colormatcher
{
    public class ColorMatcherForm : Form, System.Collections.IComparer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox ColorTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader HexColumn;
        private System.Windows.Forms.ColumnHeader RgbColumn;
        private System.Windows.Forms.ColumnHeader SwatchColumn;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem copyMenuItem;

        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Forms Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorMatcherForm));
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FindButton = new System.Windows.Forms.Button();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.HexColumn = new System.Windows.Forms.ColumnHeader();
            this.RgbColumn = new System.Windows.Forms.ColumnHeader();
            this.SwatchColumn = new System.Windows.Forms.ColumnHeader();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.copyMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Color:";
            //
            // listView1
            //
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.HexColumn,
            this.RgbColumn,
            this.SwatchColumn});
            this.listView1.ContextMenu = this.contextMenu1;
            this.listView1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(14, 68);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(395, 381);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listView1.TabIndex = 3;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            //
            // ColorTextBox
            //
            this.ColorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorTextBox.Location = new System.Drawing.Point(12, 25);
            this.ColorTextBox.Name = "ColorTextBox";
            this.ColorTextBox.Size = new System.Drawing.Size(316, 20);
            this.ColorTextBox.TabIndex = 1;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Matches:";
            //
            // FindButton
            //
            this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton.Location = new System.Drawing.Point(334, 23);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(75, 23);
            this.FindButton.TabIndex = 2;
            this.FindButton.Text = "Find";
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            //
            // NameColumn
            //
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 109;
            //
            // HexColumn
            //
            this.HexColumn.Text = "Hex";
            this.HexColumn.Width = 81;
            //
            // RgbColumn
            //
            this.RgbColumn.Text = "RGB";
            this.RgbColumn.Width = 105;
            //
            // SwatchColumn
            //
            this.SwatchColumn.Text = "Swatch";
            //
            // contextMenu1
            //
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.copyMenuItem});
            //
            // copyMenuItem
            //
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.copyMenuItem_Click);
            //
            // ColorMatcherForm
            //
            this.AcceptButton = this.FindButton;
            this.ClientSize = new System.Drawing.Size(421, 461);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ColorTextBox);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "ColorMatcherForm";
            this.Text = "Color Matcher";
            this.Load += new System.EventHandler(this.ColorMatcherForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static Regex hexregex = new Regex(@"\#?(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(<b>?[0-9A-Fa-f]{2})");
        private static Regex rgbregex = new Regex(@"(?<r>\d{1,3})\s*,\s*(?<g>\d{1,3})\s*,\s*(?<b>\d{1,3})");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new ColorMatcherForm());
        }

        public ColorMatcherForm()
        {
            InitializeComponent();
        }
        private static string GetHexString(Color c)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
        }
        private static string GetRgbString(Color c)
        {
            return string.Format("{0}, {1}, {2}", c.R, c.G, c.B);
        }
        private static Color Parse(string s)
        {
            s = s.Trim();
            Match m = hexregex.Match(s);
            if(m.Success)
            {
                return Color.FromArgb(int.Parse(m.Groups["r"].Value, System.Globalization.NumberStyles.HexNumber), int.Parse(m.Groups["g"].Value, System.Globalization.NumberStyles.HexNumber), int.Parse(m.Groups["b"].Value, System.Globalization.NumberStyles.HexNumber));
            }
            m = rgbregex.Match(s);
            if(m.Success)
            {
                return Color.FromArgb(int.Parse(m.Groups["r"].Value), int.Parse(m.Groups["g"].Value), int.Parse(m.Groups["b"].Value));
            }
            return Color.FromName(s);
        }
        class ItemTag
        {
            public Color c;
            public int f;
            public ItemTag(Color c, int f)
            {
                this.c = c;
                this.f = f;
            }
        }
        private static int Distance(Color a, Color b)
        {
            int x = a.R - b.R, y = a.G - b.G, z = a.B - b.B;
            return x * x + y * y + z * z;
        }
        private void ColorMatcherForm_Load(object sender, EventArgs e)
        {
            string[] names = Enum.GetNames(typeof(KnownColor));
            KnownColor[] constants = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
            for(int i = 0; i < constants.Length; i++)
            {
                Color c = Color.FromKnownColor(constants[i]);
                ListViewItem item = new ListViewItem();
                item.BackColor = this.listView1.BackColor;
                item.Font = this.listView1.Font;
                item.ForeColor = this.listView1.ForeColor;
                item.Text = names[i];
                item.UseItemStyleForSubItems = false;
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, GetHexString(c), item.ForeColor, item.BackColor, item.Font));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, GetRgbString(c), item.ForeColor, item.BackColor, item.Font));
                ListViewItem.ListViewSubItem subitem = new ListViewItem.ListViewSubItem();
                subitem.BackColor = c;
                subitem.ForeColor = item.ForeColor;
                subitem.Font = item.Font;
                item.SubItems.Add(subitem);
                item.Tag = new ItemTag(c, i);
                this.listView1.Items.Add(item);
            }
            this.listView1.ListViewItemSorter = this;
            this.listView1.Sort();
        }

        int System.Collections.IComparer.Compare(object x, object y)
        {
            return ((ItemTag) ((ListViewItem) x).Tag).f - ((ItemTag) ((ListViewItem) y).Tag).f;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            Color c = Parse(this.ColorTextBox.Text);
            foreach(ListViewItem item in this.listView1.Items)
            {
                ItemTag tag = (ItemTag) item.Tag;
                tag.f = Distance(c, tag.c);
            }
            this.listView1.Sort();
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            if(this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView1.SelectedItems[0];
                System.Windows.Forms.Clipboard.SetDataObject(string.Format("KnownColor.{0}\r\n\"{1}\"\r\nColor.FromArgb({2})",
                    item.Text, item.SubItems[1].Text, item.SubItems[2].Text), true);
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if((e.KeyCode == Keys.C) && (e.Modifiers == Keys.Control))
            {
                copyMenuItem_Click(this, EventArgs.Empty);
            }
        }
    }
}
