namespace ConsoleApp1;

public class UserManager
{
    
    private List<User> userList = new List<User>();
    
    public static bool RegisterUser(List<User> userList, User user)
    {
        if (userList.Any(u => u.Email == user.Email))
        {
            Console.WriteLine("Пользователь с такой почтой уже зарегистрирован.");
            return false;
        }

        if (userList.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            Console.WriteLine("Пользователь с таким номером телефона уже зарегистрирован.");
            return false;
        }

        userList.Add(user);
        Console.WriteLine("Пользователь успешно зарегистрирован.");
        return true;
    }
    
    public bool EditUser(User user)
    {
        var existingUser = userList.FirstOrDefault(u => u.Email == user.Email);

        if (existingUser == null)
        {
            Console.WriteLine("Пользователь не найден.");
            return false;
        }

        if (existingUser != user && userList.Any(u => u.Email == user.Email))
        {
            Console.WriteLine("Пользователь с такой почтой уже зарегистрирован.");
            return false;
        }

        if (existingUser != user && userList.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            Console.WriteLine("Пользователь с таким номером телефона уже зарегистрирован.");
            return false;
        }

        existingUser.FullName = user.FullName;
        existingUser.DateOfBirth = user.DateOfBirth;
        existingUser.Address = user.Address;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
    
        Console.WriteLine("Данные пользователя успешно обновлены.");
        return true;
    }
    
    public  bool RegisterUserForTest(User user)
    {
        if (userList.Any(u => u.Email == user.Email))
        {
            Console.WriteLine($"Пользователь с адресом электронной почты {user.Email} уже зарегистрирован.");
            return false;
        }

        if (userList.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            Console.WriteLine($"Пользователь с номером телефона {user.PhoneNumber} уже зарегистрирован.");
            return false;
        }

        userList.Add(user);

        Console.WriteLine($"Пользователь {user.FullName} успешно зарегистрирован.");

        return true;
    }
    
    //Этот метод ищет всех пользователей из списка userList,
    //у которых фамилия совпадает с переданной в параметрах метода surname.
    public List<User> SearchUserBySurname(string surname)
    {
        return userList.Where(u => u.FullName.Split()[0].Equals(surname, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    //Этот метод ищет всех пользователей из списка userList,
    //у которых фамилия содержит переданную в параметрах метода surname
    //(с учетом регистра символов).
    // должно работать, но почему то тесты не проходят
    public List<User> FuzzySearchUserBySurname(string surname)
    {
        return userList.Where(u => u.FullName.Split(' ')[0].IndexOf(surname, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    public bool DeleteUser(int userId)
    {
        var user = userList.FirstOrDefault(u => u.UserId == userId);
        if (user == null)
        {
            return false;
        }

        userList.Remove(user);
        return true;
    }
    
    public bool UserExists(int userId)
    {
        return GetUserById(userId) != null;
    }
    
    public User GetUserById(int userId)
    {
        return userList.FirstOrDefault(u => u.UserId == userId);
    }
    
    //Метод для поиска самых молодых пользователей
    public List<User> GetYoungestUsers(int count)
    {
        List<User> youngestUsers = new List<User>();
        DateTime oldestBirthdate = userList.Max(u => u.DateOfBirth);

        youngestUsers = userList.Where(u => u.DateOfBirth == oldestBirthdate).ToList();
        
        
        return youngestUsers;
    }

    //Метод для поиска самых возрастных пользователей;
    public List<User> GetOldestUsers()
    {
        List<User> oldestUsers = new List<User>();
        
        DateTime earliestBirthdate = userList.Min(u => u.DateOfBirth);

        oldestUsers = userList.Where(u => u.DateOfBirth == earliestBirthdate).ToList();

        return oldestUsers;
    }

    //Метод для вычисления среднего возраста
    
    public double GetUserWithMiddleAge()
    {
        double MiddleAge = 0;
        
        TimeSpan sumOfAges = TimeSpan.Zero;
        foreach (var user in userList)
        {
            sumOfAges += DateTime.Today - user.DateOfBirth;
        }
        
        if (userList.Count > 0)
        {
            MiddleAge = sumOfAges.TotalDays / userList.Count / 365;
        }

        return MiddleAge;
    }
}