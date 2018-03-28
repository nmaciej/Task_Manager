using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;

namespace Task_Manager
{
  public static class Logic
  {
    public static List<TaskModel> workingDatabase { get; private set; } = new List<TaskModel>();
    public static Dictionary<string, List<TaskModel>> workingDictionary { get; private set; } = new Dictionary<string, List<TaskModel>>();

    //MENU DEFINITION
    public static void Welcome()
    {
      ConsoleEx.WriteLine(ConsoleColor.Green, "Welcome to Task Manager!");
      Console.ReadLine();
    }
    public static string MainMenu()
    {
      Console.Clear();
      Console.WriteLine("Enter appropriate option from the list below:\n" +
                        "----------------------------------------------------\n" +
                        "ADD - adding task\n" +
                        "DEL - removing task\n" +
                        "SHOW - list all tasks\n" +
                        "SAVE - save task list to specified file\n" +
                        "----------------------------------------------------\n" +
                        "LOAD - load task list from specified file\n" +
                        "LIST - list databases loaded into memory\n" +
                        "CHANGE - change active database\n" +
                        "----------------------------------------------------\n" +
                        "EXIT - exit Task Manager (unsaved files will be lost)\n");
      return Console.ReadLine();
    }
    public static void MenuAdd()
    {
      var description = Logic.GetString("Enter the task description");
      var importance = Logic.GetBool("Is task an important task? [YES/NO]");
      var startDate = Logic.GetDateTime("Enter task start date");
      var allDayFlag = Logic.GetBool("Is task an all day task? [YES/NO]");

      DateTime? endDate;
      if (allDayFlag == false)
      {
        endDate = Logic.GetDateTime("Enter task end date");
      }
      else
      {
        endDate = null;
      }

      Logic.workingDatabase.Add(new TaskModel(description, startDate, endDate, importance));
    }
    public static void MenuDel()
    {
      ConsoleEx.WriteLine(ConsoleColor.Red, "You are in the task deletion menu\n");
      ConsoleEx.WriteLine(ConsoleColor.Red, $"Current database is {GetCurrentDatabaseName()}");

      if (workingDatabase.Count == 0)
      {
        Console.WriteLine("There are no tasks that can be deleted");
      }
      else
      {
        Console.WriteLine("\nEnter number of task to be deleted from the list below.\n");
        PrintWorkingDatabase();
        var noOfLineToDelete = Console.ReadLine();

        if (int.TryParse(noOfLineToDelete, out var result))
        {
          if (result > 0 && result < workingDatabase.Count + 1)
          {
            Logic.DeleteTaskFromDatabase(Convert.ToInt32(noOfLineToDelete));
            ConsoleEx.WriteLine(ConsoleColor.Yellow, "\nDatabase after modification\n");
            PrintWorkingDatabase();
          }
          else
          {
            ConsoleEx.WriteLine(ConsoleColor.Red, "Provided task number is out of range!");
          }
        }
        else
        {
          ConsoleEx.WriteLine(ConsoleColor.Red, "There is no such task");
        }
      }
    }
    public static void MenuShow()
    {
      ConsoleEx.WriteLine(ConsoleColor.Red,$"Printing the {GetCurrentDatabaseName()} task list!\n");
      PrintWorkingDatabase();
    }
    public static void MenuSave()
    {
      var path = GetString("Enter the file name with .csv");

      Logic.SaveDatabaseToFile(path);
      ConsoleEx.WriteLine(ConsoleColor.Green, $"Database saved to {path}");
    }
    public static void MenuLoad()
    {
      do
      {
        ListFiles();
        var dataBaseName = Logic.GetString("Enter name of database to load or EXIT to come back to Main Menu");

        if (dataBaseName.ToUpper() == "EXIT")
        {
          ConsoleEx.WriteLine(ConsoleColor.Cyan,"Leaving LOAD Menu");
          break;
        }
        else if (workingDictionary.ContainsKey(dataBaseName))
        {
          ConsoleEx.WriteLine(ConsoleColor.Yellow, "Such database is already loaded!\n");
          System.Threading.Thread.Sleep(1000);
        }
        else if (!File.Exists(dataBaseName))
        {
          ConsoleEx.WriteLine(ConsoleColor.Yellow, "Database name is incorrect\n");
          System.Threading.Thread.Sleep(1000);
        }
        else if (dataBaseName.Contains("csv"))
        {
          workingDatabase = LoadDatabaseFromFile(dataBaseName);
          workingDictionary.Add(dataBaseName, workingDatabase);
          ConsoleEx.WriteLine(ConsoleColor.Yellow, $"\nDatabase {GetCurrentDatabaseName()} loaded.\n");
        }
        else
        {
          ConsoleEx.WriteLine(ConsoleColor.Red, "Incorrect command!\n");
        }

      } while (true);
    }
    public static void MenuList()
    {
      if (workingDictionary.Count == 0)
      {
        ConsoleEx.WriteLine(ConsoleColor.Red, "No databases loaded into memory");
      }
      else
      {
        ConsoleEx.WriteLine(ConsoleColor.Magenta, "Databases loaded into memory:\n");
        foreach (var database in workingDictionary)
        {
          ConsoleEx.WriteLine(ConsoleColor.Magenta, database.Key);
        }
      }
    }
    public static void MenuChange()
    {
      Logic.ListLoadedDatabases();
      var path = Logic.GetString("Provide database name to be active");

      if (workingDictionary.ContainsKey(path))
      {
        if (GetCurrentDatabaseName() == path)
        {
          ConsoleEx.WriteLine(ConsoleColor.Red, "This database is already active!");
        }
        else
        {
          workingDatabase = workingDictionary.Single(x => x.Key == path).Value;
          ConsoleEx.WriteLine(ConsoleColor.Red, $"Active database chaged to {GetCurrentDatabaseName()}");
        }
      }
      else
      {
        ConsoleEx.WriteLine(ConsoleColor.Red, "There is no such database!");
      }
    }

    //INPUTS FROM USER
    public static string GetString(string display)
    {
      Console.WriteLine(display);
      return Console.ReadLine();
    }
    public static bool GetBool(string display)
    {
      Console.WriteLine(display);
      var input = Console.ReadLine();

      if (input != "YES" && input != "NO")
      {
        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, "Insert YES or NO");
        GetBool(display);
      }

      return input == "YES";
    }
    public static DateTime GetDateTime(string display)
    {
      Console.WriteLine(display);
      var success = DateTime.TryParse(Console.ReadLine(), out var parsedDate);

      if (success == false)
      {
        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, "Use the following data format [YYYY/MM/DD]");
        GetDateTime(display);
      }

      return parsedDate;
    }

    //DATABASE OPERATIONS
    public static string GetCurrentDatabaseName()
    {
      if (workingDictionary.Count == 0)
      {
        workingDictionary.Add("Default.csv", workingDatabase);
      }
      return workingDictionary.Single(c => c.Value == workingDatabase).Key;
    }
    public static void PrintWorkingDatabase()
    {
      StringBuilder output = new StringBuilder(String.Empty);

      output.Append("|No.".PadRight(4));
      output.Append("|");
      output.Append($"Description".PadRight(25));
      output.Append("|");
      output.Append($"Start Date".PadRight(20));
      output.Append("|");
      output.Append($"End Date".PadRight(20));
      output.Append("|");
      output.Append($"All day task".PadRight(20));
      output.Append("|");
      output.Append($"Important task".PadRight(20));
      output.Append("|");

      Console.WriteLine("".PadRight(115, '-'));
      Console.WriteLine(output);
      Console.WriteLine("".PadRight(115, '-'));

      int i = 0;
      foreach (var task in workingDatabase.OrderBy(x=>x.StartDate))
      {
        StringBuilder output2 = new StringBuilder(String.Empty);

        i++;
        output2.Append("|");
        output2.Append($"{i}".PadLeft(3));
        output2.Append(task.PrintTask());

        Console.WriteLine(Convert.ToString(output2));
        Console.WriteLine("".PadRight(115, '-'));
      }
    }
    public static void SaveDatabaseToFile(string path)
    {
      var output = new string[workingDatabase.Count];

      for (var i = 0; i < workingDatabase.Count; i++)
      {
        output[i] = workingDatabase[i].SaveTaskToFile();
      }

      File.WriteAllLines(path, output);
    }
    public static List<TaskModel> LoadDatabaseFromFile(string path)
    {


      var textFromFile = File.ReadAllLines(path);

      var output = new List<TaskModel>();

      foreach (var line in textFromFile)
      {
        var a = line.Split(',');

        DateTime? checkEndDate;

        if (a[2] == String.Empty)
        {
          checkEndDate = null;
        }
        else
        {
          checkEndDate = Convert.ToDateTime(a[2]);
        }

        output.Add(new TaskModel(a[0], Convert.ToDateTime(a[1]), checkEndDate, Convert.ToBoolean(a[4])));
      }

      return output;
    }
    public static void DeleteTaskFromDatabase(int lineToBeDeleted)
    {
      workingDatabase.RemoveAt(lineToBeDeleted - 1);
    }
    public static void ListLoadedDatabases()
    {
      if (workingDictionary.Count != 0)
      {
        ConsoleEx.WriteLine(ConsoleColor.Magenta,
          $"Databases loaded into memory.\n{GetCurrentDatabaseName()} is currently active.\n");

        foreach (var dataBase in workingDictionary)
        {
          ConsoleEx.WriteLine(ConsoleColor.Magenta, dataBase.Key);
        }
      }
      else
      {
        ConsoleEx.WriteLine(ConsoleColor.Red, "There is no loaded database");
      }
    }
    public static void ListFiles()
    {
      var fileListInDirectory = Directory.GetFiles(Directory.GetCurrentDirectory());
      var r = from f in fileListInDirectory where f.Contains(".csv") select f;
      ConsoleEx.WriteLine(ConsoleColor.Magenta, "Databases available in the current folder");
      foreach (var file in r)
      {
        ConsoleEx.WriteLine(ConsoleColor.Magenta, Path.GetFileName(file));
      }
      Console.WriteLine();
    }
  }
}