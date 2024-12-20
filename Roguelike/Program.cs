using System;
using System.Collections.Generic;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать на поле битвы!");
        Console.WriteLine("Назови себя:");
        Console.Write("> ");
        string playerName = Console.ReadLine();

        Random random = new Random();

      
        Player player = new Player(playerName, 100, new Aid("Средняя аптечка", 10), GenerateRandomWeapon(random));
        Console.WriteLine($"\nВаше имя {player.Name}!");
        Console.WriteLine($"Вы получили меч {player.Weapon.Name} ({player.Weapon.Damage}), а также {player.Aid.Name} ({player.Aid.HealthAmount}hp).");
        Console.WriteLine($"У вас {player.Health}hp.\n");

        while (player.Health > 0)
        {
            Enemy enemy = GenerateRandomEnemy(random);
            Console.WriteLine($"{player.Name} встречает врага {enemy.Name} ({enemy.Health}hp), враг вооружен {enemy.Weapon.Name} ({enemy.Weapon.Damage})");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine("\nЧто вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Использовать аптечку");
                Console.WriteLine("3. Пропустить ход");
                Console.Write("> ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": 
                        player.Attack(enemy);
                        if (enemy.Health > 0)
                        {
                            enemy.Attack(player);
                        }
                        break;

                    case "3": 
                        Console.WriteLine($"**{player.Name}** пропустил ход!");
                        enemy.Attack(player);
                        break;

                    case "2": 
                        player.UseAid();
                        break;

                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте снова.");
                        break;
                }

                Console.WriteLine($"У противника {enemy.Health}hp, у вас {player.Health}hp\n");
            }

            if (player.Health > 0)
            {
                Console.WriteLine($" {player.Name} победил врага {enemy.Name} и получает 10 очков!\n");
                player.Score += 10;
                Console.WriteLine($"Ваши очки: {player.Score}\n");
            }
            else
            {
                Console.WriteLine($" {player.Name}  проиграл... Ваш  счет: {player.Score}");
                break;
            }
        }
    }

    static Weapon GenerateRandomWeapon(Random random)
    {
        List<Weapon> weapons = new List<Weapon>
        {
            new Weapon("Фростморн", 20, 100),
            new Weapon("Бич Драконов", 25, 100),
            new Weapon("Меч", 15, 100)
        };

        return weapons[random.Next(weapons.Count)];
    }

    static Enemy GenerateRandomEnemy(Random random)
    {
        List<string> enemyNames = new List<string> { "Варвар", "Гоблин", "Орк", "Тролль", "Некромант" };
        Weapon enemyWeapon = GenerateRandomWeapon(random);
        int enemyHealth = random.Next(40, 80);

        return new Enemy(enemyNames[random.Next(enemyNames.Count)], enemyHealth, enemyWeapon);
    }
}
class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public Aid Aid { get; set; }
    public Weapon Weapon { get; set; }
    public int Score { get; set; }

    public Player(string name, int maxHealth, Aid aid, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Aid = aid;
        Weapon = weapon;
        Score = 0;
    }


    public void Attack(Enemy enemy)
    {
        Console.WriteLine($" {Name} атаковал противника {enemy.Name}!");
        enemy.Health -= Weapon.Damage;
        if (enemy.Health < 0) enemy.Health = 0;
    }

    public void UseAid()
    {
        if (Aid != null)
        {
            Health += Aid.HealthAmount;
            if (Health > MaxHealth) Health = MaxHealth;
            Console.WriteLine($" {Name} использовал аптечку и восстановил {Aid.HealthAmount}hp!");
            Aid = null; 
        }
        else
        {
            Console.WriteLine("У вас нет аптечек!");
        }
    }
}
class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }

    public Enemy(string name, int health, Weapon weapon)
    {
        Name = name;
        Health = health;
        Weapon = weapon;
    }

    public void Attack(Player player)
    {
        Console.WriteLine($"Противник {Name} ударил вас!");
        player.Health -= Weapon.Damage;
        if (player.Health < 0) player.Health = 0;
    }
}
class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Durability { get; set; }

    public Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
    }
}
class Aid
{
    public string Name { get; set; }
    public int HealthAmount { get; set; }

    public Aid(string name, int healAmount)
    {
        Name = name;
        HealthAmount = healAmount;
    }
}
