using System;


namespace MangaDexSharp.Objects
{
    public sealed class ChapterPage
    {
        //private MangaDexClient _client;
        //private string _baseUrl;
        //private string _hash;
        //private string[] _fileNames;

        public Uri CompressedImageUrl { get; }
        public Uri ImageUrl => UseCompressedImage ? CompressedImageUrl : SourceQualityImageUrl;
        public Uri SourceQualityImageUrl { get; }
        public bool UseCompressedImage { get; set; } = false;

        internal ChapterPage(
            //MangaDexClient client,
            string baseUrl,
            string hash,
            string[] fileNames,
            bool dataSaver)
        {
            //_client = client;
            //_baseUrl = baseUrl;
            //_hash = hash;
            //_fileNames = fifeNames;
            UseCompressedImage = dataSaver;
            SourceQualityImageUrl = new Uri(baseUrl + "/data/" + hash + "/"+ fileNames[0]);
            CompressedImageUrl = new Uri(baseUrl + "/data-saver/" + hash + "/" + fileNames[1]); 
        }

        //public async Task<byte[]> LoadImageData()
        //{

        //}

        //TODO: Add report methods?
        //https://api.mangadex.org/docs.html#section/Report
    }
}
