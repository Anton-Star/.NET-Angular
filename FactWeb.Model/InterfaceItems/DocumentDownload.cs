namespace FactWeb.Model.InterfaceItems
{
    public class DocumentDownload
    {
        public byte[] File { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
    }
}
