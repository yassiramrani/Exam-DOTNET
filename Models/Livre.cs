namespace BibliothequeNumerique.Models
{
    public class Livre : Document
    {
        public int NombrePages { get; set; }

        public Livre(string titre, string auteur, int annee, int nombrePages)
            : base(titre, auteur, annee)
        {
            NombrePages = nombrePages;
        }

        public override string AfficherDetails()
        {
            return $"[LIVRE] ID: {Id} | Titre: {Titre} | Auteur: {Auteur} | Année: {Annee} | Pages: {NombrePages}";
        }
    }
}