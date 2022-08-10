# Competencies Specification

Authors:
- Patric Boscolo

| Dates: |
|-------|
08.10.2021
17.01.2022
24.05.2022
13.07.2022
10.08.2022

|Version:|
|--------|
0.0.1 - October 2021
0.0.2 - January 2022
0.0.3 - May 2022
0.0.4 - July 2022
0.0.5 - August 2022

## Abstract
TBD

## Definitions

The idea of competencies has replaced the model of subjects in voccational schools since 1996 in germany.
[source bibb](https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwjJtoDo_brzAhVggf0HHcVdAQsQFnoECAIQAQ&url=https%3A%2F%2Fwww.bibb.de%2Fveroeffentlichungen%2Fde%2Fpublication%2Fdownload%2F6911&usg=AOvVaw2bJPHwApIFzkLPpwtA6qpd) And the federal employment office of germany has created a taxonomy and numerical key based taxonomy [KLDB](https://statistik.arbeitsagentur.de/DE/Navigation/Grundlagen/Klassifikationen/Klassifikation-der-Berufe/KldB2010-Fassung2020/Onlineausgabe-KldB-2010-Fassung2020/Onlineausgabe-KldB-2010-Fassung2020-Nav.html;jsessionid=78BC600D6D99981413FE12787A05E628) for occupations which are supervised by the bibb and the chamber of commerce as well as chamber of craftsman. The EU has developed over the past years a [data science driven](https://esco.ec.europa.eu/uk/node/290) approach to create more transparency about competencies and occupations in the european labor market. The result is the esco taxonomy for the european labor market which is currently available as version 1.1 released on the 28th of january 2022.  

### ESCO Occupations
The occupations pillar is one of the three pillars of ESCO. 

![](/docs/Specifications/Personas/apolloesco1.png)

It organises the occupation concepts in ESCO. It uses hierarchical relationships between them, metadata as well as mappings to the International Standard Classification of Occupations (ISCO) in order to structure the occupations.

Each occupation concept contains one preferred term and any number of non-preferred terms and hidden terms in each of the ESCO languages.

Each occupation also comes with an occupational profile. The profiles contain an explanation of the occupation in the form of description, scope note and definition. Furthermore, they list the knowledge, skills and competences that experts considered relevant terminology for this occupation on a European scale.

In ESCO, each occupation is mapped to exactly one ISCO-08 code. ISCO-08 can therefore be used as a hierarchical structure for the occupations pillar. ISCO-08 provides the top four levels for the occupations pillar. ESCO occupations are located at level 5 and lower.

Occupations are based on Data Science driven approaches. The approach is based on the following principles:

- Continous monitoring of the labour market and the employment situation in the european union.
- Classification of occupations based on Labelling of prefered ESCO Terms including green and traversal skills. 
- Collect job titles from online vacancies and use ML model to detect similarities with ESCO occupations.
- Dataset of job titles from the EURES platform.
- Set new occupations as input and rank EURES job titles based on the similarity score.
- Filter job titles already suggested by experts.
- Filter highest scores and manually validate the results. Two eyes are used to validate the results, one person validates, one person reviews the results. There is a third validater in case of a disagreement.
- Including job titles alternative labels and descriptions for the ESCO occupation. 

See the evolution of ESCO for more details.
![](https://i.postimg.cc/8PTR9vCw/Planning-1.jpg)

### Occupations in ESCO
Occupations are not the same as jobs (which are not covered in ESCO). Their 
distinction is based on the following definitions:

**Occupation**: a ‘set of jobs whose main tasks and duties are characterised by a 
high degree of similarity;

**Job**: a ‘set of tasks and duties carried out, or meant to be carried out, by one 
person for a particular employer, including self-employment’.

>Example: Being the "pilot of Boeing 747 aircraft for the route Paris-New York" is a 
job. "Commercial pilot" or "airline transport pilot" are occupations (i.e. groups of jobs, 
to which this job belongs). Occupations can be used as job titles. An employer 
recruiting for the above-mentioned position might entitle the vacancy notice with the 
name of an occupation, e.g. "airline transport pilot"

The ESCO occupations include:
- relevant occupations at EU level, regardless of the size of the business;
- self-employed occupations;
- volunteer-based occupations;
- subsistence-based occupations;
- arts and crafts occupations;
- political mandates in case they constitute a job (e.g. mayor).

### Occupational profiles
Each occupation concept describes the meaning of the occupation, and provides a 
number of useful pieces of information about it metadata.

The core element that defines an ESCO occupation is the main idea or understanding 
of what the occupation is about and how it differs from other occupations. These are 
captured in the description and scope note.

A **description** in ESCO is a text field providing a short explanation of the meaning 
of the occupation and how it should be understood. For this reason, a description is always provided for each ESCO occupation.

A **scope note** in ESCO is sometimes used to make things less ambiguous and 
clarify its semantic boundaries. For example it may list the specialisms that are 
considered to be within the scope and those that are considered to be out of scope. 

If a formal definition that is either widely accepted or legally binding throughout the 
EU is available, this is captured in the definition field. This particularly includes 
definitions agreed by social partners at a European level or definitions stipulated by 
law.

### Regulated professions

Employers and jobseekers can use ESCO to gain insights into occupations and skills 
that are relevant for the labour market. However, they also need to know if there are 
any legal requirements associated with an occupation. Therefore, the laws that 
regulate access to professions also need to be taken into account.
ESCO ensures that citizens can easily gather information about how occupations are 
regulated in each Member State when they are searching for a job. By providing a 
direct reference to the Regulated Professions Database17, ESCO increases 
transparency regarding the legal requirements of these occupations. 
Additionally, for occupations that are regulated at European level, ESCO provides a 
direct link to the Directive 2005/36/EC on the recognition of professional 
qualifications, as amended by the Directive 2013/55/EC18.

### Knowledge, skills and competences in occupational profiles

Each ESCO occupation is related to essential and optional knowledge, skill and 
competence concepts:

- **Essential** are those knowledge, skills and competences that are usually relevant 
for an occupation, independent of the work context, employer or country.
- **Optional** are those knowledge, skills and competences that may be relevant or 
occur when working in an occupation depending on the employer, working context or country. Optional knowledge, skills and competences are very important for job-matching because they reflect the diversity of jobs within the same occupation.

> Note that, while indicated as an optional skill for the ‘waiter/waitress’ occupation, 
‘prepare flambé dishes’ may be essential for a specific job (e.g. for working in a
French restaurant that serves Crêpes Suzette). 

### The structure of the occupations pillar

Occupations in ESCO are structured through their mapping to the International 
Standard Classification of Occupations (ISCO-08), which has been developed by the 
International Labour Organisation (ILO). The ESCO occupations and their ISCO-08 
hierarchy make up the ESCO occupations pillar.
ISCO-08 provides the top four levels while ESCO occupations provide the fifth and 
lower levels. Each ESCO occupation is assigned to one ISCO-08 unit group (even if 
they are not directly related to it, e.g. if they are at level six or seven).
ESCO provides the translation of ISCO in 24 languages (Icelandic and Irish are not 
included).

![](/docs/Specifications/Personas/escofigure3.png)

### Occupations primary hierarchy

Occupations in ESCO are described at different levels, depending on the language 
used and requirements of the labour market. However, these may differ between 
groups of countries (e.g. some Member States may need to cover different types of 
sommelier, while for others this occupation may have little relevance). In order for 
ESCO to accommodate both cases, some occupations have broader-narrower relations 
between them.
However, only the occupations that are relevant for the European labour market are 
included. Therefore, the more detailed occupations do not necessarily cover the entire 
scope of the more generic occupation. (e.g. not all types of sommelier are covered 
under sommelier).

![](/docs/Specifications/Personas/escofigure4.png)

References:
- [ESCO](https://esco.ec.europa.eu/en/classification/occupation_main)
- [ISCO-08](https://www.ilo.org/public/english/bureau/stat/isco/isco08/)
- [EURES](https://ec.europa.eu/eures/)
- [YT: The use of artificial intelligence in updating the ESCO classification](https://www.youtube.com/watch?v=YPECXVRagu8&t=8s)
- [Regulated professions 2005/36/EC](https://ec.europa.eu/growth/tools-databases/regprof/)

### ESCO Concepts
There are three type of terms in ESCO. 

- Preferred terms: Preferred terms are terms that are used in the industry.
- Synonyms: Synonyms are terms that are used in the industry but are not preferred.
- Hidden terms: Are terms that are used for classification.

![](/docs/Specifications/Personas/apolloterms..png)

English is the default language for ESCO. So each Concept is a thing, idea or shared understanding of something. Concepts are not language dependent. 

Example for a CONCEPT Occupation baker:

> The idea or shared understanding of a person baking bread, pastries, etc. and selling it to customers is a concept. Terms that are frequently used to refer to the concept are e.g. "Baker" in English language or "Bäcker/in" in German language.

Example for a Description of the occupation baker:

>Bakers make a wide range of breads, pastries, and other baked goods. They follow all the processes from receipt and storage of raw materials, preparation of raw materials for bread-making, measurement and mixing of ingredients into dough and proof. They tend ovens to bake products to an adequate temperature and time.

### Formulation of terms (labels) in ESCO languages

The Commission has made ESCO available in 27 languages, including all EU official 
languages as well as Icelandic, Norwegian and Arabic, in order to facilitate increased 
international transparency and cooperation in the area of skills and qualifications. 
ESCO is therefore bridging language barriers by providing terms for each concept in all 
languages covered by the classification. ESCO categorises and uses different types of 
terms such as: preferred terms, non-preferred terms and hidden terms.

ESCO collects terms used in the labour market to refer to occupations and skills in the 
different ESCO languages.
ESCO terms were first formulated in English, which is the ESCO reference language. 
The process of term formulation started with attempts to achieve the best wording for **ESCO concepts** – separately for the occupations and skills pillars. The aim was to find formulations that would best convey the original idea in the clearest, most concise and self-explanatory manner possible. The process of the formulation of terms involved 
using vocabulary commonly present in job vacancies, curricula, and national and international occupational classifications from across Europe.
Once the terms were formulated in English, terminologists and market experts ensured that the formulated terms properly reflected the meaning of the concept as captured by the description and the scope note (if available).

The English term validation process consisted of two steps:

- a linguistic (vocabulary and grammar) check by terminologists;
- a labour market reality check by market experts.

A similar process for term formulation and validation took place for all ESCO languages other than English. As opposed to the regular translation practice, 
translators did not simply translate the terms from English into the target languages of ESCO, but selected the most common expressions from among the existing terms in the given language and the given labour market. This process followed a similarly 
structured approach as term formulation in English, which consisted of:

- formulation of terms in the target languages followed by terminological checks;
- labour market checks to ensure the concepts reflect the reality at European level.

The whole process required compliance with terminological rules that took into consideration the grammatical and linguistic characteristics of each language. A number of national sources were used to facilitate this task, such as national classifications, websites of public employment services, etc.

Moreover, translators consulted designated national experts in order to ensure that the terms reflected the characteristics of the national employment structure. The Commission consulted Member States through the Member States Working Group on ESCO. The feedback received was then integrated by the translators and served to improve and adjust the terms, based on the expertise of the Member States.

### Skills
The ESCO skills pillar distinguishes between i) skill/competence concepts and ii) knowledge concepts by indicating the skill type. There is however no distinction between skills and competences. Each of these concepts comes with one preferred term and a number of non-preferred terms in each of the 28 ESCO languages. Every concept also includes an explanation in the form of description.

The skills pillar of ESCO contains 13,890 concepts structured in a hierarchy which contains four sub-classifications. Each sub-classification targets different types of knowledge and skill/competence concepts:

- **Knowledge:** The body of facts, principles, theories and practices that is related to 
a field of work or study. Knowledge is described as theoretical and/or factual, and 
is the outcome of the assimilation of information through learning.

- **Skills:** The ability to apply knowledge and use know-how to complete tasks and solve problems. Skills are described as cognitive (involving the use of logical, intuitive and creative thinking) or practical (involving manual dexterity and the use 
of methods, materials, tools and instruments).
- **Competence:** The proven ability to use knowledge, skills and personal, social 
and/or methodological abilities, in work or study situations, and in professional and 
personal development.
- **Transversal skills:**
- **Language skills and knowledge:**

In addition to the hierarchy, subsets of skills are included in the download packages:

- A transversal skill hierarchy
- A collection of languages
- A collection of green skills (only for v1.1)

The ESCO skill hierarchy is in a continuous process of improvement. 

![](/docs/Specifications/Personas/apolloskills.png)

### Content of the skills pillar
As for the occupations, ESCO provides metadata for each concept in the skills pillar including the following:

- A preferred term that is used to present the concept.
- Non-preferred terms (synonyms, spelling variants, declensions, abbreviations, etc.).
- Hidden terms (e.g. outdate, misspelled or politically incorrect terms).
- A description that explains more in depth what the skill is about in line with the action verb and level of detail used in the title.
- A scope note that clarifies the semantic boundaries of the concept.
- The skill type: i) skill/competence concepts or ii) knowledge concepts.
- The relationship with ESCO occupations. This shows for which occupations the knowledge, skill or competence is typically relevant including those for which it is essential and those for which it is optional. In some cases, a relationship will show 
how the knowledge, skill or competence is relevant for other knowledge, skills and competences. The relationship also includes the distinction between essential and optional. 
- The reusability level, which indicates how widely a knowledge, skill or competence concept can be applied. This is crucial for supporting occupational mobility. ESCO distinguishes four levels of skill reusability:
- Transversal knowledge, skills and competences are relevant to a broad range of occupations and sectors;
- Cross-sector knowledge, skills and competences are relevant to occupations across several economic sectors;
- Sector-specific knowledge, skills and competences are specific to one sector, but are relevant for more than one occupation within that sector;
- Occupation-specific knowledge, skills and competences are usually applied only within one occupation or specialism

### Transversal Skills
Transversal knowledge, skills and competences
As mentioned above, transversal knowledge, skills and competences are relevant to a 
broad range of occupations and economic sectors. They are often referred to as core, 
basic or soft skills and are the cornerstone for the personal development of a person. 
Within the skills pillar, transversal skills and competences are organised in a 
hierarchical structure with the following five headings:
- thinking 
- language
- application of knowledge
- social interaction
- attitudes and values

Both the concepts and hierarchical structure of the transversal knowledge, skills and competences were developed by the Cross-sector Reference Group20. The 
development was based on the analysis of a wide range of existing national and sectoral classifications, the European Dictionary of Skills and Competences [(DISCO)](http://disco-tools.eu/disco2_portal/) and other sources.

### Skill contextualisation
Skill contextualisation is a method to create knowledge or skill and competence concepts by analysing how transversal skills, competences or knowledge are applied in the specific context of a sector or an occupation. This allows transversal knowledge, skills and competences that are rather abstract to be brought to a more detailed level 
so that they can be directly used in occupational profiles.

>Example: The skill “measure” is too abstract to be linked directly to the occupation 
“metal furnace operator”. This relationship would produce too many results if used in 
competence-based job matching since measuring is relevant for a large number of 
occupations and sectors.
Through skill contextualisation, the skill can be made more specific. A skill named 
“measure furnace temperature” could, for example, be used in the occupational profile 
of the “metal furnace operator”.

### Structure of the skills pillar
The ESCO v1 skills pillar does not contain a full, top-down hierarchical structure. 
Instead, the 13.485 elements of the pillar are structured in four different manners:

- through their relationship with occupations, by using occupational profiles as an 
entry point;
- through a hierarchy (only for transversal knowledge, skills and competences);
- through relationships indicating how knowledge, skills and competences are 
relevant to other knowledge, skills and competences (in particular in cases of the 
contextualisation of skills);
- through functional collections that allow subsections of the skills pillar to be 
selected, according to the purpose it is going to be used for. For example, an 
organisation may want to use ESCO to implement an online CV editor where a user 
can indicate his/her language skills. The organisation would not need all the ESCO 
skills in that CV section, only the language skills. If a user searches for "Chinese" 
in this section, the system should suggest "Chinese", "understand spoken 
Chinese", “understand written Chinese” or "interact verbally in Chinese", but not 
"traditional Chinese medicine" or "give shiatsu massages". A functional collection 
would allow the user to pick exactly the skills (or occupations) s/he is looking for.
ESCO v1 includes three functional collections: Digital transversal skills (identical to 
the Digital Competence Framework)22; Language skills; Transversal skills.

### The qualification pillar
The qualifications pillar aims to collect existing information on [qualifications](https://data.consilium.europa.eu/doc/document/ST-9620-2017-INIT/en/pdf).

Qualifications in ESCO come from national qualifications databases of Member States. 
These qualifications are included in National Qualifications Frameworks that have been 
referenced to the EQF. Since 2014, the Commission has been financially supporting 
Member States and other partner countries (EFTA, EEA and candidate countries) to 
develop national qualifications databases and to interconnect these with the Learning 
Opportunities and Qualifications in Europe portal [(LOQ)](https://ec.europa.eu/ploteus) and with ESCO.
Other qualifications might be directly provided to ESCO by awarding bodies in the 
future. These are not part of national qualification frameworks but are also relevant 
for the European labour market. They include private, sectoral and international qualifications. The Commission is currently piloting this approach and discussing the results and following steps with Member States. In contrast with the occupations and skills pillars, the qualifications pillar is therefore populated exclusively by external sources, not by data created by the Commission.

- Subsidiarity: The competences of Member States, their different education and 
training system traditions, and where applicable, the autonomy of the awarding 
bodies are fully respected.
- Learning outcomes approach: ESCO follows the learning outcomes approach, 
which expresses what someone knows, understands and is able to do on 
completion of a learning process
- Bridging the communication gap: Information on qualifications can be interlinked with the skills pillar, supporting closer cooperation between 
employment and education/training.
- Transparency: Information on qualifications needs to be fully transparent. This includes information that is required by market actors to assess the quality and trustworthiness of a qualification.
- Up-to-date: Data on qualifications needs to be up-to-date, reflecting the actual qualifications landscape in Europe.
- Non-discriminatory: Transparent information, including on quality assurance, is provided, but no judgement on the quality of qualifications is made by the Commission.
- Complementarity between ESCO and the EQF: The qualifications pillar of ESCO is developed in full compliance and complementarity with the EQF.

The qualifications pillar supports the understanding of the individual qualifications needed by employers, public and private employment services, learners, workers, jobseekers, education and training providers, and other actors. This information 
should be as complete and transparent as possible to meet their needs. Information on qualifications in ESCO follows the elements for data fields for the electronic publication of information on qualifications included in Annex VI of the EQF 
Recommendation.

Therefore, only qualifications data that includes the following core information will be 
displayed in ESCO:
![](/docs/Specifications/Personas/eqf.png)

Other fields:
- credit points/ notional workload needed to achieve the learning outcomes
- internal quality assurance processes
- external quality assurance/regulatory body
- further information on the qualification
- source of information
- link to relevant supplements
- URL of the qualification
- information language (code)
- entry requirements
- expiry date (if relevant)
- ways to acquire qualification
- relationship to occupations or occupational fields

 Information on other fields is optional and should be included if available, as this complementary information increases the transparency of the qualifications for users.

In particular, information on quality assurance adds an important element of transparency that will enhance trust in the published qualifications. 
Qualifications will only be displayed in ESCO if they comply with these core data requirements.

References Qualifications

- [As defined by EQF Recommendation, a qualification is the formal outcome of an assessment and validation process which is obtained when a competent body determines that an individual has achieved learning outcomes to given standards (2016)](https://data.consilium.europa.eu/doc/document/ST-9620-2017-INIT/en/pdf)

- “Body issuing qualifications (certificates, diplomas or titles) and formally recognising the learning 
outcomes (knowledge, skills and/or competences) of an individual, following an assessment and validation 
procedure” (Cedefop, 2008)

- Based on Annex VI of the EQF Recommendation (2016) 
- http://www.uis.unesco.org/Education/Documents/isced-fields-of-education-training-2013.pdf
- http://ec.europa.eu/eurostat/web/nuts/overview

### Qualification Links with the occupations
![](/docs/Specifications/Personas/escoqualifications.png)
Links with the occupations pillar
Direct relationships between qualifications in ESCO and the occupations pillar are only 
displayed if they already exist at national level. It is up to Member States to decide if 
they develop such data. The relationship can indicate, for instance, if a qualification is 
a requirement in order to work in an occupation in the specific Member State.
Otherwise, the relationship between occupations and qualifications is indirect, via the 
skills pillar, as shown in Figure above.

### Qualification Links with the skills pillar
Organisations that provide data on qualifications can annotate learning outcomes
descriptions with ESCO skills terminology: they can add knowledge, skills and 
competence concepts in the skills pillar that correspond to the learning outcomes 
description of the qualification.
This additional information will help people better understand the content of the 
qualification. For example, educational experts might prefer to look at the learning 
outcomes description created by the awarding body to understand the content, but an 
employer in another Member State might find it easier to look at the ESCO skills 
(which are available in 26 European languages and Arabic). Interlinking learning 
outcomes descriptions of qualifications with the skills and competences concepts will 
enable better understanding of the scope of qualifications and their relevance to the 
labour market.
In the example below, the skill “maintain a vessel’s weather and watertight integrity” 
corresponds to the ESCO skill “ensure watertight integrity”.

### ESCO Implementation Details

ESCO is a European Union initiative and is part of the open linked data strategy of the european commision.

The ESCO data structure looks as follows:
![](/docs/Specifications/Personas/escomodel.png)

- https://ec.europa.eu/isa2/sites/isa/files/eif_brochure_final.pdf
- https://eur-lex.europa.eu/legal-content/en/TXT/?uri=CELEX:32013L0037
- https://eur-lex.europa.eu/legal-content/en/TXT/?uri=CELEX:32013L0037

See [ESCO Service Platform Application Programming Interface](/docs/Specifications/Personas/ESCO%20-%20API%20v03.94.pdf) for more details.

### ESCO Technical Implications and Consequences

ESCO is a complex [RDF](https://www.w3.org/TR/rdf-concepts/) [Triple](https://www.w3.org/TR/turtle/) Graph database that is ~1.85GB large.

ESCO is available as a [RESTful](https://en.wikipedia.org/wiki/Representational_state_transfer) [API](https://en.wikipedia.org/wiki/Representational_state_transfer) that can be used by other applications. It can be hosted as a [Docker](https://www.docker.com/) container.

ESCO supports the following user-cases:
- Transparency in the european labour market
- Improved transparency in the European education and training market for vocational and non-vocational trainings
- Connects People and Occupations
- Improves job search and employment opportunities as well as consultancy for jobseekers in europe

>Note: that we are currently planning to use a Labeled Property Graph (LPG) database to support the most apollo use-cases. However, ESCO is a RDF Triple. While we use import to neo4j to do experiments, we don´t have a stable import to LPG yet.

We are evaluating the following use-cases:
- ESCO as a docker container as is for the usage in apollo graph.
- ESCO import/export to cosmosdb as is for the usage in apollo graph.

A big concern is the performance and calls to the ESCO API. We are currently evaluating if the ESCO API is a good fit for the apollo graph given the mobile app background we have. 

## Apollo Competencies and Skills

Scope of Apollo competencies and skills is defined by the overall idea that companies have a demand for skilled employees. The job description or jobrole is a description of the skills and competencies needed to perform the job. These internals documents are used to define the demand for a skilled employee to be successful in the workplace. Human Resources (HR) is the body responsible for the recruitment and selection of employees. The HR body is responsible for the selection of the best employees for the company. And the presentation of a vaccancy is typically done via online job vacancies (OJVs). These job postings are used to find the best employees for the company, and are not necessarily the same as the job description. Often times the OJVs are also used for pure marketing purposes. 
A skilled employee is an employee who has the skills and competencies which are typically qualified by occupations needed to perform a job. 
These skills and competencies are aquired by the employee through training, experience, and education. Training providers are responsible for the training and upskilling of people. They can but not necessarily have to qualify or assess the people. So basically a person can learn skills and competencies by training. Take exams or certifications which verify the skills and competencies acquired. 
A now skilled and qualified perosn can apply on job posting or vacancy. And the company can hire the person if it fits the needs and enriches the corporate performance and culture.
This lifecycle of a person is called a career development. There are several obstacles and challenges that can be faced in the process of a career development. The apollo project is aimed at addressing these challenges, and helping people plan their life long learning journey. This is why we commited to ESCO in the apollo project.

When looking at the lifecycle the obvious question is: **"What are the skills and competencies a employer is looking for?"** The challenge here is identifying the skills and competencies that are required and the company is looking for. Also there are trends in the industry where the skills and competencies changes over time. So this scenario requires the detection of the skills and competencies that are required by the company since we don´t have access to the employers job or role description. The extraction on the vague formulated skills and competencies as well as candidate cababilities is one of the topics of the information extraction and aquirement process.

However in the future we can think of a software system based on apollo that would help the hr department to identify the skills and competencies that are required by a candidate which is expected to be successfull. The system would help to write and formualte not only the job description but also the job postings. More importantly the system would also be able to generate career development plans for the future employees and identifiy opportunities for them.

**"How do we identify the skills and competencies that a perons has."** A great starting place is the resume or vita of a person. So a tool which would help the peron to write a resume or cv is a good starting point. However Qualifications and experience are not always the same. So the system would need to be able to identify the skills and competencies that have been aquired due to certifications, but the user doesn´t really have. On the other hand there are also skills and competencies that a person has but are not verified or certified. So selecting and deselecting skills based on qualifications is probably the best way to do this. You can ofcourse argue that if two candidates who have the same occupation as starting point in their career but different skills and competencies due to the experience the resume of a person with skills would probably look different than the resume of the candidate with less experience.  
However the approach would always result in a lot of work for the user and the edge cases and the implementation of the system would probably result in a unfair system overall. 

So a distinction if the users have the skills aquired through qualifications or through experience can be assessed. Basically if the user has a skill selected by his choice than the system would indicate these by 0, while when a user has taken a assessment of the skills and competencies that are aquired through experience the system would indicate these by 1. If the user has a certification or qualification that is not verified or certified the system would still indicate these by 2. However occupations change over time so a qualification or certification just indicates the skill and competency that are associated to a qualification or certification at a specific point in time. While we can understand the origin date of a certification or qualification, we can not understand the skills or competencies associated to them at a given point in time, since there is no historical data available. 

So the apollo system would need to understand the  skills and competencies that are aquired through experience and the associated skills and competencies that are aquired through qualifications at a given point in time. 

**What skills and competencies do others think I have?**
Another approach we often see is the 360 feedback or Linkedin endorsement or peer feedback process, which is also an important aspect of the carrer development.
So basically these are skills and competencies that person has which are observable by poeple working with this person. The more people that are working with the person the more accurate the skills and competencies observed by others are. However the feedback of a expert in the field is probably to be ranked higher than the feedback of a person who is not working with the person. 

A lot of times people find themselfes in a situation where they need to change due to internal or external influenced circumstances. For example a person has medical conditions which doesn´t allow to work in a certain area. So the person can change his/her job role or occupation.

**Do I have the correct perception of my skills and competencies?**
When users do self assessment of their skills and competencies often time less skilled people tend to indicate and select higher skills and competency ratings then skilled and experienced people. 

**"What are the skills needed to be successful in the workplace?"**
Another discussion is the case where a craftsman occupation skilled person is working for over 10 years in a industrial vertical like car manufacturing. The person is not qualified for the job at the car manufacturer but the transversal skills and competencies aquired through the occupation are helping the person to aquire the skills and competencies needed for the job and experience defines the sucess of a person in this field. 

In a nutshell qualifications indicate which skills and competencies a person should have. Experience indicates skills and competencies a person needs to have to be succesfull, but how do you measure this. One arguement would be the reference letter. People are not necessarily working in the job where they use the skills and competencies that are indicated by a occupation. 

We should defnetly let a user explore the skills and competencies which are associate to a occupation. 

We can also identify the skills and competencies associated to a occupation over time. In terms of demand (job postings) so we can identify tends in specific occupations and industries.

One aproach could be, to implement several generic lists of skills and competencies. For example skills and competencies users are interested in can be identified by a user. Skills and competencies that are associated to a occupation can be identified by the system and associated to the user. Users can take quizes and assessments to identify their skills and competencies. 

## Technical Implementation

TBD

See Local [API Specifiacation v03.94](/docs/Specifications/Personas/ESCO%20-%20API%20v03.94.pdf) for more details.
