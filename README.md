# rate-limit-validator
<h3 align="left">
Possible Improvements:
</h3>

* A ConcurrencyDictionary is used in memory to check the rate limit, if a scale out is needed, Redis use can be implemented to handle mutiple instances of the same rate-limit-validator service
* Change the insert into the database to use the Event Driven