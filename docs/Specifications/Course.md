# Trainings and Courses Specification

Authors:
- Patric Boscolo
- Ivana Boscolo

Date:
08.10.2021
14.07.2022
19.07.2022
23.09.2022

[]: # Language: markdown
[]: # Path: docs\Specifications\Course.md

Version:
- 0.0.1 - October 2021
- 0.0.2 - July 2022
- 0.0.3 - July 2022
- 0.0.4 - September 2022

## Abstract
The purpose of this document is to summarize the data structure for courses and trainings managed in the apollo backend as well as the purpose of trainings and courses. 

## Definitions
Courses are structured in segments such as benfits, content, modules, target audience and so on. The course is the main entity and the segments are sub entities. Each of the segmentes have relevant information during the sales funnel of the course. So when a course is viewed by a user he is more interested in the outcome and benefits of the course. When the user is interested in the course he is more interested in the content of the course. When the user is interested in the content of the course he is more interested in the modules of the course. When the user is interested in the modules of the course he is interested if a specific topic or skill is teached in a module.

### Training Providers (TP)
Offering trainings and courses apollo users can enroll in.

### Organizations
An organisation can asess a users skills or competencies and can attest or certify that a individual have learned skills or competencies and met the learnings of a course or training. 

### Training Providers
The apollo consortium consits of training providers represented by [bbw](https://www.bbw.de/startseite/), [biwe](https://www.biwe.de/) and [tüv rheinland academy](https://akademie.tuv.com/). They offer voccational trainings in the context of adult education for job seekers, employees and trainees etc. While the scope of this project mainly focuses on the personas as defined in the [personas](Personas/readme.md) document. This document focuses on the products of the training providers.

### Training or Course
A training or course is a document which is unstructured or structered by the corresponding learning management system of the training providers. The course schema or datapoints may vary from provider to provider or even from subject or training or related qualification. So there is no standard information for a particular training or course. **However invite project apollo defines a course or training as a product of the training providers offering the possibility to aquire skills and competencies needed for the job or next step in the career of a potential customer.** 

Therefor a training or course is a set of files or documents describing these products. 

### Purpose of the Training Data Structure
The purpose is to manage courses and trainings and evaluate their relevance. So basically a training consists of several documents and associated information. Therefor a training data structure is something different then the offered course or training itself. While the training description and target audiences as well as competencies aquired are a different process. The idea behind apollo is to identify needs of the market and the apollo audience and the training providers. Therefor documents describing aspects of the course wheter they are imported via the apollo graph api or via the user interface (backend) they typically have a lifecycle where they needed to be managed and maintained. Also courses need to be updated over time to be relevant for the audience. Therefor a management system which allows to import documents, extract necessary and relevant information and support the training providers in updating topics or modules of a course based on feedback from the participants and the labour market are the purpose of the apollo backend. 

In order to stay relevant information like labour market demands and regulatory changes need to be tracked. In order to give advice to the training providers and give the training providers the possiblity to reach out to the participants and/or interested users of courses and inform them about updates or changes. This is a hugh asset of the invite project apollo. 

## Data Acuisition

Courses can either be submitted to a public api or in the case of the 1st party providers there are connectors to their learning management systems. 

## Course Process Management

The course management happens in two steps. First the course is imported or submitted into the apollo backend. Second the course is evaluated and the information is extracted and stored in the apollo backend. 

![Course Process Management](/docs/Specifications/process_courses.png)

Via the apollo backend a editor can validate the extracted information and update the information if necessary. The editor can also publish the course to the apollo graph api.

![Courses Management Overview](/docs/Specifications/course_management.png)

## Course Data Analysis

### 1. Course Segments

It is the job of the "machine learning" team to identify segments in the submitted course data.

The segements are:
- Course Metadata
- Target Group
- Benefits
- Descriptions
- Content/Modules
- Prequisites
- Booking Information

### 2. Direct Skill Mapping

There are several approaches to the direct skill mapping. The first approach is to use the course description, modules and the other fields such as title to identify the skills and competencies.
This can be achieved via a machine learning approach or a manual approach. The corresponding techniques used are:

- Named Entity Recognition (NER)
- custom Named Entity Recognition (cNER)
- Topic Modelling
- BiTerm Analysis

### 3. Indirect Skill Mapping

During the analysis of the course data there are typically "PersonType", "Event", "Organization", "Skill" and "Place" entities. A "PersonType" can be a Occupation or a Job Title. Therefor the "PersonType" can be mapped to a list of skills associated to a occupation. Another example is the prerequisites of a course. The prerequisites of a course can be a qualifications. A qualification can be mapped to a list of skills. 

### 4. Linked Entities

However in some cases courses contain domain specific terms which are not part of the ontology. In this case the course data is enriched with the corresponding linked entities. These entities can be linked to the ontology and result in a skill mapping.
For example a ISO 9001:2015 course contains the term "Quality Management System". This term is not part of the ontology. However the term is linked to the ontology via the linked entity "Quality Management System" this references to the transversal skill or skill concept ["manage quality"](https://esco.ec.europa.eu/en/classification/skills?uri=http://data.europa.eu/esco/skill/35ebe444-9ece-4fbc-a55d-e99ea37267ae) which resluts in a list of skills associated to a concept. 

### 5. Qualifications

Courses can be linked to a qualification or partial qualification a participant of a course or trainng could become by meeting the criterias. A qualification can be linked to a list of skills.
The european union is currently evaluating the [learning outcomes project](https://europa.eu/europass/en/find-courses) which helps link training and courses to the ontology of esco. Please note that for each european member state there is a [national qualification Framework (NQF)](https://europa.eu/europass/en/national-qualifications-frameworks-nqfs) which is used to link courses to qualifications. In our case the bmbf is using the [deutsche qualifikationsrahmen](https://www.dqr.de/dqr/de/der-dqr/der-dqr_node.html) which is a national qualification framework. In order to search for a qualification in the dqr the [dqr search](https://www.dqr.de/SiteGlobals/Forms/dqr/de/qualifikationssuche/suche_formular) can be used.

### Course Lifecycle
TBD

## API Documentation

### Technical Requirements

![Courses Service Overview](/docs/Specifications/architecture_courses.png)

### Course API - Mobile App

TBD - URL for the course api documentation

### Course API - Backend

TBD - URL for the course api documentation

### Course Endpoints
There are several API Endpoints planned for the apollo backend. 

TBD

## Data
A course consists of several segments. The segments are defined in Course Data Analysis - section 1. Segments. 

### Course Data Structure

![Course Data Structure](/docs/Specifications/course_cd.png)

Class Diagram of a course and associated services.

## Course Data Fields provided by the Training Providers
Describes Properties or Edges as well as Nodes for the given training data structure in invite apollo.

|     Property  |   Data Type   |  Segment  | Description  | 
| ------------- | ------------- | ------------- |------------- |
| CourseId  | String  | Metadata  | Used as unique identifier (ExternalId) |
| Title  | String | Content/Description  | The title of a course  |
| Subtitle  | String? | Content/Description  | The subtitle of a course  |
| ShortDescription  | String | Content/Description  | A short description of the course  |
| Description  | String | Content/Description  | description of the course  |
| Modules  | Dictionary<string(title),string(description)> | Content/Description  | Modules describing furhtermore details about the learning outcomes of a course |
| Benefits  | List<string> | Benefits  | A list or text describing the value propostion of a course |
| TargetGroup  | string | TargetGroup  | A list of persons describing the the target audience or occupations. |
| LoanOptions  | Dictionary<string,Uri>? | Booking Information  | A list of available student loans or billing options for a course. |
| Documents  | FileUpload | Metadata  | A list of documents such as flyers or additional information |
| Duration  | TimeSpan? | Content | The overall timeinvest of a participant needed |
| Tags  | List<string> | MetaData | Used for SEO optimization or tagging of the courses |
| Prerequisites  | List<string> | Prerequisites | A list of prerequisites for a course |
| Price  | double? | Booking Information | The price of a course |
| BookingUrl  | Uri | Booking Information | Can be either the website of the course or a driect link to the shopping cart with or without trackback variables |
| Provider  | string | Metadata | The name of the provider |
| ProviderLogo  | FileUpload | Metadata | The logo of the training provider |
| CourseMedia  | List<Uri> | Content | Media Files used to advertise the training or course |
| IsOnline   | bool | Booking Information | Indicates if a course is available as online training |
| IsInPerson | bool | Booking Information | Indicates if a course is available as in person training |
| IsSeminar | bool | Booking Information | Indicates if a course is available as block-course |
| Appointments | appointments | Booking Information | upcoming appointments of a training or course |

### Appointment Data Structure

| Property | DataType | Description | 
| -------- | -------- | -------- |
| StartDate | DateTime | The start date of an appointment |
| EndDate | DateTime? | The end date of an appointment |
| Dates | Dictionary<Startdate, Timespan> | A list of recurring appointments for a course |
| Location | string? | The location of an appointment |
| Price | double | The price of an appointment |
| IsOnline | bool? | Indicates if an appointment is available as online training |
| IsInPerson | bool? | Indicates if an appointment is available as in person training |
| IsGuaranteed | bool? | Indicates if an appointment is taken place |


