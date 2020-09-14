# Tracker-Webapp

This is a progress tracker webapp.
The application is split into two parts:

### Tracker-Server:
REST api providing all endpoints for the webapp. MonogDB is used to manage data storage and access.
This app uses MongoDB through the MongoDB.Driver NuGet package.
### Tracker-Web:
All UI. Implemented using blazor webassebly (similar to react framework, but all code is C#!).
