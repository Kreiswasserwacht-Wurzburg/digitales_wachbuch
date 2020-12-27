===================
Basisfunktionalität
===================

:Status:
    Draft
:Authors:
    * Philipp Jeske
    * Thomas Scheuermann
:QA:
    * Markus Wosowiecki
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

#. Als Wachleiter ist es möglich einen neuen Wachtag anzulegen.
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

#. Als Wachleiter soll möglichst schnell und einfach ein Wachgänger hinzugefügt werden können.
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.
#. Folgende Felder sollen wie folgt vorbelegt werden
    * Beginn des Wachdienstes
        aktuelle Uhrzeit (durch Wachleiter überschreibbar)

-------------------------------------------------------------------------------

Entfernen eines Wachgängers
---------------------------

    Als Wachleiter möchte ich von einem aktiven Dienst einen Wachgänger entfernen können, um einen fehlerhaften Eintrag korrigieren zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter soll ein Funktion geben, welche den Eintrag Wachgänger widerruft, sofern eine falsche Person ausgewählt wurde.
#. Der Widerruf ist zu begründen.
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.
#. In der Liste Wachgänger sollte die Person anschließend nicht mehr angezeigt werden.

-------------------------------------------------------------------------------

Dienstende eines Wachgängers eintragen
--------------------------------------

    Als Wachleiter möchte ich bei einem Wachgänger während des aktiven Dienstes ein Ender der Wachzeit eintragen können, um das vorzeitige Verlassen zu protokollieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter soll möglichst schnell und einfach das Dienstende eines Wachgänger dokumentiert werden können.
#. Folgende Felder sollen wie folgt vorbelegt werden
    * Ende des Wachdienstes
        aktuelle Uhrzeit (durch Wachleiter überschreibbar)


-------------------------------------------------------------------------------

Dienstende erfassen
-------------------

    Als Wachleiter möchte ich das Dienstende und die Zeit der Abmeldung bei der zuständigen Stelle (z.B. ILS) am Wachtag hinterlegen können, um die Dienstzeit zu protokollieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Folgende Felder sollen wie folgt vorbelegt werden
    * Ende des Wachdienstes
        aktuelle Uhrzeit (durch Wachleiter überschreibbar)
#. Bei allen noch aktiven Wachgängern soll als Dienstende das Ende des Wachdienstes hinterlegt werden.

-------------------------------------------------------------------------------

Wachgänger bearbeiten
---------------------

    Als Wachleiter möchte ich einen bereits angelegten Wachgänger bearbeiten, um die erfassten Daten zu korrigieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es möglich einen vorhandenen Wachgänger zu bearbeiten. 
#. Als technischer Leiter ist es möglich einen vorhandenen Wachgänger zu bearbeiten.
#. Beim bearbeiten des Wachgängers können folgende Daten geändert werden:
    * Vorname
    * Nachname
    * Ortsgruppe
    * Qualifikation Rettungsschwimmen
        * Keine
        * Rettungsschwimmer bronze/silber/gold
        * RS im WRD
        * Wasserretter
    * Führungsqualifikation (keine, FIE 1 - 5)
#. Beim Speichern wird geprüft, dass folgenden Pflichtfelder eingegeben wurden
    * Vorname
    * Nachname
    * Ortsgruppe
#. Ein Speichern mit einem fehlenden Pflichtfeld ist nicht möglich.

-------------------------------------------------------------------------------

Ortsgruppe bearbeiten
---------------------

    Als technischer Leiter möchte ich eine Ortsgruppe bearbeiten können, um die erfassten Daten korrigieren zu können.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als technischer Leiter ist es möglich die Daten einer vorhandener Ortsgruppe zu bearbeiten.
#. Als Wachleiter ist es nicht möglich eine vorhandene Ortsgruppe zu bearbeiten.
#. Beim bearbeiten einer Ortsgruppe können folgende Daten geändert werden:
    * Name
    * Ortsgruppennummer
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.

-------------------------------------------------------------------------------

Wachgänger deaktivieren
-----------------------

    Als Wachleiter möchte ich einen bereits angelegten Wachgänger deaktivieren können, sofern dieser als Mitglied ausgeschieden ist oder aus anderen Gründen keine Wachdienste mehr übernimmt.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es möglich einen vorhandenen Wachgänger, welcher keine Dienste mehr übernimmt, zu deaktivieren. 
#. Als technischer Leiter ist es möglich einen vorhandenen Wachgänger, welcher keine Dienste mehr übernimmt, zu deaktivieren. 
#. Der Datensatz wird auf inaktiv gesetzt und nicht mehr in der Personalliste für Wachdienste angezeigt. 
#. Die Daten sind weiterhin vorhanden und werden erst nach einem Fristablauf gelöscht.

-------------------------------------------------------------------------------

Inaktiven Wachgänger reaktivieren
---------------------------------

    Als Wachleiter möchte ich einen bereits angelegten, inaktiven Wachgänger reaktivieren können, sofern dieser wieder Wachdienste übernimmt. Hierdurch wird die Anlage von doppelten Datensätzen vermieden.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es möglich einen vorhandenen, inaktiven Wachgänger, welcher wieder Dienste übernimmt, zu reaktivieren. 
#. Als technischer Leiter ist es möglich einen vorhandenen, inaktiven Wachgänger, welcher wieder Dienste übernimmt, zu reaktivieren. 
#. Der Datensatz wird auf aktiv gesetzt und wieder in der Personalliste für Wachdienste angezeigt. 

-------------------------------------------------------------------------------

Ortsgruppe deaktivieren
-----------------------

    Als technischer Leiter möchte ich eine Ortsgruppe deaktivieren können, sofern diese aufgelöst wurde oder aus anderen Gründen keine Wachdienste mehr übernimmt.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es mir nicht möglich eine vorhandene Ortsgruppe zu deaktivieren. 
#. Als technischer Leiter ist es möglich eine vorhandene Ortgruppe, welcher keine Dienste mehr übernimmt, zu deaktivieren. 
#. Der Datensatz wird auf inaktiv gesetzt und nicht mehr in der Liste für Wachdienste angezeigt.
#. Es soll eine Abfrage erscheinen, ob die Wachgänger der Ortsgruppe auch auf den Status inaktiv gesetzt werden sollen oder weiterin als aktiv in der Liste der Wachgänger verbleiben.
#. Die Daten sind weiterhin vorhanden und werden erst nach einem Fristablauf gelöscht.

-------------------------------------------------------------------------------

Inaktive Ortsgruppe reaktivieren
--------------------------------

    Als technischer Leiter möchte ich eine bereits angelegte, inaktive Ortsgruppe reaktivieren können, sofern diese wieder Wachdienste übernimmt. Hierdurch wird die Anlage von doppelten Datensätzen vermieden.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter ist es mir nicht möglich eine vorhandene, inaktive Ortsgruppe zu reaktivieren. 
#. Als technischer Leiter ist es möglich eine vorhandene, inaktive Ortgruppe, welche wieder Dienste übernimmt, zu reaktivieren. 
#. Der Datensatz wird auf aktiv gesetzt und wieder in der Liste für Wachdienste angezeigt. 
#. Es soll eine Abfrage erscheinen, ob die Wachgänger der Ortsgruppe auch wieder auf den Status Aktiv gesetzt werden sollen oder weiterin als inktiv geführt werden.

-------------------------------------------------------------------------------

Duplikate von Wachgängern zusammenfassen
----------------------------------------

    Als technischer Leiter möchte ich einen bereits angelegten Wachgänger mit einem anderen zusammenfassen können, um Duplikate zu entfernen.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als technischer Leiter ist es möglich einen vorhandenen Wachgänger, welche doppelt angelegt wurde, zusammenzuführen.
#. Beim zusammenführen des Wachgängers kann entschieden werden, welcher Datensatz in Zukunft erhalten bleibt. Aus diesem werden die folgende Daten übernommen:
    * Vorname
    * Nachname
    * Ortsgruppe
#. Beim zusammenführen des Wachgängers wird die höchste Qualifikation aus beiden Datensätzen in den verbleibenden aktiven übernommen.
    * Qualifikation Rettungsschwimmen
        * Keine
        * Rettungsschwimmer bronze/silber/gold
        * RS im WRD
        * Wasserretter
    * Führungsqualifikation (keine, FIE 1 - 5)
#. Beim Speichern wird geprüft, dass folgenden Pflichtfelder eingegeben wurden
    * Vorname
    * Nachname
    * Ortsgruppe
#. Ein Speichern mit einem fehlenden Pflichtfeld ist nicht möglich.


-------------------------------------------------------------------------------

Hinzufügen von Notizen
----------------------

    Als Wachleiter möchte ich besondere Vorkommnisse im Rahmen des Wachdienstes im Wachbuch erfassen können, um diese zu dokumentieren.

Akzeptanzkriterien
~~~~~~~~~~~~~~~~~~

#. Als Wachleiter soll möglichst schnell und einfach eine Notiz erfasst werden können.
#. Bei der Eingabe wird geprüft, ob alle Felder eingegeben wurden.
#. Ein Speichern mit einem fehlenden Feld ist nicht möglich.
#. Folgende Felder sollen wie folgt vorbelegt werden
    * aktuelle Uhrzeit (durch Wachleiter überschreibbar)

-------------------------------------------------------------------------------
