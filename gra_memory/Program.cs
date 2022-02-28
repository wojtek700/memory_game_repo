using System.Text.RegularExpressions;

class Program
{
    public static void Main()
    {
        bool kontynuujGre = true;

        while (kontynuujGre == true)
        {
            int pozostaleProby = 10;
            int kolumny = 4;
            int wiersze = 2;
            int iloscWybranychSlow = 4;
            bool trybLatwy = true;

            Console.WriteLine("Witaj w grze memory!");
            Console.WriteLine("Wybierz poziom:\nLATWY (wybierz 1) \nTRUDNY (wybierz 2)");
            var poziom = Console.ReadLine();
            if (poziom.Equals("1"))
            {
                Console.WriteLine("Wybrano poziom LATWY");
            }
            else if (poziom.Equals("2"))
            {
                Console.WriteLine("Wybrano poziom TRUDNY");
                pozostaleProby = 15;
                kolumny = 8;
                iloscWybranychSlow = 8;
                trybLatwy = false;
            }
            else
            {
                Console.WriteLine("Blad, wybrano nieprawidlowy tryb. Wybrano domyslnie poziom LATWY.");
            }
            System.Threading.Thread.Sleep(2000);

            var wszystkieSlowa = WczytajSlowa();
            List<string> wybraneSlowa = WylosujSlowa(wszystkieSlowa, iloscWybranychSlow);
            List<List<string>> macierzSlow = GenerujSlowa(wiersze, kolumny, wybraneSlowa);

            Dictionary<char, int> indeksyWierszy = new Dictionary<char, int>();
            indeksyWierszy.Add('A', 0);
            indeksyWierszy.Add('B', 1);
            List<string> odgadnieteSlowa = new List<string>();

            while (pozostaleProby > 0)
            {
                Console.Clear();
                Console.WriteLine("Ilosc pozostalych prob: " + pozostaleProby);
                WypiszMacierz(macierzSlow, odgadnieteSlowa, trybLatwy);
                Console.WriteLine("\nWprowadz wspolrzedne pierwszego slowa:");
                string wspolrzedne = Console.ReadLine();
                char[] wspolrzedne1 = wspolrzedne.ToCharArray(0, 2);

                OdkryjPierwszeSlowo(macierzSlow, odgadnieteSlowa, wspolrzedne1, trybLatwy);

                Console.WriteLine("\nWprowadz wspolrzedne pierwszego slowa:");
                wspolrzedne = Console.ReadLine();
                char[] wspolrzedne2 = wspolrzedne.ToCharArray(0, 2);

                OdkryjDrugieSlowo(macierzSlow, odgadnieteSlowa, wspolrzedne1, wspolrzedne2, trybLatwy);
                bool teSameSlowa = CzyTeSameSlowa(macierzSlow, wspolrzedne1, wspolrzedne2, indeksyWierszy);
                if (teSameSlowa)
                {
                    int indeks11 = indeksyWierszy[wspolrzedne1[0]];
                    int indeks12 = (int)Char.GetNumericValue(wspolrzedne1[1]) - 1;
                    if (odgadnieteSlowa.Contains(macierzSlow[indeks11][indeks12]) == false)
                    {
                        odgadnieteSlowa.Add(macierzSlow[indeks11][indeks12]);
                    }
                }
                Console.WriteLine("\n\n\n\n");
                if (odgadnieteSlowa.Count == 4)
                {
                    break;
                }
                pozostaleProby = pozostaleProby - 1;
                System.Threading.Thread.Sleep(2000);
            }
            if (odgadnieteSlowa.Count == iloscWybranychSlow)
            {
                Console.WriteLine("Gratulacje, wygrales!!!");
            }
            else
            {
                Console.WriteLine("Niestety nie udalo sie, moze innym razem :(");
            }

            Console.WriteLine("Czy chcesz zaczac gre od nowa? \nTak (wybierz t) \nNie (wybierz n)");
            var odpowiedz = Console.ReadLine();
            if (odpowiedz.Equals("t"))
            {
                kontynuujGre = true;
            }
            else
            {
                kontynuujGre = false;
            }
        }

        Console.WriteLine("\n\n\n\n");
        Console.WriteLine("Koniec gry.");
        Console.WriteLine("\n\n\n\n");
    }

    public static string[] WczytajSlowa()
    {
        var wszystkieSlowa = Regex.Split(File.ReadAllText("Words.txt"), @"[\n,\s,;:.!?-]+");
        var sr = new StreamReader("Words.txt");
        return wszystkieSlowa;
    }

    public static List<List<string>> GenerujSlowa(int wiersze, int kolumny, List<string> wybraneSlowa)
    {
        Random randObj = new Random();
        List<List<string>> lists = new List<List<string>>();
        lists.Add(new List<string>());
        lists.Add(new List<string>());

        for (int x = 0; x < wiersze; x++)
        {
            for (int y = 0; y < kolumny; y++)
            {
                var dlugosc = wybraneSlowa.Count;
                Console.WriteLine(dlugosc);
                var randomowyindeks = randObj.Next(dlugosc);
                lists[x].Add(wybraneSlowa.ElementAt(randomowyindeks));
                wybraneSlowa.RemoveAt(randomowyindeks);
            }
        }
        return lists;
    }

    public static List<string> WylosujSlowa(string[] slowa, int iloscWybranychSlow)
    {
        Random randObj = new Random();
        var dlugosc = slowa.Length;
        Console.WriteLine("Length of array is " + dlugosc);

        HashSet<int> indeksySlow = new HashSet<int>();

        //Losowanie randomowych indeksów, które posłużą do wyznaczenia losowych słów 
        while (indeksySlow.Count < iloscWybranychSlow)
        {
            var randomowyIndeks = randObj.Next(dlugosc);
            indeksySlow.Add(randomowyIndeks);
        }

        //Lista przechowujaca wylosowane do gry slowa
        var wybraneSlowa = new List<string>();

        foreach (var indeks in indeksySlow)
        {
            wybraneSlowa.Add(slowa[indeks]);
            wybraneSlowa.Add(slowa[indeks]);
        }

        return wybraneSlowa;
    }

    public static void WypiszSlowa(List<List<string>> macierzSlow)
    {
        foreach (var listaSlow in macierzSlow)
        {
            Console.WriteLine("\n");
            Console.WriteLine("-------");
            foreach (var slowa in listaSlow)
            {
                Console.Write(slowa + " ");
            }
        }
    }

    public static void WypiszMacierz(List<List<string>> macierzSlow, List<string> odgadnieteSlowa, bool trybLatwy)
    {
        Console.WriteLine("-------------------------------");
        if (trybLatwy == true)
        {
            Console.WriteLine("\t1 2 3 4");
        }
        else
        {
            Console.WriteLine("\t1 2 3 4 5 6 7 8");
        }
        Console.Write("\nA\t");
        foreach (string slowo in macierzSlow[0])
        {
            if (odgadnieteSlowa.Contains(slowo))
            {
                Console.Write(slowo + " ");
            }
            else 
            { 
                Console.Write("X "); 
            }
        }
        Console.Write("\nB\t");
        foreach (string slowo in macierzSlow[1])
        {
            if (odgadnieteSlowa.Contains(slowo))
            {
                Console.Write(slowo + " ");
            }
            else
            {
                Console.Write("X ");
            }
        }
        Console.WriteLine("\n-------------------------------");
    }
    public static void OdkryjPierwszeSlowo(List<List<string>> macierzSlow, List<string> odgadnieteSlowa, char[] wspolrzedne, bool trybLatwy)
    {
        Console.WriteLine("-------------------------------");
        if (trybLatwy == true)
        {
            Console.WriteLine("\t1 2 3 4");
        }
        else
        {
            Console.WriteLine("\t1 2 3 4 5 6 7 8");
        }
        Console.Write("\nA\t");
        for (int indeks=0; indeks<macierzSlow[0].Count; indeks++)
        {
            if (odgadnieteSlowa.Contains(macierzSlow[0][indeks]))
            {
                Console.Write(macierzSlow[0][indeks] + " ");
            }
            else if ((wspolrzedne[0].Equals('A')) & ((Char.GetNumericValue(wspolrzedne[1]) - 1) == indeks))
            {
                Console.Write(macierzSlow[0][indeks] + " ");
            }
            else
            {
                Console.Write("X ");
            }

        }
        Console.Write("\nB\t");
        for (int indeks = 0; indeks < macierzSlow[1].Count; indeks++)
        {
            if (odgadnieteSlowa.Contains(macierzSlow[1][indeks]))
            {
                Console.Write(macierzSlow[1][indeks] + " ");
            }
            else if ((wspolrzedne[0].Equals('B')) & ((Char.GetNumericValue(wspolrzedne[1]) - 1) == indeks))
            {
                Console.Write(macierzSlow[1][indeks] + " ");
            }
            else
            {
                Console.Write("X ");
            }
        }
        Console.WriteLine("\n-------------------------------");
    }

    public static void OdkryjDrugieSlowo(List<List<string>> macierzSlow, List<string> odgadnieteSlowa, char[] wspolrzedne1, char[] wspolrzedne2, bool trybLatwy)
    {
        Console.WriteLine("-------------------------------");
        if (trybLatwy == true)
        {
            Console.WriteLine("\t1 2 3 4");
        }
        else
        {
            Console.WriteLine("\t1 2 3 4 5 6 7 8");
        }
        Console.Write("\nA\t");
        for (int indeks = 0; indeks < macierzSlow[0].Count; indeks++)
        {
            if (odgadnieteSlowa.Contains(macierzSlow[0][indeks]))
            {
                Console.Write(macierzSlow[0][indeks] + " ");
            }
            else if ((wspolrzedne1[0].Equals('A')) & (Char.GetNumericValue(wspolrzedne1[1]) - 1) == indeks)
            {
                Console.Write(macierzSlow[0][indeks] + " ");
            }
            else if ((wspolrzedne2[0].Equals('A')) & (Char.GetNumericValue(wspolrzedne2[1]) - 1) == indeks)
            {
                Console.Write(macierzSlow[0][indeks] + " ");
            }
            else
            {
                Console.Write("X ");
            }

        }
        Console.Write("\nB\t");
        for (int indeks = 0; indeks < macierzSlow[1].Count; indeks++)
        {
            if (odgadnieteSlowa.Contains(macierzSlow[1][indeks]))
            {
                Console.Write(macierzSlow[1][indeks] + " ");
            }
            else if ((wspolrzedne1[0].Equals('B')) & (Char.GetNumericValue(wspolrzedne1[1]) - 1) == indeks)
            {
                Console.Write(macierzSlow[1][indeks] + " ");
            }
            else if ((wspolrzedne2[0].Equals('B')) & (Char.GetNumericValue(wspolrzedne2[1]) - 1) == indeks)
            {
                Console.Write(macierzSlow[1][indeks] + " ");
            }
            else
            {
                Console.Write("X ");
            }
        }
        Console.WriteLine("\n-------------------------------");
    }

    public static bool CzyTeSameSlowa(List<List<string>> macierzSlow, char[] wspolrzedne1, char[] wspolrzedne2, Dictionary<char, int> indeksyWierszy)
    {
        bool wynik = false;

        int indeks11 = indeksyWierszy[wspolrzedne1[0]];
        int indeks12 = (int)Char.GetNumericValue(wspolrzedne1[1]) - 1;
        int indeks21 = indeksyWierszy[wspolrzedne2[0]];
        int indeks22 = (int)Char.GetNumericValue(wspolrzedne2[1]) - 1;

        if (macierzSlow[indeks11][indeks12] == macierzSlow[indeks21][indeks22])
        {
            wynik = true;
        }
        return wynik;
    }
}
