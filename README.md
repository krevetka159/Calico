# Calico

## Motivace
Cílem je vytvořit konzolovou aplikaci pro deskovou hru Calico ve verzi pro jednoho nebo dva hráče. Součástí programu bude i jednoduchá umělá inteligence. Tuto aplikaci budu dále rozvíjet ve své baklářské práci.

## Popis hry
Každý hráč má vlastní herní desku s 30 políčky v mřížce velikosti 5x5. Na desku postupně doplňuje dílky s různými barvami a vzory a získává body podle zadaných pravidel na seskupování barev a vzorů v mřížce. Hráči se při vybírání dílků střídají. Hra končí, když mají všichni hráči vyplněné celé herní pole

## Popis programu
* jednoduchý agent s různými strategiemi
* možnost hrát hru pomocí příkazů v konzoli
* hra pro jednoho hráče nebo proti agentovi
* testovací prostředí pro agenty
* udržování skupin podobných dílků pro výpočet bodů pomocí union-find