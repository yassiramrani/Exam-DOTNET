namespace BibliothequeNumerique.Models
{
    public class Magazine : Document
    {
        public int Numero { get; set; }

        public Magazine(string titre, string auteur, int annee, int numero)
            : base(titre, auteur, annee)
        {
            Numero = numero;
        }

        public override string AfficherDetails()
        {
            return $"[MAGAZINE] ID: {Id} | Titre: {Titre} | Auteur: {Auteur} | Année: {Annee} | Numéro: {Numero}";
        }
    }
}