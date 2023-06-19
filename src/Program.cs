using System;
using System.Threading;
using System.IO;

public class Program {
  public static void Main(string[] args) {
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine(" * THE CY COMPILER * ");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("Enter the operation to be executed:\n");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("[C]");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("ompile...");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("[N]");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("ew...");
    var ans = Console.ReadKey().KeyChar.ToString().ToUpper();
    Console.Write("\n");
    switch(ans) {
      case "C":
        // COMPILER
        
        // Finding Main.cy...
        var maincypath = "";
        var file = "";
        var output = "";
        try {
          maincypath = Directory.GetCurrentDirectory().ToString()+"/src/Main.cy";
          file = File.ReadAllText(maincypath);
        }
        catch {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERR.: Could not find Main.cy. Try running this at the project's root directory.");
          Console.ForegroundColor = ConsoleColor.Gray;
          break;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Cyntax checking...");
        Console.ForegroundColor = ConsoleColor.Gray;
        
        var state = "normal";
        int[] line  = new int[] {0, 0};
        var ch = 0;
        var cancompile = true;
        string[] datatypes = new string[] {
          "in",
          "db",
          "ch",
          "fl",
        };
        var alreadyappend = false;

        while(ch < file.Length) {
          alreadyappend = false;
          line[1]++;
          if (file[ch] == '\n') {
            line[1] = 1;
            line[0]++;
          }
          if (state == "normal") {
            // DATA TYPE CHECKING
            if (file[ch] == ' ' || file[ch] == '\n') {
              foreach (var datatype in datatypes) {
                if (ch+3 < file.Length) {
                  if (file[ch+1] == datatype[0] && file[ch+2] == datatype[1] && file[ch+3] == ' ') {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERR.: Expected @ before type '"+datatype+"' at "+(line[0]+1)+", "+(line[1])+".");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    cancompile = false;
                  }
                }
              }
              foreach (var datatype in datatypes) {
                if (ch+4 < file.Length) {
                  if (file[ch+1] == '@' && file[ch+2] == datatype[0] && file[ch+3] == datatype[1] && file[ch+4] == ' ') {
                    switch (datatype) {
                      case "in":
                        output += " int";
                        alreadyappend = true;
                        ch+=3;
                        break;
                      case "db":
                        output += " double";
                        alreadyappend = true;
                        ch+=3;
                        break;
                      case "ch":
                        output += " char";
                        alreadyappend = true;
                        ch+=3;
                        break;
                      case "fl":
                        output += " float";
                        alreadyappend = true;
                        ch+=3;
                        break;
                    }
                  }
                }
              }
            }
          }
          if (state == "string") {
            if (ch+1 < file.Length) {
              if (file[ch] == '$' && file[ch+1] == '$') {
                output += '$';
                alreadyappend = true;
                ch++;
              }
              else if (file[ch] == '$' && file[ch+1] != '$') {
                output += '%';
                alreadyappend = true;
              }
            }
          }

          if (file[ch] == '\"') {
            switch (state) {
              case "normal":
                state = "string";
                break;
              case "string":
                state = "normal";
                break;
            }
          }
          if (!alreadyappend) {
            output += file[ch];
          }
          ch++;
        }
        if (cancompile) {
          File.WriteAllText(Directory.GetCurrentDirectory()+"/src/Main.c", output);
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine("All right! Cyntax checking couldn't find any syntax issues. Created './src/Main.c' as an output.");
          Console.ForegroundColor = ConsoleColor.Gray;
        }
        break;
      case "N":
        // NEW PROJECT

        Console.WriteLine("Enter the name of the project...");
        var nfileans = Console.ReadLine().ToString();
        var curpath = Directory.GetCurrentDirectory().ToString();
        if (nfileans != "") {
          Directory.CreateDirectory(curpath+"/"+nfileans);
          Directory.CreateDirectory(curpath+"/"+nfileans+"/src");
          Directory.CreateDirectory(curpath+"/"+nfileans+"/bin");
          File.Create(curpath+"/"+nfileans+"/src/Main.cy");
        }
        else {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERR.: Blank names are not valid for directories.");
          Console.ForegroundColor = ConsoleColor.Gray;
        }
        break;
    }
    if (ans!="C" && ans!="N") {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("ERR.: Not a valid answer.");
      Console.ForegroundColor = ConsoleColor.Gray;
    }
  }
} 
