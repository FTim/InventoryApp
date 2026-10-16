# InventoryApp
Inventory app test
.NET 8-at használtam, ez long-term supported .NET verzió és jelenleg ez volt a fejlesztői gépemre telepítve, emiatt előfordulhat, hogy a Visual Studio fejlesztői környezet "out of support" figyelmeztetést ír.


iOs fejlesztői fiók és környezet hiányában a megoldásomat fizikai Android telefonomon teszteltem és próbáltam ki.


Kivitelezés során törekedtem az MVVM minta alkalmazására és a kódom logikus, átlátható szervezésére:
- Converters mappába kerültek a view-k által használt konvertelek, például a kért 0 készletmennyiségű termékek piros háttérszínnel való megjelenítéséhez.
- Models mappába került a kapott Product model osztály némi kiegészítéssel.
- Resources/Styles/Color.xaml fájl a projekttel együtt generálódott, de ehhez adtam hozzá a többször használt saját színeket.
- Services mappába került az SQL adatelérési réteg kódja. Amennyiben az alkalmazás fejlesztői módban/debug profile-al indul és nincs az adatbázisban egy termék sem, akkor egy teszteléshez/demozáshoz az adatbázisba 1000 teszt termék kerül. Ezt a DB seed-elést akár egy másik service segítségével is meg lehet oldani a későbbiekben, hogy pl. egy HTTP kérés eredményéből töltődjön fel a kezdeti adatbázis.
- ViewModels mappába kerültek a logika nagy részét tartalmazó viewmodelek. Adatkötéshez az INotifyPropertyChanged megvalósítást egy ViewModelBase osztályba szerveztem ki, melyből a többi viewmodel leszármazik. Ezt tovább lehetne fejleszteni valamilyen MVVM könyvtár használatával (pl: MVVM toolkit, Prism, MVVMCross).
- Views mappába kerültek a UI nézetek.


Dependency injection-höz a MauiProgram.cs-be felvettem a DB service-t és a view-kal, viewmodeleket. Ehhez lehetséges lett volna valamilyen más DI könyvtár használata is.


A view-k konstruktoron keresztül kapják meg a hozzájuk tartozó viewmodel-t (és így kerül a Binding Context-nek megadásra az átadott viewmodel), a viewmodel-ek pedig a DB service-t.


Navigációhoz AppShell-t használtam, ehhez a szükséges route-okat az AppShell.xaml.cs-ben felvettem.
Navigációt lehetett volna máshogy is megoldani, pl: saját navigation service-el vagy valamilyen MVVM keretrendszer segítségével. Jelenleg a view-k code behind-jából indul a navigáció a Shell-en keresztül.


A kötelező funciók közül mindent sikerült implementálni.


Nagyobb mennyiségű termék kezelésére lapozás kezelést implementáltam, 1 oldalon 10 termék jelenik meg, első, utolsó, következő és előző oldalakra lehet váltani, ehhez a lokális adatbázisból mindig csak az aktuális max 10 elemet kérem le. Alternatíva a "végtelen görgetés" lett volna.


Szűrés/keresés funkció egy szöveg beírásával, majd a megfelelő gombbal (névre vagy vonalkódra keresés) működik. Ezt lehetett volna máshogy is megoldani, "írás közbeni" szűréssel.
A jelenlegi implementációt is lehetne tovább átszervezni, hogy még általánosabb és egyszerűbben paraméterezhető legyen.


Nem listázott funkció a törlés, ezt is implementáltam.


Nem listázott funkció új termék létrehozásához és már meglévő szerkesztéséhez a validálás. Ezt máshogy is meg lehetett volna oldani vagy külső könyvtárakat lehetett volna használni (pl: FluentValidation), de ehhez egy kezdetleges egyszerű megoldást implementáltam.


Több rendelkezésre álló idő esetén többet foglalkoztam volna a felhasználói felület finomításán: világos és sötét megjelenítési téma kezelése, többnyelvűség támogatása, animációk, színeken kívül többször használt szüneteket/térközöket is kiszerveztem volna közös erőforrásokba.


Az alkalmazás ikonját és az indításnál megjelenő rövid "splash screen"-t is jobban testreszabtam volna, ha több időm lett volna rá.
