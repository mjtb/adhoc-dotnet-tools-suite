using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Displays a navigatable WinForms calendar.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class CalendarForm : Form
{
	[STAThread]
	static void Main(string[] args)
	{
		if(0 == args.Length)
		{
			Application.Run(new CalendarForm(DateTime.Today));
		}
		else
		{
			Application.Run(new CalendarForm(DateTime.Parse(string.Join(" ", args))));
		}
	}

	MonthCalendar mc;

	CalendarForm(DateTime date) : base()
	{
		ResourceManager res = new ResourceManager(typeof(CalendarForm));
		this.Icon = res.GetObject("$this.Icon") as Icon;
		this.SuspendLayout();
		this.mc = new MonthCalendar();
		this.mc.Dock = DockStyle.Fill;
		this.mc.SetDate(date);
		this.Controls.Add(this.mc);
		this.ClientSize = this.mc.SingleMonthSize + new Size(23,3);
		this.Text = "Calendar";
		this.ResumeLayout(false);
	}
}
