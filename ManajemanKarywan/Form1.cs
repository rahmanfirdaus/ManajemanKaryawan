using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ManajemanKarywan
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;user=root;password=;database=db_karyawan";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbJenisKelamin.Items.Add("Laki-laki");
            cmbJenisKelamin.Items.Add("Perempuan");
            cmbJenisKelamin.SelectedIndex = 0;
        }

        private void LoadData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM karyawan";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    if (dataGridView1.Columns.Contains("id"))
                        dataGridView1.Columns["id"].Visible = false;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridView1.AllowUserToResizeRows = false;
                    dataGridView1.RowTemplate.Height = 24;
                    dataGridView1.Font = new Font("Segoe UI", 9F);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.GridColor = Color.LightSteelBlue;
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void CariData(string keyword)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM karyawan WHERE Nama LIKE @keyword OR Jabatan LIKE @keyword";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    if (dataGridView1.Columns.Contains("id"))
                        dataGridView1.Columns["id"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat mencari data: " + ex.Message);
                }
            }
        }




        private void ClearForm()
        {
            txtNama.Text = "";
            txtJabatan.Text = "";
            txtGaji.Text = "";
            dateTanggalLahir.Value = DateTime.Today;
            cmbJenisKelamin.SelectedIndex = 0;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO karyawan (Nama, Jabatan, gaji, tanggal_lahir, jenis_kelamin) " +
                                   "VALUES (@Nama, @Jabatan, @gaji, @tanggal_lahir, @jenis_kelamin)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                    cmd.Parameters.AddWithValue("@Jabatan", txtJabatan.Text);
                    cmd.Parameters.AddWithValue("@gaji", Convert.ToInt32(txtGaji.Text));
                    cmd.Parameters.AddWithValue("@tanggal_lahir", dateTanggalLahir.Value.Date);
                    cmd.Parameters.AddWithValue("@jenis_kelamin", cmbJenisKelamin.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil ditambahkan!");
                    LoadData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE karyawan SET Nama=@Nama, jabatan=@jabatan, gaji=@gaji, tanggal_lahir=@tanggal_lahir, jenis_kelamin=@jenis_kelamin WHERE id=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Jabatan", txtJabatan.Text);
                        cmd.Parameters.AddWithValue("@gaji", Convert.ToInt32(txtGaji.Text));
                        cmd.Parameters.AddWithValue("@tanggal_lahir", dateTanggalLahir.Value.Date);
                        cmd.Parameters.AddWithValue("@jenis_kelamin", cmbJenisKelamin.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diperbarui!");
                        LoadData();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih baris yang ingin diupdate.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM karyawan WHERE id=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil dihapus!");
                        LoadData();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih baris yang ingin dihapus.");
            }
        }
        private void btnCari_Click(object sender, EventArgs e)
        {
            string keyword = txtCari.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                CariData(keyword);
            }
            else
            {
                MessageBox.Show("Masukkan kata kunci untuk mencari.");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCari.Text = "";
            LoadData();
        }

        private void btnLihatSemua_Click(object sender, EventArgs e)
        {
            FormDataKaryawan formData = new FormDataKaryawan();
            formData.ShowDialog();
        }




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                txtJabatan.Text = row.Cells["Jabatan"].Value.ToString();
                txtGaji.Text = row.Cells["gaji"].Value.ToString();

                if (row.Cells["tanggal_lahir"].Value != DBNull.Value)
                    dateTanggalLahir.Value = Convert.ToDateTime(row.Cells["tanggal_lahir"].Value);
                else
                    dateTanggalLahir.Value = DateTime.Today;

                // Menyesuaikan ComboBox dengan data jenis_kelamin yang ada di row
                cmbJenisKelamin.SelectedItem = row.Cells["jenis_kelamin"].Value?.ToString() ?? "Laki-laki";
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
