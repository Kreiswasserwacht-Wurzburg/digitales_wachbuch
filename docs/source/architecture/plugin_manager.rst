Plugin Manager
--------------

Der Plugin Manager durchsucht beim Programmstart den Ordner\n
plugins nach unterordnern und versucht die gefundenen Erweiterungen zu \n
initialisieren.

.. uml:: ../plantuml/arch_pm_folder_structure.puml

Bei der initialisierung registriert der Manager vordefinierte Plugin- \n
Funktionen damit diese später ausgeführt werden können.

.. uml:: ../plantuml/arch_pm_function.puml

Alle plugins müssen eine Klasse bereitstellen die von Plugin erbt.
