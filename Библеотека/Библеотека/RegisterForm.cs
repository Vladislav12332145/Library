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
    public partial class RegisterForm : Form
    {
        private const string AdminSecretCode = "ADMIN2024";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string secretCode = txtSecretCode.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните логин и пароль!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            // Проверяем через UserManager
            if (UserManager.UserExists(username))
            {
                MessageBox.Show("Этот логин уже занят!");
                return;
            }

            string role = "user";
            if (!string.IsNullOrEmpty(secretCode) && secretCode == AdminSecretCode)
            {
                role = "admin";
            }

            User newUser = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            // Добавляем через UserManager (автоматически сохраняется в файл)
            UserManager.AddUser(newUser);

            if (role == "admin")
            {
                MessageBox.Show("Регистрация администратора успешна!");
            }
            else
            {
                MessageBox.Show("Регистрация пользователя успешна!");
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void linkLabelAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            labelSecretCode.Visible = true;
            txtSecretCode.Visible = true;
            linkLabelAdmin.Visible = false;
        }
    }
}