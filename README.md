# zad2 — System wypożyczania sprzętu (C#)

Projekt implementuje prosty system wypożyczalni sprzętu akademickiego (użytkownicy, sprzęt, wypożyczenia, raport).

## Opis projektu
Aplikacja składa się z trzech czytelnych części:
- `Domain/` — model domeny (`User`, `Equipment`, `Rental` oraz ich specjalizacje),
- `Services/` — logika biznesowa i reguły systemowe,
- `ConsoleUI/` + `Program.cs` — warstwa prezentacji (interfejs konsolowy).

## Uzasadnienie decyzji projektowych
Podział na **domenę**, **serwisy** i **UI** został wybrany, aby oddzielić model problemu od sposobu obsługi przypadków użycia i od interfejsu użytkownika. Taki układ ułatwia rozwój projektu (np. zamianę UI bez naruszania logiki biznesowej) oraz testowanie reguł.

## Kohezja, coupling i odpowiedzialności klas — gdzie to widać
- **Kohezja**: klasy domenowe w `Domain/` skupiają się na danych i zachowaniu jednego pojęcia (np. `Rental` odpowiada za cykl wypożyczenia), a reguły biznesowe są skoncentrowane w `Services/`.
- **Niski coupling**: `RentalService` korzysta z `RentalPolicy` i `OperationResult`, dzięki czemu polityki (limity, kary) są odseparowane od orkiestracji operacji i mogą być zmieniane niezależnie.
- **Odpowiedzialności**: `RentalPolicy` definiuje zasady, `RentalService` realizuje przypadki użycia, `RentalReport` przygotowuje podsumowanie, a `ConsoleApplication` odpowiada wyłącznie za interakcję z użytkownikiem.
