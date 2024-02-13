This file contains useful information about the APOLLO project.


https://github.com/HDBW/APOLLO/tree/development/src/cloud/invite-apollo.app

https://github.com/HDBW/APOLLO/blob/development/src/cloud/invite-apollo.app/Apollo.API.Public.Training/Models/Contact.cs


# Auth


AppId: 6a17b692-2ce3-4c94-9d07-3b54a2d2662d

TenantId: 857332e7-9da9-46a2-ba04-5616116258e1

apolloappb2c.onmicrosoft.com


# REST API Postman Collection

https://dark-resonance-7673.postman.co/workspace/Apollo-Workspace~062f247f-5ff6-4e49-91d9-ac4fde051a85/overview


# Where is the data?

## 1000000 user profiles
https://portal.azure.com/#@student.hdbw-hochschule.de/resource/subscriptions/f83b64c1-d950-44e7-9004-80d6c393dafa/resourceGroups/rg-apollo-dac/overview

https://portal.azure.com/#view/Microsoft_Azure_Storage/ContainerMenuBlade/~/overview/storageAccountId/%2Fsubscriptions%2Ff83b64c1-d950-44e7-9004-80d6c393dafa%2FresourceGroups%2Frg-apollo-dac%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fapollotxtanalytics/path/ingressbaprofile/etag/%220x8DB4249DC95079C%22/defaultEncryptionScope/%24account-encryption-key/denyEncryptionScopeOverride~/false/defaultId//publicAccessVal/None

## Trainings Examples

https://portal.azure.com/#view/Microsoft_Azure_Storage/ContainerMenuBlade/~/overview/storageAccountId/%2Fsubscriptions%2Ff83b64c1-d950-44e7-9004-80d6c393dafa%2FresourceGroups%2Frg-apollo-dac%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fapollotxtanalytics/path/ingressbbw/etag/%220x8DAFEDC26850AF9%22/defaultEncryptionScope/%24account-encryption-key/denyEncryptionScopeOverride~/false/defaultId//publicAccessVal/None



  ### Lists

  Supported entities:

  - ContactType
  - CompletionState
  - DriversLicense
  - EducationType
  - LanguageNiveau
  - RecognitionType
  - SchoolGraduation
  - ServiceType
  - StaffResponsibility
  - UniversityDegree
  - TypeOfSchool
  - VoluntaryServiceType
  - Willing
  - YearRange
  - WorkingTimeModel
  - CareerType


POST Query contains following properties

"Language": null (Invariant), DE, EN etc.
"Contains": Anz case insensitive string
"ItemType": Definse the list txpe name.


## Examples

  Search for all elements of education type in invariant language:

     ~~~
      {
        "Language": "Invariant",
        "Contains": null,
        "ItemType": "EducationType"
        }
     ~~~

   Search for all elements of education type in invariant language. Notice that the language can be omitted:

     ~~~
      {
        "Language": null,
        "Contains": null,
        "ItemType": "EducationType"
        }
     ~~~

   Search for all elements of education type in German language:

     ~~~
      {
        "Language": "DE",
        "Contains": null,
        "ItemType": "EducationType"
        }
     ~~~

  
# AI Concept Draft

## Approach
The goal: Match the profile to trainings

Useful properties engaged in the embedding calculation

### Training

~~~JSON
"basics"{
    "shortTeaser": Der optimale Einstieg in das internationale Business-English. Anhand von authentischen Gespr&auml;chssituationen und praxisnahen Kommunikationsbeispielen lernen Sie schnell, auf Englisch in Ihrem vertrauten beruflichen Umfeld zu kommunizieren.
    "subTitle": Grundlegende  Englischsprachkenntnisse f\u00fcr das Gesch\u00e4ftsleben",
}
~~~

~~~JSON
"content": {
    "AdditonalContent3": "<p><strong>Gruppengr\u00f6\u00dfe:<\/strong> <br \/> max. 6 Personen<\/p>",
    "AdditonalContent4": "",
    "Lernziele": "<ul>\r\n<li>Sie vertiefen Ihre vorhandenen Kenntnisse<\/li>\r\n<li>Sie erweitern den businessrelevanten Wortschatz<\/li>\r\n<li>Sie lernen den allt&auml;glichen Umgang f&uuml;r die Gesch&auml;ftswelt<\/li>\r\n<li>Sie erwerben einfache kommunikative Kompetenzen um im Gesch&auml;ftsleben zu Recht zu kommen.<\/li>\r\n<\/ul>",
    "longTeaser": "<p>Der optimale Einstieg in das internationale Business-English. Anhand von authentischen Gespr&auml;chssituationen und praxisnahen Kommunikationsbeispielen lernen Sie schnell, auf Englisch in Ihrem vertrauten beruflichen Umfeld zu kommunizieren. Der Kurs ist f&uuml;r diejenigen geeignet, die Business Basics beherrschen, aber ihre Englischkenntnisse in Gesch&auml;ftssituationen effektiver und professioneller anwenden wollen. Mit dem Abschluss dieser Kursstufe erreichen Sie das Niveau A2\/B1<\/p>",
    "matters": "<p>Grundlegende Englischsprachkenntnisse f&uuml;r das Gesch&auml;ftsleben<\/p>\r\n<ul>\r\n<li>Writing and answering emails<\/li>\r\n<li>Talking with customers &ndash; on the phone or face to face in your company<\/li>\r\n<li>Small Talk and Socializing<\/li>\r\n<li>Visitors,&nbsp; trade fairs<\/li>\r\n<li>Talking about your company and its products<\/li>\r\n<li>Customer Services &ndash; Complaints &ndash; Payments &ndash; Orders<\/li>\r\n<li>Meetings<\/li>\r\n<li>Making arrangements<\/li>\r\n<li>Business travel<\/li>\r\n<\/ul>\r\n<p>&nbsp;<\/p>",
    "targetDescription": "F\u00fchrungskr\u00e4fte, Projektleiter, Fachkr\u00e4fte, Assistenz, Nachwuchsf\u00fchrungskr\u00e4fte, Trainer",
    "MetaDescription": "<p>Im Kurs Business English A2\/B1 erwerben Sie die Englischkenntnisse, die Sie ben&ouml;tigen, um im Gesch&auml;ftsleben sicher und professionell zu kommunizieren.&nbsp;<\/p>",
},
~~~


### Profile

~~~JSON

  "erwartungAnDieStelle" : "Erste Erfahrungen im Bürobereich konnte ich während meines Zivildienstes als Hausmeister mit Verwaltungstätigkeiten sammeln. Zu meinen Aufgabenbereichen zählten: - Schließdienst - Erledigungen kleinerer Bankgeschäfte - Bearbeitung der Post - Reinigungsarbeiten Nach einer Orientierungsphase, besetzte ich die Position einer Verwaltungskraft bei der Schuldnerberatung des Diakonischen Werkes. Dort übernahm ich weitergehende Aufgaben wie: - Telefondienst - Betreuung des EDV-Datenbestandes - Kunden-Betreuung - den Postein-/Ausgang Teamfähigkeit und Flexibilität konnte ich bei dem Transportdienst für einen Second- Hand- Laden der Diakonie unter Beweis stellen",
  "abschluss" : "Mittlere Reife / Mittlerer Bildungsabschluss",

  "kenntnisse" : {
    "Erweiterte Kenntnisse" : [ "E-Mail-Programm Outlook (MS Office)", "Postbearbeitung", "Kunden-, Besucherempfang", "Inventur", "Auskünfte erteilen", "E-Mail-Kommunikation, -Korrespondenz", "Büromaterialverwaltung", "Büro- und Verwaltungsarbeiten", "Ablage, Registratur", "Textverarbeitung Word (MS Office)", "Handwerkliche Kenntnisse" ],
    "Grundkenntnisse" : [ "Tabellenkalkulation Excel (MS Office)", "Auftragsannahme, -bearbeitung", "Telefondienst", "Besucherberatung, -betreuung (Veranstaltungen)", "Terminplanung, -überwachung", "Daten-, Texterfassung", "Garten-, Grünflächenpflege", "Büromaschinen bedienen", "Kurierdienst" ]
  },

   "erfahrung" : {
    "gesamterfahrung" : "P1Y2M26D",
    "berufsfeldErfahrung" : [ {
      "berufsfeld" : "Büro und Sekretariat",
      "erfahrung" : "P11M29D"
    }

    "kenntnisse" : {
    "Erweiterte Kenntnisse" : [ "E-Mail-Programm Outlook (MS Office)", "Postbearbeitung", "Kunden-, Besucherempfang", "Inventur", "Auskünfte erteilen", "E-Mail-Kommunikation, -Korrespondenz", "Büromaterialverwaltung", "Büro- und Verwaltungsarbeiten", "Ablage, Registratur", "Textverarbeitung Word (MS Office)", "Handwerkliche Kenntnisse" ],
    "Grundkenntnisse" : [ "Tabellenkalkulation Excel (MS Office)", "Auftragsannahme, -bearbeitung", "Telefondienst", "Besucherberatung, -betreuung (Veranstaltungen)", "Terminplanung, -überwachung", "Daten-, Texterfassung", "Garten-, Grünflächenpflege", "Büromaschinen bedienen", "Kurierdienst" ]
  },
  ~~~
  

  ### Prompt Draft for qualification
  User's qualification is described by the following pseudo object list in JSON. Your task is to 
generate the plain text from the list of objects to describe the qualification of the user by all listed occupations.
To create the plain text use following template for each occupation found in JSON. 
If the qualification ihas expired one or more years ago, please remark that the user's quilifaction might be old.
If the user's qualification is between 5 and 10 years remark user as a senior. If it is less than 5 years remark user as a junior. Qualifications with longer than 10 years belong to expert users.

Follow example bellow.

START TEMPLATE
The user has achieved following QUAL_NUMBER qulifications, which we have found in the user's profile:

1. The name of the qualification 1
The description of the qualification1.
When is the qualification achieved, till when is the qualification1 valid. How much time is the qualification1 valid.

2. The name of the qualification 2
The description of the qualification 2.
When is the qualification2 achieved, till when is the qualification2 valid. How much time is the qualification2 valid.

...

Last qualification. The name of the last qualification.
The description of the last qualification.
When is the last qualification achieved, till when is the last qualification valid. How much time is the last qualification valid.

END TEMPLATE

Following example demonstrats how to produce the text from example JSON.

START EXAMPLE
PSEUDO OBJECT LIST START
[
{
	Name:"Gabelstaplerschein",
	Description: "Fahren und Bedienen eines Gabelstaplerscheines"
	IssuedDate: "17.10.2021",
	ExpirationDate:"01.01.2024",
},
{
	Name:"IATA New Gen ISS Workshop",
	Description: "IATA-Kunde und START/Amadeus"
	IssuedDate: "17.10.2021",
	ExpirationDate:"01.01.2024",
}
]
PSEUDO OBJECT LIST END

Text created from template based on the given pseudo list example:

The user has following two qualifications: 

1. Gabelstaplerschein
Fahren und Bedienen eines Gabelstaplerscheines.
Achieved at 01.01.2021, valid till 01.01.2024, 3 years Junior user

2. IATA New Gen ISS Workshop
IATA-Kunde und START/Amadeus
Achieved at 17.10.2010, valid till 17.10.2015, The user has 5 qualification as a senior. However, the qualification has expired a long time ago and user must refresh skills.

END EXAMPLE


PSEUDO OBJECT LIST START
[
{
	Name:"Gabelstaplerschein",
	Description: "Fahren und Bedienen eines Gabelstaplerscheines"
	IssuedDate: "17.10.2021",
	ExpirationDate:"01.01.2024",
},
{
	Name:"IATA New Gen ISS Workshop",
	Description: "IATA-Kunde und START/Amadeus"
	IssuedDate: "17.10.2021",
	ExpirationDate:"01.01.2024",
}

]
PSEUDO OBJECT LIST END

The user has following two qualifications: 
