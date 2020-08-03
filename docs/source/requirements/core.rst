===================
Basisfunktionalität
===================

:Status:
    Draft
:Authors:
    Philipp Jeske
:QA:
    * Markus Wosowicki
    * Björn Rausch
    * Lars Irimie


Ziel
====
Im Rahmen dieses Epics wird die grundlegende Basisfunktionalität entwickelt. Der Wachleiter ist also in der Lage einen Dienst zu erfassen, zu beenden, sowie die Anwesenheitszeiten der Wachmannschaft zu dokumentieren.

Abgrenzung
==========
Kein Teil von diesem Epic sind

* Synchronisierung mit Fremdsystemen
* Weiterführende Dokumentation von Tauchgängen, Behandlungen etc.

Stories
=======

Anlegen einer Ortsgruppe
------------------------

    Als technischer Leiter möchte ich eine neue Ortsgruppe anlegen, um dieser Wachgänger zuordnen zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~
#. Als technischer Leiter ist es möglich eine neue Ortsgruppe anzulegen.
#. Als Wachleiter ist es nicht möglich eine neue Ortsgruppe anzulegen.
#. Bei der Anlage einer Ortsgruppe werden folgende Daten erfasst
    * Name
    * Ortsgruppennummer
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.

---------------------------------------------------------------------------------------------

Anlegen eines Wachgängers
-------------------------

    Als Wachleiter möchte ich einen neuen Wachgänger anlegen, wenn dieser noch nicht existiert, um diesen einen Dienst zuordnen zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es möglich einen neuen Wachgänger zu erfassen. 
#. Als technischer Leiter ist es möglich einen neuen Wachgänger zu erfassen.
#. Bei der Anlage werden folgende Daten erfasst:
    * Vorname
    * Nachname
    * Ortsgruppe
    * Qualifikation Rettungsschwimmen
        * Keine
        * Rettungsschwimmer bronze/silber/gold
        * RS im WRD
        * Wasserretter
    * Führungsqualifikation (keine, FIE 1 - 5)
#. Bei der Eingabe wird geprüft, ob folgenden Pflichtfelder eingegeben wurden
    * Vorname
    * Nachname
    * Ortsgruppe
#. Ein Speichern mit einem fehlenden Pflichtfeld ist nicht möglich.
#. Es wird geprüft, ob der Wachgänger bereits angelegt ist. Ist dieser bereits angelegt, so wird dies dem Wachleiter angezeigt. Daraufhin kann dieser entscheiden, ob er den Wachgänger trotzdem erfassen möchte.

-----------------------------------------------------------------------------

Anlegen eines Wachtags
----------------------

    Als Wachleiter möchte ich einen Wachtag anlegen können, um diesen zu dokumentieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es möglich einen neune Wachtag anzulegen.
#. Beim Anlegen eines neuen Wachtags werden folgende Daten erfasst
    * Datum des Wachtages
    * Beginn des Wachdienstes 
    * Uhrzeit der Anmeldung bei zuständigen Stelle (z.B. ILS)
    * Diensthabender Wachleiter
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.
#. Beim Anlegen eines neuen Wachtages sind die Felder wie folgt vorbelegt
    * Datum des Wachtages
        aktuelles Datum
    * Beginn des Wachdienstes
        aktuelle Uhrzeit
    * Uhrzeit der Anmeldung bei der zuständigen Stelle
        aktuelle Uhrzeit

-----------------------------------------------------------------------------

Hinzufügen eines Wachgängers
----------------------------

    Als Wachleiter möchte ich zu einem Wachdienst einen oder mehrere Wachgänger hinzufügen können, um die Anwesenheit zu dokumentieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Entfernen eines Wachgängers
---------------------------

    Als Wachleiter möchte ich von einem aktiven Dienst einen Wachgänger entfernen können, um einen fehlerhaften Eintrag korrigieren zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Dienstende eines Wachgängers eintragen
--------------------------------------

    Als Wachleiter möchte ich bei einem Wachgänger während des aktiven Dienstes ein Ender der Wachzeit eintragen können, um das vorzeitige Verlassen zu protokollieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Dienstende erfassen
-------------------

    Als Wachleiter möchte ich das Dienstende und die Zeit der Abmeldung bei der zuständigen Stelle (z.B. ILS) am Wachtag hinterlegen können, um die Dienstzeit zu protokollieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Wachgänger bearbeiten
---------------------

    Als Wachleiter möchte ich einen bereits angelegten Wachgänger bearbeiten, um die erfassten Daten zu korrigieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Ortsgruppe bearbeiten
---------------------

    Als technischer Leiter möchte ich eine Ortsgruppe bearbeiten können, um die erfassten Daten korrigieren zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------

Duplikate von Wachgängern zusammenfassen
----------------------------------------

    Als technischer Leiter möchte ich einen bereits angelegten Wachgänger mit einem anderen zusammenfassen können, um Duplikate zu entferen.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

*tbd*

-------------------------------------------------------------------------------