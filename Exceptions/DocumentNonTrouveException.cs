using System;

namespace BibliothequeNumerique.Exceptions
{
    public class DocumentNonTrouveException : Exception
    {
        public Guid IdDocument { get; }

        public DocumentNonTrouveException(Guid idDocument)
            : base($"Document avec l'ID {idDocument} non trouvé.")
        {
            IdDocument = idDocument;
        }

        public DocumentNonTrouveException(Guid idDocument, string message)
            : base(message)
        {
            IdDocument = idDocument;
        }
    }
}