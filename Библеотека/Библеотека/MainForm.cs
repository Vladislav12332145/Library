using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Библеотека
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CheckUserPermissions();
        }

        private void CheckUserPermissions()
        {
            string role = LoginForm.CurrentUserRole;

            if (role == "user")
            {
                SaveButton.Visible = false;
                button1.Visible = false;

                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToOrderColumns = false;

                this.Text = "Библиотека - Просмотр (Пользователь)";
            }
            else if (role == "admin")
            {
                this.Text = "Библиотека - Администратор";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // НОВЫЙ МЕТОД: Загрузка данных
        private void LoadData()
        {
            try
            {
                this.booksTableAdapter.Fill(this.базаДанныхDataSet.Books);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (LoginForm.CurrentUserRole != "admin")
            {
                MessageBox.Show("Нет прав для сохранения!");
                return;
            }

            SaveData();
        }

        // НОВЫЙ МЕТОД: Сохранение данных
        private void SaveData()
        {
            try
            {
                // Завершаем редактирование
                dataGridView1.EndEdit();

                // Проверяем есть ли изменения
                if (базаДанныхDataSet.HasChanges())
                {
                    // Сохраняем изменения в базу данных
                    booksTableAdapter.Update(базаДанныхDataSet.Books);

                    // Подтверждаем изменения в DataSet
                    базаДанныхDataSet.AcceptChanges();

                    MessageBox.Show("Изменения сохранены в базу данных!", "Успех",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}\n\nИзменения не сохранены!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Откатываем изменения в DataSet
                базаДанныхDataSet.RejectChanges();
                LoadData(); // Перезагружаем данные
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (LoginForm.CurrentUserRole != "admin")
            {
                MessageBox.Show("Нет прав для удаления!");
                e.Cancel = true;
                return;
            }

            DialogResult dr = MessageBox.Show("Удалить запись из базы данных?", "Удаление",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LoginForm.CurrentUserRole != "admin")
            {
                MessageBox.Show("Нет прав для добавления!");
                return;
            }

            AddForm af = new AddForm();
            af.Owner = this;
            af.ShowDialog();

            // После закрытия формы добавления обновляем данные
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchForm sf = new SearchForm();
            sf.Owner = this;
            sf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем есть ли несохраненные изменения
            if (базаДанныхDataSet.HasChanges())
            {
                DialogResult result = MessageBox.Show("Есть несохраненные изменения. Сохранить перед выходом?",
                                                    "Подтверждение",
                                                    MessageBoxButtons.YesNoCancel,
                                                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveData();
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Отменяем выход
                }
            }

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        // НОВАЯ КНОПКА: Обновить данные
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (базаДанныхDataSet.HasChanges())
            {
                DialogResult result = MessageBox.Show("Есть несохраненные изменения. Обновить данные без сохранения?",
                                                    "Подтверждение",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            LoadData();
            MessageBox.Show("Данные обновлены!", "Информация",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // НОВАЯ КНОПКА: Отменить изменения
        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            if (базаДанныхDataSet.HasChanges())
            {
                DialogResult result = MessageBox.Show("Отменить все несохраненные изменения?",
                                                    "Подтверждение",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    базаДанныхDataSet.RejectChanges();
                    MessageBox.Show("Изменения отменены!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Нет изменений для отмены.", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Проверяем несохраненные изменения при закрытии формы
                if (базаДанныхDataSet.HasChanges())
                {
                    DialogResult result = MessageBox.Show("Есть несохраненные изменения. Сохранить перед выходом?",
                                                        "Подтверждение",
                                                        MessageBoxButtons.YesNoCancel,
                                                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SaveData();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true; // Отменяем закрытие
                    }
                }
            }
        }
    }
}