using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Библеотека
{
    public static class UserManager
    {
        private static string usersFile = "users.xml";
        private static List<User> _users;

        public static List<User> Users
        {
            get
            {
                if (_users == null)
                {
                    LoadUsers();
                }
                return _users;
            }
        }

        // Загружаем пользователей из файла
        private static void LoadUsers()
        {
            if (File.Exists(usersFile))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                    using (FileStream stream = new FileStream(usersFile, FileMode.Open))
                    {
                        _users = (List<User>)serializer.Deserialize(stream);
                    }
                }
                catch
                {
                    CreateDefaultUsers();
                }
            }
            else
            {
                CreateDefaultUsers();
            }
        }

        // Сохраняем пользователей в файл
        public static void SaveUsers()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream stream = new FileStream(usersFile, FileMode.Create))
                {
                    serializer.Serialize(stream, Users);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ошибка сохранения пользователей: {ex.Message}");
            }
        }

        // Создаем стандартных пользователей
        private static void CreateDefaultUsers()
        {
            _users = new List<User>
            {
                new User { Username = "admin", Password = "admin123", Role = "admin" },
                new User { Username = "user", Password = "user123", Role = "user" }
            };
            SaveUsers();
        }

        // Добавляем нового пользователя
        public static void AddUser(User newUser)
        {
            if (!UserExists(newUser.Username))
            {
                Users.Add(newUser);
                SaveUsers();
            }
        }

        // Проверяем существование пользователя
        public static bool UserExists(string username)
        {
            return Users.Any(u => u.Username == username);
        }

        // Получаем пользователя по логину и паролю
        public static User GetUser(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}