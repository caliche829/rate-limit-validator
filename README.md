# rate-limit-validator
<h3 align="left">
Implementation
</h3>

* The solution is following clean architecture and SOLID principles
* The solution enforces the two specific limits (per phone number and per account) using a ConcurrencyDictionary inside a singleton service.
* The solution is using sql server to track the request
* The solution is using a background job that runs daily to delete the older records
* The solution is using ef migrations to initial set up for the db
* All the applications can be deployed in docker using the docker compose
* All the values (rate limits, job frequency, period of days for old records, etc) can be set in the appsettings

<h3 align="left">
Possible Improvements:
</h3>

* A ConcurrencyDictionary is used in memory to check the rate limit, if a scale out is needed, Redis use can be implemented to handle mutiple instances of the same rate-limit-validator service
* Change the insert into the database to use the Event Driven