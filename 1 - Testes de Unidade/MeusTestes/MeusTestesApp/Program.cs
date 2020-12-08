using System;
using BogusBiblioteca;

namespace MeusTestesApp
{
  class Program
  {
    private static BogusLib bogus;

    static void Main(string[] args)
    {
      bogus = new BogusLib();

      for (int i = 0; i < 20; i++)
        Console.WriteLine(bogus.GetEmail());
    }
  }
}
