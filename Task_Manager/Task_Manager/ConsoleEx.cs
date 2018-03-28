using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Manager
{
  static class ConsoleEx
  {
    //CONSOLE WRITE IN COLORS
    public static void Write(ConsoleColor color, string text, params object[] args)
    {
      var currentColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.Write(text);
      Console.ForegroundColor = currentColor;
    }
    public static void WriteLine(ConsoleColor color, string text, params object[] args)
    {
      var currentColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.WriteLine(text);
      Console.ForegroundColor = currentColor;
    }
  }
}
