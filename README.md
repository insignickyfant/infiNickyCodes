# Keep Talking and Nicky Codes

Implementatie van een Open Source codeeropdracht van Infi.

De opdracht bestaat uit drie onderdelen, waarin ik gebruik moest maken van een dataset [data/cameras-defb.csv](data/cameras-defb.csv).
De opdracht is geschreven in C#.

## CLI

Dit programma stelt de gebruiker in staat om via de CLI te zoeken op een deel van een camera _name_, of hun identificerende _number_. Het programma negeert case en is te gebruiken op de volgende wijze:

Input:
```sh
# Geeft de camera's uit de dataset die "asw" in de naam hebben
dotnet run --name asw
```
Output:
```none
757 | UTR-CM-757-ASW / Tiendstraat | 5.105752 | 52.097562
758 | UTR-CM-758-ASW / Anjelierstraat | 5.102595 | 52.09915
759 | UTR-CM-759-ASW / Goudsbloemstraat | 5.101403 | 52.09975
760 | UTR-CM-760-ASW / Mimosastraat | 5.099137 | 52.101094
761 | UTR-CM-761-ASW / Egelantierstraat | 5.097114 | 52.102241
```
Input:
```sh
# Geeft de camera uit de dataset die het nummer '801' heeft
dotnet run --num 801
```
Output:
```none
801 | UTR-CM-801-Herculeslaan / Herculesplein | 5.143749 | 52.079461
```


