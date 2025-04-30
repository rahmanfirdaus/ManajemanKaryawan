using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ManajemanKarywan
{
    public partial class FormDataKaryawan : Form
    {
        string connectionString = "server=localhost;user=root;password=;database=db_karyawan";

        public FormDataKaryawan()
        {
            InitializeComponent();
            this.Load += FormDataKaryawan_Load;
        }

        private void FormDataKaryawan_Load(object sender, EventArgs e)
        {
            LoadDataKaryawan();
        }

        private void LoadDataKaryawan(string filter = "")
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM karyawan";

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        query += " WHERE Nama LIKE @filter OR Jabatan LIKE @filter";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridViewKaryawan.DataSource = table;

                    if (dataGridViewKaryawan.Columns.Contains("id"))
                        dataGridViewKaryawan.Columns["id"].Visible = false;

                    // === Penyesuaian tampilan ===
                    dataGridViewKaryawan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewKaryawan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridViewKaryawan.AllowUserToResizeRows = false;
                    dataGridViewKaryawan.RowTemplate.Height = 24;
                    dataGridViewKaryawan.Font = new Font("Segoe UI", 9F);
                    dataGridViewKaryawan.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dataGridViewKaryawan.EnableHeadersVisualStyles = false;
                    dataGridViewKaryawan.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewKaryawan.GridColor = Color.LightSteelBlue;
                    dataGridViewKaryawan.RowHeadersVisible = false;
                    dataGridViewKaryawan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menampilkan data: " + ex.Message);
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string keyword = txtFilter.Text.Trim();
            LoadDataKaryawan(keyword);
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            LoadDataKaryawan(); // Menampilkan ulang semua data
        }

    }
}
