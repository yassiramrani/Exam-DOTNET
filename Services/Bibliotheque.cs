using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BibliothequeNumerique.Exceptions;
using BibliothequeNumerique.Models;

namespace BibliothequeNumerique.Services
{
    public class Bibliotheque
    {
        private List<Document> Documents { get; set; } = new List<Document>();

        public void AjouterDocument(Document document)
        {
            Documents.Add(document);
            Console.WriteLine($"Document ajouté : {document.Titre}");
        }

        public void SupprimerDocument(Guid id)
        {
            var document = Documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                throw new DocumentNonTrouveException(id);
            }

            Documents.Remove(document);
            Console.WriteLine($"Document supprimé : {document.Titre}");
        }

        public List<Document> Rechercher(string motCle)
        {
            motCle = motCle.ToLower();
            return Documents.Where(d =>
                d.Titre.ToLower().Contains(motCle) ||
                d.Auteur.ToLower().Contains(motCle)
            ).ToList();
        }

        public void AfficherTous()
        {
            if (Documents.Count == 0)
            {
                Console.WriteLine("Aucun document dans la bibliothèque.");
                return;
            }

            foreach (var doc in Documents)
            {
                Console.WriteLine(doc.AfficherDetails());
            }
        }

        // PARTIE 3 : Sauvegarde dans un fichier CSV
        public void Sauvegarder(string cheminFichier)
        {
            try
            {
                using (var fileStream = new FileStream(cheminFichier, FileMode.Create))
                using (var writer = new StreamWriter(fileStream))
                {
                    foreach (var doc in Documents)
                    {
                        string ligne = "";
                        if (doc is Livre livre)
                        {
                            ligne = $"Livre;{livre.Id};{livre.Titre};{livre.Auteur};{livre.Annee};{livre.NombrePages}";
                        }
                        else if (doc is Magazine mag)
                        {
                            ligne = $"Magazine;{mag.Id};{mag.Titre};{mag.Auteur};{mag.Annee};{mag.Numero}";
                        }
                        else if (doc is DocumentPDF pdf)
                        {
                            ligne = $"PDF;{pdf.Id};{pdf.Titre};{pdf.Auteur};{pdf.Annee};{pdf.TailleEnMo}";
                        }

                        writer.WriteLine(ligne);
                    }
                }

                Console.WriteLine($"Bibliothèque sauvegardée dans {cheminFichier} ({Documents.Count} documents).");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Erreur d'accès au fichier : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
            }
        }

        // PARTIE 3 : Chargement depuis un fichier CSV
        public void Charger(string cheminFichier)
        {
            try
            {
                if (!File.Exists(cheminFichier))
                {
                    throw new FileNotFoundException($"Fichier {cheminFichier} introuvable.");
                }

                var nouveauxDocuments = new List<Document>();

                using (var fileStream = new FileStream(cheminFichier, FileMode.Open))
                using (var reader = new StreamReader(fileStream))
                {
                    string ligne;
                    int ligneNum = 0;

                    while ((ligne = reader.ReadLine()) != null)
                    {
                        ligneNum++;
                        var parts = ligne.Split(';');

                        if (parts.Length < 6)
                        {
                            Console.WriteLine($"Ligne {ligneNum} ignorée : format incorrect.");
                            continue;
                        }

                        try
                        {
                            var type = parts[0];
                            var id = Guid.Parse(parts[1]);
                            var titre = parts[2];
                            var auteur = parts[3];
                            var annee = int.Parse(parts[4]);
                            var valeurSpecifique = parts[5];

                            Document doc = type switch
                            {
                                "Livre" => new Livre(titre, auteur, annee, int.Parse(valeurSpecifique)) { Id = id },
                                "Magazine" => new Magazine(titre, auteur, annee, int.Parse(valeurSpecifique)) { Id = id },
                                "PDF" => new DocumentPDF(titre, auteur, annee, double.Parse(valeurSpecifique)) { Id = id },
                                _ => throw new FormatException($"Type de document inconnu : {type}")
                            };

                            nouveauxDocuments.Add(doc);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Ligne {ligneNum} ignorée : {ex.Message}");
                        }
                    }
                }

                Documents = nouveauxDocuments;
                Console.WriteLine($"Bibliothèque chargée depuis {cheminFichier} ({Documents.Count} documents).");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Erreur d'accès au fichier : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
            }
        }
    }
}