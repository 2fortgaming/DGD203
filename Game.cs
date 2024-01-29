using System;
using System.Collections.Generic;

class Game
{
    #region Variables

    static int playerX = 0;
    static int playerY = 0;
    static List<int> monsterXList = new List<int>();
    static List<int> monsterYList = new List<int>();
    static int diamondX;
    static int diamondY;
    static int playerHealth = 100;
    static int mapSize = 7;

    #endregion

    #region Main Method

    static void Main()
    {
        Console.WriteLine("------------------ Diamond Hunter ------------------");
        Console.WriteLine("====================================================");
        Console.WriteLine("You are an explorer who wants to find the Diamond in the cave!");
        Console.WriteLine("But there will be Monsters in the cave and you have to fight them!");
        Console.WriteLine(" \n\n\n         PRESS ANY BUTTON TO START                    ");
        Console.ReadKey();

        InitializeGame();
        while (true)
        {
            PrintLocation();
            Console.Write("Enter a command (N: North, S: South, W: West, E: East, A: Attack): ");
            string command1 = Console.ReadLine()?.ToUpper();

            if (command1 == null)
            {
                Console.WriteLine("Invalid command! Please enter a valid command.");
                continue;
            }

            if (command1 == "A")
            {
                if (IsMonsterAt(playerX, playerY))
                {
                    Battle();
                }
                else
                {
                    Console.WriteLine("There is no monster to attack.");
                }
            }
            else
            {
                Move(command1);
            }
        }
    }

    #endregion

    #region Game Initialization

    static void InitializeGame()
    {
        playerHealth = 100;

        Random rand = new();

        // Player Starting Coordinates
        playerX = 0;
        playerY = 0;

        // Monsters random locations
        InitializeMonsters(5); // İki canavar ekledik

        // Diamonds random location
        diamondX = rand.Next(mapSize);
        diamondY = rand.Next(mapSize);
    }

    #endregion

    #region Monster Initialization

    static void InitializeMonsters(int count)
    {
        Random rand = new();

        monsterXList.Clear();
        monsterYList.Clear();

        for (int i = 0; i < count; i++)
        {
            int monsterX = rand.Next(mapSize);
            int monsterY = rand.Next(mapSize);

            // Adding monster if there is not a monster or a player there
            if ((monsterX != playerX || monsterY != playerY) && !IsMonsterAt(monsterX, monsterY))
            {
                monsterXList.Add(monsterX);
                monsterYList.Add(monsterY);
            }
            else
            {
                i--; // Pick a random location again if there is a player or monster there
            }
        }
    }

    #endregion

    #region Helper Methods

    static bool IsMonsterAt(int x, int y)
    {
        for (int i = 0; i < monsterXList.Count; i++)
        {
            if (monsterXList[i] == x && monsterYList[i] == y)
            {
                return true;
            }
        }
        return false;
    }

    static void PrintLocation()
    {
        Console.Clear();
        Console.WriteLine($"Location: ({playerX},{playerY}), Health: {playerHealth}");
    }

    #endregion

    #region Player Movement

    static void Move(string direction)
    {
        int newPlayerX = playerX;
        int newPlayerY = playerY;

        switch (direction)
        {
            case "N":
                newPlayerY--;
                break;
            case "S":
                newPlayerY++;
                break;
            case "W":
                newPlayerX--;
                break;
            case "E":
                newPlayerX++;
                break;
            default:
                Console.WriteLine("Invalid command! Please enter a valid command.");
                return;
        }

        // Check location
        if (newPlayerX >= 0 && newPlayerX < mapSize && newPlayerY >= 0 && newPlayerY < mapSize)
        {
            // Check health
            if (playerHealth <= 0)
            {
                Console.WriteLine("You ran out of health! You lost the game.");
                EndGame();
                return;
            }

            // Battle Session with Monster upon encountering
            if (IsMonsterAt(newPlayerX, newPlayerY))
            {
                Console.WriteLine("You encountered with a monster!The battle begins.");
                Battle();
            }

            // Reaching the diamond
            if (newPlayerX == diamondX && newPlayerY == diamondY)
            {
                Console.WriteLine("You have reached the Diamond! You won the game.");
                EndGame();
                return;
            }

            // Move to new location

            playerX = newPlayerX;
            playerY = newPlayerY;
        }
        else
        {
            Console.WriteLine("Invalid location! You can't pass the limits of the map.");
        }
    }

    #endregion

    #region Battle and Attack

    static void Battle()
    {
        Console.WriteLine("The battle begins! (A: Attack, press another button to run away from the monster.)");

        string command2 = Console.ReadLine()?.ToUpper(); 
        if (command2 == null)
        {
            Console.WriteLine("Invalid command! Please enter a valid command.");
            return;
        }

        if (command2 == "A")
        {
            Attack();
        }
        else
        {
            Console.WriteLine("You ran away! You ditched the battle.");
        }
    }

    static void Attack()
    {
        Console.WriteLine("You are attacking the Monster!");
        playerHealth -= 20;

        // Checking if monster is dead
        for (int i = 0; i < monsterXList.Count; i++)
        {
            if (playerX == monsterXList[i] && playerY == monsterYList[i])
            {
                monsterXList.RemoveAt(i);
                monsterYList.RemoveAt(i);
                i--; // Decreasing the index since the list is changed because a monster died.
            }
        }

        // Check Monsters
        if (monsterXList.Count == 0)
        {
            Console.WriteLine("You killed the Monsters! Get ready for a new adcenture!");
            InitializeMonsters(5);
        }
        else
        {
            Console.WriteLine("The Monster is dead. Get ready for a new adventure!");
        }

        // Check if win
        if (monsterXList.Count == 0 && playerX == diamondX && playerY == diamondY)
        {
            Console.WriteLine("You have reached the Diamond! You won the game.");
            EndGame();
        }
    }

    #endregion

    #region End Game

    static void EndGame()
    {
        Console.WriteLine("The game is over! Press any key to restart.");
        Console.ReadKey();
        InitializeGame();
    }

    #endregion
}
