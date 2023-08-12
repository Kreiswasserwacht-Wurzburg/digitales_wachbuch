Digitales Wachbuch
=================

Überblick
---------

*Hier könnte noch ein Bild unseres Produktkartons sein ;-)*

Das digitale Wachbuch ist ein einfach zu bedienendes System, welches dem Wachleiter ermöglicht den Dienst zu dokumentieren.

Die erfassten Dienste können von der Kreiswasserwacht statistisch ausgewertet werden.

Wesentliche Features
--------------------

- Import der Meldungen aus dem HiOrg
- Einfache Erfassung der Wachmannschaft
- Erfassung der bereitgehaltenen Einsatzmittel
- Dokumentation der Dienstzeiten
- Dokumentation besonderer Vorkommnisse
- Meldungen an die Kreiswasserwacht (z.B. Defekte, Mängel, ...)
- Auswertungen der geleisteten Dienste
- Erweiterbar um neue Exporte und Module

Voraussetzungen
---------------

- Laufende MongoDB-Instanz

Start des Wachbuches
--------------------

- Starten des Backends mittels
```bash
    src$ dotnet run
```
- Starten des Frontends mittels
```bash
    digital-guardbook-ui$ npm run dev
```

Ausblick
--------

Das Wachbuch soll mittels docker-compose gestartet werden können, das alle notwendingen Instanzen hochzieht und nur das UI exposed.