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

namespace SMDBB_TabelAdmin
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=LAPTOP-97JFDRMG\\AZIZAH_A_AZMI;Initial Catalog=DonasiBarangBekas;user id = sa; password=@@SuguruMyLove777";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearForm()
        {
            txtIDAdmin.Clear();
            txtNamaAdmin.Clear();
            txtPasswd.Clear();

            txtIDAdmin.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_Admin AS [ID Admin], Nama_Admin AS [Nama Admin], Passwd AS [Password] FROM Admin";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAdmin.AutoGenerateColumns = true;
                    dgvAdmin.DataSource = dt;

                    ClearForm(); // Kosongkan inputan
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonTambah(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (txtIDAdmin.Text == "" || txtNamaAdmin.Text == "" || txtPasswd.Text == "")
                    {
                        MessageBox.Show("Harap isi semua data", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    conn.Open();
                    string query = "INSERT INTO Admin (ID_Admin, Nama_Admin, Passwd) VALUES (@ID_Admin, @Nama_Admin, @Passwd)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID_Admin", txtIDAdmin.Text.Trim());
                    cmd.Parameters.AddWithValue("@Nama_Admin", txtNamaAdmin.Text.Trim());
                    cmd.Parameters.AddWithValue("@Passwd", txtPasswd.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil ditambahkan!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonHapus(object sender, EventArgs e)
        {
            if (dgvAdmin.SelectedRows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            string id = dgvAdmin.SelectedRows[0].Cells["ID_Admin"].Value.ToString();
                            string query = "DELETE FROM Admin WHERE ID_Admin = @ID_Admin";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                conn.Open();
                                cmd.Parameters.AddWithValue("@ID_Admin", id);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearForm(); // Kosongkan form
                                    LoadData();  // Tampilkan data terbaru
                                }
                                else
                                {
                                    MessageBox.Show("Data tidak ditemukan atau gagal dihapus!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonRefresh(object sender, EventArgs e)
        {
            LoadData();

            MessageBox.Show($"Jumlah Kolom: {dgvAdmin.ColumnCount}\nJumlah Baris: {dgvAdmin.RowCount}",
                "Debugging DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
