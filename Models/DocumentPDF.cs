namespace BibliothequeNumerique.Models
{
    public class DocumentPDF : Document
    {
        public double TailleEnMo { get; set; }

        public DocumentPDF(string titre, string auteur, int annee, double tailleEnMo)
            : base(titre, auteur, annee)
        {
            TailleEnMo = tailleEnMo;
        }

        public override string AfficherDetails()
        {
            return $"[PDF] ID: {Id} | Titre: {Titre} | Auteur: {Auteur} | Année: {Annee} | Taille: {TailleEnMo} Mo";
        }
    }
}