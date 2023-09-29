# Uživatelská dokumentace

## O hře

Hra Calico je založena na principu postupného přikládání dílků. 

Každý hráč má vlastní herní desku s políčky v mřížce velikosti 5x5. Na desku postupně doplňuje dílky s různými barvami a vzory a získává body podle zadaných pravidel na seskupování barev a vzorů v mřížce. Hráči se při vybírání dílků střídají. Hra končí, když mají všichni hráči vyplněné celé herní pole.

Vítězí hráč, kterému se podaří získat nejvíce bodů.

## Průběh hry
### Hra pro jednoho hráče

Herní mód 1
V každém kole hry hráč vybere 1 ze 3 možných dílků a přidá ho na svou hrací desku.
Možnosti použitelných dílků se poté obnoví.
Toto se opakuje dokud hráč nenaplní celé hrací pole.

### Hra proti agentovi

Herní mód 2
Hra probíhá stejně jako ve verzi s jedním hráčem, pouze po každém tahu hráče provede svůj tah i agent, který má své vlastní hrací pole.
Hráč vidí své hrací pole i pole agenta.
Možnosti dílků k přiložení jsou pro hráče i agenta stejné.

## Pravidla pro získávání bodů

Za každou trojici sousedících stejnobarevných dílků hráč získává 3 body.

Vzory jsou náhodně rozděleny do 3 dvojic s následujícím bodováním:
1. Sousedící trojice se stejným vzorem = 3 body
2. Sousedící čtveřice se stejným vzorem = 5 bodů
3. Sousedící pětice se stejným vzorem = 7 bodů
(v rámci stavu hry se do konzole tiskne i které vzory odpovídají jak velkým clusterům)

## Ovládání konzolové aplikace

Pro spuštění a ovládání hry postupujte podle instrukcí v konzoli.
Všechny instrukce zadávejte ve formě celých čísel, (popř "y"/"n", pokud vás o to instrukce žádá).

Barvy jsou kódovány čísly 1-6, vzory velkými písmeny A-F.
Prázdná políčka jsou označena "--".
Políčka označená "XX" jsou blokovaná a nelze je obsadit.

### Stav hry

V každém kole hry se v konzoli znovu vytiskne stav hry.
1. Dílky k použití
2. Bodování pro jednotlivé vzory
3. Průběžné skóre
4. Hrací deska
5. Instrukce pro provedení dalšího tahu


### Testování

Herní módy 3 a 4

Mód 3 slouží k otestování 1 konkrétního agenta. Uživatel dostane na výběr seznam agentů. Po výběru agenta dostane na výběr, zda chce do konzole tisknout průběh hry.
Pokud se uživatel rozhodne tisknout průběh hry, agent odehraje 1 hru. Pokud ne, agent odehraje 50 her a v konzoli uživatel uvidí pouze finální skóre všech her a průměr ze všech her.

Mód 4 slouží k porovnání agentů. Každý agent odehraje 100 her a do konzole se vytisknou pouze průměrné výsledky.

#### Agenti, které lze testovat
1. Základní náhodný agent, náhodně vybraný dílek přidá na náhodně vybranou (volnou) pozici herní desky
2. Náhodně vybraný dílek přidá na první nalezenou pozici se stejně barevným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
3. Náhodně vybraný dílek přidá na první nalezenou pozici se stejně vzorovaným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
4. Náhodně vybraný dílek přidá na první nalezenou pozici se stejně barevným nebo stejně vzorovaným sousedícím dílkem, pokud takovou pozici nenalezne, vybere ji náhodně
5. Na náhodně vybranou pozici přiřadí dílek, který nejvíce zvýší skóre nebo má na pozici alespoň stejně barevného/vzorovaného souseda, pokud žádný takový dílek v možnostech není, vybere si s možností dílků náhodně
6. Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre z barevných seskupení, popř. tak, aby měl dílek stejně barevného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
7. Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre z vzorovaných seskupení, popř. tak, aby měl dílek stejně vzorovaného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
8. Vybere takový dílek a pozici, aby co nejvíce zvýšil skóre, popř. tak, aby měl dílek stejně barevného/vzorovaného souseda. Pokud taková kombinace neexistuje, vybere dílek a pozici náhodně
9. S pravděpodobností 95% se chová jako AgentComplet, jinak jako základní náhodný agent

