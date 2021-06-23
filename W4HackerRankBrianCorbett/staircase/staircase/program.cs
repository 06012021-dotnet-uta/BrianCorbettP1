using System;

namespace staircase
{
  class Result
  {

    /*
     * Complete the 'staircase' function below.
     *
     * The function accepts INTEGER n as parameter.
     */

    public static void staircase(int n)
    {
      for (int i = 1; i <= n; i++)
      {
        string level = "";
        for (int j = 0; j < (n - i); j++)
        {
          level += " ";
        }
        for (int j = 0; j < i; j++)
        {
          level += "#";
        }
        Console.WriteLine(level);
      }
    }

  }

  class Solution
  {
    public static void Main(string[] args)
    {
      int n = Convert.ToInt32(Console.ReadLine().Trim());

      Result.staircase(n);
    }
  }

}
