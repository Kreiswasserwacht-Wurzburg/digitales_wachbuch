Plugin Manager
--------------

Der Plugin Manager durchsucht beim Programmstart den Ordner
plugins nach Unterordnern und versucht die gefundenen Erweiterungen zu
initialisieren.

.. uml:: ../plantuml/arch_pm_folder_structure.puml

Bei der Initialisierung registriert der Manager vordefinierte Plugin-Funktionen damit diese später ausgeführt werden können.

.. uml:: ../plantuml/arch_pm_function.puml

Alle Plugins müssen eine Klasse bereitstellen die von Plugin erbt.
