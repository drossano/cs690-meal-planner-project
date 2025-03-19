namespace MealPlanner;

using System.IO;
using Microsoft.VisualBasic;

public class FileManager
{
  string fileName;

  public FileManager(string fileName)
  {
    this.fileName = fileName;
    if (!File.Exists(this.fileName))
    {
      File.Create(this.fileName).Close();
    }
  }

  public void AppendLine(string line)
  {
    File.AppendAllText(this.fileName, line + Environment.NewLine);
  }


}

