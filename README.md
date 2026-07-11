# Tulo.eInvoiceCreatorZUGFeRD

🇬🇧 English version: [README_EN.md](README_EN.md)

> Eine Open-Source, professionelle WPF-Desktopanwendung zum Erstellen, Anzeigen, Archivieren und digitalen Signieren
> vollständig konformer elektronischer Rechnungen im Format **ZUGFeRD 2.4 EXTENDED / Factur-X 1.0** — entwickelt auf Basis von **.NET 8**,
> für den realen geschäftlichen Einsatz konzipiert und ohne Installer lauffähig.

![Bild 1](./ReadMeAsserts/UiImage01.png)  

<img src="./ReadMeAsserts/UiImage02.png" width="300"/> 

[📄 Beispiel-PDF-Rechnung — 6063636771001.pdf](./ReadMeAsserts/6063636771001.pdf)

[📄 Beispiel-XML-Rechnung — 6063636771001.xml](./ReadMeAsserts/6063636771001.xml)

[📄 Beispiel-Pdf/A3-Rechnung — 6063636771001_PdfA3.pdf](./ReadMeAsserts/6063636771001_PdfA3.pdf)

[📄 Beispiel-signierte Pdf/A3-Rechnung — 6063636771001_SignedPdfA3.pdf](./ReadMeAsserts/6063636771001_SignedPdfA3.pdf)

---

## Was diese Anwendung macht

`Tulo.eInvoiceCreatorZUGFeRD` ist ein vollständiges Werkzeug zur Rechnungserstellung, das weit über die einfache Anzeige von XML hinausgeht.

Es ermöglicht Anwendern, alle relevanten Rechnungsdaten einzugeben — Verkäufer, Käufer, Positionen, Zahlungsbedingungen,
Skonti — und erzeugt ein vollständiges, **ZUGFeRD 2.4 EXTENDED / Factur-X 1.0**-konformes Dokumentenpaket:
CII-XML, PDF, PDF/A, PDF/A-3 mit eingebettetem XML sowie optional ein digital signiertes PDF.

Die Anwendung ist auf das Profil **ZUGFeRD 2.4 EXTENDED** ausgerichtet.
Andere Profile werden nicht aktiv getestet und sind nicht das eigentliche Ziel dieses Projekts.

Der Nutzer behält jederzeit die volle Kontrolle über seine Daten.
Verkäuferinformationen, Käuferdaten und alle Rechnungsinhalte werden lokal verwaltet —
nichts wird an einen externen Dienst gesendet.

---

## Wichtiger Haftungsausschluss

Bitte lies die im Programm verfügbare Haftungsausschluss-Information, bevor du die Anwendung
in einem produktiven, rechtlichen oder compliance-relevanten Kontext verwendest.

Du findest sie unter:

**Ansicht → Über**

---

## Voraussetzungen

| Voraussetzung | Detail |
|---|---|
| Betriebssystem | Windows x64 |
| Runtime | .NET 8 (muss separat installiert werden) |
| .NET Download | https://dotnet.microsoft.com/en-us/download/dotnet/8.0 |

---

## Erste Schritte

1. Gehe auf die Seite [Releases](../../releases)
2. Lade die neueste `.zip`-Datei herunter
3. Erstelle eine Ordnerstruktur wie im Abschnitt [Konfiguration — Endanwender](#konfiguration--endanwender) beschrieben
4. Entpacke die ZIP-Datei in den Ordner `Tulo.eInvoiceCreatorZUGFeRD/`
5. Bearbeite deine `appsettings.json` im Ordner `Tulo.eInvoiceCreatorZUGFeRD-appsettings/` mit deinen Verkäuferdaten und Einstellungen
6. Starte `Tulo.eInvoiceCreatorZUGFeRD.exe`

Kein Installer erforderlich.

---

## Validierung

Nach dem Erzeugen einer Rechnung wird die Validierung mit offiziellen Werkzeugen dringend empfohlen:

- **[Kosit Validator](https://github.com/itplr-kosit/validator)**
- ⭐ **[Online ZUGFeRD Validator](https://www.portinvoice.com/en/)**

---

## Unterstützte Rechnungsstandards

| Standard | Details |
|---|---|
| **ZUGFeRD 2.4 EXTENDED** | Hauptziel — vollständig unterstützt und getestet |
| **Factur-X 1.0 EXTENDED** | Französisches/europäisches Äquivalent — vollständig unterstützt und getestet |
| **CII** | Cross Industry Invoice (UN/CEFACT) — wird als zugrunde liegendes Datenformat verwendet |
| **PDF/A-3** | Teil 3, Konformitätsstufe B — Archiv-PDF mit eingebettetem XML |
| **XRechnung SubLine EXTENDED** | 🚧 Demnächst verfügbar |

> **Hinweis:** Andere ZUGFeRD-Profile (MINIMUM, BASIC WL, BASIC, EN16931) werden nicht aktiv
> getestet. Sie könnten funktionieren, sind jedoch nicht garantiert. Der Fokus dieser Anwendung liegt auf
> **ZUGFeRD 2.4 EXTENDED**.

---

## Rechnungsdaten — was du ausfüllen kannst

### Kopfbereich

| Feld | Beschreibung |
|---|---|
| Rechnungsnummer | Eindeutige Dokumentkennung |
| Währung | z. B. EUR |
| Dokumentname | Frei wählbarer Dokumentname |
| Dokumenttyp-Code | 380 Rechnung / 381 Gutschrift / 383 Lastschriftanzeige |

### Käuferpartei

| Feld | Beschreibung |
|---|---|
| Firmenname | Rechtlicher Name des Käufers |
| Steuernummer | Steuerliche Registrierungsnummer |
| USt-IdNr. | Umsatzsteuer-Identifikationsnummer |
| ERP-Kundennummer | Interne Kundenreferenz |
| Leitweg-ID | Deutsche Routing-ID für den öffentlichen Sektor |
| Ansprechpartner | Name des Kontakts beim Käufer |
| Straße / Hausnummer | Adresse |
| Postleitzahl / Ort / Land | Adresse |
| Telefon / E-Mail | Kontaktdaten |

> 💾 Käuferdaten können als **JSON gespeichert und geladen** werden — keine erneute Eingabe bei jeder Rechnung nötig.

### Zahlungsinformationen

| Feld | Beschreibung |
|---|---|
| Zahlungsart-Code | 58 Überweisung / 59 SEPA / 49 Lastschrift / 10 Barzahlung / 48 Karte |
| Zahlungsreferenz | z. B. Rechnung + Kundennummer |
| Zahlungsbedingungen | Freitextbedingungen |
| Fälligkeitsdatum | Datum, bis zu dem die Zahlung erwartet wird |

### Zahlungsbedingungen — Skonto

| Feld | Beschreibung |
|---|---|
| Skonto % | Skonto-Prozentsatz bei früher Zahlung |
| Skontotage | Anzahl der Tage, in denen Skonto gültig ist |
| Skonto-Basisdatum | Startdatum für den Skontozeitraum |

### Rechnungspositionen

Jede Position enthält:

| Feld | Beschreibung |
|---|---|
| Positions-Nr. | Automatisch verwaltete Zeilennummer |
| Beschreibung | Hauptbeschreibung der Position |
| Produktbeschreibung | Zusätzliche Produktdetails |
| Artikel-Nr. / EAN | Artikelnummer des Verkäufers und Barcode |
| Menge / Einheit | Menge und Maßeinheit (UN/ECE) |
| Einzelpreis | Nettopreis pro Einheit |
| MwSt.-Satz / MwSt.-Kategorie | Steuersatz und Kategoriecode |
| Rabatt | Rabattbetrag auf Positionsebene und Grund |
| Bestellreferenz | Bestell-ID und Datum |
| Lieferschein | Lieferschein-ID, Datum und Zeilenreferenz |
| Referenzdokument | Externe Dokumentreferenz (z. B. VN / 130) |

### Verkäuferdaten — über appsettings konfiguriert

Verkäuferinformationen werden nicht manuell in der UI eingegeben.
Sie werden in `appsettings.json` vorkonfiguriert und automatisch auf jede Rechnung angewendet.
Siehe den Abschnitt [Verkäuferkonfiguration](#verkäuferkonfiguration) für Details.

---

## Vorschaumodus

Bevor die Dateien endgültig erzeugt werden, kann direkt aus der UI eine Vorschau angefordert werden.

Im Vorschaumodus:
- Die Rechnung wird vollständig im Speicher als PDF gerendert
- Ein **PREVIEW**-Wasserzeichen wird über das Dokument gelegt
- Das Ergebnis wird innerhalb der Anwendung im integrierten PDF-Viewer angezeigt
- **Es werden keine Dateien auf die Festplatte geschrieben**

Dadurch ist eine vollständige visuelle Prüfung von Layout, Daten und Struktur möglich,
bevor das endgültige PDF/A-3 erstellt wird.

---

## Archiv und Ausgabedateien

Wenn die Rechnungserstellung ausgelöst wird, werden die folgenden Dateien in das konfigurierte Ausgabeverzeichnis geschrieben:

| Datei | Beschreibung |
|---|---|
| `{InvoiceNumber}.pdf` | Roh erzeugtes PDF |
| `{InvoiceNumber}.xml` | CII-XML (ZUGFeRD 2.4 EXTENDED) |
| `{InvoiceNumber}_PdfA.pdf` | PDF/A-Zwischenschritt (Archivformat) |
| `{InvoiceNumber}_PdfA3.pdf` | Finales PDF/A-3 mit eingebettetem XML |
| `{InvoiceNumber}_SignedPdfA3.pdf` | Digital signiertes PDF/A-3 (falls konfiguriert) |

Konfiguriere den Ausgabepfad in `appsettings.json`:

```json
"Archive": {
  "OutputPath": "C:\\Invoices\\Output",
  "CanOpenPdfWithDefaultApp": true
}
