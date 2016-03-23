using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Displays a list of standard color constant names and RGB triplet values.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace colorpicker
{
	public class ColorPickerMainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView webListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.ComponentModel.Container components = null;

		public ColorPickerMainForm()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if(disposing)
			{
				if(null != this.components)
				{
					this.components.Dispose();
					this.components = null;
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColorPickerMainForm));
			this.webListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			//
			// webListView
			//
			this.webListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader1,
																							this.columnHeader2,
																							this.columnHeader3});
			this.webListView.ContextMenu = this.contextMenu1;
			this.webListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webListView.FullRowSelect = true;
			this.webListView.GridLines = true;
			this.webListView.Location = new System.Drawing.Point(0, 0);
			this.webListView.Name = "webListView";
			this.webListView.Size = new System.Drawing.Size(292, 266);
			this.webListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.webListView.TabIndex = 2;
			this.webListView.View = System.Windows.Forms.View.Details;
			this.webListView.ItemActivate += new System.EventHandler(this.webListView_ItemActivate);
			//
			// columnHeader1
			//
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 122;
			//
			// columnHeader2
			//
			this.columnHeader2.Text = "Hex";
			//
			// columnHeader3
			//
			this.columnHeader3.Text = "Decimal";
			this.columnHeader3.Width = 79;
			//
			// contextMenu1
			//
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.menuItem1});
			//
			// menuItem1
			//
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Copy\tCtrl+C";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			//
			// MainForm
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.webListView);
			this.Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
			this.Name = "ColorPickerMainForm";
			this.Text = "Color Picker";
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		public static void Main()
		{
			Application.Run(new ColorPickerMainForm());
		}

		public static string ToHexColorString(System.Drawing.Color color)
		{
			return String.Format("#{0:x}{1:x}{2:x}{3:x}{4:x}{5:x}", (color.R >> 4) & 15, color.R & 15, (color.G >> 4) & 15, color.G & 15, (color.B >> 4) & 15, color.B & 15);
		}

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			System.Type t = typeof(System.Drawing.KnownColor);
			foreach(string n in System.Enum.GetNames(t))
			{
				System.Drawing.Color c = System.Drawing.Color.FromKnownColor((System.Drawing.KnownColor) Enum.Parse(t, n, true));
				System.Windows.Forms.ListViewItem i = new System.Windows.Forms.ListViewItem();
				i.Text = n;
				i.SubItems.Add(ToHexColorString(c));
				i.SubItems.Add(System.String.Format("{0}, {1}, {2}", c.R, c.G, c.B));
				i.BackColor = c;
				i.ForeColor = System.Drawing.Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
				this.webListView.Items.Add(i);
			}
		}

		private void webListView_ItemActivate(object sender, System.EventArgs e)
		{
			System.Text.StringBuilder b = new System.Text.StringBuilder();
			bool neednewline = false;
			foreach(System.Windows.Forms.ListViewItem i in this.webListView.SelectedItems)
			{
				if(neednewline)
				{
					b.Append("\r\n");
				}
				else
				{
					neednewline = true;
				}
				bool needsemicolon = false;
				foreach(ListViewItem.ListViewSubItem s in i.SubItems)
				{
					if(needsemicolon)
					{
						b.Append("; ");
					}
					else
					{
						needsemicolon = true;
					}
					b.Append(s.Text);
				}
			}
			System.Windows.Forms.Clipboard.SetDataObject(b.ToString(), true);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			this.webListView_ItemActivate(this.webListView, e);
		}
	}
}
