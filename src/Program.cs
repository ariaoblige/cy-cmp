using System;
using System.Threading;
using System.IO;
using System.Diagnostics;

public class Program {
  public static void Main(string[] args) {
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine(" * THE CY COMPILER * ");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\nVersion 0.1.1.\nSource code available at https://github.com/ariaoblige/cy-cmp.\n");
    Console.WriteLine("Enter the operation to be executed:\n");
    Console.ForegroundColor = ConsoleColor.Yellow;
    
    string[] operations = new string[]
    {
      "Compile project...",
      "Compile file...",
      "New project...",
      "New .cy file...",
    };

    for(var op=0;op<operations.Length;op++) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("[ "+(op+1).ToString()+" ] ");
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine(operations[op]);
    }
    Console.ForegroundColor = ConsoleColor.Gray;
    var ans = Console.ReadKey().KeyChar.ToString().ToUpper();
    Console.Write("\n");
    var maincypath = "";
    var file = "";
    var output = "";
    switch(ans) {

      case "1":
        // COMPILER
        
        // Finding Main.cy...

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
        
        foreach (var cyfile in Directory.GetFiles(Directory.GetCurrentDirectory()+"\\src", "*.cy")) {
          file = File.ReadAllText(cyfile);
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
                      Console.WriteLine("      at "+cyfile+".");
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
            // GETTING FILE NAME
            var filename = "";
            var write = false;
            var character = cyfile.Length-1;
            while (true) {
              if (cyfile[character] == '\\' || cyfile[character] == '/') {
                break;
              }
              if (character+1 < cyfile.Length) {
                if (cyfile.ToString()[character+1] == '.') {
                  write = true;
                }
              }
              if (write) {
                filename+=cyfile.ToString()[character];
              }
              character--;
            }
            char[] filerev = filename.ToCharArray();
            Array.Reverse(filerev);
            string f = new string(filerev);

            File.WriteAllText(Directory.GetCurrentDirectory()+"/src/"+f+".c", output);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All right! Cyntax checking couldn't find any syntax issues. Created .c source files at ./src.");
            Console.ForegroundColor = ConsoleColor.Gray;
            ProcessStartInfo gccinfo = new ProcessStartInfo("cmd.exe", "/C gcc -o bin/"+f+" src/"+f+".c");
            gccinfo.UseShellExecute = false;
            var gcc = Process.Start(gccinfo);
            gcc.WaitForExit();
          }
        }
        break;
      case "2":
        // COMPILE FILE
        Console.Write("Enter the file name without extension: ");
        var cpans = Console.ReadLine().ToString();
        if (cpans.Contains(".")) {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERR.: Cy files are not expected to have any dots besides the extension.");
          Console.ForegroundColor = ConsoleColor.Gray;
          break;
        }
        if (File.Exists(Directory.GetCurrentDirectory()+"/"+cpans+".cy")) {
          var cyfile = Directory.GetCurrentDirectory()+"/"+cpans+".cy";
          file = File.ReadAllText(Directory.GetCurrentDirectory()+"/"+cpans+".cy");
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
                      Console.WriteLine("      at "+cyfile+".");
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
            // GETTING FILE NAME
            var filename = "";
            var write = false;
            var character = cyfile.Length-1;
            while (true) {
              if (cyfile[character] == '\\' || cyfile[character] == '/') {
                break;
              }
              if (character+1 < cyfile.Length) {
                if (cyfile.ToString()[character+1] == '.') {
                  write = true;
                }
              }
              if (write) {
                filename+=cyfile.ToString()[character];
              }
              character--;
            }
            char[] filerev = filename.ToCharArray();
            Array.Reverse(filerev);
            string f = new string(filerev);

            File.WriteAllText(Directory.GetCurrentDirectory()+"/"+f+".c", output);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All right! Cyntax checking couldn't find any syntax issues. Created .c source files at ./src.");
            Console.ForegroundColor = ConsoleColor.Gray;
            ProcessStartInfo gccinfo = new ProcessStartInfo("cmd.exe", "/C gcc -o "+f+" "+f+".c");
            gccinfo.UseShellExecute = false;
            var gcc = Process.Start(gccinfo);
            gcc.WaitForExit();
          }
        }
        else {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERR.: Could not find a file with the given name.");
          Console.ForegroundColor = ConsoleColor.Gray;
        }
        break;
      case "3":
        // NEW PROJECT

        Console.WriteLine("Enter the name of the project...");
        var nfileans = Console.ReadLine().ToString();
        var curpath = Directory.GetCurrentDirectory().ToString();
        if (nfileans != "") {
          Directory.CreateDirectory(curpath+"/"+nfileans);
          Directory.CreateDirectory(curpath+"/"+nfileans+"/src");
          Directory.CreateDirectory(curpath+"/"+nfileans+"/bin");
          File.WriteAllText(curpath+"/"+nfileans+"/src/Main.cy", "#include <stdio.h>\n\n@in main() {\n  \n  return 0;\n}");
        }
        else {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("ERR.: Blank names are not valid for directories.");
          Console.ForegroundColor = ConsoleColor.Gray;
        }
        break;
    case "4":
      Console.Write("Enter the file name without extension: ");
      var newcy = Console.ReadLine().ToString();
      if (newcy != "" || newcy.Contains(".")) {
        File.Create(Directory.GetCurrentDirectory()+"/"+newcy+".cy");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Successfully created {0}.cy.", newcy);
        Console.ForegroundColor = ConsoleColor.Gray;
      }
      else {
        Console.ForegroundColor = ConsoleColor.Red;
        if (newcy.Contains(".")) {
          Console.WriteLine("ERR.: Dots are not allowed in .cy files.");
        }
        else {
          Console.WriteLine("ERR.: Files with blank names are not valid.");
        }
        Console.ForegroundColor = ConsoleColor.Gray;
      }
      break;
    }
    if (ans!="1"&&ans!="2"&&ans!="3"&&ans!="4") {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("ERR.: Not a valid answer.");
      Console.ForegroundColor = ConsoleColor.Gray;
    }

  }
} 
