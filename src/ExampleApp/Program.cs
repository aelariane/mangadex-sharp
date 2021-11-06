using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MangaDexSharp;
using MangaDexSharp.Api;
using MangaDexSharp.Api.Data;
using MangaDexSharp.Collections;
using MangaDexSharp.Enums;
using MangaDexSharp.Objects;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Order;
using MangaDexSharp.Resources;

namespace ExampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //ID of manga to test (Pandora hearts)
            //https://mangadex.org/title/25aaabb1-9f74-4469-a8d6-1eac5924cc79/pandora-hearts
            Guid pandoraHearts = new Guid("25aaabb1-9f74-4469-a8d6-1eac5924cc79");
            var client = new MangaDexClient();

            //----Auth part----
            //User's login data
            //Uncomment code below if you have account/or want to create one and test
            //var credentials = new UserCredentials("username", null, "password");

            //client.SetUserCredentials(credentials);
            //Console.WriteLine("Start login...");
            //await client.Auth.Login();
            //Console.WriteLine("Logged in");

            //Displaying current user's name
            //Console.WriteLine(client.CurrentUser.Username);
            //-----------------

            //Get single manga test
            Console.WriteLine("Getting single manga");
            Manga pandora = await client.Manga.ViewManga(pandoraHearts);
            Console.WriteLine(pandora.Title.English);

            //--------------------------
            //Get manga status test (Only uncomment if auth part is uncommented)
            //Console.WriteLine("Testing get/set reading statuses");
            //MangaReadingStatus? status = await client.Manga.GetMangaReadingStatus(pandoraHearts);
            //Console.WriteLine(status == null ? "null" : status.ToString());

            //Test updating reading status
            //await client.Manga.UpdateReadingStatus(pandoraHearts, MangaReadingStatus.Completed);
            //Console.WriteLine("Updated");
            //await Task.Delay(5000);
            //status = await client.Manga.GetMangaReadingStatus(pandoraHearts);
            //Console.WriteLine(status == null ? "null" : status.ToString());
            //--------------------------

            //Test getting volumes info
            Console.WriteLine("Testing volumes info");
            IReadOnlyCollection<MangaVolumeInfo> volumes = await client.Manga.Aggregate(pandoraHearts);
            foreach (var volume in volumes)
            {
                Console.WriteLine("{");
                Console.WriteLine("  Volume: " + volume.Volume);
                Console.WriteLine("  Count: " + volume.Count);
                foreach (var chapter in volume.Chapters)
                {
                    Console.WriteLine("  {");
                    Console.WriteLine("     Key: " + chapter.Key);
                    var val = chapter.Value;
                    Console.WriteLine("     Id: " + val.Id);
                    Console.WriteLine("     Count: " + val.Count);
                    Console.WriteLine("     Chapter: " + val.Chapter);
                    Console.WriteLine("  }");
                    Console.WriteLine("{");
                }
                Console.WriteLine("}");
            }

            //Test getting tag list
            Console.WriteLine("Getting tags");
            ResourceCollection<Tag> tags = await client.Manga.GetTagList();
            foreach (var tag in tags)
            {
                Console.WriteLine($"{tag.Name.English} - {tag.Group}");
            }

            Console.WriteLine("Simple tests completed. Press any key to exit...");
            Console.ReadLine();
        }
    }
}
