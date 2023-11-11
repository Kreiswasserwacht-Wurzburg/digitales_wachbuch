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

Voraussetzungen für die Entwicklung
-----------------------------------

- Laufende MongoDB-Instanz

### Start des Wachbuches


- Starten des Backends mittels
```bash
    src$ dotnet run
```
- Starten des Frontends mittels
```bash
    digital-guardbook-ui$ npm run dev
```

Ausführen des Wachbuches
------------------------

Das Starten des Wachbuchs ist mittels docker-compose möglich.

```bash
    $ docker-compose up
```

