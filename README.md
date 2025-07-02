# Homeoffice Zeiterfassung – Ein Full-Stack-Projekt

In dieser README.md-Datei beschreibe ich die Entwicklung einer vollständigen Full-Stack-Anwendung – vom Backend-Konzept bis zur Umsetzung des Frontends.

<!-- TOC start (generated with https://github.com/derlin/bitdowntoc) -->

- [Homeoffice Zeiterfassung – Ein Full-Stack-Projekt](#homeoffice-zeiterfassung-ein-full-stack-projekt)
   * [Motivation](#motivation)
   * [Projektstruktur im Backend](#projektstruktur-im-backend)
         - [`Homeoffice.Models`](#homeofficemodels)
         - [`Homeoffice.DataAccess`](#homeofficedataaccess)
         - [`Homeoffice.Contracts`](#homeofficecontracts)
         - [`Homeoffice.Services`](#homeofficeservices)
         - [`Homeoffice.API` (implizite Schicht)](#homeofficeapi-implizite-schicht)
   * [Modellerstellung](#modellerstellung)
         - [Beziehung zwischen User und HomeOfficeEntry](#beziehung-zwischen-user-und-homeofficeentry)
   * [Die DataAccess-Schicht: Das Herz der Datenbankverbindung](#die-dataaccess-schicht-das-herz-der-datenbankverbindung)
      + [Der ApplicationDbContext](#der-applicationdbcontext)
         - [Wichtige Konzepte im `DbContext`](#wichtige-konzepte-im-dbcontext)
      + [Fluent API Konfigurationen](#fluent-api-konfigurationen)
   * [Datenbank-Migrationen](#datenbank-migrationen)
         - [`dotnet ef migrations add Create_Database -p Homeoffice.DataAccess -s Homeoffice.API -c ApplicationDbContext -o Data/Migrations`](#dotnet-ef-migrations-add-create_database-p-homeofficedataaccess-s-homeofficeapi-c-applicationdbcontext-o-datamigrations)
         - [`dotnet ef database update -p Homeoffice.DataAccess -s Homeoffice.API -c ApplicationDbContext`](#dotnet-ef-database-update-p-homeofficedataaccess-s-homeofficeapi-c-applicationdbcontext)
         - [Warum muss `Microsoft.EntityFrameworkCore.Design` in `Homeoffice.API` sein?](#warum-muss-microsoftentityframeworkcoredesign-in-homeofficeapi-sein)
   * [Die Service-Schicht und die Authentifizierung](#die-service-schicht-und-die-authentifizierung)
      + [Was ist ein JWT (JSON Web Token)?](#was-ist-ein-jwt-json-web-token)
      + [Implementierung im Backend (.NET)](#implementierung-im-backend-net)
         - [1\. Token-Validierung (`Program.cs`)](#1-token-validierung-programcs)
         - [2\. Token-Erstellung (`TokenService.cs`)](#2-token-erstellung-tokenservicecs)
      + [Implementierung im Frontend (Angular)](#implementierung-im-frontend-angular)
      + [Das Zusammenspiel von Backend und Frontend](#das-zusammenspiel-von-backend-und-frontend)
   * [Die Service-Schicht und die Authentifizierung](#die-service-schicht-und-die-authentifizierung-1)
      + [Was ist ein JWT (JSON Web Token)?](#was-ist-ein-jwt-json-web-token-1)
      + [Implementierung im Backend (.NET)](#implementierung-im-backend-net-1)
         - [1\. Token-Validierung (`Program.cs`)](#1-token-validierung-programcs-1)
         - [2\. Token-Erstellung (`TokenService.cs`)](#2-token-erstellung-tokenservicecs-1)
      + [Implementierung im Frontend (Angular)](#implementierung-im-frontend-angular-1)
      + [Das Zusammenspiel von Backend und Frontend](#das-zusammenspiel-von-backend-und-frontend-1)
      + [Der `AuthService`: Login-Logik](#der-authservice-login-logik)
      + [Der `AccountController`: Die API-Schnittstelle](#der-accountcontroller-die-api-schnittstelle)
      + [Der `EmailService`: Automatisierte Benachrichtigungen](#der-emailservice-automatisierte-benachrichtigungen)
         - [1\. Konfiguration](#1-konfiguration)
         - [2\. Implementierung](#2-implementierung)
      + [Der `TimeTrackingService`: Die Kernlogik der Anwendung](#der-timetrackingservice-die-kernlogik-der-anwendung)
         - [Aufgaben des `TimeTrackingService`](#aufgaben-des-timetrackingservice)
      + [Der `TimeTrackingController`: Die API-Schnittstelle](#der-timetrackingcontroller-die-api-schnittstelle)
         - [Die Endpunkte im Detail](#die-endpunkte-im-detail)
      + [Das `Contracts`-Projekt: Die gemeinsame Sprache](#das-contracts-projekt-die-gemeinsame-sprache)
      + [Data Transfer Objects (DTOs) und die Wahl von `record`](#data-transfer-objects-dtos-und-die-wahl-von-record)
         - [Warum `record` anstelle von `class`?](#warum-record-anstelle-von-class)
   * [Das Backend abschließen: Die `Program.cs`](#das-backend-abschließen-die-programcs)
      + [Die Konfiguration im Detail](#die-konfiguration-im-detail)
         - [1\. Services konfigurieren (`builder.Services...`)](#1-services-konfigurieren-builderservices)
         - [2\. HTTP-Request-Pipeline aufbauen (`app.Use...`)](#2-http-request-pipeline-aufbauen-appuse)
         - [3\. Datenbank Seeding beim Start (`SeedDatabaseAsync`)](#3-datenbank-seeding-beim-start-seeddatabaseasync)
   * [Das Frontend: Eine moderne Angular-Anwendung](#das-frontend-eine-moderne-angular-anwendung)
      + [1\. Architektur und Ordnerstruktur](#1-architektur-und-ordnerstruktur)
      + [2\. Guards: Die Türsteher der Anwendung](#2-guards-die-türsteher-der-anwendung)
      + [3\. Interceptors: Die Poststelle der Anwendung](#3-interceptors-die-poststelle-der-anwendung)
      + [4\. Routing: Der Wegweiser der Anwendung](#4-routing-der-wegweiser-der-anwendung)
- [Dokumentation: Token-Refresh-Prozess (Frontend)](#dokumentation-token-refresh-prozess-frontend)
   * [1. Warum ist der Refresh-Token-Prozess wichtig?](#1-warum-ist-der-refresh-token-prozess-wichtig)
   * [2. Wie funktioniert der Prozess in unserer App?](#2-wie-funktioniert-der-prozess-in-unserer-app)
   * [3. Implementierung im Detail](#3-implementierung-im-detail)
      + [`jwtInterceptor`](#jwtinterceptor)
      + [`TokenService`](#tokenservice)
      + [`refreshToken()` Methode (in `AuthService`)](#refreshtoken-methode-in-authservice)
      + [Diagramm: Sichere JWT-Refresh-Token-Ablaufsteuerung mit paralleler Anfrageverarbeitung](#diagramm-sichere-jwt-refresh-token-ablaufsteuerung-mit-paralleler-anfrageverarbeitung)

<!-- TOC end -->


## Motivation
Dieses Projekt hat mich von Anfang an begeistert. Die Idee, eine praxisnahe und vollständige Anwendung mit einem modernen Tech-Stack zu entwickeln, war für mich der ideale Anlass, meine Fähigkeiten gezielt einzusetzen und weiterzuentwickeln. Mein Ziel war es, eine Lösung zu schaffen, die stabil, gut wartbar und sicher ist – genau so, wie man es auch in einem professionellen Entwicklungsteam erwarten würde.

Aufgabenstellung:

> Es soll eine Anwendung erstellt werden, die es Mitarbeitern ermöglicht, sich im HomeOffice anzumelden und die automatisch die Zeiten an das Personalburo übermittelt.
> Dazu soll der Mitarbeiter sich zuerst einloggen, dann die Möglichkeit haben seine HomeOffice Zeit zu starten, sie wieder zu stoppen und sich für verschiedene Tag eine Übersicht ausgeben zu lassen.
Beim Stoppen der HomeOffice-Tatigkeit soll automatisch eine Mail an das Personalbüro verschickt werden.
> 
> Voraussetzungen:
> Es soll eine Anwendung erstellt werden, die sich aus drei Teilen zusammensetzt:
> 1. Als Grundlage der Datenhaltung dient eine Datenbank (SQLServer | MongoDB).
> 2. Ein Webservice (ASP.NET WebAPI | NodeJS] dient zum Bereitstellen und Ändern der Daten in der Datenbank.
> 3. Das Frontend zur Anzeige bildet eine einfache [HTML-Anwendung | WPF-Anwendung | Angular-Anwendung | App].
> 
> Anforderungen:
> 1. Es ist ein Datenmodell der genutzten Datentypen zu erstellen. Auf diesem sollte die
Datenbank beruhen.
> 2. Der Webservice muss einen Login zur Verfügung stellen. Dieser Webservice kann entweder ein REST-Service oder ein GraphQL-Service sein.
> 3. Die Konfiguration der Personalbüro-Mail-Adresse soll im Webservice hinterlegt sein. Allerdings soll sie nicht im Code liegen, sondern eine Konfigurationsdatei nutzen!
> 4. User werden als gegeben in der Datenbank vorausgesetzt. Das bedeutet sie können per Import oder Skript in die DB eingespielt werden. Es ist keine eigene Nutzerverwaltung zu erstellen!
> 5. Der Mailversand soll über ein einzubindendes Paket aus einem der Technologie zugrunde liegenden Paketmanager realisiert werden.
> 6. Design spielt keine Rolle.

Für die Umsetzung habe ich mich für ein **.NET 9 Backend** und ein **Angular 20 Frontend** entschieden, um moderne und leistungsstarke Technologien zu nutzen.

## Projektstruktur im Backend

Um das Backend sauber und wartbar zu halten, wurde eine klassische Schichtenarchitektur (Layered Architecture) gewählt. Jede Schicht hat eine klar definierte Aufgabe und kommuniziert nur mit den direkt angrenzenden Schichten.

#### `Homeoffice.Models`

Das Fundament der Anwendung. Diese Schicht enthält die C\#-Klassen (Entities), die unsere Datenbank-Tabellen repräsentieren (z.B. `User`, `HomeOfficeEntry`). Alle anderen Projekte im Backend sind von dieser Schicht abhängig, da sie die grundlegenden Datenstrukturen definiert.

#### `Homeoffice.DataAccess`

Der "Arbeiter" für die Datenbank. Diese Schicht ist für die gesamte Kommunikation mit der Datenbank zuständig. Sie enthält den `DbContext` von Entity Framework Core und die Konfigurationen (Fluent API), die festlegen, wie unsere C\#-Klassen auf Datenbank-Tabellen abgebildet werden. Sie ist direkt von der `Models`-Schicht abhängig.

#### `Homeoffice.Contracts`

Das "Regelbuch" oder der Vertrag der Anwendung. Diese Schicht enthält die `Interfaces` für unsere Services (z.B. `ITimeTrackingService`) und die DTOs (Data Transfer Objects). Sie definiert, WIE die Schichten miteinander kommunizieren, ohne dass sie sich direkt kennen müssen. Sowohl die `Services`- als auch die `API`-Schicht sind von den `Contracts` abhängig.

#### `Homeoffice.Services`

Das "Gehirn" der Anwendung. Hier lebt die gesamte Geschäftslogik. Ein Service nimmt Anfragen entgegen (z.B. von einem API-Controller), orchestriert die notwendigen Schritte – holt Daten aus der `DataAccess`-Schicht, führt Berechnungen durch, ruft andere Services auf (wie den `EmailService`) – und gibt das Ergebnis zurück. Diese Schicht ist von `DataAccess` und `Contracts` abhängig.

#### `Homeoffice.API` (implizite Schicht)

Das "Tor zur Außenwelt". Diese Schicht empfängt HTTP-Anfragen vom Frontend, ruft die passende Methode in der `Services`-Schicht auf und sendet die Antwort zurück an den Client.

## Modellerstellung

Das Herz des Datenmodells besteht aus zwei zentralen Entitäten: `User` und `HomeOfficeEntry`.

```csharp
namespace Homeoffice.Models.Entities
{
    public abstract class BaseEntity : IBaseEntity<int>, IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
```

```csharp
namespace Homeoffice.Models.Entities
{
    public class HomeOfficeEntry : BaseEntity
    {
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public string? Description { get; set; }
        public bool IsEmailSent { get; set; } = false;
        public string UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
```

```csharp
namespace Homeoffice.Models.Entities.Identity
{
    public class User : IdentityUser, IBaseEntity<string>, IAuditable
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public virtual ICollection<HomeOfficeEntry> HomeOfficeEntries { get; set; } = new List<HomeOfficeEntry>();
    }
}
```

#### Beziehung zwischen User und HomeOfficeEntry

Zwischen diesen beiden Tabellen besteht eine klassische **Eins-zu-Viele-Beziehung (One-to-Many)**:

  * Ein `User` kann viele `HomeOfficeEntry`-Einträge haben.
  * Jeder `HomeOfficeEntry` gehört aber immer zu genau einem `User`.

Dies wird im Code so umgesetzt:

  * Die `User`-Klasse hat eine `ICollection<HomeOfficeEntry> HomeOfficeEntries`, um auf alle zugehörigen Zeiteinträge zugreifen zu können.
  * Die `HomeOfficeEntry`-Klasse hat eine `UserId`-Eigenschaft (der Fremdschlüssel) und eine `User`-Eigenschaft (die Navigation Property), um auf den zugehörigen Benutzer zu verweisen.


## Die DataAccess-Schicht: Das Herz der Datenbankverbindung

Die `DataAccess`-Schicht ist die einzige Schicht, die direkt mit der Datenbank kommuniziert. Ihr Kernstück ist der `ApplicationDbContext`, der von Entity Framework Core (EF Core) bereitgestellt wird.

### Der ApplicationDbContext

Der `DbContext` ist wie eine "Sitzung" mit der Datenbank. Er weiß, welche Entitäten wir haben, und verfolgt alle Änderungen, die wir an ihnen vornehmen, um sie dann gesammelt zu speichern.

<details>
<summary><b>Code: ApplicationDbContext.cs</b></summary>
<br>
  
```csharp
using Homeoffice.Models.Entities;
using Homeoffice.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Homeoffice.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HomeOfficeEntry> HomeOfficeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTimeOffset.UtcNow;
                }
                entry.Entity.ModifiedDate = DateTimeOffset.UtcNow;
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }
    }
}
```
</details>


#### Wichtige Konzepte im `DbContext`

  * **`modelBuilder.ApplyConfigurationsFromAssembly(...)`**: Dies ist eine saubere Methode, um EF Core zu konfigurieren. Anstatt alle Regeln für unsere Datenbanktabellen direkt in die `OnModelCreating`-Methode zu schreiben und sie unübersichtlich zu machen, suchen wir mit diesem Befehl automatisch nach allen Konfigurations-Klassen im `DataAccess`-Projekt und wenden sie an.

  * **`SaveChangesAsync` Überschreibung**: Wir überschreiben diese Methode, um vor jedem Speichervorgang unsere eigene Logik auszuführen. In diesem Fall rufen wir `UpdateTimestamps()` auf, um sicherzustellen, dass die `CreatedDate`- und `ModifiedDate`-Felder **automatisch und konsistent** für jede Änderung gesetzt werden. Das ist eine zentrale Stelle für Logik, die immer ausgeführt werden muss.

  * **`CancellationToken cancellationToken = default`**: Dies ist ein Standard-Mechanismus in .NET, um Operationen abbrechen zu können. Stellen Sie sich vor, eine Datenbankabfrage dauert sehr lange und der Benutzer schließt die Anwendung. Der `CancellationToken` kann ein Signal senden, um die laufende Operation abzubrechen und Ressourcen zu sparen. Es ist eine gute Praxis, ihn durch alle asynchronen Methoden weiterzureichen.

  * **`ChangeTracker.Entries<IAuditable>()`**: Der `ChangeTracker` ist das "Gedächtnis" von EF Core. Er weiß genau, welche Objekte aus der Datenbank geladen wurden und welche geändert, hinzugefügt oder gelöscht wurden. Die Abfrage `Entries<IAuditable>()` ist sehr clever: Sie filtert alle bekannten Objekte und gibt uns nur diejenigen zurück, die unser `IAuditable`-Interface implementieren – also alle, die `CreatedDate`- und `ModifiedDate`-Felder haben.

-----

### Fluent API Konfigurationen

Um detaillierte Regeln für unsere Datenbank-Tabellen festzulegen (z.B. Beziehungen, Längenbeschränkungen, Indizes), verwenden wir Konfigurations-Klassen.

<details>
<summary><b>Code: UserConfiguration.cs</b></summary>
<br>
  
```csharp
using Homeoffice.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homeoffice.DataAccess.Data.Config.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Diese Konfiguration ist leer, da ASP.NET Core Identity
            // und die EF-Core-Konventionen bereits alles Notwendige
            // für die User-Tabelle korrekt einrichten.
        }
    }
}
```
</details>

<details>
<summary><b>Code: HomeOfficeEntryConfiguration.cs</b></summary>
<br>
  
```csharp
using Homeoffice.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homeoffice.DataAccess.Data.Config
{
    public class HomeOfficeEntryConfiguration : IEntityTypeConfiguration<HomeOfficeEntry>
    {
        public void Configure(EntityTypeBuilder<HomeOfficeEntry> builder)
        {
            builder.Property(e => e.StartTime)
                .IsRequired()
                .HasColumnType("datetimeoffset");
            
            builder.Property(e => e.EndTime)
                .IsRequired(false) // Muss optional sein für die Start/Stop-Logik
                .HasColumnType("datetimeoffset");
            
            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500);
            
            builder.Property(e => e.IsEmailSent)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Definiert die Beziehung: Ein HomeOfficeEntry hat einen User.
            // Ein User hat viele HomeOfficeEntries.
            builder.HasOne(e => e.User).WithMany(u => u.HomeOfficeEntries)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Schützt vor versehentlichem Löschen der Historie
            
            // Fügt einen Index auf der UserId-Spalte hinzu, um Abfragen zu beschleunigen.
            builder.HasIndex(e => e.UserId);
        }
    }
}
```

</details>

## Datenbank-Migrationen

Nachdem das Datenmodell definiert ist, müssen wir EF Core anweisen, eine "Übersetzung" davon für die Datenbank zu erstellen. Das passiert mit den `dotnet ef`-Befehlen.

#### `dotnet ef migrations add Create_Database -p Homeoffice.DataAccess -s Homeoffice.API -c ApplicationDbContext -o Data/Migrations`

Dieser Befehl vergleicht Ihr aktuelles Code-Modell mit dem letzten Stand in den Migrationsdateien. Wenn er Unterschiede findet, erstellt er eine neue **Migrations-Datei**. Diese Datei enthält C\#-Code, der die notwendigen SQL-Befehle beschreibt, um die Datenbank auf den neuen Stand zu bringen (z.B. `CreateTable`, `AddColumn`).

  * `InitialCreate`: Ist der Name, den wir dieser Migration geben.
  * `-p Homeoffice.DataAccess`: Das **Projekt**, in dem sich der `DbContext` befindet.
  * `-s Homeoffice.API`: Das **Start-Projekt**, das ausgeführt wird, um Konfigurationen (wie den Connection String) zu laden.
  * `-c ApplicationDbContext`: Der Name des zu verwendenden `DbContext`.
  * `-o Data/Migrations`: Der **Ausgabe-Ordner** für die generierten Migrationsdateien.

#### `dotnet ef database update -p Homeoffice.DataAccess -s Homeoffice.API -c ApplicationDbContext`

Dieser Befehl nimmt die neueste Migrations-Datei und führt die darin enthaltenen Anweisungen tatsächlich auf der Zieldatenbank aus. Er erstellt also die Tabellen und Spalten. Die Parameter sind dieselben wie oben.

#### Warum muss `Microsoft.EntityFrameworkCore.Design` in `Homeoffice.API` sein?

Die `dotnet ef`-Tools müssen Ihre Anwendung zur Laufzeit analysieren können, um z.B. den Connection String aus der `appsettings.json` zu lesen. Sie tun das, indem sie das **Start-Projekt** (`-s Homeoffice.API`) im Hintergrund starten. Das `Design`-Paket ist die "Brücke", die es dem Start-Projekt ermöglicht, mit den externen Kommandozeilen-Tools zu kommunizieren. Es muss also im ausführbaren Projekt (`API`) vorhanden sein.

Absolut. Hier ist die Dokumentation für die Service-Schicht mit dem Fokus auf die JWT-Authentifizierung, genau wie Sie es gewünscht haben.

-----

## Die Service-Schicht und die Authentifizierung

Die Service-Schicht ist das Herzstück der Anwendungslogik. Hier werden die Anfragen von der API verarbeitet, Berechnungen durchgeführt und Daten über die `DataAccess`-Schicht ausgetauscht.

Ein zentraler Aspekt unseres Backends ist die Sicherheit. Da wir keine traditionellen Sessions verwenden, brauchen wir einen modernen Weg, um den Benutzer bei jeder Anfrage sicher zu identifizieren. Dafür habe ich **JWT (JSON Web Tokens)** verwendet.

### Was ist ein JWT (JSON Web Token)?

Stellen Sie sich einen JWT wie einen digitalen **Ausweis oder Reisepass** vor.

1.  **Ausstellung:** Wenn sich ein Benutzer mit seinem korrekten Benutzernamen und Passwort anmeldet, stellt ihm der Server (unser Backend) einen JWT aus.
2.  **Inhalt:** In diesem "Ausweis" stehen einige Informationen (genannt "Claims") über den Benutzer, z.B. seine `UserId` und sein `UserName`.
3.  **Sicherheit:** Der Ausweis ist digital signiert mit einem geheimen Schlüssel, den nur der Server kennt. Das macht ihn fälschungssicher.
4.  **Verwendung:** Das Frontend speichert diesen Ausweis nach dem Login. Bei jeder weiteren Anfrage an geschützte Bereiche der API (z.B. um die Zeit zu starten) zeigt das Frontend diesen Ausweis vor, indem es ihn im `Authorization`-Header mitschickt.
5.  **Prüfung:** Der Server prüft bei jeder Anfrage die Signatur des Ausweises. Wenn sie gültig ist, vertraut der Server den Informationen im Ausweis und weiß, welcher Benutzer die Anfrage stellt, ohne dass das Passwort erneut gesendet werden muss.

Der größte Vorteil dieses Ansatzes ist, dass er **zustandslos (stateless)** ist. Der Server muss sich nicht merken, wer eingeloggt ist. Er muss nur die Gültigkeit des "Ausweises" (Tokens) bei jeder Anfrage prüfen.

### Implementierung im Backend (.NET)

Die Implementierung im Backend besteht aus zwei Teilen: dem Validieren von Tokens und dem Erstellen von Tokens.

#### 1\. Token-Validierung (`Program.cs`)

Zuerst bringen wir unserer API bei, wie sie die vom Frontend gesendeten Tokens lesen und auf ihre Echtheit prüfen kann. Das passiert in der `Program.cs`. Dafür benötigen wir das NuGet-Paket `Microsoft.AspNetCore.Authentication.JwtBearer`.

<details>
<summary><b>Code: JWT-Authentifizierungskonfiguration in Program.cs</b></summary>
<br>

```csharp
// JWT-authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuer = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateAudience = true
    };
});
```

</details>

Dieser Codeblock konfiguriert den "Türsteher" unserer API. Er prüft, ob der Token mit unserem geheimen Schlüssel (`Jwt:Key`) signiert wurde und ob der Aussteller (`Issuer`) und die Zielgruppe (`Audience`) mit den Werten in unserer `appsettings.json` übereinstimmen.

<details>
<summary><b>Code: JWT-Einstellungen in appsettings.json</b></summary>
<br>

```json
"Jwt": {
  "Key": "A_VERY_LONG_RANDOM_STRING_AT_LEAST_64_CHARACTERS_LONG_1234567890ABCDEF",
  "Issuer": "HomeofficeAPI",
  "Audience": "HomeofficeClient"
}
```

</details>

#### 2\. Token-Erstellung (`TokenService.cs`)

Der zweite Teil ist das Erstellen des Tokens nach einem erfolgreichen Login. Diese Logik haben wir sauber in einen eigenen `TokenService` ausgelagert.

<details>
<summary><b>Code: TokenService.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Homeoffice.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Token creation failed", ex);
            }
        }
    }
}
```

</details>

### Implementierung im Frontend (Angular)

Das Frontend hat zwei einfache Aufgaben: den Token nach dem Login speichern und ihn bei jeder Anfrage an geschützte Endpunkte mitsenden. Um dies nicht bei jedem einzelnen API-Aufruf manuell machen zu müssen, verwenden wir einen **`HttpInterceptor`**.

Der `JwtInterceptor` fängt jede ausgehende HTTP-Anfrage automatisch ab, holt sich den gespeicherten Token vom `AuthService` und fügt ihn zum `Authorization`-Header hinzu.

<details>
<summary><b>Code: jwt.interceptor.ts</b></summary>
<br>

```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authservice = inject(AuthService);
  const token = authservice.currentUser()?.token;

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  return next(req);
};
```

</details>

### Das Zusammenspiel von Backend und Frontend

Der gesamte Prozess funktioniert also so:

1.  **Login:** Das Angular-Frontend sendet Username/Passwort an `POST /api/account/login`.
2.  **Token-Erstellung:** Das .NET-Backend verifiziert die Daten. Bei Erfolg ruft der `AuthService` den `TokenService` auf, der einen JWT erstellt.
3.  **Token-Antwort:** Das Backend sendet den Token zurück zum Frontend.
4.  **Token speichern:** Der `AuthService` im Frontend speichert den Token im `localStorage` des Browsers.
5.  **Geschützte Anfrage:** Der Benutzer klickt auf "Start". Das Frontend sendet eine Anfrage an `POST /api/timetracking/start`.
6.  **Intercept & Header hinzufügen:** Der `JwtInterceptor` fängt die Anfrage ab, liest den Token aus dem Speicher und fügt ihn zum Header hinzu: `Authorization: Bearer <der_lange_token>`.
7.  **Token-Validierung:** Das Backend empfängt die Anfrage. Die `[Authorize]`-Middleware sieht den Token, prüft seine Signatur, den Aussteller, die Zielgruppe und das Ablaufdatum.
8.  **Zugriff gewährt:** Wenn der Token gültig ist, wird die Anfrage an den `TimeTrackingController` weitergeleitet und die Aktion ausgeführt.

Absolut. Hier ist die Dokumentation für die Service-Schicht mit dem Fokus auf die JWT-Authentifizierung, genau wie Sie es gewünscht haben.

-----

## Die Service-Schicht und die Authentifizierung

Die Service-Schicht ist das Herzstück der Anwendungslogik. Hier werden die Anfragen von der API verarbeitet, Berechnungen durchgeführt und Daten über die `DataAccess`-Schicht ausgetauscht.

Ein zentraler Aspekt unseres Backends ist die Sicherheit. Da wir keine traditionellen Sessions verwenden, brauchen wir einen modernen Weg, um den Benutzer bei jeder Anfrage sicher zu identifizieren. Dafür habe ich **JWT (JSON Web Tokens)** verwendet.

### Was ist ein JWT (JSON Web Token)?

Stellen Sie sich einen JWT wie einen digitalen **Ausweis oder Reisepass** vor.

1.  **Ausstellung:** Wenn sich ein Benutzer mit seinem korrekten Benutzernamen und Passwort anmeldet, stellt ihm der Server (unser Backend) einen JWT aus.
2.  **Inhalt:** In diesem "Ausweis" stehen einige Informationen (genannt "Claims") über den Benutzer, z.B. seine `UserId` und sein `UserName`.
3.  **Sicherheit:** Der Ausweis ist digital signiert mit einem geheimen Schlüssel, den nur der Server kennt. Das macht ihn fälschungssicher.
4.  **Verwendung:** Das Frontend speichert diesen Ausweis nach dem Login. Bei jeder weiteren Anfrage an geschützte Bereiche der API (z.B. um die Zeit zu starten) zeigt das Frontend diesen Ausweis vor, indem es ihn im `Authorization`-Header mitschickt.
5.  **Prüfung:** Der Server prüft bei jeder Anfrage die Signatur des Ausweises. Wenn sie gültig ist, vertraut der Server den Informationen im Ausweis und weiß, welcher Benutzer die Anfrage stellt, ohne dass das Passwort erneut gesendet werden muss.

Der größte Vorteil dieses Ansatzes ist, dass er **zustandslos (stateless)** ist. Der Server muss sich nicht merken, wer eingeloggt ist. Er muss nur die Gültigkeit des "Ausweises" (Tokens) bei jeder Anfrage prüfen.

### Implementierung im Backend (.NET)

Die Implementierung im Backend besteht aus zwei Teilen: dem Validieren von Tokens und dem Erstellen von Tokens.

#### 1\. Token-Validierung (`Program.cs`)

Zuerst bringen wir unserer API bei, wie sie die vom Frontend gesendeten Tokens lesen und auf ihre Echtheit prüfen kann. Das passiert in der `Program.cs`. Dafür benötigen wir das NuGet-Paket `Microsoft.AspNetCore.Authentication.JwtBearer`.

<details>
<summary><b>Code: JWT-Authentifizierungskonfiguration in Program.cs</b></summary>
<br>

```csharp
// JWT-authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuer = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateAudience = true
    };
});
```

</details>

Dieser Codeblock konfiguriert den "Türsteher" unserer API. Er prüft, ob der Token mit unserem geheimen Schlüssel (`Jwt:Key`) signiert wurde und ob der Aussteller (`Issuer`) und die Zielgruppe (`Audience`) mit den Werten in unserer `appsettings.json` übereinstimmen.

<details>
<summary><b>Code: JWT-Einstellungen in appsettings.json</b></summary>
<br>

```json
"Jwt": {
  "Key": "A_VERY_LONG_RANDOM_STRING_AT_LEAST_64_CHARACTERS_LONG_1234567890ABCDEF",
  "Issuer": "HomeofficeAPI",
  "Audience": "HomeofficeClient"
}
```

</details>

#### 2\. Token-Erstellung (`TokenService.cs`)

Der zweite Teil ist das Erstellen des Tokens nach einem erfolgreichen Login. Diese Logik haben wir sauber in einen eigenen `TokenService` ausgelagert.

<details>
<summary><b>Code: TokenService.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Homeoffice.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Token creation failed", ex);
            }
        }
    }
}
```

</details>

### Implementierung im Frontend (Angular)

Das Frontend hat zwei einfache Aufgaben: den Token nach dem Login speichern und ihn bei jeder Anfrage an geschützte Endpunkte mitsenden. Um dies nicht bei jedem einzelnen API-Aufruf manuell machen zu müssen, verwenden wir einen **`HttpInterceptor`**.

Der `JwtInterceptor` fängt jede ausgehende HTTP-Anfrage automatisch ab, holt sich den gespeicherten Token vom `AuthService` und fügt ihn zum `Authorization`-Header hinzu.

<details>
<summary><b>Code: jwt.interceptor.ts</b></summary>
</br>

```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authservice = inject(AuthService);
  const token = authservice.currentUser()?.token;

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  return next(req);
};

```
</details>

### Das Zusammenspiel von Backend und Frontend

Der gesamte Prozess funktioniert also so:

1.  **Login:** Das Angular-Frontend sendet Username/Passwort an `POST /api/account/login`.
2.  **Token-Erstellung:** Das .NET-Backend verifiziert die Daten. Bei Erfolg ruft der `AuthService` den `TokenService` auf, der einen JWT erstellt.
3.  **Token-Antwort:** Das Backend sendet den Token zurück zum Frontend.
4.  **Token speichern:** Der `AuthService` im Frontend speichert den Token im `localStorage` des Browsers.
5.  **Geschützte Anfrage:** Der Benutzer klickt auf "Start". Das Frontend sendet eine Anfrage an `POST /api/timetracking/start`.
6.  **Intercept & Header hinzufügen:** Der `JwtInterceptor` fängt die Anfrage ab, liest den Token aus dem Speicher und fügt ihn zum Header hinzu: `Authorization: Bearer <der_lange_token>`.
7.  **Token-Validierung:** Das Backend empfängt die Anfrage. Die `[Authorize]`-Middleware sieht den Token, prüft seine Signatur, den Aussteller, die Zielgruppe und das Ablaufdatum.
8.  **Zugriff gewährt:** Wenn der Token gültig ist, wird die Anfrage an den `TimeTrackingController` weitergeleitet und die Aktion ausgeführt.

Dieser Kreislauf sorgt für eine sichere und zustandslose Kommunikation zwischen Frontend und Backend.


-----

### Der `AuthService`: Login-Logik

Der `AuthService` ist dafür verantwortlich, die Anmeldedaten eines Benutzers zu überprüfen. Er nutzt dafür die mächtigen Werkzeuge von ASP.NET Core Identity:

  * **`UserManager<User>`:** Zum Finden des Benutzers in der Datenbank.
  * **`SignInManager<User>`:** Zum sicheren Überprüfen des Passworts, ohne es im Klartext zu sehen.
  * **`ITokenService`:** Unser eigener Service, um nach erfolgreicher Prüfung einen JWT zu erstellen.

Der Service kapselt diese Logik und gibt bei Erfolg ein `LoginResponseDto` zurück, das den Token für das Frontend enthält.

<details>
<summary><b>Code: AuthService.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Homeoffice.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user is null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var token = _tokenService.CreateToken(user);

            return new LoginResponseDto(user.Id, user.UserName!, token);
        }
    }
}
```

</details>

### Der `AccountController`: Die API-Schnittstelle

Der `AccountController` ist die Brücke zwischen dem Frontend und dem `AuthService`. Er stellt die HTTP-Endpunkte zur Verfügung, die das Frontend aufrufen kann.

  * **`POST /api/account/login`**: Ein öffentlicher Endpunkt (`[AllowAnonymous]`), der die Anmeldedaten entgegennimmt, den `AuthService` aufruft und bei Erfolg den Token zurückgibt. Bei einem Fehler wird ein `401 Unauthorized`-Statuscode gesendet.
  * **`GET /api/account/currentuser`**: Ein geschützter Endpunkt (`[Authorize]`), der nur mit einem gültigen Token aufgerufen werden kann. Er demonstriert, wie man die Benutzerinformationen direkt aus dem validierten Token auslesen kann.

<details>
<summary><b>Code: AccountController.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homeoffice.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /api/account/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest);

            if (loginResponse is null)
            {
                // Error: 401 Unauthorized
                return Unauthorized(new { message = "Ungültiger Benutzername oder ungültiges Passwort." });
            }

            return Ok(loginResponse);
        }

        // GET /api/account/currentuser
        [HttpGet("currentuser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(new { id = userId, username = userName });
        }
    }
}
```

</details>


-----

### Der `EmailService`: Automatisierte Benachrichtigungen

Eine Kernanforderung des Projekts ist es, nach Abschluss einer Zeiterfassung automatisch eine E-Mail an das Personalbüro zu senden. Um diese Aufgabe robust und entkoppelt von der restlichen Geschäftslogik zu lösen, wurde ein eigener `EmailService` erstellt.

Für den Versand wird der externe Dienst **SendGrid** verwendet. Dies hat den Vorteil, dass wir uns nicht selbst um die Komplexität von SMTP-Servern kümmern müssen und eine hohe Zustellrate sichergestellt ist.

#### 1\. Konfiguration

Um den Service flexibel zu halten, werden alle wichtigen Einstellungen (wie der API-Schlüssel oder E-Mail-Adressen) nicht fest im Code verankert, sondern über die `appsettings.json`-Datei geladen. Dies geschieht mit dem **Options Pattern** von ASP.NET Core.

**Die Einstellungs-Klasse (`SendGridSettings`)**
Zuerst definieren wir eine Klasse, die die Struktur unserer Einstellungen exakt abbildet.

<details>
<summary><b>Code: Contracts/Configurations/SendGridSettings.cs</b></summary>
<br>

```csharp
namespace Homeoffice.Contracts.Configurations
{
    public class SendGridSettings
    {
        public string HrEmailAddress { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
```

</details>

**Die Werte in `appsettings.json`**
Hier werden die konkreten Werte hinterlegt.

<details>
<summary><b>Code: appsettings.json</b></summary>
<br>

```json
"EmailSenderProvider": {
  "SendGrid": {
    "HrEmailAddress": "myemail@example.com",
    "SenderName": "Homeoffice App",
    "SenderEmail": "s.r.alem@gmail.com",
    "ApiKey": "SEND-GRID-API-KEY",
    "FromName": "Homeoffice App"
  }
}
```

</details>

> **Wichtiger Sicherheitshinweis:** Der oben gezeigte `ApiKey` ist ein "Secret" (Geheimnis). In einem echten Projekt darf ein solcher Schlüssel **niemals** in eine öffentliche Git-Repository eingecheckt werden.

**Registrierung in `Program.cs`**
Diese beiden Zeilen verbinden die `appsettings.json` mit der `SendGridSettings`-Klasse und machen den `EmailService` in der gesamten Anwendung verfügbar.

<details>
<summary><b>Code: Program.cs</b></summary>
<br>

```csharp
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("EmailSenderProvider:SendGrid"));
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
```

</details>

#### 2\. Implementierung

Die Implementierung selbst ist nun sehr sauber, da sie die konfigurierten Einstellungen einfach entgegennimmt und die Logik des E-Mail-Versands ausführt.

**Das Interface (`IEmailService`)**
Das Interface definiert den "Vertrag" – was der Service können muss.

<details>
<summary><b>Code: Contracts/Services/IEmailService.cs</b></summary>
<br>

```csharp
using Homeoffice.Models.Entities;

namespace Homeoffice.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(HomeOfficeEntry homeOfficeEntry);
    }
}
```

</details>

**Die Klasse (`SendGridEmailService`)**
Diese Klasse enthält die konkrete Logik für den Versand mit SendGrid.

<details>
<summary><b>Code: Services/SendGridEmailService.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Homeoffice.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;

        public SendGridEmailService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task SendEmailAsync(HomeOfficeEntry homeOfficeEntry)
        {
            var apiKey = _sendGridSettings.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_sendGridSettings.SenderEmail, _sendGridSettings.FromName);
            var toEmail = new EmailAddress(_sendGridSettings.HrEmailAddress);

            var subject = $"Home Office Zeit für {homeOfficeEntry.User.UserName}";
            
            var body = $"""
            <!DOCTYPE html>
            <html lang="de">
            <head>
                <meta charset="UTF-8">
                <title>{subject}</title>
            </head>
            <body style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6;">
            
                <h3 style="color: #2c3e50;">Abgeschlossener Homeoffice-Eintrag</h3>
                
                <p><strong>Mitarbeiter:</strong> {homeOfficeEntry.User.UserName}</p>
                <p><strong>Start:</strong> {homeOfficeEntry.StartTime:dd.MM.yyyy HH:mm} Uhr</p>
                <p><strong>Ende:</strong> {homeOfficeEntry.EndTime:dd.MM.yyyy HH:mm} Uhr</p>
                <p><strong>Beschreibung:</strong> {homeOfficeEntry.Description ?? "Keine Angabe"}</p>
            
            </body>
            </html>
            """;


            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, "", body);
            
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"Failed to send email: {response.StatusCode} - {responseBody}");

                throw new InvalidOperationException($"Failed to send email: {response.StatusCode} - {response.Body.ReadAsStringAsync().Result}");
            }
        }
    }
}
```

</details>

Absolut. Hier ist die Fortsetzung Ihrer Dokumentation, die sich auf die Kernlogik Ihrer Anwendung konzentriert: den `TimeTrackingService` und den dazugehörigen `TimeTrackingController`.

-----

### Der `TimeTrackingService`: Die Kernlogik der Anwendung

Nachdem die Authentifizierung steht, kümmern wir uns um das Herzstück der Anwendung. Der `TimeTrackingService` ist für die gesamte Geschäftslogik rund um die Zeiterfassung zuständig. Er ist die zentrale Instanz, die direkt mit der Datenbank interagiert und andere Services, wie den `IEmailService`, aufruft.

<details>
<summary><b>Code: Services/TimeTrackingService.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Homeoffice.Services
{
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TimeTrackingService> _logger;

        public TimeTrackingService(
            IEmailService emailService,
            ApplicationDbContext context,
            ILogger<TimeTrackingService> logger)
        {
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public async Task<HomeOfficeEntryDto> StartTrackingAsync(string userId, string? description)
        {
            var isAlreadyTracking = await _context.HomeOfficeEntries
                .AnyAsync(e => e.UserId == userId && e.EndTime == null);

            if (isAlreadyTracking)
            {
                throw new InvalidOperationException("User is already tracking time.");
            }

            var newEntry = new HomeOfficeEntry
            {
                UserId = userId,
                Description = description,
                StartTime = DateTimeOffset.UtcNow,
                IsEmailSent = false
            };

            await _context.HomeOfficeEntries.AddAsync(newEntry);
            await _context.SaveChangesAsync();

            return new HomeOfficeEntryDto(
                newEntry.Id, newEntry.StartTime, newEntry.EndTime, 
                newEntry.Description, newEntry.IsEmailSent, newEntry.UserId
            );
        }

        public async Task<HomeOfficeEntryDto?> StopTrackingAsync(string userId)
        {
            var activeEntry = await _context.HomeOfficeEntries
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.EndTime == null);

            if (activeEntry == null)
            {
                return null;
            }

            activeEntry.EndTime = DateTimeOffset.UtcNow;

            try
            {
                await _emailService.SendEmailAsync(activeEntry);
                activeEntry.IsEmailSent = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification email for entry {EntryId}", activeEntry.Id);
            }
            
            await _context.SaveChangesAsync();

            return new HomeOfficeEntryDto(
                activeEntry.Id, activeEntry.StartTime, activeEntry.EndTime,
                activeEntry.Description, activeEntry.IsEmailSent, activeEntry.UserId
            );
        }

        public async Task<IEnumerable<HomeOfficeEntryDto>> GetOverviewAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var entries = await _context.HomeOfficeEntries
                .Where(e => e.UserId == userId && e.StartTime >= startDate && e.StartTime <= endDate)
                .OrderByDescending(e => e.StartTime)
                .ToListAsync();

            var dtos = entries.Select(e => new HomeOfficeEntryDto(
                e.Id, e.StartTime, e.EndTime, e.Description, e.IsEmailSent, e.UserId
            )).ToList();

            return dtos;
        }
    }
}
```

</details>

#### Aufgaben des `TimeTrackingService`

  * **`StartTrackingAsync`**: Prüft zuerst, ob der Benutzer bereits einen aktiven Timer hat. Wenn nicht, wird ein neuer `HomeOfficeEntry`-Datensatz mit der aktuellen Startzeit erstellt und in der Datenbank gespeichert.
  * **`StopTrackingAsync`**: Sucht den laufenden Eintrag des Benutzers. Wird einer gefunden, wird die `EndTime` gesetzt. Danach wird der `EmailService` aufgerufen, um die Benachrichtigung zu versenden. Dank eines `try-catch`-Blocks stürzt die Anwendung nicht ab, falls der E-Mail-Versand fehlschlägt.
  * **`GetOverviewAsync`**: Holt eine Liste aller Zeiteinträge für einen bestimmten Benutzer und einen definierten Zeitraum aus der Datenbank.

### Der `TimeTrackingController`: Die API-Schnittstelle

Dieser Controller macht die Logik des `TimeTrackingService` über sichere HTTP-Endpunkte für das Frontend verfügbar.

<details>
<summary><b>Code: API/Controllers/TimeTrackingController.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homeoffice.API.Controllers
{
    [Authorize]
    public class TimeTrackingController : BaseApiController
    {
        private readonly ITimeTrackingService _timeTrackingService;
        public TimeTrackingController(ITimeTrackingService timeTrackingService)
        {
            _timeTrackingService = timeTrackingService;
        }

        // POST /api/TimeTracking/start
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] string? description)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            try
            {
                var resultDto = await _timeTrackingService.StartTrackingAsync(userId, description);
                return Ok(resultDto);
            }
            catch (InvalidOperationException ex)
            {
                // This exception is thrown if the user is already tracking time (Error 409)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // POST /api/TimeTracking/stop
        [HttpPost("stop")]
        public async Task<IActionResult> Stop()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var resultDto = await _timeTrackingService.StopTrackingAsync(userId);

            if (resultDto is null)
            {
                return NotFound(new { message = "No active time tracking entry found to stop." });
            }

            return Ok(resultDto);
        }

        // GET /api/TimeTracking/overview?startDate=''&endDate=''
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview([FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new { message = "Start date cannot be after end date." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized();
            }

            var results = await _timeTrackingService.GetOverviewAsync(userId, startDate, endDate);

            return Ok(results);
        }
    }
}
```

</details>

#### Die Endpunkte im Detail

  * **`POST /start`**: Startet eine neue Zeiterfassung. Erwartet eine optionale Beschreibung im Textkörper.
  * **`POST /stop`**: Stoppt die laufende Zeiterfassung für den authentifizierten Benutzer.
  * **`GET /overview`**: Ruft eine Liste der Zeiteinträge ab. Erwartet `startDate` und `endDate` als Query-Parameter in der URL.

-----

### Das `Contracts`-Projekt: Die gemeinsame Sprache

Das `Homeoffice.Contracts`-Projekt ist wie ein **gemeinsames Wörterbuch oder Regelbuch** für die gesamte Anwendung. Es enthält keine ausführbare Logik, sondern definiert die "Verträge", auf die sich die anderen Projekte einigen. Dies sorgt für eine saubere Trennung der Verantwortlichkeiten und verhindert unerwünschte Abhängigkeiten.

Die Struktur ist wie folgt aufgeteilt:

  * **`Configurations`**: Enthält einfache Klassen, die die Struktur der Einstellungen aus der `appsettings.json`-Datei widerspiegeln (z.B. `SendGridSettings`).
  * **`Dtos`**: Definiert die Form der Daten, die zwischen dem Frontend und dem Backend ausgetauscht werden. Mehr dazu unten.
  * **`Services`**: Enthält die **Interfaces** für unsere Services (z.B. `IAuthService`, `ITimeTrackingService`). Diese Interfaces legen fest, *was* ein Service können muss, aber nicht, *wie* er es tut.

### Data Transfer Objects (DTOs) und die Wahl von `record`

Ein DTO (Data Transfer Object) ist ein einfacher Datencontainer. Sein einziger Zweck ist es, Daten zwischen verschiedenen Schichten zu transportieren – insbesondere zwischen unserer API (Backend) und dem Client (Frontend). Wir verwenden DTOs, um unsere internen Datenbank-Modelle (Entities) von dem zu entkoppeln, was wir der Außenwelt zeigen.

Für die Definition dieser DTOs habe ich mich für den modernen C\#-Typ **`record`** anstelle einer traditionellen `class` entschieden.

#### Warum `record` anstelle von `class`?

Die Verwendung von `record` für DTOs ist eine moderne "Best Practice" aus mehreren Gründen:

1.  **Unveränderlichkeit (Immutability):** Records sind standardmäßig unveränderlich. Sobald ein DTO-Objekt erstellt ist, können seine Werte nicht mehr geändert werden. Das ist perfekt für Datencontainer, da es die Datenintegrität sicherstellt und unbeabsichtigte Änderungen verhindert, während die Daten durch das System reisen.
2.  **Prägnante Syntax:** Man benötigt viel weniger Code, um einen `record` zu definieren. Der Compiler generiert automatisch den Konstruktor, die Eigenschaften und Methoden für den Wertevergleich.
3.  **Wertebasierter Vergleich:** Zwei `record`-Instanzen gelten als gleich, wenn alle ihre Eigenschaften die gleichen Werte haben. Bei einer `class` wären sie nur gleich, wenn sie exakt dasselbe Objekt im Speicher wären. Dieses Verhalten ist für reine Datenobjekte viel intuitiver und erleichtert das Testen enorm.

Hier sind die in diesem Projekt verwendeten DTOs:

<details>
<summary><b>Code: LoginRequestDto/LoginResponseDto/HomeOfficeEntryDto.cs</b></summary>
<br>

```csharp
using System.ComponentModel.DataAnnotations;

namespace Homeoffice.Contracts.Dtos
{
    public record LoginRequestDto(
        [Required] string Username,
        [Required] string Password
    );
}
```


```csharp
namespace Homeoffice.Contracts.Dtos
{
    public record LoginResponseDto(
        string UserId,
        string Username,
        string Token
    );
}
```


```csharp
namespace Homeoffice.Contracts.Dtos
{
    public record HomeOfficeEntryDto(
        int Id,
        DateTimeOffset StartTime,
        DateTimeOffset? EndTime,
        string? Description,
        bool IsEmailSent,
        string UserId
    );
}
```

</details>


-----

## Das Backend abschließen: Die `Program.cs`

Die `Program.cs`-Datei ist in modernen .NET-Anwendungen der zentrale Startpunkt. Man kann sie sich wie das Drehbuch oder den Bauplan für die gesamte Anwendung vorstellen. Hier wird in einer klaren Reihenfolge festgelegt:

1.  **Welche Dienste** der Anwendung zur Verfügung stehen (z.B. Datenbankzugriff, Authentifizierung, eigene Services). Dies nennt man **Dependency Injection**.
2.  **Wie jede einzelne HTTP-Anfrage** verarbeitet wird, bevor sie einen Controller erreicht. Dies nennt man **Middleware-Pipeline**.

<details>
<summary><b>Code: Der finale Stand der Program.cs</b></summary>
<br>

```csharp
using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities.Identity;
using Homeoffice.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- 1. Services für Dependency Injection konfigurieren ---
    
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
    builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("EmailSenderProvider:SendGrid"));

    // Database context konfigurieren
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Identity Core
#pragma warning disable IL2026
    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Hier können Sie Passwort-Regeln etc. anpassen
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
#pragma warning restore IL2026

    // JWT-authentication configuration
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateAudience = true
        };
    });

    // Eigene Services
    builder.Services.AddScoped<ITimeTrackingService, TimeTrackingService>();
    builder.Services.AddScoped<IEmailService, SendGridEmailService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    // API-Controller
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        // Verhindert Endlosschleifen bei verknüpften Entitäten
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    // CORS-Policy für das Angular-Frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Oder die Adresse Ihres Frontends
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // --- 2. Anwendung bauen ---
    var app = builder.Build();

    // --- 3. HTTP Request Pipeline (Middleware) konfigurieren ---
    app.UseCors("CorsPolicy");
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // --- 4. Datenbank Seeding beim Start ---
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await SeedDatabaseAsync(services);

    // --- 5. Anwendung starten ---
    await app.RunAsync();
}
catch (Exception exception)
{
    Console.WriteLine($"Stopped program because of exception:{exception}");
    throw;
}

async Task SeedDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    if (!await userManager.Users.AnyAsync())
    {
        // Create User 1
        var user1 = new User
        {
            UserName = "testuser",
            Email = "test@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user1, "Pa$$word123");

        // Create User 2
        var user2 = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user2, "Pa$$word123");

        Console.WriteLine("Default users created successfully.");
    }
}
```

</details>

### Die Konfiguration im Detail

#### 1\. Services konfigurieren (`builder.Services...`)

In diesem ersten großen Block teilen wir der Anwendung mit, welche "Werkzeuge" sie zur Verfügung hat.

  * **`Configure<...>`**: Lädt die Einstellungen aus der `appsettings.json` in unsere Einstellungs-Klassen (`MailSettings`, `SendGridSettings`).
  * **`AddDbContext`**: Richtet den Datenbankzugriff mit Entity Framework Core und dem richtigen Connection String ein.
  * **`AddIdentity`**: Aktiviert das gesamte Benutzer- und Rollen-Management von ASP.NET Core Identity.
  * **`AddAuthentication` & `AddJwtBearer`**: Bringt der API bei, wie sie JWTs (JSON Web Tokens) lesen und validieren muss, um Benutzer zu authentifizieren.
  * **`AddScoped<...>`**: Registriert unsere eigenen, selbst geschriebenen Services (`TimeTrackingService`, `EmailService` etc.), damit sie in anderen Teilen der Anwendung (z.B. den Controllern) verwendet werden können.
  * **`AddControllers` & `AddCors`**: Bereitet die Anwendung auf die Verarbeitung von API-Controller-Anfragen und auf Cross-Origin-Requests vom Frontend vor.

#### 2\. HTTP-Request-Pipeline aufbauen (`app.Use...`)

Dieser zweite Block definiert die "Montagelinie", die jede einzelne HTTP-Anfrage durchläuft. **Die Reihenfolge hier ist extrem wichtig.**

1.  **`UseCors`**: Prüft, ob die Anfrage von einer erlaubten Quelle (unserem Angular-Frontend) kommt.
2.  **`UseRouting`**: Findet heraus, welcher API-Endpunkt für die Anfrage zuständig ist.
3.  **`UseAuthentication`**: Prüft, ob die Anfrage einen gültigen JWT enthält und identifiziert den Benutzer.
4.  **`UseAuthorization`**: Prüft, ob der identifizierte Benutzer die Berechtigung hat, auf den angefragten Endpunkt zuzugreifen.
5.  **`MapControllers`**: Leitet die Anfrage schließlich an die korrekte Methode im richtigen Controller weiter.

#### 3\. Datenbank Seeding beim Start (`SeedDatabaseAsync`)

Dieser letzte Block ist ein nützlicher Helfer. Bei jedem Start der Anwendung wird sichergestellt, dass die Datenbank auf dem neuesten Stand ist (`MigrateAsync`). Anschließend wird geprüft, ob bereits Benutzer vorhanden sind. Wenn nicht, werden automatisch zwei Standard-Benutzer angelegt. Das erleichtert das Testen und die erste Inbetriebnahme der Anwendung enorm.

Mit dieser `Program.cs`-Datei ist das Backend nun ein vollständig konfiguriertes, sicheres und funktionsbereites System. Die Dokumentation des Backends ist damit abgeschlossen.



-----

## Das Frontend: Eine moderne Angular-Anwendung

Das Frontend wurde mit **Angular 20** umgesetzt und folgt modernen Entwicklungspraktiken. Der Fokus lag auf einer sauberen Architektur, guter Performance und einer reaktiven Benutzeroberfläche.

### 1\. Architektur und Ordnerstruktur

Um das Projekt von Anfang an übersichtlich und wartbar zu halten, wurde eine bewährte Ordnerstruktur gewählt, die die Verantwortlichkeiten klar trennt:

  * **`features`**: Hier leben die eigentlichen "Seiten" oder Hauptfunktionen der Anwendung. Jeder Ordner hier drin ist eine eigenständige Funktionalität. In unserem Fall sind das die `auth`-Komponente (für den Login) und die `homeoffice`- sowie `overview`-Komponenten.

  * **`core`**: Dies ist der Maschinenraum der Anwendung. Hier befindet sich die zentrale Logik, die nur einmal für die gesamte Anwendung benötigt wird.

      * `services`: Anwendungsweite Singleton-Services (`AuthService`, `TimeTrackingService`).
      * `guards`: Die "Türsteher" für unsere Routen (`AuthGuard`).
      * `interceptors`: Globale "Poststellen", die jede HTTP-Anfrage abfangen (`JwtInterceptor`).
      * `models`: TypeScript-Interfaces für die Datenstrukturen (DTOs).

  * **`shared`**: Eine Sammlung von wiederverwendbaren UI-Bausteinen. Wenn wir zum Beispiel einen speziellen Button oder einen Lade-Spinner hätten, den wir auf mehreren Seiten verwenden, würde er hier leben.

Diese Trennung sorgt dafür, dass der Code logisch gruppiert und leicht zu finden ist.

### 2\. Guards: Die Türsteher der Anwendung

**Was ist ein Guard?**
Ein "Route Guard" in Angular ist eine Funktion, die wie ein Türsteher vor einer Route steht. Bevor ein Benutzer eine Seite aufrufen darf, fragt der Angular-Router den Guard um Erlaubnis. Der Guard kann dann entscheiden: "Ja, du darfst passieren" oder "Nein, du wirst woanders hingeleitet".

**Warum benötigen wir ihn?**
Wir müssen sicherstellen, dass nur eingeloggte Benutzer auf das Dashboard (`/homeoffice`) oder die Übersicht (`/overview`) zugreifen können. Unser `authGuard` prüft genau das: Ist der Benutzer im `AuthService` als eingeloggt markiert? Wenn nicht, wird er sofort und automatisch zur `/login`-Seite umgeleitet.

<details>
<summary><b>Code: core/guards/auth.guard.ts</b></summary>
<br>

```typescript
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router      = inject(Router);

  if (authService.isLoggedIn()) {
    return true; // Zugriff erlaubt
  }

  // Zugriff verweigert: Zum Login umleiten
  return router.parseUrl('/login');
};
```

</details>

### 3\. Interceptors: Die Poststelle der Anwendung

**Was ist ein Interceptor?**
Ein "HTTP Interceptor" ist wie eine zentrale Poststelle für alle API-Anfragen. Jede Anfrage, die die Anwendung an das Backend sendet, geht durch den Interceptor. Dort kann die Anfrage eingesehen und modifiziert werden, bevor sie tatsächlich über das Netzwerk gesendet wird.

**Warum benötigen wir ihn?**
Nach dem Login erhalten wir einen JWT. Wir müssten diesen Token bei jeder einzelnen geschützten API-Anfrage manuell im Header mitsenden. Das ist mühsam und fehleranfällig. Der `jwtInterceptor` automatisiert diesen Prozess. Er fängt jede Anfrage ab, prüft, ob ein Token vorhanden ist, und fügt ihn automatisch zum `Authorization: Bearer <token>`-Header hinzu. Das ist eine saubere "Don't Repeat Yourself"-Lösung.

<details>
<summary><b>Code: core/interceptors/jwt.interceptor.ts</b></summary>
<br>

```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authservice = inject(AuthService);
  const token = authservice.currentUser()?.token;

  if (token) {
    // Klonen der Anfrage und Hinzufügen des Headers
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  // Die modifizierte Anfrage weitersenden
  return next(req);
};
```

</details>

### 4\. Routing: Der Wegweiser der Anwendung

Die `app.routes.ts`-Datei ist die zentrale Landkarte der Anwendung. Sie definiert, welche Komponente bei welcher URL geladen werden soll.

<details>
<summary><b>Code: app.routes.ts</b></summary>
<br>

```typescript
import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/auth.component').then(c => c.AuthComponent)
  },
  {
    path: 'homeoffice',
    loadComponent: () =>
      import('./features/homeoffice/homeoffice.component').then(c => c.HomeofficeComponent),
    canActivate: [authGuard]
  },
  {
    path: 'overview',
    loadComponent: () =>
      import('./features/overview/overview.component').then(c => c.OverviewComponent),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'homeoffice',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'homeoffice'
  }
];
```

</details>

**Die wichtigsten Merkmale dieser Konfiguration sind:**

  * **Lazy Loading (`loadComponent`):** Dies ist eine entscheidende Performance-Optimierung. Der Code für eine Seite (z.B. die `OverviewComponent`) wird erst dann aus dem Netz geladen, wenn der Benutzer sie wirklich besucht. Das macht die Anwendung beim ersten Laden viel schneller.
  * **Geschützte Routen (`canActivate`):** Die Routen `/homeoffice` und `/overview` sind mit unserem `authGuard` versehen, was den Zugriff für nicht eingeloggte Benutzer verhindert.
  * **Weiterleitungen (`redirectTo`):** Sorgen für ein gutes Benutzererlebnis, indem sie den Benutzer immer auf eine gültige Seite leiten, auch wenn er eine falsche URL eingibt.


Absolut. Hier ist ein Entwurf für die Dokumentation des Token-Refresh-Prozesses, geschrieben in einfachem Deutsch und aus deiner Perspektive, bereit für dein Git-Repository.

---

# Dokumentation: Token-Refresh-Prozess (Frontend)

In diesem Dokument erkläre ich, wie der automatische Refresh-Prozess für Authentifizierungs-Tokens in unserer Frontend-Anwendung funktioniert. Das Ziel ist es, eine sichere und benutzerfreundliche Erfahrung zu gewährleisten.

## 1. Warum ist der Refresh-Token-Prozess wichtig?

Für die Sicherheit unserer Anwendung verwenden wir kurzlebige **Access Tokens**. Diese Tokens sind wie ein temporärer Schlüssel, der dem Benutzer für eine kurze Zeit (z. B. 15 Minuten) Zugriff auf die API gibt.

**Das Problem:** Ohne einen automatischen Prozess müsste sich der Benutzer alle 15 Minuten neu anmelden. Das ist eine sehr schlechte Benutzererfahrung (User Experience).

**Die Lösung:** Wir verwenden einen **Refresh Token**. Dies ist ein langlebiger Schlüssel, der sicher als `HttpOnly`-Cookie im Browser gespeichert wird. Wenn der kurzlebige `Access Token` abläuft, können wir mit dem `Refresh Token` im Hintergrund einen neuen `Access Token` anfordern, ohne dass der Benutzer etwas davon merkt.

Dieser Prozess bietet uns zwei Hauptvorteile:
* **Sicherheit:** Die `Access Tokens`, die bei jeder API-Anfrage gesendet werden, sind nur für kurze Zeit gültig.
* **Benutzerfreundlichkeit:** Der Benutzer bleibt angemeldet und kann die Anwendung ohne Unterbrechungen nutzen.

## 2. Wie funktioniert der Prozess in unserer App?

Der gesamte Prozess ist für den Benutzer unsichtbar. Er bemerkt höchstens eine geringfügig längere Ladezeit bei einer Anfrage. So funktioniert der Ablauf im Detail:

1.  Die Anwendung sendet eine Anfrage an die API (z. B. um Profildaten zu laden) mit dem aktuellen `Access Token`.
2.  Der Server stellt fest, dass der `Access Token` abgelaufen ist und sendet den HTTP-Status `401 Unauthorized` zurück.
3.  Unser `jwtInterceptor` fängt diesen speziellen `401`-Fehler ab, anstatt ihn als Fehler in der App anzuzeigen.
4.  Der Interceptor startet nun den "Token-Refresh-Prozess".
5.  Er ruft die `refreshToken()`-Funktion auf, die eine Anfrage an den `/account/refresh-token`-Endpunkt sendet. Diese Anfrage enthält den `Refresh Token` (über das Cookie).
6.  Der Server überprüft den `Refresh Token`. Wenn er gültig ist, erstellt der Server einen neuen `Access Token` und sendet ihn an die Anwendung zurück.
7.  Die Anwendung speichert den neuen `Access Token`.
8.  Der `jwtInterceptor` wiederholt nun die ursprünglich fehlgeschlagene Anfrage (z. B. das Laden der Profildaten), aber dieses Mal mit dem neuen, gültigen `Access Token`.
9.  Die Anfrage ist erfolgreich und die Daten werden in der Anwendung angezeigt.

## 3. Implementierung im Detail

Drei Hauptkomponenten arbeiten zusammen, um diesen Prozess zu ermöglichen: `jwtInterceptor`, `TokenService` und die `refreshToken()`-Methode im `AuthService`.

### `jwtInterceptor`

Der `jwtInterceptor` ist eine Funktion, die **jede** ausgehende HTTP-Anfrage abfängt, bevor sie an den Server gesendet wird. Er hat zwei Hauptaufgaben:

1.  **Token hinzufügen:** Er fügt den aktuellen `Access Token` zum `Authorization`-Header jeder Anfrage hinzu.
    * **Ausnahmen:** Anfragen an `/account/login` und `/account/refresh-token` werden ignoriert. Das ist wichtig, weil wir beim Login noch keinen Token haben und für den Refresh-Prozess keinen (abgelaufenen) Token senden wollen.
2.  **Fehler behandeln:** Er verwendet `catchError`, um die Antworten vom Server zu überwachen. Wenn ein `401`-Fehler auftritt, startet er die Funktion `handleTokenExpiration`, die den gesamten Refresh-Logik steuert.

<details>
<summary><b>Code: jwt-interceptor.ts</b></summary>
<br>

```typescript
import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { TokenService } from '../services/token.service';
import { catchError, filter, switchMap, take, throwError } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const tokenService = inject(TokenService);

  // Skip token handling for auth endpoints
  if (req.url.includes('/account/login') ||
    req.url.includes('/account/refresh-token')) {
    return next(req);
  }

  const token = authService.currentUser()?.token;
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && authService.isLoggedIn()) {
        return handleTokenExpiration(req, next, authService, tokenService);
      }
      return throwError(() => error);
    })
  );
};

function handleTokenExpiration(
  req: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: AuthService,
  tokenService: TokenService
) {
  if (tokenService.isRefreshing()) {
    return tokenService.getRefreshTokenSubject().pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => {
        req = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
        return next(req);
      })
    );
  }

  tokenService.setRefreshing(true);
  tokenService.setNewToken(null);

  return authService.refreshToken().pipe(
    switchMap(newToken => {
      tokenService.setRefreshing(false);
      tokenService.setNewToken(newToken);

      req = req.clone({
        setHeaders: { Authorization: `Bearer ${newToken}` }
      });
      return next(req);
    }),
    catchError(error => {
      tokenService.setRefreshing(false);
      authService.logout();
      return throwError(() => error);
    })
  );
}

```

</details>

### `TokenService`

Der `TokenService` ist der **Manager** oder "Verkehrspolizist" des Refresh-Prozesses. Sein Hauptzweck ist die Lösung eines Problems, das als **Race Condition** bekannt ist.

**Das Problem (Race Condition):** Was passiert, wenn mehrere API-Anfragen gleichzeitig gesendet werden (z. B. auf einem Dashboard) und der Token genau in diesem Moment abläuft? Alle Anfragen würden gleichzeitig fehlschlagen und jede würde versuchen, einen neuen Token anzufordern. Das ist ineffizient.

**Die Lösung durch `TokenService`:**
Der Service stellt sicher, dass **nur eine einzige** Anfrage den Token erneuert, während die anderen warten. Er verwendet dafür zwei Werkzeuge:

* `refreshingToken`: Ein einfacher `boolean`-Flag. Wenn er `true` ist, bedeutet das: "Ein Refresh-Prozess läuft bereits, bitte warten." Das verhindert mehrfache Anfragen an den `/refresh-token`-Endpunkt.
* `refreshTokenSubject`: Dies ist ein `BehaviorSubject` von RxJS. Man kann es sich als einen **Kanal** oder einen **Warteraum** vorstellen. Anfragen, die fehlschlagen, während bereits ein Refresh läuft, "warten" in diesem Kanal. Sobald der neue Token verfügbar ist, wird er über diesen Kanal an alle wartenden Anfragen gesendet.

Dadurch wird der Prozess sauber und effizient. Die erste fehlgeschlagene Anfrage erledigt die Arbeit, und alle anderen Anfragen warten einfach auf das Ergebnis.

<details>
<summary><b>Code: token.service.ts</b></summary>
<br>

```typescript

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private refreshingToken = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  isRefreshing(): boolean {
    return this.refreshingToken;
  }

  setRefreshing(value: boolean): void {
    this.refreshingToken = value;
  }

  getRefreshTokenSubject(): Observable<string | null> {
    return this.refreshTokenSubject.asObservable();
  }

  setNewToken(token: string | null): void {
    this.refreshTokenSubject.next(token);
  }
}

```

</details>

### `refreshToken()` Methode (in `AuthService`)

Diese Methode hat eine sehr spezifische und einfache Aufgabe: Sie führt die eigentliche HTTP-Anfrage zur Erneuerung des Tokens durch.

* Sie sendet eine `POST`-Anfrage an den Endpunkt `/account/refresh-token`.
* Sie verwendet die Option `withCredentials: true`. Das ist extrem wichtig, denn es weist den Browser an, Cookies (insbesondere unser sicheres `HttpOnly`-Cookie mit dem `Refresh Token`) mit der Anfrage zu senden.
* Bei Erfolg:
    * Extrahiert sie den neuen `token` aus der Antwort des Servers.
    * Aktualisiert sie die Benutzerdaten in der Anwendung mit dem neuen Token.
    * Gibt sie den neuen Token an den Interceptor zurück, damit dieser die ursprüngliche Anfrage wiederholen kann.
* Bei einem Fehler (z. B. wenn auch der `Refresh Token` abgelaufen ist):
    * Loggt sie den Benutzer aus (`authService.logout()`), da die Sitzung endgültig ungültig ist.


<details>
<summary><b>Code: auth.service.ts</b></summary>
<br>

```typescript

   // The refresh token is in an HttpOnly cookie, so we send an empty body.
  // The backend will return the new access token as json object.
  refreshToken(): Observable<string> {
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/refresh-token`,
      {},
      { withCredentials: true }
    ).pipe(
      map(response => response.token), // Extract token from response object
      tap(newToken => {
        const currentUser = this.currentUser();
        if (currentUser) {
          const updatedUser: ILoginResponse = {
            ...currentUser,
            token: newToken
          };
          this.storeUserData(updatedUser);
        }
      }),
      catchError(error => {
        console.error('Refresh token failed:', error);
        this.logout();
        return throwError(() => error);
      })
    );
  }

  private storeUserData(response: ILoginResponse): void {
    this.storageService.set(LOCAL_STORAGE_KEYS.AUTH_TOKEN, response);
    this.currentUser.set(response);
  }

```

</details>

### Diagramm: Sichere JWT-Refresh-Token-Ablaufsteuerung mit paralleler Anfrageverarbeitung

Dieses Diagramm veranschaulicht den sicheren end-to-end Aktualisierungstoken-Mechanismus, der zeigt, wie ein Interceptor mehrere gleichzeitige API-Anfragen mit abgelaufenen Token durch eine zentralisierte Token-Aktualisierung und automatische Wiederholung verwaltet.

![refresh-token](https://github.com/user-attachments/assets/87c7420c-45d2-4437-9cb7-bf3e125e1b61)

