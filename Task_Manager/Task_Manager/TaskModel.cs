using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Task_Manager
{
  public class TaskModel
  {
    public string Description { get; }
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; }
    public bool IsAllDayTask { get; }
    public bool IsImportant { get; }

    //CONSTRUCTOR
    public TaskModel(string description, DateTime startDate, DateTime? endDate, bool isImportant)
    {
      Description = description;
      StartDate = startDate;
      EndDate = endDate;
      IsAllDayTask = !endDate.HasValue;
      IsImportant = isImportant;
    }

    //HANDY TASK FUNCTIONS
    public string PrintTask()
    {
      var output = new StringBuilder(string.Empty);
      output.Append("|");

      if (Description.Length < 25)
      {
        output.Append($"{Description}".PadRight(25));
      }
      else
      {
        output.Append($"{Description.Substring(0,20)}...".PadRight(25));
      }

      output.Append("|");
      output.Append($"{StartDate.ToShortDateString()}".PadRight(20));
      output.Append("|");
      output.Append(EndDate.HasValue ? $"{EndDate.Value.ToShortDateString()}".PadRight(20) : $"".PadRight(20));
      output.Append("|");
      output.Append($"{IsAllDayTask}".PadRight(20));
      output.Append("|");
      output.Append($"{IsImportant}".PadRight(20));
      output.Append("|");

      return Convert.ToString(output);
    }
    public string SaveTaskToFile()
    {
      var output = new StringBuilder(string.Empty);

      output.Append($"{Description}");
      output.Append($",");
      output.Append($"{StartDate}");
      output.Append($",");
      output.Append(EndDate.HasValue ? $"{EndDate}" : $"");
      output.Append($",");
      output.Append($"{IsAllDayTask}");
      output.Append($",");
      output.Append($"{IsImportant}");

      return Convert.ToString(output);
    }
  }
}
