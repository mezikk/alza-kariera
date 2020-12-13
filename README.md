# Zprovoznění testů AlzaKariera projektu (selenium c#)
1. Naklonovat repozitář alza-kariera
2. Nastavit do Path složku s drivery (.\AlzaKariera\AlzaKariera\Selenium\Drivers), případně s prohlížeči
3. Otevřít a případně upravit soubor '.\AlzaKariera\run_tests.cmd'
   - proměnná TestIsRemote může nabývat hodnot true/false
   - proměnná TestBrowser může nabývat hodnot chrome/firefox
4. V případě spuštění přes selenium grid spustit hub a node z adresáře '.\AlzaKariera\AlzaKariera\Selenium' pomocí příkazů
   - java -jar selenium-server-standalone-3.141.59.jar -role hub -hubConfig HUB_configuration.json
   - java -jar selenium-server-standalone-3.141.59.jar -role node -nodeConfig NODE_configuration.json
5. Spustit soubor '.\AlzaKariera\run_tests.cmd'

## Poznámky
- konfigurace logování a properties pro testy jsou uložené v adresáři '.\AlzaKariera\AlzaKariera\Config'
- exportovaná dokumentace je uložena v 'bin' složce projektu
- logy z běhu testů jsou uloženy v adresáři '.\logs' v 'bin' složce projektu
- testy jsou vytvořeny dva, jeden pro oddělení IT Development a jeden pro QC (aby bylo vidět, že dokáže pracovat se seznamem), není implementováno přecházení na další stránku 'Další' v případě, že je toto tlačítko aktivní
