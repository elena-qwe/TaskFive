using ConsoleApp1;
using Program = Microsoft.VisualStudio.TestPlatform.TestHost.Program;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public static void TestRegisterUser()
    {
        var userList = new List<User>();
        var user = new User { 
            FullName = "Иванов Иван Иванович", 
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "Москва",
            Email = "ivanov@mail.ru",
            PhoneNumber = "+7 (999) 123-45-67"
        };

        // Регистрация нового пользователя
        Assert.IsTrue(UserManager.RegisterUser(userList, user));

        // Повторная регистрация с тем же e-mail
        Assert.IsFalse(UserManager.RegisterUser(userList, user));

        // Повторная регистрация с тем же номером телефона
        user.Email = "ivanov2@mail.ru";
        Assert.IsFalse(UserManager.RegisterUser(userList, user));
    }

    [Test]
    public static void TestEditUser()
    {
        var userManager = new UserManager();
        var user1 = new User
        {
            FullName = "Иванов Иван Иванович",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "Москва",
            Email = "ivanov@mail.ru",
            PhoneNumber = "+7 (999) 123-45-67"
        };
        userManager.RegisterUserForTest(user1);

        var user2 = new User
        {
            FullName = "Петров Петр Петрович",
            DateOfBirth = new DateTime(1985, 5, 5),
            Address = "Санкт-Петербург",
            Email = "petrov@mail.ru",
            PhoneNumber = "+7 (999) 765-43-21"
        };
        userManager.RegisterUserForTest(user2);

        // Изменение данных существующего пользователя
        user1.Address = "Новый адрес";
        Assert.IsTrue(userManager.EditUser(user1));

        // Изменение e-mail на свой же
        user1.Email = "ivanov@mail.ru";
        Assert.IsTrue(userManager.EditUser(user1));

        // Изменение e-mail на новый свободный
        user1.Email = "ivanov3@mail.ru";
        Assert.IsTrue(userManager.EditUser(user1));

        // Изменение номера телефона на занятый другим пользователем
        user1.PhoneNumber = "+7 (999) 765-43-21";
        Assert.IsTrue(userManager.EditUser(user1));

        // Изменение номера телефона на свой же
        user1.PhoneNumber = "+7 (999) 123-45-67";
        Assert.IsTrue(userManager.EditUser(user1));
    }
    
    [Test]
    public void TestSearchUserBySurname()
    {
        var userManager = new UserManager();

        var user1 = new User
        {
            FullName = "Иванов Иван Иванович",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "Москва",
            Email = "ivanov@mail.ru",
            PhoneNumber = "+7 (999) 123-45-67"
        };
        userManager.RegisterUserForTest(user1);

        var user2 = new User
        {
            FullName = "Петров Петр Петрович",
            DateOfBirth = new DateTime(1985, 5, 5),
            Address = "Санкт-Петербург",
            Email = "petrov@mail.ru",
            PhoneNumber = "+7 (999) 765-43-21"
        };
        userManager.RegisterUserForTest(user2);

        var user3 = new User
        {
            FullName = "Иванов Александр Иванович",
            DateOfBirth = new DateTime(1990, 10, 10),
            Address = "Казань",
            Email = "ivanov2@mail.ru",
            PhoneNumber = "+7 (999) 111-22-33"
        };
        userManager.RegisterUserForTest(user3);

        var user4 = new User
        {
            FullName = "Сидоров Сидор Сидорович",
            DateOfBirth = new DateTime(1995, 3, 15),
            Address = "Новосибирск",
            Email = "sidorov@mail.ru",
            PhoneNumber = "+7 (999) 555-44-33"
        };
        userManager.RegisterUserForTest(user4);

        var searchResult = userManager.SearchUserBySurname("Иванов");

        Assert.AreEqual(2, searchResult.Count);
        Assert.IsTrue(searchResult.Any(u => u.FullName == user1.FullName));
        Assert.IsTrue(searchResult.Any(u => u.FullName == user3.FullName));
    } 
    
    [Test]
    public void TestFuzzySearchUserBySurname()
    {
        var userManager = new UserManager();

        var user1 = new User
        {
            FullName = "Иванов Иван Иванович",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "Москва",
            Email = "ivanov@mail.ru",
            PhoneNumber = "+7 (999) 123-45-67"
        };
        userManager.RegisterUserForTest(user1);

        var user2 = new User
        {
            FullName = "Петров Петр Петрович",
            DateOfBirth = new DateTime(1985, 5, 5),
            Address = "Санкт-Петербург",
            Email = "petrov@mail.ru",
            PhoneNumber = "+7 (999) 765-43-21"
        };
        userManager.RegisterUserForTest(user2);

        var user3 = new User
        {
            FullName = "Иванов Александр Иванович",
            DateOfBirth = new DateTime(1990, 10, 10),
            Address = "Казань",
            Email = "ivanov2@mail.ru",
            PhoneNumber = "+7 (999) 111-22-33"
        };
        userManager.RegisterUserForTest(user3);

        var user4 = new User
        {
            FullName = "Сидоров Сидор Сидорович",
            DateOfBirth = new DateTime(1995, 3, 15),
            Address = "Новосибирск",
            Email = "sidorov@mail.ru",
            PhoneNumber = "+7 (999) 555-44-33"
        };
        userManager.RegisterUserForTest(user4);

        var searchResult = userManager.FuzzySearchUserBySurname("ро");

        Assert.AreEqual(3, searchResult.Count);
        Assert.IsTrue(searchResult.Any(u => u.FullName == user1.FullName));
        Assert.IsTrue(searchResult.Any(u => u.FullName == user2.FullName));
        Assert.IsTrue(searchResult.Any(u => u.FullName == user4.FullName));
    }
    
    [Test]
    public void DeleteUser_ValidUserId_ReturnsTrue()
    {
        var user = new User { UserId = 1, FullName = "John Smith" };
        var userManager = new UserManager();
        userManager.RegisterUserForTest(user);

        var result = userManager.DeleteUser(user.UserId);

        Assert.IsTrue(result);
        Assert.IsFalse(userManager.UserExists(user.UserId));
    }
    
    [Test]
    public void GetYoungestUsers_ShouldReturnYoungestUsers()
    {
        var userManager = new UserManager();
        var user1 = new User
        {
            FullName = "Иванов Иван Иванович",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "Москва",
            Email = "ivanov@mail.ru",
            PhoneNumber = "+7 (999) 123-45-67"
        };
        userManager.RegisterUserForTest(user1);

        var user2 = new User
        {
            FullName = "Петров Петр Петрович",
            DateOfBirth = new DateTime(1985, 5, 5),
            Address = "Санкт-Петербург",
            Email = "petrov@mail.ru",
            PhoneNumber = "+7 (999) 765-43-21"
        };
        userManager.RegisterUserForTest(user2);

        var user3 = new User
        {
            FullName = "Иванов Александр Иванович",
            DateOfBirth = new DateTime(1990, 10, 10),
            Address = "Казань",
            Email = "ivanov2@mail.ru",
            PhoneNumber = "+7 (999) 111-22-33"
        };
        userManager.RegisterUserForTest(user3);

        var user4 = new User
        {
            FullName = "Сидоров Сидор Сидорович",
            DateOfBirth = new DateTime(1995, 3, 15),
            Address = "Новосибирск",
            Email = "sidorov@mail.ru",
            PhoneNumber = "+7 (999) 555-44-33"
        };
        userManager.RegisterUserForTest(user4);
        
        var result = userManager.GetYoungestUsers(2);

        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Any(u => u.FullName == "Иванов Александр Иванович"));
        Assert.IsTrue(result.Any(u => u.FullName == "Сидоров Сидор Сидорович"));
    }
    
    
}