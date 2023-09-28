# Programátorská dokumentace

## Úvod
Jedná se o konzolovou aplikaci v jazyce C# .NET6.

## Hlavní komponenty
### Třída Game
Spravuje celou hru = hráče, jejich tahy, zobrazení stavu hry a výsledků.

### Třída Player
Třída Player spravuje vše potřebné pro jednoho hráče v průběhu hry.
Obsahuje instanci třídy GameBoard a metody, které obstarávají výběr dalšího tahu.
Z této třídy dědí třída Agent, která navíc implementuje metody na náhodný výběr dílku a pozice pro další tah.
Z třídy Agent dále dědí všechny třídy implementující agenty.

### Třída GameBoard
Spravuje herní pole. Herní deska je inicializovaná s okraji, které se rovněž počítají při spojování dílků do clusterů se stejnými vlastnostmi.
Obsahuje metody pro kontrolu sousedních polí (používané zejména umělými agenty).

### Třída ScoreCounter
Zde probíhá průběžné vyhodnocování hry. Vyhodnocení se řídí bodováním z instance třídy Scoring.
Pro průběžné vyhodnocování je využitý algoritmus Union Find.

## UnionFind
UnionFind je implementovaný pomocí slovníku, kde klíčem je prvek struktury a hodnotou je id clusteru ve kterém se prvek vyskytuje.
Clusterem chápeme skupinu propojených (sousedících) políček se stejnou vlastností. 

* Add(T x) = přidání prvku (vytvoření nového jednoprvkového clusteru)
* Union(T x, T y) = sloučení dvou clusterů
* Find(T x, T y) = bool, zda se oba prvky nachází ve stejném clusteru
* Count(T x) = metoda vracející velikost clusteru, ve kterém se nachází prvek x

## Ovládání konzolové aplikace


