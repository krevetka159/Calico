# Uživatelská dokumentace

## O hře

Hra Calico je založena na principu postupného přikládání dílků. 

Každý hráč má vlastní herní desku s políčky v mřížce velikosti 5x5. Na desku postupně doplňuje dílky s různými barvami a vzory a získává body podle zadaných pravidel na seskupování barev a vzorů v mřížce. Hráči se při vybírání dílků střídají. Hra končí, když mají všichni hráči vyplněné celé herní pole.

Vítězí hráč, kterému se podaří získat nejvíce bodů.

## Průběh hry
### Hra pro jednoho hráče

V každém kole hry hráč vybere 1 ze 3 možných dílků a přidá ho na svou hrací desku.
Možnosti použitelných dílků se poté obnoví.
Toto se opakuje dokud hráč nenaplní celé hrací pole.

### Hra proti agentovi

Hra probíhá stejně jako ve verzi s jedním hráčem, pouze po každém tahu hráče provede svůj tah i agent, který má své vlastní hrací pole.
Hráč vidí své hrací pole i pole agenta.
Možnosti dílků k přiložení jsou pro hráče i agenta stejné.

## Ovládání konzolové aplikace

Pro spuštění a ovládání hry postupujte podle instrukcí v konzoli.
Všechny instrukce zadávejte ve formě celých čísel, (popř "y"/"n", pokud vás o to instrukce žádá).

## Pravidla pro získávání bodů

Za každou trojici sousedících stejnobarevných dílků hráč získává 3 body.

Vzory jsou náhodně rozděleny do 3 dvojic s následujícím bodováním:
1. Sousedící trojice se stejným vzorem = 3 body
2. Sousedící čtveřice se stejným vzorem = 5 bodů
3. Sousedící pětice se stejným vzorem = 7 bodů