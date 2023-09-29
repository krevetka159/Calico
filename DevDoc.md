# Programátorská dokumentace

## Úvod
Jedná se o konzolovou aplikaci v jazyce C# .NET6.

## Hlavní komponenty
### Třída Game
Spravuje celou hru = hráče, jejich tahy, zobrazení stavu hry a výsledků.
Pro zobrazení stavu hry v konzoli využívá instanci třídy GameStatePrinter.w

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

## Počítání skóre
Třída ScoreCounter počítá skóre s použitím intancí UnionFind -> 1 instance pro barevná seskupení a jedna pro vzorová seskupení.
Skóre se tak počítá jako součet "barevného skóre" a "vzorového skóre".

## Umělí agenti

Implementovaní jednoduší agenti
1. Agent = Základní náhodný agent, náhodně vybraný dílek přidá na náhodně vybranou (volnou) pozici herní desky
2. RandomAgentColor = Náhodně vybraný dílek přidá na první nalezenou pozici se stejně barevným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
3. RandomAgentPatter = Náhodně vybraný dílek přidá na první nalezenou pozici se stejně vzorovaným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
4. RandomAgentComplet = Náhodně vybraný dílek přidá na první nalezenou pozici se stejně barevným nebo stejně vzorovaným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
5. RandomPositionAgent = Na náhodně vybranou pozici přiřadí dílek, který nejvíce zvýší skóre nebo má na pozici alespoň stejně barevného/vzorovaného souseda, pokud žádný takový dílek v možnostech není, vybere si s možností dílků náhodně
6. AgentColor = Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre z barevných seskupení, popř. tak, aby měl dílek stejně barevného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
7. AgentPattern = Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre z vzorovaných seskupení, popř. tak, aby měl dílek stejně vzorovaného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
8. AgentComplet = Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre, popř. tak, aby měl dílek stejně barevného/vzorovaného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
9. AgentCompletWithProb = S pravděpodobností 95% se chová jako AgentComplet, jinak jako základní náhodný agent (Agent)

### Metody na evaluaci zlepšení skóre při přidání dílku

Metody EvaluateNeighborsColor, EvaluateNeighborsPattern a EvaluateNeighbors třídy GameBoard.
Paremetry = GamePiece a pozice na kterou chci dílek umístit (int,int)
Metody podle svých názvů evaluují sousedy z hlediska barvy, vzoru nebo kombinace obojího.
Vracené hodnoty:
1. EvaluateNeighborsColor, EvaluateNeighborsPattern
* 0 = neexistuje soused se stejným parametrem (barva/vzor)
* 1 = existuje soused se stejnou barvou nebo vzorem, ale přidání dílku nezvýší skóre
* n>1 = přidání dílku zvýší barevné/vzorové skóre o n-1

2. EvaluateNeighbors
* 0 = neexistuje soused se stejným parametrem (barva ani vzor)
* 1 = existuje soused se stejnou barvou nebo vzorem (pouze 1 z nich), ale přidání dílku nezvýší skóre
* 2 = existuje soused se stejnou barvou i soused se stejným vzorem, ale přidání dílku nezvýší skóre
* n+1, n>0 = existuje soused se stejnou barvou nebo vzorem (pouze 1 z nich) a přidání dílku zvýší skóre o n
* n+2, n>0 = existuje soused se stejnou barvou i soused se stejným vzorem a přidání dílku zvýší skóre o n

