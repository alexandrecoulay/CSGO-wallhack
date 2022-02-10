# Glow Hack CSGO
CSGlow est un wallhack qui permet de voir les ennemies à travers les murs.
Le hack active la fonction native du jeu qui permet de mettre en lumière les joueurs avec un "glow" qui s'active si on est spectateur d'une partie. J'ai juste changé la couleur de la lueur en rouge pour que ça ce voit mieux à travers les murs.

Cette command fonctionne au niveau client et pas serveur ce qui est important à savoir avant de se lancer dans la création d'un cheat.

### Compatibilté :
Pour maximiser la compatibilité avec les autres cheats et les autres composant externe utilisé dans le script, la version 4.8 de .NET sera utilisé.

### Capture d'écran :
![Image activé](https://github.com/alexandrecoulay/CSGO-wallhack/blob/main/Screen/activated.png)
![Image désactivé](https://github.com/alexandrecoulay/CSGO-wallhack/blob/main/Screen/desactivated.png)

### 1. Risques
⛔ Valve n'autorise pas les cheats en jeu, c'est pourquoi vous l'utiliserez à votre propre risque et je ne suis pas responsable de tout bannissement.⛔
Cependant, il est possible de lancer le jeu avec la commande `-insecure` qui permet de lancer le jeu sans activer l'anti-cheat  ce qui permet de tester n'importe quel script sans problème.
```
Clique droit sur CSGO dans steam -> propriétés -> Général -> Options de lancement
```
Et ouvrir le jeu en plein écran fenetré sinon ça ne fonctionnera pas
### 2. Comment récupérer les offsets ?
Il existe plusieurs solutions pour récupérer les offsets sur CSGO, la 1er et de faire une liste de variable au format `Int32` et de récupérer les valeurs du repo Github le plus utilisé [https://github.com/frk1/hazedumper](https://github.com/frk1/hazedumper) et de copier coller les adresses du fichier `csgo.cs`. Cette technique ne permet pas de publier un cheat maintenable facilement (ayant déjà utilisé cette technique avec un script AHK indétectable j'ai abandonnée après quelques mise à jour) mais très pratique pour un cheat personnel.
Pour y remédier, il existe donc une technique beaucoup plus compliqué qui permet de récupérer les offsets au lancement du jeu.
Voici en bref comment ça fonctionne.

### 3. Récupération automatique des offsets
Récupérer automatiquement des offsets et TRES compliquer. Cela demande de bien connaître le fonctionnement de l'application, et des processus. 
Pour faire simple, il y a 2 modules qui nous intéressent : client.dll et engine.dll. Chaque offsets a son propre pattern qui change à chaque nouvelle mise a jour du jeu (ou pas). 
Par exemple, pour la variable `dwRadarBase` :
Le module dans lequel ce trouve cette variable c'est `client.dll` et on sait que le pattern c'est `A1 ? ? ? ? 8B 0C B0 8B 01 FF 50 ? 46 3B 35 ? ? ? ? 7C EA 8B 0D`. Grâce à ça, le dumper va rechercher en scannant tout les bytes du module `client.dll` une adresse qui contient les codes hexadécimal en claire jusqu'à trouver une adresse existante. Une fois trouvé, il ressort l'offset assigné a ce pattern.
Dans mon cas, la valeur ressortie est  l'adresse `85997812`, ensuite on la met en hexadécimal ce qui nous donne la valeur `52038F4` qui sera utilisé en `0x52038F4` parce que le C# lis uniquement cette syntaxe.
Cependant il existe également des pattern fixes comme pour la variable `dwForceBackward` avec le pattern `55 8B EC 51 53 8A 5D 0` et l'offset `287` le même pattern est utilisé pour la variable `dwForceForward` mais avec l'offset `245` d'où l'importance de bien renseigner toutes les valeurs.
 
Comme c'est long et difficile à faire et que je connais pas assez bien les patterns des offsets utiles (ni le C# ayant que des bases), j'ai préférer copier le code d'un CSGO dumper écrit C# qui lui même à "traduit" le code de HazeDumper écrit en rust. Les fichiers sont `./Memory/SigScanner.cs`, `./Memory/NetVarManager.cs`, `./Memory/Memory.cs` et `./Objects/Classes/CClass.cs`. 
Les 4 scripts permettent de créer le fichier `./Memory/OffsetsManager.cs` qui lui même permet de modifier le fichier `./Misc/Offsets.cs` qui contient toutes les variables statique de type `Int32` utiles partout dans le code.

Une fois les offsets récupérer, la partie intéressante peut commencer.

### 4. Le code

 1. L'arborescence :
	```
	├───Program.cs
	├───Forms
	│   └───EntryForm.cs
	│   └───MainForm.cs
	├───Hacks
	│   ├───WallHack.cs
	├───Memory
	│   └───Memory.cs
	│   └───NetvarManagers.cs
	│   └───OffsetManager.cs
	│   └───SigScanner.cs
	├───Misc
	│   └─── Globals.cs
	│   └───Offsets.cs
	│   └─── Threads.cs
	├───Objects
	│   ├───Classes
	│   |  	└───CClass.cs
 	│   ├───Entity
	│   |  	└───CBaseEntity.cs
	│   |  	└───CBasePlayer.cs
	│   |  	└─── CCSPlayer.cs
	│   └───Structs
	│   |  	└───GlowObjects.cs
	│   └───ClientDLL.cs
	│   └───EngineDLL.cs
	```
 2. Fonctionnement :
	 1. On initialise le script avec `Program.cs` qui ouvre la 1ère boite de dialogue `./Forms/EntryForm.cs` qui permet d'ouvrir le jeu ou d'initialiser le script.
	 2. La console qui s'ouvre simule un chargement qui permet de charger les offsets automatiquement, une fois les offsets chargé, une autre fenêtre s'ouvre `./Forms/MainForm.cs` avec un bouton pour activer/désactiver le wallhack. Il est possible d'ajouter facilement des boutons si d'autres options sont créé.
	 3. Le script `./Misc/Threads.cs` est une sorte de routeur qui charge tout les scripts activable dans le fenêtre principal. Pour l'instant il n'y en a que 1 mais peut être que plus tard d'autre options seront créé (je pense pas ça prend trop de temps)

Les scripts `EngineDLL.cs` et `ClientDLL.cs` servent juste à lire les modules `engine.dll` et `client.dll` qui contiennent des informations différentes et complémentaire.
Ensuite, c'est que de l'utilisation de la mémoire en forçant ou modifiant l'utilisation des offsets.

Le script qui nous intéresse c'est surtout le `./Hacks/WallHack.cs`.

### 5. WallHack.cs
1. On regarde si le bouton est activé
```csharp
if (!Globals.WallHackEnabled)
{
	Thread.Sleep(Globals.IdleWait);
	continue;
}
```
2. On regarde si le joueur est bien dans un partie
```csharp
if (!EngineDLL.InGame)
{
	Thread.Sleep(Globals.IdleWait);
	continue;
}
```
3. On récupère le nombre maximum de joueur dans la partie (change en fonction du serveur et du mode de jeu donc on l'automatise) 
```csharp
int mp = EngineDLL.MaxPlayer;
```
4. On fait une boucle `for` qui pour chaque joueur regarde si
```csharp
# il existe bien
if (entity == null) continue;
# il est pas mort sinon on arrête la
if (entity.Dormant) continue;
# Sa vie est plus grand que 0
if (entity.Health <= 0) continue;
```
5. Si ces conditions sont vraies, on regarde dans quel équipe il est puis on modifie les informations du glow
```csharp
GlowObject glowObject = entityList[i].GlowObject;

glowObject.r = Globals.WallHackEnemy.R / 255;
glowObject.g = Globals.WallHackEnemy.G / 255;
glowObject.b = Globals.WallHackEnemy.B / 255;
glowObject.a = 0.7f;
glowObject.m_bFullBloom = Globals.WallHackFullEnabled;
glowObject.BloomAmount = Globals.FullBloomAmount;
glowObject.m_nGlowStyle = Globals.WallHackGlowOnly ? 1 : 0;
glowObject.m_bRenderWhenOccluded = true;
glowObject.m_bRenderWhenUnoccluded = false;

entityList[i].GlowObject = glowObject;
```


### Sources :
Les sources suivantes sont les plus fiable, je fais entièrement confiance aux scripts et aux tutoriels partagé sur ces forums. Les risques sont évalués, les cheats sont vérifiés et les sources sont également données si il y en a.

[About - Game Hacking Academy](https://gamehacking.academy/about)
https://guidedhacking.com/
		
- https://www.youtube.com/c/GuidedHacking
 -  [Guided Hacking Offset Dumper - GH Offset Dumper | Guided Hacking](https://guidedhacking.com/resources/guided-hacking-offset-dumper-gh-offset-dumper.51/)

https://www.unknowncheats.me/

[Tutoriel Introduction cheat externe et Trouver les offsets localplayer entityplayer. (cs-hackers.com)](https://forum.cs-hackers.com/Thread-Tutoriel-Tutoriel-Introduction-cheat-externe-et-Trouver-les-offsets-localplayer-entityplayer)
