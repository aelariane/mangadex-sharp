# MangaDexSharp

MangaDexSharp is an *unofficial* .NET library implementing some basic MangaDex(https://mangadex.org) API operations (Mostly read ones)<br>
MangaDex API documentation can be found here: https://api.mangadex.org
<br>

### About ###
MangaDex is an ad-free manga reader offering high-quality images!: https://mangadex.org<br>
<br>

The library is *not official* and made for personal purposes.<br>
It implements most of common api operations for common users and is oriented to provide simple access to most used and basic read operations<br>

***Nothing related to uploading/creating new resources via API is implemented***<br>

<br>

Be aware to do alot of requests because of rate limiters (https://api.mangadex.org/docs.html#section/Rate-limits), as these aren't implemented, and taking care of them is on the side of application, using the library.<br>

### Usage ###
* `MangaDexSharp.Parameters` - This namespace provides access to query parameters that are used in API requests
* `MangaDexSharp.Resources` - This namespace contains Entities/Objects retrieved by API
<br>

`MangaDexSharp.MangaDexClient` is the main entry point.<br>
**Api endpoints can be accessed via client, and currently presented Apis are:**
* `MangaDexClient.AtHome` 
* `MangaDexClient.Auth`
* `MangaDexClient.Author`
* `MangaDexClient.Follows`
* `MangaDexClient.Chapter`
* `MangaDexClient.Cover`
* `MangaDexClient.List`
* `MangaDexClient.Manga`
* `MangaDexClient.Group`
* `MangaDexClient.User`

<br>

Properties of `MangaDexClient` listed above are instances of the APIs.<br>
It is very recommended to check official api documentation (https://api.mangadex.org) to ensure call is correct.

<br>

Simple example below:<br>

```cs
using System;
using MangaDexSharp;
using MangaDexSharp.Resources;

class Program
{
    public static void Main(String[] args)
    {
            //Creating client
            var client = new MangaDexClient();
            
            //Defining ID of manga to get. (Pandora Hearts in the test)
            //https://mangadex.org/title/25aaabb1-9f74-4469-a8d6-1eac5924cc79/pandora-hearts
            Guid pandoraHearts = new Guid("25aaabb1-9f74-4469-a8d6-1eac5924cc79");
            //Getting manga resource
            Manga pandora = await client.Manga.ViewManga(pandoraHearts);

            //Displaying default title of Manga
            Console.WriteLine(pandora.Title.Default);
    }
}
```

### Authentication ###

Authenticating is also supported, and some requests only availible for logged in user.<br>
To authenticate user, it is needed to provide `MangaDexSharp.UserCredentials` to the client.<br>
Currently logged in can be obtained by `MangaDexClient.CurrentUser` property and presents unique Resource type `LocalUser` that provides actions availible only for logged in User. (Such as get followed manga/authors, check User's follow)

<br>

Refreshing token happens automatically in background, but it is still possible to update session token manually via `AuthApi.RefreshToken()`

<br>

Example of authenticating below:

```cs
using System;
using MangaDexSharp;
using MangaDexSharp.Resources;

class Program
{
    public static void Main(String[] args)
    {
            var client = new MangaDexClient();

            //Creating and passing User's login data to client
            var credentials = new UserCredentials("username", "email", "password");
            client.SetUserCredentials(credentials);
            
            //Waiting for authenticating
            await client.Auth.Login();

            //Displaying current user's name
            Console.WriteLine(client.CurrentUser.Username);
    }
}
```


### Calls on entities ###
While API provide raw implementation of enpoint, Resources have some methods (For example, `Manga` Resource has method called GetFeed)<br>
***Methods executed on entities/resource automatically apply `MangaDexSharp.MangaDexClient.Settings` where possible in requests. Such as ItemsPerPage, or content filters, or translated languages.<br> These filters will be added automatically where possible if method is called on Resource (Direct API calls aren't affected by Settings)***
<br>

Example:

```cs
using System;
using MangaDexSharp;
using MangaDexSharp.Constants;
using MangaDexSharp.Resources;

class Program
{
    public static void Main(String[] args)
    {
            //Creating client
            var client = new MangaDexClient();
            //Adding English language code (en) to languages
            client.Settings.AddTranslatedLanguage(LanguageKeys.English);

            //Defining ID of manga to get. (Pandora Hearts in the test)
            //https://mangadex.org/title/25aaabb1-9f74-4469-a8d6-1eac5924cc79/pandora-hearts
            Guid pandoraHearts = new Guid("25aaabb1-9f74-4469-a8d6-1eac5924cc79");
            //Getting manga resource
            //This call is not affected by Settings, because it is direct API call.
            Manga pandora = await client.Manga.ViewManga(pandoraHearts);

            //This call IS affected by Settings, because executed on Resource.
            //The feed only includes chapters translated to English
            var feed = await pandora.GetFeed();
    }
}
```


### Project structure ###
* Api - Contains raw API implementations.
* Constants - Contains some useful constant values (Such as language keys).
* Collections - Contains some collections.
* Enums - Contains enums for Resources.
* Exceptions - Custom exceptions that can be thrown by API calls.
* Internal - Some scary internal things, such as objects mapping, etc...
* Objects - Entities that make aren't actual Resources, but have logical relation to them.
* Parameters - Query parameters for API calls.
* Resourcse - Contains Resources objects themselves.

<br>

### Not everything is tested ###
