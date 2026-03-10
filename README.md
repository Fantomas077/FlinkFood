#  FlinkFood – Blazor Bestellplattform für Lebensmittel

FlinkFood ist eine vollständige Webanwendung, entwickelt mit **Blazor Server (.NET 8)**.  
Die Plattform ermöglicht Kunden das Bestellen von Produkten und bietet gleichzeitig ein sicheres, rollenbasiertes Admin-Panel zur Verwaltung von Bestellungen und Produkten.

---

##  Hauptfunktionen

###  Kundenbereich
- Registrierung & Login
- Produktübersicht
- Warenkorb & Bestellprozess
- Bestellhistorie
- Detailansicht einer Bestellung

###  Adminbereich
- Vollständiges Produktmanagement (CRUD)
- Bild-Upload & -Löschung
- Kategorienverwaltung
- Bestellübersicht mit Workflow
- Statusverwaltung:
  - **Pending → ReadyForPickUp → Completed**
  - Optional: **Cancelled**
- Radzen DataGrid mit:
  - Sortierung
  - Filtern
  - Pagination
  - Aktionen direkt in der Tabelle

###  Sicherheit
- ASP.NET Identity Authentifizierung
- Rollen: **Admin** / **Customer**
- Zugriffsbeschränkungen pro Seite
- Serverseitige Validierung
- Sichere Navigation & Autorisierung

---

##  Verwendete Technologien

| Bereich | Technologien |
|--------|--------------|
| Frontend | Blazor Server, Radzen Components, Bootstrap |
| Backend | .NET 8, C#, Entity Framework Core |
| Datenbank | SQL Server |
| Authentifizierung | ASP.NET Identity |
| Architektur | Services + Repository Pattern |
| UI/UX | Radzen DataGrid, Badges, dynamische Aktionen |
| Testing | NUnit, Moq, Unit‑Tests für Services|



