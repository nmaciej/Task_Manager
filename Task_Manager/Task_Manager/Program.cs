using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Task_Manager
{
  class Program
  {
    static void Main(string[] args)
    {
      Logic.Welcome();

      do
      {
        string command = Logic.MainMenu();

        if (command.ToUpper() == "ADD")
        {
          Logic.MenuAdd();
          ConsoleEx.WriteLine(ConsoleColor.Green, "Task was addded successfully!");
          Console.ReadLine();
        }
        else if (command.ToUpper() == "DEL")
        {
          Console.Clear();
          Logic.MenuDel();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "SHOW")
        {
          Logic.MenuShow();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "SAVE")
        {
          Logic.MenuSave();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "LOAD")
        {
          Console.Clear();
          Logic.MenuLoad();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "LIST")
        {
          Logic.MenuList();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "CHANGE")
        {
          Logic.MenuChange();
          Console.ReadLine();
        }
        else if (command.ToUpper() == "EXIT")
        {
          break;
        }
        else
        {
          ConsoleEx.WriteLine(ConsoleColor.Yellow, "Enter valid command!");
          System.Threading.Thread.Sleep(1000);
        }
      } while (true);
    }
  }
}
