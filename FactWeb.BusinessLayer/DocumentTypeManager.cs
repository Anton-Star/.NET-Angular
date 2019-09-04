using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class DocumentTypeManager : BaseManager<DocumentTypeManager, IDocumentTypeRepository, DocumentType>
    {
        public DocumentTypeManager(IDocumentTypeRepository repository) : base(repository)
        {
        }
    }
}
