# Technical Debt Invite Apollo


>A Lannister Always Pays His Debts!

## Cloud

### Invite.Apollo.App.Graph.Commons

Protobuf-net contracts in Interfaces requires CallContext which is not CLS compliant.
See Warning
Severity	Code	Description	Project	File	Line	Suppression State
**Warning	CA1014**	Mark assemblies with CLSCompliant	Invite.Apollo.App.Graph.Common	C:\Users\PatricBoscolo\source\gh\APOLLO\src\cloud\backend\apollo.cloud\graph.invite-apollo.app.common\CSC	1	Active

[See documentation](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1014)

### Course and Courseappointment both implement price!

There is however are implementation of CourseOffer which is the relatio between courseappointment and courseprice
This however is not scope of the december testing! See Issue #

### Qualification Provider

Problem a list of Qualification Providers could be implemented as EduProviders.
However which Qualificatoin Providers are available in different region?
Which is the closest Qualification Provider based on users home address,
or based on workplace? Also note for example - qualification providers sometimes are not located at the workplace but the corporate headquaters? How to implement this?

### Learning Qutcomes could be a relation to Skills?
Learning Outcome is a set of Qualifications or Skills a user gains by taking the course - out of scope for december correct?
→ how do we implement certification for (Fachwirt)

### Should Course descriptions be HTML or Markdown?
Should Course Description be Markdown or HTML?
Für Testung ist das String! Syndication!
> NOTE: Most Training Providers use HTML as Input String needs syndication in the first place

> NOTE: We also have the issue with the domain specific knowledge as well as the skills we want to present the user as lookup?

### Implementation for Languages missing in december Prototype!

Course available in Languages are determined by the course appointments?
Would mean no appointment no language? maybe on demand languages? Languages - Appointments

## Client

