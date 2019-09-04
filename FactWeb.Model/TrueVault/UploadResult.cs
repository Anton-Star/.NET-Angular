namespace FactWeb.Model.TrueVault
{
    public class UploadResult
    {
        public BlobResult Blob { get; set; }
        public string Blob_filename { get; set; }
        public string Blob_id { get; set; }
        public string Blob_size { get; set; }
        public string Result { get; set; }
    }

    public class BlobResult
    {
        public string Filename { get; set; }
        public string Id { get; set; }
        public string Size { get; set; }

    }
}
