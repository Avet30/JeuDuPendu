using AsciiPoutre;

namespace JeuDuPendu
{
    internal class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for(int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];

                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " " );
                }
                else
                {
                    Console.Write("_ ");
                }               
            }
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            foreach(var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }
            if (mot.Length == 0)
            {
                return true;
            }
            return false;
        }

        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {
            while (true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("Erreur : Vous devez rentrer une lettre");
            }
        }

        static void DevinerMot(string mot)
        {
            List<char> lettresDevinees = new List<char>();
            List<char> lettresExclues = new List<char>();

            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {   
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Cette lettre est dans le mot");
                    Console.ForegroundColor = ConsoleColor.White;
                    lettresDevinees.Add(lettre);

                    if(ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        break;
                    }
                }
                else
                {   
                    if(!lettresExclues.Contains(lettre))
                    {
                        viesRestantes--;
                        lettresExclues.Add(lettre);
                    }
     
                    Console.WriteLine($"Vies restantes : {viesRestantes}");
                }

                if(lettresExclues.Count > 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(", ", lettresExclues));
                }
                
                Console.WriteLine();                             
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Perdu ! le mot était : {mot}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("C'est Gagné !!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static string[] ChargesLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier : " + nomFichier + "(" + ex.Message + ")");
            }
            return null;
        }

        static bool DemanderDeRejouer()
        {
            
            char reponse = DemanderUneLettre("Voulez-vous rejouer (O/N) :  ");
            string ReponseString = reponse.ToString().ToUpper();
            if (ReponseString == "O")
            {
                return true;
            }
            else if (ReponseString == "N")
            {
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erreur : Vous devez répondre par o ou n");
                Console.ForegroundColor = ConsoleColor.White;
                return DemanderDeRejouer();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargesLesMots("mots.txt");

            if ((mots == null || mots.Length == 0))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("La liste de mots est vide! ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                
                while(true)
                {
                    Random random = new Random();
                    int i = random.Next(0, mots.Length);
                    string mot = mots[i].Trim().ToUpper();
                    DevinerMot(mot);

                    if(!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.WriteLine("Merci et à bientôt !!!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}