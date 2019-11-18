using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETFTestTask
{
	public partial class Form1 : Form
	{
		private string connectionString = "Server= localhost\\sqlexpress; Database= ETFTestTaskBase; Integrated Security=True;";

		public Form1()
		{
			InitializeComponent();
		}

		private void connectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String sql = "SELECT id, parent_id, name FROM catalog_level;";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								int id = reader.GetSqlInt32(0).Value;
								System.Data.SqlTypes.SqlInt32 parent_id = reader.GetSqlInt32(1);

								string name = reader.GetString(2);

								treeView1.BeginUpdate();

								if (parent_id.IsNull)
								{
									treeView1.Nodes.Add(id.ToString(), name);
								}
								else
								{
									TreeNode[] treeNodes = treeView1.Nodes.Find(parent_id.Value.ToString(), true);
									for (int i = 0; i < treeNodes.Length; i++)
									{
										treeNodes[i].Nodes.Add(id.ToString(), name);
									}
								}
								treeView1.EndUpdate();
							}
						}
					}
				}
			}
			catch (SqlException exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}
	}
}
