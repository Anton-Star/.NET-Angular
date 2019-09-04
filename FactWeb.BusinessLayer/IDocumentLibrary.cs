using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System.IO;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public interface IDocumentLibrary
    {
        Document AddFile(string organizationName, string fileName, byte[] file);
        Task<Document> AddFileAsync(string organizationName, string fileName, byte[] file);

        Document AddFile(string organizationName, string fileName, string fileType, byte[] file);
        Task<Document> AddFileAsync(string organizationName, string fileName, string fileType, byte[] file);

        DocumentDownload GetFile(string organizationName, string fileName);
        Task<DocumentDownload> GetFileAsync(string organizationName, string fileName);

        void Remove(string organizationName, string fileName);
        Task RemoveAsync(string organizationName, string fileName);

        MemoryStream GetFileAsStream(string organizationName, string fileName);
    }
}
