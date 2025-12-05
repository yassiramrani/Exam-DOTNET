using System;
using BibliothequeNumerique.Models;
using BibliothequeNumerique.Services;

namespace BibliothequeNumerique
{
    class Program
    {
        static void Main(string[] args)
        {
            var bibliotheque = new Bibliotheque();
            bool continuer = true;

            while (continuer)
            {
                AfficherMenu();

                string choix = Console.ReadLine();

                try
                {
                    switch (choix)
                    {
                        case "1":
                            AjouterDocument(bibliotheque);
                            break;
                        case "2":
                            bibliotheque.AfficherTous();
                            break;
                        case "3":
                            RechercherDocument(bibliotheque);
                            break;
                        case "4":
                            SupprimerDocument(bibliotheque);
                            break;
                        case "5":
                            SauvegarderBibliotheque(bibliotheque);
                            break;
                        case "6":
                            ChargerBibliotheque(bibliotheque);
                            break;
                        case "7":
                            continuer = false;
                            Console.WriteLine("Au revoir !");
                            break;
                        default:
                            Console.WriteLine("Option invalide. Veuillez réessayer.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur : {ex.Message}");
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }

        static void AfficherMenu()
        {
            Console.Clear();
            Console.WriteLine("=== BIBLIOTHÈQUE NUMÉRIQUE ===");
            Console.WriteLine("1. Ajouter un document");
            Console.WriteLine("2. Afficher tous les documents");
            Console.WriteLine("3. Rechercher par mot-clé");
            Console.WriteLine("4. Supprimer un document");
            Console.WriteLine("5. Sauvegarder dans un fichier");
            Console.WriteLine("6. Charger depuis un fichier");
            Console.WriteLine("7. Quitter");
            Console.Write("Votre choix : ");
        }

        static void AjouterDocument(Bibliotheque biblio)
        {
            Console.WriteLine("\n--- Ajouter un document ---");
            Console.WriteLine("1. Livre");
            Console.WriteLine("2. Magazine");
            Console.WriteLine("3. PDF");
            Console.Write("Type : ");
            var type = Console.ReadLine();

            Console.Write("Titre : ");
            var titre = Console.ReadLine();

            Console.Write("Auteur : ");
            var auteur = Console.ReadLine();

            Console.Write("Année : ");
            int annee = int.Parse(Console.ReadLine());

            Document doc = null;

            switch (type)
            {
                case "1":
                    Console.Write("Nombre de pages : ");
                    int pages = int.Parse(Console.ReadLine());
                    doc = new Livre(titre, auteur, annee, pages);
                    break;
                case "2":
                    Console.Write("Numéro : ");
                    int numero = int.Parse(Console.ReadLine());
                    doc = new Magazine(titre, auteur, annee, numero);
                    break;
                case "3":
                    Console.Write("Taille en Mo : ");
                    double taille = double.Parse(Console.ReadLine());
                    doc = new DocumentPDF(titre, auteur, annee, taille);
                    break;
                default:
                    Console.WriteLine("Type invalide.");
                    return;
            }

            biblio.AjouterDocument(doc);
        }

        static void RechercherDocument(Bibliotheque biblio)
        {
            Console.Write("\nMot-clé à rechercher : ");
            var motCle = Console.ReadLine();
            var resultats = biblio.Rechercher(motCle);

            if (resultats.Count == 0)
            {
                Console.WriteLine("Aucun document trouvé.");
            }
            else
            {
                Console.WriteLine($"{resultats.Count} document(s) trouvé(s) :");
                foreach (var doc in resultats)
                {
                    Console.WriteLine(doc.AfficherDetails());
                }
            }
        }

        static void SupprimerDocument(Bibliotheque biblio)
        {
            Console.Write("\nID du document à supprimer : ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                biblio.SupprimerDocument(id);
            }
            else
            {
                Console.WriteLine("ID invalide.");
            }
        }

        static void SauvegarderBibliotheque(Bibliotheque biblio)
        {
            Console.Write("\nChemin du fichier de sauvegarde : ");
            var chemin = Console.ReadLine();
            biblio.Sauvegarder(chemin);
        }

        static void ChargerBibliotheque(Bibliotheque biblio)
        {
            Console.Write("\nChemin du fichier à charger : ");
            var chemin = Console.ReadLine();
            biblio.Charger(chemin);
        }
    }
}